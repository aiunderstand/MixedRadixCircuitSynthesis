using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static BtnInput;
using static ReaderCSV;

public class LogicTestRunner : MonoBehaviour
{
    public string csvFile;
    public int start = 0; //start symbol for output comparison, eg. if we have 4 trits as output, we might want to check only the bottom 2 by doing start =2

    public void OnClickRun()
    {
        ReaderCSV.ReadCSV(csvFile);
        RunTests();
    }
    
    public void RunTests()
    {
        bool simSetting = SimulationManager.Instance.AllowUnstableCircuits;
        //check simulator if stable before continuing
        SimulationManager.Instance.AllowUnstableCircuits = true;
        TestThread();

        //set simulator to previous setting
        SimulationManager.Instance.AllowUnstableCircuits = simSetting;
    }

    public async void TestThread()
    {
        //do the processing
        //find inputs and outpus, note output can be ambiguious as port outputs are also output tags. We should refactor this
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");

        List<BtnInput> inputs = new List<BtnInput>();
        List<BtnInput> outputs = new List<BtnInput>();

        foreach (var c in components)
        {
            if (c.name.Contains("Input"))
            {
                var controller = c.GetComponentInChildren<InputController>();

                for (int i = 0; i < controller.Buttons.Count; i++)
                    inputs.Add(controller.Buttons[i].GetComponent<BtnInput>());
            }

            if (c.name.Contains("Output"))
            {
                var controller = c.GetComponentInChildren<InputController>();

                for (int i = 0; i < controller.Buttons.Count; i++)
                    outputs.Add(controller.Buttons[i].GetComponent<BtnInput>());
            }
        }


        int totalFailed = 0;
        foreach (var t in Tests)
        {
            //input testvector, we assume input amount equals input test vector symbols, we also assume radix is balanced ternary and order of inputs is order of order of test inputs 
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].SetValue(RadixOptions.BalancedTernary, convertInputToInt(t.Input[i]), true);
                await Task.Yield(); //WAIT 1 FRAME AS EVERY COMPNENT IS 1 UNIT DELAY
                await Task.Yield();
                await Task.Yield();
                await Task.Yield();


            }
            //read output
            string output = "";
            for (int i = 0; i < outputs.Count; i++)
            {
                var o = outputs[i].label.text;

                switch (o)
                {
                    case "-1":
                        output += "-";
                        break;
                    case "1":
                        output += "+";
                        break;
                    default:
                        output += o;
                        break;
                }

            }

            //compare to groundtruth
            if (ParseAndCompare(t.Output, output, start) == 0)
                t.TestResult = "ok";
            else
            {
                Debug.Log("[Error] input: " + t.Input + " result in output: " + output + " but should be:" + t.Output);
                t.TestResult = "fail";
                totalFailed++;
            }
        }

        Debug.Log("Done. Total failed: " + totalFailed);
    }

    //write to file
    //using (StreamWriter writer = new StreamWriter(path,false))
    //{
    //    writer.WriteLine(headerLine);
    //    foreach (var test in Tests)
    //    {
    //        //example data: 
    //        //id  type	        n	id_n	id_n-1	...	    id0	    radix_n     radix_n-1       ...	             radix_0	        value_n	value_n-1	...	value_0	decimal	test_result
    //        //0   input         4   b0      b1      b2      b3      binary      binary          binary           binary             0   0   0   0   0   na
    //        //0   groundtruth   4   t0      t1      t2      t3      bal.Ternary bal. Ternary    bal. Ternary     bal. Ternary       0   0   0   0   0   na
    //        //0   output        4   t0      t1      t2      t3      bal.Ternary bal. Ternary    bal. Ternary     bal. Ternary       0   0   0   0   0   not_tested

    //        //write inputline
    //        line = test.Id.ToString() + ",";
    //        line += "input,";
    //        line += test.N.ToString() +",";
    //        for (int i = test.N-1; i >= 0; i--)
    //        {
    //            line += test.Input.IoNames[i] + ",";
    //        }
    //        for (int i = test.N - 1; i >= 0; i--)
    //        {
    //            line += test.Input.IoRadixTypes[i].ToString() + ",";
    //        }
    //        for (int i = test.N - 1; i >= 0; i--)
    //        {
    //            line += test.Input.IoValues[i].ToString() + ",";
    //        }
    //        line += test.Input.DecimalValue + ",";
    //        line += "na";
    //        writer.WriteLine(line);

    //        //write groundtruth line
    //        line = test.Id.ToString() + ",";
    //        line += "groundtruth,";
    //        line += test.N.ToString() + ",";
    //        for (int i = test.N - 1; i >= 0; i--)
    //        {
    //            line += test.Groundtruth.IoNames[i] + ",";
    //        }
    //        for (int i = test.N - 1; i >= 0; i--)
    //        {
    //            line += test.Groundtruth.IoRadixTypes[i].ToString() + ",";
    //        }
    //        for (int i = test.N - 1; i >= 0; i--)
    //        {
    //            line += test.Groundtruth.IoValues[i].ToString() + ",";
    //        }
    //        line += test.Groundtruth.DecimalValue +",";
    //        line += "na";
    //        writer.WriteLine(line);

    //        //write outputline
    //        line = test.Id.ToString() + ",";
    //        line += "output,";
    //        line += test.N.ToString() + ",";
    //        for (int i = test.N - 1; i >= 0; i--)
    //        {
    //            line += test.Output.IoNames[i] + ",";
    //        }
    //        for (int i = test.N - 1; i >= 0; i--)
    //        {
    //            line += test.Output.IoRadixTypes[i].ToString() + ",";
    //        }
    //        for (int i = test.N - 1; i >= 0; i--)
    //        {
    //            line += test.Output.IoValues[i].ToString() + ",";
    //        }
    //        line += test.Output.DecimalValue + ",";
    //        line += test.TestResult;
    //        writer.WriteLine(line);
    //    }
    //}

    private static int ParseAndCompare(string truth, string output, int start)
    {
        int invalidPairs = 0;

        for (int i = start; i < output.Length; i++)
        {
            if (truth[i] != output[i])
                invalidPairs++;
        }

        return invalidPairs;
    }

    private static int convertInputToInt(char v)
    {
        int result;
        switch (v)
        {
            case '-':
                result = -1;
                break;
            case '+':
                result = 1;
                break;
            default:
                result = int.Parse(v.ToString());
                break;
        }

        return result;
    }

}

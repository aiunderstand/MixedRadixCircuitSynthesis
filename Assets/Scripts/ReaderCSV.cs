using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public static class ReaderCSV
{
    public static List<Test> Tests = new List<Test>(); 
    public static void ReadCSV(string path)
    {
        //init variables
        Tests.Clear();

        path = Application.dataPath + "/AutomatedTests/" + path;
       
        if (!File.Exists(path))
        {
            return;
        }

        int limit = 1000;
        var lineCount = 0;
        string line;
        int id =0;
        int n = 0;
        string testResult="";
        TestPart input = new TestPart();
        TestPart output = new TestPart();
        TestPart groundtruth = new TestPart();
        int decimalValue = 0;
        string headerLine ="";

        //read the file
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                if (lineCount == 0)
                    headerLine = line;

                if (lineCount > 0 && lineCount < limit) //skip header
                {
                    var attr = line.Split(',');

                    if (attr.Length > 0) // skip empty lines
                    {
                        //example data: 
                        //id  type	        n	id_n	id_n-1	...	    id0	    radix_n     radix_n-1       ...	             radix_0	        value_n	value_n-1	...	value_0	decimal	test_result
                        //0   input         4   b0      b1      b2      b3      binary      binary          binary           binary             0   0   0   0   0   na
                        //0   groundtruth   4   t0      t1      t2      t3      bal.Ternary bal. Ternary    bal. Ternary     bal. Ternary       0   0   0   0   0   na
                        //0   output        4   t0      t1      t2      t3      bal.Ternary bal. Ternary    bal. Ternary     bal. Ternary       0   0   0   0   0   not_tested

                        //parse general attributes to datatypes
                        id = int.Parse(attr[0]);
                        n = int.Parse(attr[2]);

                        //parse specific attributes to datatypes
                        List<string> ioNames = new List<string>();
                        for (int i = 3; i < 3 + n; i++)
                        {
                            ioNames.Add(attr[i]);
                        }
                        List<ioRadixType> ioRadixTypes = new List<ioRadixType>();
                        for (int i = 3 + n; i < 3 + 2 * n; i++)
                        {
                            ioRadixTypes.Add((ioRadixType)Enum.Parse(typeof(ioRadixType), attr[i]));
                        }
                        List<int> ioValues = new List<int>();
                        for (int i = 3 + 2 * n; i < 3 + 3 * n; i++)
                        {
                            ioValues.Add(int.Parse(attr[i]));
                        }

                        decimalValue = int.Parse(attr[attr.Length-2]);

                        //create testParts
                        ioType type = (ioType)Enum.Parse(typeof(ioType), attr[1]);
                        switch (type)
                        {
                            case ioType.input:
                                {
                                    input = new TestPart(ioNames,ioRadixTypes,ioValues, decimalValue);     
                                }
                                break;
                            case ioType.output:
                                {
                                    output = new TestPart(ioNames, ioRadixTypes, ioValues, decimalValue);
                                    testResult = attr[attr.Length - 1];
                                }
                                break;

                            case ioType.groundtruth:
                                {
                                    groundtruth = new TestPart(ioNames, ioRadixTypes, ioValues, decimalValue);
                                }
                                break;
                        }
                    }

                    if (lineCount % 3 == 0)
                    {
                        Test test = new Test(id, n, input, output, groundtruth, testResult);
                        Tests.Add(test);
                    }
                }
                lineCount++;
            }

            Debug.Log("Finished reading: " + path + ". Total tests: " + Tests.Count);

            



        }

        //do the processing
        //find inputs and outpus, note output can be ambiguious as port outputs are also output tags. We should refactor this
        GameObject[] inputGOs = GameObject.FindGameObjectsWithTag("Input");
        GameObject[] outputGOs = GameObject.FindGameObjectsWithTag("Output");

        for (int t=0; t< Tests.Count; t++)
        {
            //we first need to find the correct input
            Test test = Tests[t];
            
            string status ="pass";
            for (int i = 0; i < test.N; i++)
            {
                //set input
                foreach (var iGO in inputGOs)
                {
                    var ioName = iGO.GetComponentInChildren<TMP_InputField>().text;
                    if (test.Input.IoNames[i].Equals(ioName))
                    {
                        BtnInput.RadixOptions radix = (test.Input.IoRadixTypes[i] ==  ioRadixType.binary  ) ? BtnInput.RadixOptions.Binary : BtnInput.RadixOptions.BalancedTernary;

                        //set input to radix
                        if (radix == BtnInput.RadixOptions.Binary)
                            iGO.transform.parent.GetComponentInChildren<TMP_Dropdown>().value = 2;
                        else
                            iGO.transform.parent.GetComponentInChildren<TMP_Dropdown>().value = 0;


                        //set input to value
                        iGO.GetComponent<BtnInput>().SetValue(radix, test.Input.IoValues[i], true);
                    }
                }

                //read output
                foreach (var oGO in outputGOs)
                {
                    var ioName = oGO.GetComponentInChildren<TMP_InputField>();
                    if (ioName != null)
                    {
                        if (test.Output.IoNames[i].Equals(ioName.text))
                        {
                            BtnInput.RadixOptions radix = (test.Output.IoRadixTypes[i] == ioRadixType.binary) ? BtnInput.RadixOptions.Binary : BtnInput.RadixOptions.BalancedTernary;

                            //set output to radix
                            if (radix == BtnInput.RadixOptions.Binary)
                                oGO.transform.parent.GetComponentInChildren<TMP_Dropdown>().value = 2;
                            else
                                oGO.transform.parent.GetComponentInChildren<TMP_Dropdown>().value = 0;

                            //read output 
                            test.Output.IoValues[i] = int.Parse(oGO.GetComponent<BtnInput>().label.text);
                            test.Output.DecimalValue = int.Parse(oGO.GetComponent<BtnInput>().transform.GetComponentInParent<InputController>().CounterLabel.text);
                        }
                    }
                }

                //compare to groundtruth
                if (test.Output.DecimalValue != test.Groundtruth.DecimalValue)
                    status = "fail"; 
            }

            test.TestResult = status;
        }
    
        //write to file
        using (StreamWriter writer = new StreamWriter(path,false))
        {
            writer.WriteLine(headerLine);
            foreach (var test in Tests)
            {
                //example data: 
                //id  type	        n	id_n	id_n-1	...	    id0	    radix_n     radix_n-1       ...	             radix_0	        value_n	value_n-1	...	value_0	decimal	test_result
                //0   input         4   b0      b1      b2      b3      binary      binary          binary           binary             0   0   0   0   0   na
                //0   groundtruth   4   t0      t1      t2      t3      bal.Ternary bal. Ternary    bal. Ternary     bal. Ternary       0   0   0   0   0   na
                //0   output        4   t0      t1      t2      t3      bal.Ternary bal. Ternary    bal. Ternary     bal. Ternary       0   0   0   0   0   not_tested
                
                //write inputline
                line = test.Id.ToString() + ",";
                line += "input,";
                line += test.N.ToString() +",";
                for (int i = test.N-1; i >= 0; i--)
                {
                    line += test.Input.IoNames[i] + ",";
                }
                for (int i = test.N - 1; i >= 0; i--)
                {
                    line += test.Input.IoRadixTypes[i].ToString() + ",";
                }
                for (int i = test.N - 1; i >= 0; i--)
                {
                    line += test.Input.IoValues[i].ToString() + ",";
                }
                line += test.Input.DecimalValue + ",";
                line += "na";
                writer.WriteLine(line);

                //write groundtruth line
                line = test.Id.ToString() + ",";
                line += "groundtruth,";
                line += test.N.ToString() + ",";
                for (int i = test.N - 1; i >= 0; i--)
                {
                    line += test.Groundtruth.IoNames[i] + ",";
                }
                for (int i = test.N - 1; i >= 0; i--)
                {
                    line += test.Groundtruth.IoRadixTypes[i].ToString() + ",";
                }
                for (int i = test.N - 1; i >= 0; i--)
                {
                    line += test.Groundtruth.IoValues[i].ToString() + ",";
                }
                line += test.Groundtruth.DecimalValue +",";
                line += "na";
                writer.WriteLine(line);

                //write outputline
                line = test.Id.ToString() + ",";
                line += "output,";
                line += test.N.ToString() + ",";
                for (int i = test.N - 1; i >= 0; i--)
                {
                    line += test.Output.IoNames[i] + ",";
                }
                for (int i = test.N - 1; i >= 0; i--)
                {
                    line += test.Output.IoRadixTypes[i].ToString() + ",";
                }
                for (int i = test.N - 1; i >= 0; i--)
                {
                    line += test.Output.IoValues[i].ToString() + ",";
                }
                line += test.Output.DecimalValue + ",";
                line += test.TestResult;
                writer.WriteLine(line);
            }
        }
    }

}

public class Test
{
    public int Id;
    public int N;
    public TestPart Input;
    public TestPart Output;
    public TestPart Groundtruth;
    public string TestResult;

    public Test(int id, int n, TestPart input, TestPart output, TestPart groundtruth, string testResult)
    {
        Id = id;
        N = n;
        Input = input;
        Output = output;
        Groundtruth = groundtruth;
        TestResult = testResult;
    }
}

public class TestPart
{
    public List<string> IoNames;
    public List<ioRadixType> IoRadixTypes;
    public List<int> IoValues;
    public int DecimalValue;
    
    public TestPart() { }
    public TestPart(List<string> ioNames, List<ioRadixType> ioRadixTypes, List<int> ioValues, int decimalValue)
    {
        IoNames = ioNames;
        IoRadixTypes = ioRadixTypes;
        IoValues = ioValues;
        DecimalValue = decimalValue;
    }
}

public enum ioType
{
    input,
    output,
    groundtruth
}

public enum ioRadixType
{
    binary,
    balanced_ternary
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitGenerator : MonoBehaviour
{
    public string path = "";
  public void OnClick()
    {
        GenerateCircuit(path);
    }

    private void GenerateCircuit(string path)
    {
        int compCount = 0;
        List<string> ttIndices = new List<string>();
        List<int> arityArray = new List<int>();
        List<int> invArray = new List<int>();
        List<string> connectionArray = new List<string>();

        path = Application.dataPath + "/GeneratedCircuits/" + path;
        //get all connections and truth tables
        var connections = GameObject.FindGameObjectsWithTag("Wire");
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");

        //foreach tt, generate a netlist
        foreach (var c in components)
        {
            if (c.name.Contains("LogicGate"))
            {
                var controller = c.GetComponentInChildren<InputControllerLogicGate>();
                int[] tt = controller.GetTruthTable();
                int arity = controller.GetArity();
                arityArray.Add(arity);

                compCount++;

                //we should just create a c++ data structure and marshall this. Now it is important that we first call create netlist!
                TruthtableFunctionHelper.CreateNetlist(path, tt, arity); //from unoptimized tt
                int[] optimizedTT = TruthtableFunctionHelper.GetOptimizedTT(arity);
                string optimizedTTindex = TruthtableFunctionHelper.ConvertTTtoHeptEncoding(optimizedTT);
                ttIndices.Add(optimizedTTindex);

                int[] tempInvArray = TruthtableFunctionHelper.GetAndConvertInvArrayFormat(arity);

                for (int i = 0; i < 9; i++) //always 9
                {
                    invArray.Add(tempInvArray[i]);
                }

                //find all connections belonging to logic gate
                string[] tempConnArray = new string[4];

                foreach (var conn in connections)
                {
                    if (conn.name.Contains(controller.Id.ToString()))
                    {
                        var parts = conn.name.Split(';');


                        //is output
                        if (parts[2] == controller.Id.ToString())
                            tempConnArray[3] = parts[6]+ parts[7];
                        else //it is input
                        {
                            //find out if input 1,2 or 3 (port A,B,C)
                            switch (parts[7])
                            {
                                case "PortA":
                                    tempConnArray[0] = parts[2]+parts[3];
                                    break;
                                case "PortB":
                                    tempConnArray[1] = parts[2]+parts[3];
                                    break;
                                case "PortC":
                                    tempConnArray[2] = parts[2]+parts[3];
                                    break;

                            }
                        }
                    }
                    
                }

           
                
                
                
                //fill in the missing
                if (arity == 1)
                {
                    tempConnArray[1] = "";
                    tempConnArray[2] = "";
                }

                if (arity == 2)
                {
                    tempConnArray[2] = "";
                }

                //add temp to array
                for (int i = 0; i < tempConnArray.Length; i++)
                {
                    connectionArray.Add(tempConnArray[i]);
                }
            }
        }
        //    connectionArray
        //         return {
        //        { "in0", "in1", "", "out_ckt0"}  ,       //20K
        //{ "out_ckt0", "in2", "", "out0"}  ,      //20K
        //{ "in0", "in1", "", "out_ckt2"},         //K00
        //{ "out_ckt0", "in2", "", "out_ckt3"},    //K00
        //{ "out_ckt2", "out_ckt3", "", "out2"},   //ZKK
        //};


        int input = 0;
        int output = 0;
       
        //find inputs and outputs, currently we look at the created amount not the connected amount. This could lead to bugs?
        //also only one input and one output component is allowed (for PoC)
        foreach (var c in components)
        {
            if (c.name.Contains("Input"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                input = inputControler.Buttons.Count;
            }

            if (c.name.Contains("Output"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                output = inputControler.Buttons.Count;
            }
        }

        TruthtableFunctionHelper.CreateCircuit(input, output, compCount, ttIndices.ToArray(), arityArray.ToArray(), connectionArray.ToArray(), invArray.ToArray());
    }
}

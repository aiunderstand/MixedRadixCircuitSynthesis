using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CircuitGenerator : MonoBehaviour
{
    public void OnClick()
    {
        GenerateCircuit("Temp/", "circuit");
    }

    private void GenerateCircuit(string filePath, string fileName)
    {
        int compCount = 0;
        List<string> ttIndices = new List<string>();
        List<int> arityArray = new List<int>();
        List<int> invArray = new List<int>();
        List<string> connectionArray = new List<string>();
        List<string> inputNames = new List<string>();
        List<string> outputNames = new List<string>();

        string path = Application.persistentDataPath + "/User/Generated/" + filePath;

        //check if folder exist otherwise create folder 
        System.IO.Directory.CreateDirectory(path);

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
                    if (conn.name.Contains(controller.GetInstanceID().ToString()))
                    {
                        var parts = conn.name.Split(';');


                        //is output
                        if (parts[2] == controller.GetInstanceID().ToString())
                        {
                            //check if this is a terminal output, if so add to list
                            if (parts[5].Contains("Output"))
                            {
                                tempConnArray[3] = parts[7].Replace(" ", string.Empty) + "_" + parts[6];
                            }
                            else
                            {
                                tempConnArray[3] = parts[3] + parts[2] + "_to_" + parts[7] + parts[6];                                
                            }
                        }
                        else //it is input
                        {
                            //find out if input 1,2 or 3 (port A,B,C)
                            switch (parts[7])
                            {
                                case "PortA":
                                    if (parts[1].Contains("Input"))
                                        tempConnArray[0] = parts[3].Replace(" ", string.Empty) + "_" + parts[2];
                                    else
                                        tempConnArray[0] = parts[3] + parts[2] + "_to_" + parts[7] + parts[6];
                                    break;
                                case "PortB":
                                    if (parts[1].Contains("Input"))
                                        tempConnArray[1] = parts[3].Replace(" ", string.Empty) + "_" + parts[2];
                                    else
                                        tempConnArray[1] = parts[3] + parts[2] + "_to_" + parts[7] + parts[6];
                                    break;
                                case "PortC":
                                    if (parts[1].Contains("Input"))
                                        tempConnArray[2] = parts[3].Replace(" ", string.Empty) + "_" + parts[2];
                                    else
                                        tempConnArray[2] = parts[3] + parts[2] + "_to_" + parts[7] + parts[6];
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
       
        foreach (var c in components)
        {
            if (c.name.Contains("Input"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                string id = inputControler.GetInstanceID().ToString();
                
                foreach (var b in inputControler.Buttons)
                {
                    //double check if button is connected to something
                    var bi = b.GetComponent<BtnInput>();

                    if (bi.Connections.Count > 0)
                    {
                        int portIndex = bi._portIndex;
                        string port = b.GetComponentInChildren<TMP_InputField>().text;
                        inputNames.Add(port.Replace(" ", string.Empty) + portIndex + "_" + id);
                    }
                }
            }
        }

        foreach (var c in components)
        {
            if (c.name.Contains("Output"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                string id = inputControler.GetInstanceID().ToString();

                foreach (var b in inputControler.Buttons)
                {
                    //double check if button is connected to something
                    var bi = b.GetComponent<BtnInput>();

                    if (bi.Connections.Count > 0)
                    {
                        int portIndex = bi._portIndex;
                        string port = b.GetComponentInChildren<TMP_InputField>().text;
                        outputNames.Add(port.Replace(" ", string.Empty) + portIndex + "_" + id);
                    }
                }
            }
        }

        TruthtableFunctionHelper.CreateCircuit(path, fileName, inputNames.ToArray(), inputNames.Count, outputNames.ToArray(), outputNames.Count,compCount, ttIndices.ToArray(), arityArray.ToArray(), connectionArray.ToArray(), invArray.ToArray());
    }

    public void SaveComponent(string name)
    {
        GenerateCircuit(name +"/", name);
    }
}

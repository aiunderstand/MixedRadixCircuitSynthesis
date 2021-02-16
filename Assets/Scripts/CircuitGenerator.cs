using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CircuitGenerator : MonoBehaviour
{
    public void OnClick()
    {
        GenerateCircuit("Temp/", "circuit");
    }

    private bool GenerateCircuit(string filePath, string fileName)
    {
        int uid = 0;
        int compCount = 0;
        List<string> ttIndices = new List<string>();
        List<int> arityArray = new List<int>();
        List<int> invArray = new List<int>();
        List<string> connectionArray = new List<string>();
        List<string> inputNames = new List<string>();
        List<string> outputNames = new List<string>();

        string path = Application.persistentDataPath + "/User/Generated/" + filePath;

        bool exists = System.IO.Directory.Exists(path);

        if (!exists || filePath.Equals("Temp/"))
        {
            System.IO.Directory.CreateDirectory(path);

            //get all connections and truth tables
            var connections = GameObject.FindGameObjectsWithTag("Wire");
            var components = GameObject.FindGameObjectsWithTag("DnDComponent");

            Hashtable inputLOT = new Hashtable();

            //PASS 1: input names with semantic names
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
                            //check if connection is directly to output, since then it should not be added
                            int outputCounter = 0;
                            foreach (var conn in bi.Connections)
                            {
                                var parts = conn.name.Split(';');

                                if (parts[5].Contains("Output"))
                                {
                                    outputCounter++;
                                }
                            }

                            //only add input if there is at least one connection to a logicgate
                            if (outputCounter != bi.Connections.Count)
                            {
                                int portIndex = bi._portIndex;
                                string portLabel = b.GetComponentInChildren<TMP_InputField>().text;
                                inputNames.Add(portLabel + "_" + uid); //refactor to only use inputLOT, build the tree and then convert the inputlot to input names
                                inputLOT.Add(portIndex + "_" + id, portLabel + "_" + uid);
                                uid++;
                            }
                        }
                    }
                }
            }

            //PASS 2: output names with semantic names
            Hashtable outputLOT = new Hashtable();
            //create output names hashtable, this code section is solely for adding the output label to an logic gate terminal, refactor?
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
                            string portLabel = b.GetComponentInChildren<TMP_InputField>().text;
                            outputLOT.Add(portIndex + "_" + id, portLabel);
                        }
                    }
                }
            }

            //PASS 3: logic gate generate TT netlist and LOT with semantic names 
            Hashtable logicgateLOT = new Hashtable();
            Hashtable logicgateIndicesLOT = new Hashtable();
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
                    logicgateIndicesLOT.Add(controller.GetInstanceID().ToString(), optimizedTTindex);
                    //controller.DropDownFunctionLabel.text = optimizedTTindex;

                    var functionDropDown = controller.GetComponentInChildren<AutoCompleteComboBox>();
                    functionDropDown._mainInput.text = optimizedTTindex;
                    //functionDropDown.ToggleDropdownPanel();

                    int[] tempInvArray = TruthtableFunctionHelper.GetAndConvertInvArrayFormat(arity);

                    for (int i = 0; i < 9; i++) //always 9
                    {
                        invArray.Add(tempInvArray[i]);
                    }

                }
            }

            //PASS 4: connections with output (backwards pass with only nodes thats are connected to output)
            foreach (var c in components)
            {
                if (c.name.Contains("LogicGate"))
                {
                    var controller = c.GetComponentInChildren<InputControllerLogicGate>();
                    int arity = controller.GetArity();

                    foreach (var conn in connections)
                    {
                        if (conn.name.Contains(controller.GetInstanceID().ToString()))
                        {
                            var parts = conn.name.Split(';');

                            if (parts[2] == controller.GetInstanceID().ToString())  //check if output
                            {
                                if (parts[5].Contains("Output"))
                                {
                                    //not really efficient code
                                    bool isFound = false;
                                    foreach (DictionaryEntry id in logicgateLOT)
                                    {
                                        if ((string)id.Key == (parts[3] + "_" + parts[2])) //match identifier of component
                                        {
                                            isFound = true;

                                            //check if output is the same if not, add
                                            var current = (string)outputLOT[(string)(parts[7] + "_" + parts[6])];
                                            var stored = (string)logicgateLOT[id.Key];
                                            if (current != stored)
                                            {
                                                outputNames.Add((string)logicgateLOT[id.Key]);
                                                outputLOT[(string)(parts[7] + "_" + parts[6])] = stored;
                                            }
                                        }
                                    }

                                    if (!isFound) //add to hastables and tempConnArray
                                    {
                                        //update the output LOT with a UID and add to final outputNames list
                                        var id = (string)outputLOT[(string)(parts[7] + "_" + parts[6])] + "_" + uid;
                                        outputLOT[(string)(parts[7] + "_" + parts[6])] = id; //update id with uid
                                        outputNames.Add(id);
                                        uid++;

                                        logicgateLOT.Add((parts[3] + "_" + parts[2]), id);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //PASS 5: forward pass, fill connection array
            foreach (var c in components)
            {
                if (c.name.Contains("LogicGate"))
                {
                    var controller = c.GetComponentInChildren<InputControllerLogicGate>();
                    int arity = controller.GetArity();

                    //find all connections belonging to logic gate
                    string[] tempConnArray = new string[4];

                    foreach (var conn in connections)
                    {
                        if (conn.name.Contains(controller.GetInstanceID().ToString()))
                        {
                            var parts = conn.name.Split(';');

                            if (parts[2] == controller.GetInstanceID().ToString())  //check if output
                            {
                                if (parts[5].Contains("Output"))
                                {
                                    foreach (DictionaryEntry id in logicgateLOT)
                                    {
                                        if ((string)id.Key == (parts[3] + "_" + parts[2])) //match identifier of component
                                        {
                                            tempConnArray[3] = (string)logicgateLOT[id.Key];
                                        }
                                    }
                                }
                                else
                                {
                                    bool isFound = false;
                                    foreach (DictionaryEntry id in logicgateLOT)
                                    {
                                        if ((string)id.Key == (parts[3] + "_" + parts[2])) //match identifier of component
                                        {
                                            isFound = true;
                                            tempConnArray[3] = (string)logicgateLOT[id.Key];
                                        }
                                    }

                                    if (!isFound) //add to hastables and tempConnArray
                                    {
                                        //update the output LOT with a UID and add to final outputNames list
                                        var id = (string)logicgateIndicesLOT[(string)(parts[2])] + "_" + uid;
                                        uid++;

                                        logicgateLOT.Add((parts[3] + "_" + parts[2]), id);
                                        tempConnArray[3] = id;
                                    }
                                }
                            }
                            else //it is input
                            {
                                int index = int.Parse(parts[7]);

                                if (parts[1].Contains("Input"))
                                {
                                    foreach (DictionaryEntry id in inputLOT)
                                    {
                                        if ((string)id.Key == (parts[3] + "_" + parts[2])) //match identifier of component
                                        {
                                            tempConnArray[index] = (string)inputLOT[id.Key];
                                        }
                                    }
                                }
                                else
                                {
                                    bool isFound = false;
                                    foreach (DictionaryEntry id in logicgateLOT)
                                    {
                                        if ((string)id.Key == (parts[3] + "_" + parts[2])) //match identifier of component
                                        {
                                            tempConnArray[index] = (string)logicgateLOT[id.Key];
                                            isFound = true;
                                        }
                                    }

                                    if (!isFound)
                                    {
                                        //update the output LOT with a UID and add to final outputNames list
                                        var id = (string)logicgateIndicesLOT[(string)(parts[2])] + "_" + uid;
                                        uid++;

                                        logicgateLOT.Add((parts[3] + "_" + parts[2]), id);
                                        tempConnArray[index] = id;
                                    }
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

            //doube check if everything is there before submitting to memory sensitive c++ land
            bool fail = false;
            foreach (var conn in connectionArray)
            {
                if (conn == null)
                    fail = true;
            }

            if (!fail)
                TruthtableFunctionHelper.CreateCircuit(path, fileName, inputNames.ToArray(), inputNames.Count, outputNames.ToArray(), outputNames.Count, compCount, ttIndices.ToArray(), arityArray.ToArray(), connectionArray.ToArray(), invArray.ToArray());

            return true;
        }
        else
        {
            Debug.Log("Folder Exist, choose different name");
            return false;
        }
    }

    public bool SaveComponent(string name)
    {
        bool success = GenerateCircuit(name +"/", name);

        return success;

    }
}

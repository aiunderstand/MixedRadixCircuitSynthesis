using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;
using static BtnInput;

public class CircuitGenerator : MonoBehaviour
{
    private Stats GenerateCircuit(string filePath, string fileName)
    {
        int uid = 0;
        List<string> ttIndices = new List<string>();
        List<int> arityArray = new List<int>();
        List<int> invArray = new List<int>();
        List<string> connectionArray = new List<string>();
        List<string> inputNames = new List<string>();
        List<string> outputNames = new List<string>();
        List<string> positionArray = new List<string>();
        List<string> inputRadix = new List<string>();
        List<string> outputRadix = new List<string>();
        List<string> savedCircuitNames = new List<string>();
        List<int> connectionIndexArray = new List<int>();
        string path = Application.persistentDataPath + "/User/Generated/" + filePath;

        bool exists = System.IO.Directory.Exists(path);
        Stats stats = new Stats();
        stats.transistorCount = 0;
        stats.abstractionLevelCount = 0;
        stats.success = false;
        stats.netlistPath = "";
        stats.totalLogicGateCount = 0;
        stats.uniqueLogicGateCount = 0;
        stats.netlistName = "";
        if (!exists)
        {
            System.IO.Directory.CreateDirectory(path);

            //get all connections and truth tables
            var connections = GameObject.FindGameObjectsWithTag("Wire");
            var components = GameObject.FindGameObjectsWithTag("DnDComponent");

            OrderedDictionary inputLOT = new OrderedDictionary();

            //PASS 1: input names with semantic names
            Dictionary<int, float> inputOrder = new Dictionary<int, float>(); //needed for ordering the labels from low to high (using transform.position.y)
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
                                inputOrder.Add(inputRadix.Count, bi.transform.position.y);
                               
                                int portIndex = bi._portIndex;
                                string portLabel = b.GetComponentInChildren<TMP_InputField>().text;
                                inputNames.Add(portLabel + "_" + uid); //refactor to only use inputLOT, build the tree and then convert the inputlot to input names
                                inputLOT.Add(portIndex + "_" + id, portLabel + "_" + uid);
                                uid++;
                                inputRadix.Add(bi.DropdownLabel.text);
                            }
                        }
                    }
                }
            }

            //reorder the input labels to get correct interface order
            List<string> tempinputNames = new List<string>();
            List<string> tempInputRadix = new List<string>();

            //sort inputOrder
            var sorted = inputOrder.OrderBy(key => key.Value);

            //assign tempLabels in correct order
            foreach (var item in sorted)
            {
                tempinputNames.Add(inputNames[item.Key]);
                tempInputRadix.Add(inputRadix[item.Key]);
            }

            inputNames = tempinputNames;
            inputRadix = tempInputRadix;

            //PASS 2: output names with semantic names
            OrderedDictionary outputLOT = new OrderedDictionary();
            //create output names OrderedDictionary/hashtable, this code section is solely for adding the output label to an logic gate terminal, refactor?
            Dictionary<int, float> outputOrder = new Dictionary<int, float>();
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
                            outputOrder.Add(outputRadix.Count, bi.transform.position.y);

                            int portIndex = bi._portIndex;
                            string portLabel = b.GetComponentInChildren<TMP_InputField>().text;
                            outputLOT.Add(portIndex + "_" + id, portLabel);
                            outputRadix.Add(bi.DropdownLabel.text);
                        }
                    }
                }
            }

            //PASS 3a: logic gate generate TT netlist and LOT with semantic names 
            OrderedDictionary logicgateLOT = new OrderedDictionary();
            OrderedDictionary logicgateIndicesLOT = new OrderedDictionary();
            foreach (var c in components)
            {
                if (c.name.Contains("LogicGate"))
                {
                    var controller = c.GetComponentInChildren<InputControllerLogicGate>();
                    int[] tt = controller.GetTruthTable();
                    int arity = controller.GetArity();
                    arityArray.Add(arity);

                    //we should just create a c++ data structure and marshall this. Now it is important that we first call create netlist!
                    stats.transistorCount += TruthtableFunctionHelper.CreateNetlist(path, tt, arity); //from unoptimized tt
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

                    //get positions
                    Vector2 pos = c.GetComponentInParent<DragDrop>().gameObject.transform.position;
                    positionArray.Add(pos.x.ToString());
                    positionArray.Add(pos.y.ToString());
                }
            }

            //PASS 3b: saved gate dont generate TT netlist but do add to LOT with semantic names 
            foreach (var c in components)
            {
                if (c.name.Contains("SavedGate"))
                {
                    var controller = c.GetComponentInChildren<SavedComponentController>();

                    stats.transistorCount += controller.savedComponent.Stats.transistorCount;
                    stats.totalLogicGateCount += controller.savedComponent.Stats.totalLogicGateCount;
                    if (controller.savedComponent.Stats.abstractionLevelCount > stats.abstractionLevelCount)
                        stats.abstractionLevelCount = controller.savedComponent.Stats.abstractionLevelCount;

                    savedCircuitNames.Add("c_" + controller.savedComponent.ComponentName);
                    logicgateIndicesLOT.Add(controller.GetInstanceID().ToString(), controller.savedComponent.ComponentName);

                    //get positions
                    Vector2 pos = c.GetComponentInParent<DragDrop>().gameObject.transform.position;
                    positionArray.Add(pos.x.ToString());
                    positionArray.Add(pos.y.ToString());

                    //move all .sp files to path, remove/overwrite existing f_* files
                    var fpath = Path.GetDirectoryName(controller.savedComponent.ComponentNetlistPath);
                    var files = System.IO.Directory.GetFiles(fpath, "*.sp", SearchOption.TopDirectoryOnly);
                    for (int i = 0; i < files.Length; i++)
                    {
                        var filenameWithExtension = new DirectoryInfo(System.IO.Path.GetFileName(files[i]));
                        var targetFilePath = path + filenameWithExtension;
                        //first, delete target file if exists, as File.Move() does not support overwrite
                        if (File.Exists(targetFilePath))
                        {
                            File.Delete(targetFilePath);
                        }

                        System.IO.File.Copy(files[i], targetFilePath);
                    }
                }
            }

            //PASS 4a: connections with output (backwards pass with only nodes thats are connected to output)
            foreach (var c in components)
            {
                if (c.name.Contains("LogicGate"))
                {
                    var controller = c.GetComponentInChildren<InputControllerLogicGate>();

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

            //PASS 4b: connections with output (backwards pass with only nodes thats are connected to output)
            foreach (var c in components)
            {
                if (c.name.Contains("SavedGate"))
                {
                    var controller = c.GetComponentInChildren<SavedComponentController>();

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

            //reorder the output labels to get correct interface order
            List<string> tempOutputNames = new List<string>();
            List<string> tempOutputRadix = new List<string>();

            //sort inputOrder
            var sorted1 = outputOrder.OrderBy(key => key.Value);

            //assign tempLabels in correct order
            foreach (var item in sorted1)
            {
                tempOutputNames.Add(outputLOT[item.Key].ToString());
                tempOutputRadix.Add(outputRadix[item.Key]);
            }

            outputNames = tempOutputNames;
            outputRadix = tempOutputRadix;

            //PASS 5a: forward pass, fill connection array
            foreach (var c in components)
            {
                if (c.name.Contains("LogicGate"))
                {
                    var controller = c.GetComponentInChildren<InputControllerLogicGate>();
                    var arity = controller.GetArity();

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

                    connectionIndexArray.Add(tempConnArray.Length);
                }
            }

            //PASS 5b: forward pass, fill connection array
            foreach (var c in components)
            {
                if (c.name.Contains("SavedGate"))
                {
                    var controller = c.GetComponentInChildren<SavedComponentController>();
                    int indexIn = controller.savedComponent.Inputs.Count;
                    int indexOut = controller.savedComponent.Outputs.Count;

                    string[] tempConnArray = new string[indexIn+indexOut];

                    foreach (var conn in connections)
                    {
                        if (conn.name.Contains(controller.GetInstanceID().ToString()))
                        {
                            var parts = conn.name.Split(';');

                            if (parts[2] == controller.GetInstanceID().ToString())  //check if output
                            {
                                int index = int.Parse(parts[3]);

                                if (parts[5].Contains("Output")) //special rule for output terminals
                                {
                                    foreach (DictionaryEntry id in logicgateLOT)
                                    {
                                        if ((string)id.Key == (parts[3] + "_" + parts[2])) //match identifier of component
                                        {
                                            tempConnArray[indexIn + index] = (string)logicgateLOT[id.Key];
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
                                            tempConnArray[indexIn+ index] = (string)logicgateLOT[id.Key];
                                        }
                                    }

                                    if (!isFound) //add to hastables and tempConnArray
                                    {
                                        //update the output LOT with a UID and add to final outputNames list
                                        var id = (string)logicgateIndicesLOT[(string)(parts[2])] + "_" + uid;
                                        uid++;

                                        logicgateLOT.Add((parts[3] + "_" + parts[2]), id);
                                        tempConnArray[indexIn + index] = id;
                                    }
                                }
                            }
                            else //it is input
                            {
                                int index = int.Parse(parts[7]);

                                if (parts[1].Contains("Input")) //special rule for input terminals
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

                    //add temp to array
                    for (int i = 0; i < tempConnArray.Length; i++)
                    {
                        connectionArray.Add(tempConnArray[i]);
                    }

                    connectionIndexArray.Add(tempConnArray.Length);
                }
            }

            //doube check if everything is there before submitting to memory sensitive c++ land
            bool fail = false;
            //bool fail = true; 
            foreach (var conn in connectionArray)
            {
                if (conn == null)
                    fail = true;
            }

            if (!fail)
            {
                var inverterCount = TruthtableFunctionHelper.CreateCircuit(
                    path, 
                    fileName,
                    inputNames.Count,
                    inputNames.ToArray(),
                    outputNames.Count,
                    outputNames.ToArray(), 
                    ttIndices.Count, 
                    ttIndices.ToArray(),
                    arityArray.Count,
                    arityArray.ToArray(),
                    connectionArray.Count, 
                    connectionArray.ToArray(), 
                    invArray.Count,
                    invArray.ToArray(),
                    positionArray.Count,
                    positionArray.ToArray(), 
                    savedCircuitNames.Count, 
                    savedCircuitNames.ToArray(),
                    connectionIndexArray.Count, 
                    connectionIndexArray.ToArray());


                stats.transistorCount += inverterCount;
                stats.totalLogicGateCount += ttIndices.Count; //from logic gates, the saved gates are already stored
                var files = System.IO.Directory.GetFiles(path, "f_*", SearchOption.TopDirectoryOnly);
                stats.uniqueLogicGateCount = files.Length ; //count the total f_ files in the directory
                stats.abstractionLevelCount += 1;
                stats.success = true;

                //add stats to main circuit file, we should refactor this to search in file for specific headers and replace content in them
                string fPath = path + fileName + ".sp";
                List<string> lines = new List<string>();
                using (StreamReader reader = new StreamReader(fPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                using (StreamWriter writer = new StreamWriter(fPath, false))
                {
                    writer.WriteLine(lines[0]);
                    writer.WriteLine("*** @tcount "  + stats.transistorCount.ToString());
                    writer.WriteLine("*** @gcount " + stats.totalLogicGateCount.ToString());
                    writer.WriteLine("*** @ugcount " + stats.uniqueLogicGateCount.ToString());
                    writer.WriteLine("*** @abslvl " + stats.abstractionLevelCount.ToString());
                    writer.WriteLine(lines[5]);
                    writer.WriteLine(lines[6]);

                    string inputR = "";
                    for (int i = 0; i < inputRadix.Count; i++)
                    {
                        inputR += inputRadix[i] + " ";
                    }
                    writer.WriteLine("*** @inputs " + inputR.TrimEnd(' '));

                    writer.WriteLine(lines[8]);
                    
                    string outputR = "";
                    for (int i = 0; i < outputRadix.Count; i++)
                    {
                        outputR += outputRadix[i] + " ";
                    }
                    writer.WriteLine("*** @outputs " + outputR.TrimEnd(' '));
                    
                    writer.WriteLine(lines[10]);

                    for (int i = 11; i < lines.Count; i++)
                    {
                        writer.WriteLine(lines[i]);
                    }
                }
            }
            else
            {
                Debug.Log("Connections error, check if everything is connected");
                stats.success = false;
            }

            return stats;
        }
        else
        {
            Debug.Log("Folder Exist, choose different name");
            stats.success = false;
            return stats;
        }
    }

    public Stats SaveComponent(string name)
    {
        Stats stats = GenerateCircuit(name +"/", "c_" + name);

        return stats;
    }
}

public class Stats
{
    public int transistorCount;
    public int uniqueLogicGateCount;
    public int totalLogicGateCount;
    public int abstractionLevelCount;
    public string netlistPath;
    public string netlistName;
    public bool success;
} 

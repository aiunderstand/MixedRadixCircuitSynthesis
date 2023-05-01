using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;
using static BtnInput;

public class CircuitGenerator : MonoBehaviour
{
    //start: we should use a singleton pattern for this, refactor! :(
    public static GameObject LogicGatePrefab;
    public GameObject _logicGatePrefab;
    public static GameObject InputPrefab;
    public GameObject _inputPrefab;
    public static GameObject OutputPrefab;
    public GameObject _outputPrefab;
    public static GameObject ClockPrefab;
    public GameObject _clockPrefab;


    private void Awake()
    {
        LogicGatePrefab = _logicGatePrefab;
        InputPrefab = _inputPrefab;
        OutputPrefab = _outputPrefab;
        ClockPrefab = _clockPrefab;
    }
    //end

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
        List<string> idArray = new List<string>();
        OrderedDictionary nodesLOT = new OrderedDictionary();
        OrderedDictionary componentLOT = new OrderedDictionary();
        int inputComponents = 0;
        int outputComponents = 0;
        List<string> ioPositionArray = new List<string>();
        List<string> functionRadixTypeArray = new List<string>();
        List<string> ioRadixTypeArray = new List<string>();
        List<int> inputOutputSizeArray = new List<int>();
        List<string> connectionPairArray = new List<string>();
        string path = Application.persistentDataPath + "/User/Generated/" + filePath;
        string hspicePath = path + "/HSPICE/";
        string verilogPath = path + "/Verilog/";
        string hspiceAssetsPath = Application.streamingAssetsPath + "/HSPICE/";
        bool exists = System.IO.Directory.Exists(path);
        Stats stats = new Stats();
        stats.transistorCount = 0;
        stats.abstractionLevelCount = 0;
        stats.success = false;
        stats.netlistPath = "";
        stats.totalLogicGateCount = 0;
        stats.uniqueLogicGateCount = 0;
        stats.netlistName = "";

        List<string> outputNamesHack = new List<string>();
        Dictionary<string,string> radixLookup = new Dictionary<string, string>();
        //map input port which might be of different radii to a unique physical port. We do this because we want the transformation 
        //from ternary to binary be as painless for the user as possible. We hide the io port complexity:
        //a ternary input port is converted to 2 binary inputs port. Since that messes up the port id, we need to
        //construct yet another lookup table. We dont need this for functions, because there is no physical port mapping
        Dictionary<string, int> portLookup = new Dictionary<string, int>();

        //REFACTOR INFO:
        //make every input/output node unique even if the have same wire (due to fan-out)
        //use lookup tables with semnatic names and use these with comments instead of node names
        //have a intermediate language "1 or more circuit elements with circuit behavior that has 1 or more connections to 1 or more circuit elements)
        //with intermediate language we can make add other hardware mappings and circuit optimizers more easily
        //introduce DRC (verilog/netlist lint) to verify syntax + additional rules for user
        //introduce unit tests for developers
        //should we use the term subcircuit instead of saved gate?

        if (exists)
        {
            //remove 
            System.IO.Directory.Delete(path, true);
            Debug.Log("Overwritten folder!");
        }

        System.IO.Directory.CreateDirectory(path);
        System.IO.Directory.CreateDirectory(hspicePath);

        //get all connections and truth tables
        var connections = GameObject.FindGameObjectsWithTag("Wire");
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");

        OrderedDictionary inputLOT = new OrderedDictionary();

        //PASS 0: collect connectionData
        foreach (var conn in connections)
        {
            var parsedName = conn.name.Split(';');
            //0 =; input ; id; port; -->; output; id; port
            connectionPairArray.Add(parsedName[2] + " " + parsedName[3] + " " + parsedName[6] + " " + parsedName[7]);
        }


        //PASS 1: input names with semantic names
        int uniquePortId = 0; //see Verilog section to reason for this extra mapping

        Dictionary<int, float> inputOrder = new Dictionary<int, float>(); //needed for ordering the labels from low to high (using transform.position.y)
        foreach (var c in components)
        {
            int validButtons = 0;
            if (c.name.Contains("Input"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                string id = inputControler.GetInstanceID().ToString();

                for (int i = 0; i < inputControler.Buttons.Count; i++)
                {
                    //double check if button is connected to something
                    var bi = inputControler.Buttons[i].GetComponent<BtnInput>();

                    //if (bi.Connections.Count > 0)
                    //{

                        //check if connection is directly to output, since then it should not be added
                        //int outputCounter = 0;
                        //foreach (var conn in bi.Connections)
                        //{
                        //    var parts = conn.name.Split(';');

                        //    if (parts[5].Contains("Output"))
                        //    {
                        //        outputCounter++;
                        //    }
                        //}

                        ////only add input if there is at least one connection to a logicgate
                        //if (outputCounter != bi.Connections.Count)
                        //{
                            validButtons++;
                            inputOrder.Add(inputRadix.Count, bi.transform.position.y);

                            int portIndex = bi._portIndex;
                            string portLabel = inputControler.Buttons[i].GetComponentInChildren<TMP_InputField>().text;
                            string inputValidatedName = "i_" + portLabel + "_" + uid;
                            inputNames.Add(inputValidatedName); //refactor to only use inputLOT, build the tree and then convert the inputlot to input names
                            inputLOT.Add(portIndex + "_" + id, inputValidatedName);
                            nodesLOT.Add(portIndex + "_" + id, inputValidatedName); // refactor out inputLOT?
                            uid++;
                            inputRadix.Add(bi.DropdownLabel.text);
                            radixLookup.Add(portIndex + "_" + id, bi.DropdownLabel.text);
                            if (bi.DropdownLabel.text.Contains("Binary"))
                            {
                                portLookup.Add(portIndex + "_" + id, uniquePortId);
                                uniquePortId += 1;
                            }
                            else
                            {
                                portLookup.Add(portIndex + "_" + id, uniquePortId);
                                uniquePortId += 2;
                            }
                }

                if (validButtons > 0)
                {
                    inputComponents++;
                    Vector2 pos2d = c.GetComponentInParent<DragDrop>().transform.localPosition;
                    ioPositionArray.Add(pos2d.x.ToString("F", CultureInfo.InvariantCulture));
                    ioPositionArray.Add(pos2d.y.ToString("F", CultureInfo.InvariantCulture));
                    ioRadixTypeArray.Add(inputControler.DropdownLabel.text);
                    inputOutputSizeArray.Add(validButtons);
                    idArray.Add(id);
                }
            }
        }

        ////reorder the input labels to get correct interface order
        //List<string> tempinputNames = new List<string>();
        //List<string> tempInputRadix = new List<string>();

        ////sort inputOrder
        //var sorted = inputOrder.OrderBy(key => key.Value);

        ////assign tempLabels in correct order
        //foreach (var item in sorted)
        //{
        //    tempinputNames.Add(inputNames[item.Key]);
        //    tempInputRadix.Add(inputRadix[item.Key]);
        //}

        //inputNames = tempinputNames;
        //inputRadix = tempInputRadix;

        int uidInput = uid;
        uniquePortId = 0;
        //PASS 2: output names with semantic names
        OrderedDictionary outputLOT = new OrderedDictionary();
        //create output names OrderedDictionary/hashtable, this code section is solely for adding the output label to an logic gate terminal, refactor?
        Dictionary<int, float> outputOrder = new Dictionary<int, float>();
        foreach (var c in components)
        {
            int validButtons = 0;
            if (c.name.Contains("Output"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                string id = inputControler.GetInstanceID().ToString();

                for (int i = 0; i < inputControler.Buttons.Count; i++)
                {
                    //double check if button is connected to something
                    var bi = inputControler.Buttons[i].GetComponent<BtnInput>();

                    //if (bi.Connections.Count > 0)
                    //{
                        outputOrder.Add(outputRadix.Count, bi.transform.position.y);
                        validButtons++;
                        int portIndex = bi._portIndex;
                        string portLabel = inputControler.Buttons[i].GetComponentInChildren<TMP_InputField>().text;
                        string inputValidatedName = "o_" + portLabel;
                        outputLOT.Add(portIndex + "_" + id, inputValidatedName);
                        nodesLOT.Add(portIndex + "_" + id, inputValidatedName + "_" + uid); // refactor out outputLOT?
                        outputNamesHack.Add(inputValidatedName + "_" + uid); //this late night hacky version preserves order
                        outputRadix.Add(bi.DropdownLabel.text);
                        uid++;
                        radixLookup.Add(portIndex + "_" + id, bi.DropdownLabel.text);
                    if (bi.DropdownLabel.text.Contains("Binary"))
                    {
                        portLookup.Add(portIndex + "_" + id, uniquePortId);
                        uniquePortId += 1;
                    }
                    else
                    {
                        portLookup.Add(portIndex + "_" + id, uniquePortId);
                        uniquePortId += 2;
                    }
                    //}
                }

                if (validButtons > 0)
                {
                    outputComponents++;
                    Vector2 pos2d = c.GetComponentInParent<DragDrop>().transform.localPosition;
                    ioPositionArray.Add(pos2d.x.ToString("F", CultureInfo.InvariantCulture));
                    ioPositionArray.Add(pos2d.y.ToString("F", CultureInfo.InvariantCulture));
                    ioRadixTypeArray.Add(inputControler.DropdownLabel.text);
                    inputOutputSizeArray.Add(validButtons);
                    idArray.Add(id);
                }
            }
        }

        //uid hack to reset uid to amount after input processing , this is an ugly fix to a label matching issue that 
        //occurs in verilog while in netlist this is not an issue due to a different wire label approach.
        uid = uid - uidInput;

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
                var radix = controller.GetRadixHack();
                arityArray.Add(arity);

                //we should just create a c++ data structure and marshall this. Now it is important that we first call create netlist!

                //Netlist format
                int mode = (int)TruthtableFunctionHelper.HardwareMappingModes.variantA_woBody; //variantB_wBodyDividersTransistors;
                stats.transistorCount += TruthtableFunctionHelper.CreateNetlist(mode, hspicePath, tt, arity); //from unoptimized tt
                int[] optimizedTT = TruthtableFunctionHelper.GetOptimizedTT(arity);
                string optimizedTTindex = TruthtableFunctionHelper.ConvertTTtoHeptEncoding(optimizedTT);
                ttIndices.Add(optimizedTTindex);
                logicgateIndicesLOT.Add(controller.GetInstanceID().ToString(), "f_" + optimizedTTindex);
                
                //Verilog format
                TruthtableFunctionHelper.CreateVerilogLogicGates(verilogPath, "f_" + optimizedTTindex, optimizedTT, radix, arity);
                

                int[] tempInvArray = TruthtableFunctionHelper.GetAndConvertInvArrayFormat(arity);

                for (int i = 0; i < 9; i++) //always 9
                {
                    invArray.Add(tempInvArray[i]);
                }

                //get positions
                Vector2 pos = c.GetComponentInParent<DragDrop>().gameObject.transform.localPosition;
                positionArray.Add(pos.x.ToString("F", CultureInfo.InvariantCulture));
                positionArray.Add(pos.y.ToString("F", CultureInfo.InvariantCulture));

                //add radixtype
                var radixDropdown = controller.transform.GetComponentInChildren<TMP_Dropdown>();
                functionRadixTypeArray.Add(radixDropdown.options[radixDropdown.value].text);
               
                //add to id list if not existing (could do this faster with a hashtable/dictionary)
                var id = controller.GetInstanceID().ToString();
                bool found = false;

                foreach (var item in idArray)
                {
                    if (item == id)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    idArray.Add(id);

                    //also add ports to index
                    switch (arity)
                    {
                        case 1:
                            radixLookup.Add("0_" + id, radixDropdown.options[radixDropdown.value].text);
                            radixLookup.Add("1_" + id, radixDropdown.options[radixDropdown.value].text);
                            break;
                        case 2:
                            radixLookup.Add("0_" + id, radixDropdown.options[radixDropdown.value].text);
                            radixLookup.Add("1_" + id, radixDropdown.options[radixDropdown.value].text);
                            radixLookup.Add("2_" + id, radixDropdown.options[radixDropdown.value].text);
                            break;
                        case 3:
                            radixLookup.Add("0_" + id, radixDropdown.options[radixDropdown.value].text);
                            radixLookup.Add("1_" + id, radixDropdown.options[radixDropdown.value].text);
                            radixLookup.Add("2_" + id, radixDropdown.options[radixDropdown.value].text);
                            radixLookup.Add("3_" + id, radixDropdown.options[radixDropdown.value].text);
                            break;

                    }
                    
                }
            }
        }

        //PASS 3b: saved gate dont generate TT netlist but do add to LOT with semantic names 
        foreach (var c in components)
        {
            if (c.name.Contains("SavedGate"))
            {
                var controller = c.GetComponentInChildren<InputController>();

                //add to id list if not existing (could do this faster with a hashtable/dictionary)
                var id = controller.GetInstanceID().ToString();
                bool found = false;

                foreach (var item in idArray)
                {
                    if (item == id)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    idArray.Add(id);

                    for (int i = 0; i < controller.Buttons.Count; i++)
                        radixLookup.Add(i.ToString() + "_" + id, controller.Buttons[i].GetComponent<BtnInput>()._radix);
                }

                stats.transistorCount += controller.savedComponent.Stats.transistorCount;
                stats.totalLogicGateCount += controller.savedComponent.Stats.totalLogicGateCount;
                if (controller.savedComponent.Stats.abstractionLevelCount > stats.abstractionLevelCount)
                    stats.abstractionLevelCount = controller.savedComponent.Stats.abstractionLevelCount;

                savedCircuitNames.Add("c_" + controller.savedComponent.ComponentName);
                logicgateIndicesLOT.Add(controller.GetInstanceID().ToString(), "f_" + controller.savedComponent.ComponentName);

                //get positions
                Vector2 pos = c.GetComponentInParent<DragDrop>().gameObject.transform.localPosition;
                positionArray.Add(pos.x.ToString("F", CultureInfo.InvariantCulture));
                positionArray.Add(pos.y.ToString("F", CultureInfo.InvariantCulture));

                //move all .sp files to path, remove/overwrite existing f_* files
                var fpath = Path.GetDirectoryName(controller.savedComponent.ComponentNetlistPath);
                var files = System.IO.Directory.GetFiles(fpath, "*.sp", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Length; i++)
                {
                    var filenameWithExtension = new DirectoryInfo(System.IO.Path.GetFileName(files[i]));
                    var targetFilePath = hspicePath + filenameWithExtension;
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
                                            outputLOT[(string)(parts[7] + "_" + parts[6])] = stored;
                                            //idLOT[(string)(parts[7] + "_" + parts[6])] = stored;
                                        }
                                    }
                                }

                                if (!isFound) //add to hastables and tempConnArray
                                {
                                    //update the output LOT with a UID and add to final outputNames list
                                    var id = (string)outputLOT[(string)(parts[7] + "_" + parts[6])] + "_" + uid;
                                    outputLOT[(string)(parts[7] + "_" + parts[6])] = id; //update id with uid
                                                                                         //idLOT[(string)(parts[7] + "_" + parts[6])] = id;
                                    uid++;

                                    logicgateLOT.Add((parts[3] + "_" + parts[2]), id);
                                    nodesLOT.Add((parts[3] + "_" + parts[2]), id); // refactor out logicgateLOT?
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
                var controller = c.GetComponentInChildren<InputController>();

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
                                            outputLOT[(string)(parts[7] + "_" + parts[6])] = stored;
                                            //idLOT[(string)(parts[7] + "_" + parts[6])] = stored;
                                        }
                                    }
                                }

                                if (!isFound) //add to hastables and tempConnArray
                                {
                                    //update the output LOT with a UID and add to final outputNames list
                                    var id = (string)outputLOT[(string)(parts[7] + "_" + parts[6])] + "_" + uid;
                                    outputLOT[(string)(parts[7] + "_" + parts[6])] = id; //update id with uid
                                                                                         //idLOT[(string)(parts[7] + "_" + parts[6])] = id;
                                    uid++;

                                    logicgateLOT.Add((parts[3] + "_" + parts[2]), id);
                                    nodesLOT.Add((parts[3] + "_" + parts[2]), id); // refactor out logicgateLOT?
                                }
                            }
                        }
                    }
                }
            }
        }

        ////reorder the output labels to get correct interface order
        //List<string> tempOutputNames = new List<string>();
        //List<string> tempOutputRadix = new List<string>();

        ////sort inputOrder
        //var sorted1 = outputOrder.OrderBy(key => key.Value);

        ////assign tempLabels in correct order
        //foreach (var item in sorted1)
        //{
        //    tempOutputNames.Add(outputLOT[item.Key].ToString());
        //    tempOutputRadix.Add(outputRadix[item.Key]);
        //}

        //outputNames = tempOutputNames;
        //outputRadix = tempOutputRadix;

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
                                    nodesLOT.Add((parts[3] + "_" + parts[2]), id); // refactor out logicgateLOT?
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
                                    nodesLOT.Add((parts[3] + "_" + parts[2]), id); // refactor out logicgateLOT?
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
                var controller = c.GetComponentInChildren<InputController>();
                int indexIn = controller.savedComponent.Inputs.Count;
                int indexOut = controller.savedComponent.Outputs.Count;

                string[] tempConnArray = new string[indexIn + indexOut];

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
                                        tempConnArray[index] = (string)logicgateLOT[id.Key];
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
                                        tempConnArray[index] = (string)logicgateLOT[id.Key];
                                    }
                                }

                                if (!isFound) //add to hastables and tempConnArray
                                {
                                    //update the output LOT with a UID and add to final outputNames list
                                    var id = (string)logicgateIndicesLOT[(string)(parts[2])] + "_" + uid;
                                    uid++;

                                    logicgateLOT.Add((parts[3] + "_" + parts[2]), id);
                                    nodesLOT.Add((parts[3] + "_" + parts[2]), id); // refactor out logicgateLOT?
                                    tempConnArray[index] = id;
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
                                    nodesLOT.Add((parts[3] + "_" + parts[2]), id); // refactor out logicgateLOT?
                                    tempConnArray[index] = id;
                                }
                            }
                        }
                    }

                }

                //add temp to array
                for (int i = 0; i < tempConnArray.Length; i++)
                {
                    if (tempConnArray[i] == null)
                        tempConnArray[i] = "";

                    connectionArray.Add(tempConnArray[i]);
                }

                connectionIndexArray.Add(tempConnArray.Length);
            }
        }

        //FINALLY
        //Most of the complexity in this file is due to the semantic labels that could be duplicate eg if output#0 has name "sum" coming from gate A and output#1 coming from gate B but got its input from GATE A then it should have the same name since it is the same signal
        //the order is important (should match with pass 2)
        for (int i = 0; i < outputLOT.Count; i++)
        {
            outputNames.Add(outputLOT[i].ToString());
        }

        //doube check if everything is there before submitting to memory sensitive c++ land
        bool fail = false;
        //bool fail = true; 
        for (int i = 0; i < connectionArray.Count; i++)
        {
            if (connectionArray[i] == null)
            {
                connectionArray[i] = "";
                //fail = true;
                Debug.Log("Warning: Not all connections were connected");
            }
        }

        //extra unit-like checks
        if ((inputComponents + outputComponents) != ioRadixTypeArray.Count)
        {
            fail = true;
            Debug.Log("ioRadixGTypeArray: " + ioRadixTypeArray.Count + " i: " + inputComponents + " o: " + outputComponents);
        }

        if ((inputComponents + outputComponents) != inputOutputSizeArray.Count)
        {
            fail = true;
            Debug.Log("inputOutputSizeArray: " + inputOutputSizeArray.Count + " i: " + inputComponents + " o: " + outputComponents);
        }

        if (((inputComponents + outputComponents) * 2) != ioPositionArray.Count)
        {
            fail = true;
            Debug.Log("ioPositionArray: " + ioPositionArray.Count + " i: " + inputComponents + " o: " + outputComponents + " * 2 vector pos");
        }

        if (connectionPairArray.Count == 0)
        {
            fail = true;
            Debug.Log("connectionPairArray: " + connectionPairArray.Count);
        }

        if ((inputComponents + outputComponents + logicgateIndicesLOT.Count) != idArray.Count)
        {
            fail = true;
            Debug.Log("idArray: " + idArray.Count + " i: " + inputComponents + " o: " + outputComponents + " gates: " + logicgateIndicesLOT.Count);
        }

        if (!fail)
        {
            var inverterCount = TruthtableFunctionHelper.CreateCircuit(
                hspicePath,
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
                connectionIndexArray.ToArray(),
                functionRadixTypeArray.ToArray(),
                functionRadixTypeArray.Count,
                inputComponents,
                outputComponents,
                ioRadixTypeArray.ToArray(),
                ioRadixTypeArray.Count,
                inputOutputSizeArray.ToArray(),
                inputOutputSizeArray.Count,
                ioPositionArray.ToArray(),
                ioPositionArray.Count,
                connectionPairArray.ToArray(),
                connectionPairArray.Count,
                idArray.ToArray(),
                idArray.Count
                );

            stats.transistorCount += inverterCount;
            stats.totalLogicGateCount += ttIndices.Count; //from logic gates, the saved gates are already stored
            var files = System.IO.Directory.GetFiles(hspicePath, "f_*", SearchOption.TopDirectoryOnly);
            stats.uniqueLogicGateCount = files.Length; //count the total f_ files in the directory
            stats.abstractionLevelCount += 1;
            stats.success = true;

            //add/update stats to main circuit file, we should refactor this to search in file for specific headers and replace content in them. Currently it is alwasy at the top 5 lines
            string fPath = hspicePath + fileName + ".sp";
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            //update the statistics part of the .sp file
            using (StreamWriter writer = new StreamWriter(fPath, false))
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine("*** @tcount " + stats.transistorCount.ToString());
                writer.WriteLine("*** @gcount " + stats.totalLogicGateCount.ToString());
                writer.WriteLine("*** @ugcount " + stats.uniqueLogicGateCount.ToString());
                writer.WriteLine("*** @abslvl " + stats.abstractionLevelCount.ToString());

                for (int i = 5; i < lines.Count; i++)
                {
                    writer.WriteLine(lines[i]);
                }
            }

            //ADD common HSPICE files like the CNTFET model and empty simulation files. 
            var dir = new DirectoryInfo(hspiceAssetsPath);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (!file.Extension.Contains("meta"))
                {
                    string targetFilePath = Path.Combine(hspicePath, file.Name);
                    if (!File.Exists(targetFilePath))
                        file.CopyTo(targetFilePath);
                }
            }



            //CREATE VERILOG OUTPUT
            ////Note: this is different from netlist format where the endpoint names are wire names, resulting in duplicate wire names when fanout >1. 
            ////that is not the case in verilog where every connections must be uniquely labelled 
            ////this also means that in verilog we cannot use a node name as a wire name

            //update the idLOT with semantic names for the logic gates. 
            int lg_uid = 0;
            for (int i = inputComponents + outputComponents; i < (inputComponents + outputComponents +ttIndices.Count); i++)
            {
                ////I dont like C# dictionaries anymore. No error is given when assigning to a non existing ID and instead it is added to the dictionary whilst not using the .Add function. Dangerous
                ////find all keys in idLOT that contains idArray[i]. There can be multiple as the input and outputs of a logic gate are not named but have unique ports
                //List<string> keys = new List<string>();
                //foreach (DictionaryEntry id in nodesLOT)
                //{
                //    if (id.Key.ToString().Contains(idArray[i])) //match identifier of component
                //        keys.Add(id.Key.ToString());
                //}

                ////update the value since we cannot do that during enumeration
                //foreach (var key in keys)                
                //    nodesLOT[key] = "f_" + ttIndices[i - inputComponents - outputComponents] + "_" + lg_uid.ToString();

                ////we need to add "target" keys as well, since in netlist format source and target are the same name and thus only make source keys
                foreach (var conn in connectionPairArray)
                {
                    //Parse string 
                    var connParts = conn.Split(' ');

                    if (connParts[0].Equals(idArray[i])) //match identifier of component
                    {
                        if (nodesLOT.Contains(connParts[1] + "_" + connParts[0]))
                            nodesLOT[connParts[1] + "_" + connParts[0]] = "f_" + ttIndices[i - inputComponents - outputComponents] + "_" + lg_uid.ToString();
                        else
                            nodesLOT.Add(connParts[1] + "_" + connParts[0], "f_" + ttIndices[i - inputComponents - outputComponents] + "_" + lg_uid.ToString());
                    }

                    if (connParts[2].Equals(idArray[i])) //match identifier of component
                    {
                        if (nodesLOT.Contains(connParts[3] + "_" + connParts[2]))
                            nodesLOT[connParts[3] + "_" + connParts[2]] = "f_" + ttIndices[i - inputComponents - outputComponents] + "_" + lg_uid.ToString();
                        else
                            nodesLOT.Add(connParts[3] + "_" + connParts[2], "f_" + ttIndices[i - inputComponents - outputComponents] + "_" + lg_uid.ToString());
                    }
                }

                //increase uid
                lg_uid++;
            }

            //update the idLOT with semantic names for the saved gates. 
            int sg_uid = 0;
            int savedgateoffset = inputComponents + outputComponents + ttIndices.Count;
            for (int i = inputComponents + outputComponents + ttIndices.Count; i < idArray.Count; i++)
            {
                foreach (var conn in connectionPairArray)
                {
                    //Parse string 
                    var connParts = conn.Split(' ');

                    //source
                    if (connParts[0].Equals(idArray[i])) //match identifier of component
                    {
                        if (nodesLOT.Contains(connParts[1] + "_" + connParts[0]))
                            nodesLOT[connParts[1] + "_" + connParts[0]] = savedCircuitNames[i - savedgateoffset] + "_" + sg_uid.ToString();
                        else
                            nodesLOT.Add(connParts[1] + "_" + connParts[0], savedCircuitNames[i - savedgateoffset] + "_" + sg_uid.ToString());
                    }

                    //target
                    if (connParts[2].Equals(idArray[i])) //match identifier of component
                    {
                        if (nodesLOT.Contains(connParts[3] + "_" + connParts[2]))
                            nodesLOT[connParts[3] + "_" + connParts[2]] = savedCircuitNames[i - savedgateoffset] + "_" + sg_uid.ToString();
                        else
                            nodesLOT.Add(connParts[3] + "_" + connParts[2], savedCircuitNames[i - savedgateoffset] + "_" + sg_uid.ToString());
                    }
                }

                //increase uid
                sg_uid++;
            }

            //parse connections. 
            //create LOT so we can do ComponentLOT[ID] = component
            //each component has:
            //   type: "source" or "target" or "logicgate" or "savedgate"
            //   in: array of 0 or more nodes with each node having: port#, label_uid, net# 
            //   out: array of 0 or more nodes with each node having: port#, label_uid, net# 

            //create array with empty components
           
            for (int i = 0; i < idArray.Count; i++)
            {
                string type = "SavedGate";
                string name = (string) nodesLOT["0_" + idArray[i]]; //we use port 0 since the label/name of the component is the same for all logic gates or savedgates
                string radix = "unknown"; //this could be a mix of radii if saved gate!

                if (i < inputComponents)
                {
                    type = "Input";
                    name = "InputComponent" + i.ToString();
                    radix = (string)radixLookup["0_" + idArray[i]];
                }

                if (i >= inputComponents && i < (inputComponents + outputComponents))
                {
                    type = "Output";
                    name = "OutputComponent" + (i-inputComponents).ToString();
                    radix = (string)radixLookup["0_" + idArray[i]];
                }

                if (i >= inputComponents + outputComponents && i < (inputComponents + outputComponents + ttIndices.Count))
                {
                    type = "LogicGate";
                    radix = (string)radixLookup["0_" + idArray[i]];
                }
                

                //create empty component
                componentLOT.Add(idArray[i], new MRCS_Component(idArray[i], type, name, radix));
            }


            //build components from connections
            //we need to build the nets a little bit better by integating the order of inputs such that
            //connection order should be irrelevant. For readability it should always start with 0,1,2,3 for input and ends with n-1, n for output
            //in addition if a node has fanout>1 it should be consecutive. We use a linked list structure to insert fanout in between

            //PASS 1: first find how many connections are direct input-output connections
            //int inputoutputEqual = 0;
            //for (int i = 0; i < connectionPairArray.Count; i++)
            //{
            //    var connParts = connectionPairArray[i].Split(' ');
            //    MRCS_Component tempSrc = (MRCS_Component)componentLOT[connParts[0]];
            //    MRCS_Component tempTgt = (MRCS_Component)componentLOT[connParts[2]];
            //    if (tempSrc.Type.Equals("Input") && tempTgt.Type.Equals("Output"))
            //        inputoutputEqual++;
            //}

            //PASS 2: second, reorder connections such that nodes with fanout >1 are below eachother
            //Parse again connectionpairarray and build a temparray with correct order (MRCS uses MSB first order for both input and output) from the 
            //For each fanout > 1 of input component node or fanout > 1 of logic gate/saved gate output node add them
            //

         
            List<int> head = new List<int>();
            List<int> tail = new List<int>();
            
            //check entire connection array for name, this includes fanout
            foreach (var iname in inputNames)
            {
                for (int i = 0; i < connectionPairArray.Count; i++)
                {
                    var connParts = connectionPairArray[i].Split(' ');
                    var label_uid = (string)nodesLOT[connParts[1] + "_" + connParts[0]];

                    if (iname.Equals(label_uid))
                        head.Add(i);
                }
            }

            //check rest for outputnames, this include fanout
            foreach (var oname in outputNamesHack)
            {
                for (int i = 0; i < connectionPairArray.Count; i++)
                {
                    if (head.IndexOf(i) == -1) //only process indices that are not added to the list
                    {
                        var connParts = connectionPairArray[i].Split(' ');
                        var label_uid = (string)nodesLOT[connParts[3] + "_" + connParts[2]];

                        if (oname.Equals(label_uid))
                            tail.Add(i);
                    }
                }
            }

            //check rest for logic gates, this include fanout
            for (int i = 0; i < ttIndices.Count; i++)
            {
                for (int j = 0; j < connectionPairArray.Count; j++)
                {
                    if ((head.IndexOf(j) == -1) && (tail.IndexOf(j) == -1)) //only process indices that are not added to the list
                    {
                        var connParts = connectionPairArray[j].Split(' ');
                        var label_uid = (string)nodesLOT[connParts[1] + "_" + connParts[0]];
                        var label_tt = "f_" + ttIndices[i] + "_" + i;
                        if (label_tt.Equals(label_uid))
                            head.Add(j);
                    }
                }
            }

            //check rest for saved gates, this include fanout
            for (int i = inputComponents + outputComponents + ttIndices.Count; i < idArray.Count; i++)
            {
                for (int j = 0; j < connectionPairArray.Count; j++)
                {
                    if ((head.IndexOf(j) == -1) && (tail.IndexOf(j) == -1)) //only process indices that are not added to the list
                    {
                        var connParts = connectionPairArray[j].Split(' ');
                        if (idArray[i].Equals(connParts[0])) //if the IDs match
                            head.Add(j);
                    }
                }
            }

            //finally, convert from tempArray to ordered list of connections.
            
            List<string> orderedConnectionPairArray = new List<string>();

            //add head
            foreach (var i in head)
            {
                orderedConnectionPairArray.Add(connectionPairArray[i]);
            }

            //add tail
            foreach (var i in tail)
            {
                orderedConnectionPairArray.Add(connectionPairArray[i]);
            }

            //now we can fill the empty MRCS components with connection data
            //for future refactor: when adding nodes add them port sorted (either directly when adding or as post processing) 
            //or using named input/outputs, see SavedGates

            for (int i = 0; i < orderedConnectionPairArray.Count; i++)
            {
                string conn = orderedConnectionPairArray[i];

                //Parse string 
                var connParts = conn.Split(' ');

                //Source
                MRCS_Component temp = (MRCS_Component)componentLOT[connParts[0]];

                //find radix
                string radixSource = (string)radixLookup[connParts[1] + "_" + connParts[0]]; //Port_id .This should be refactored. The new data model should have a node struct.

                string netType = radixSource.Contains("Binary") ? "b" : "t";

                if (temp.Type.Equals("Input"))
                    temp.InputNodes.Add(new Node(connParts[0], connParts[1], (string) "in_" + connParts[1], netType + "net_" + i.ToString(), radixSource)); //the node of an input component is a source
                else
                    temp.OutputNodes.Add(new Node(connParts[0], connParts[1], (string) "out_" + connParts[1], netType + "net_" + i.ToString(), radixSource)); //the node of an logic gate component is a that is input to other components is actually an output from a component view. 

                componentLOT[connParts[0]] = temp;

                //Target 
                temp = (MRCS_Component)componentLOT[connParts[2]];
             
                //find radix
                string radixTarget = (string)radixLookup[connParts[3] + "_" + connParts[2]];

                string netName = "";
                //no net conversion
                if (radixSource == radixTarget)
                    netName = netType + "net_" + i.ToString();

                //binary to ternary net conversion
                if (radixSource.Contains("Binary") && radixTarget.Contains("Ternary"))
                    netName = "{" + "bnet_" + i.ToString()+ ",!" + "bnet_" + i.ToString() + "}";

                if (radixSource.Contains("Ternary") && radixTarget.Contains("Binary"))
                    netName = "tnet_" + i.ToString() + "[1]";



                if (temp.Type.Equals("Output"))
                    temp.OutputNodes.Add(new Node(connParts[2], connParts[3], (string)"out_" + connParts[3], netName, radixTarget));
                else
                    temp.InputNodes.Add(new Node(connParts[2], connParts[3], (string)"in_" + connParts[3], netName, radixTarget)); //the node of an logic gate component is a output to an other component is acutally an input from a component view.

                componentLOT[connParts[2]] = temp;
            }

            //Hacky pass to correct the label_uid of output nodes as they start with an offset
            for (int i = 0; i < componentLOT.Count; i++)
            {
                MRCS_Component c = (MRCS_Component) componentLOT[i];
                if (c.Type.Equals("LogicGate") || c.Type.Equals("SavedGate"))
                {
                    for (int j = 0; j < c.OutputNodes.Count; j++)
                    {
                        var parts = c.OutputNodes[j].Label_uid.Split('_');
                        var portWithOffset = parts[1];
                        int portAsInt = int.Parse(portWithOffset);

                        //subtract offset
                        portAsInt = portAsInt - c.InputNodes.Count;
                        c.OutputNodes[j].Label_uid = "out_" + portAsInt.ToString();
                    }
                }
            }

            
            // START CONSTRUCTING VERILOG CIRCUIT FILE
            List<string> verilines = new List<string>();
            verilines.Add("module " + fileName + " (");

            //process inputs
            int inputPorts = 0;
            foreach (var or in inputRadix)
            {
                if (or.Contains("Binary"))
                    inputPorts += 1;
                if (or.Contains("Ternary"))
                    inputPorts += 2;
            }
            verilines.Add("     input [" + (inputPorts - 1) + ":0] io_in,");

            //int index = 0;
            //foreach (DictionaryEntry id in componentLOT)
            //{
            //    var c = (MRCS_Component) id.Value;

            //    if (c.Type.Equals("Input"))
            //    {
            //        Dictionary<string, string> fanoutLOT = new Dictionary<string, string>();

            //foreach (var n in c.InputNodes)
            //{
            //    //prevent multiple definitions if a single input has fanout >1 
            //    if (!fanoutLOT.ContainsKey(n.Label_uid))
            //    { 
            //        fanoutLOT.Add(n.Label_uid, n.Net);
            //        //verilines.Add("     input [" + index.ToString() + ":" + index.ToString() + "] " + n.Label_uid + ",");
            //        verilines.Add("     input [0:0] in_" + n.Port + ",");
            //        index++;
            //    }                       
            //}
            //    }   
            //}

            //process outputs
            int outputPorts = 0;
            foreach (var or in outputRadix)
            {
                if (or.Contains("Binary"))
                    outputPorts += 1;
                if (or.Contains("Ternary"))
                    outputPorts += 2;
            }
            verilines.Add("     output [" + (outputPorts-1) + ":0] io_out");
            
            //index = 0;
            //foreach (DictionaryEntry id in componentLOT)
            //{
            //    var c = (MRCS_Component)id.Value;

            //    if (c.Type.Equals("Output"))
            //    {
            //        foreach (var n in c.OutputNodes)
            //        {
            //            //verilines.Add("     output [" + index.ToString() + ":" + index.ToString() + "] " + n.Label_uid + ",");
            //            verilines.Add("     output [0:0] out_" + n.Port + ",");
            //            index++;
            //        }
            //    }
            //}

            //we need to remove the last comma
            //var templine = verilines[verilines.Count - 1];
            //templine = templine.Substring(0, templine.Length - 1); //remove the last symbol which is a comma
            //verilines[verilines.Count - 1] = templine;
            var templine = "";
            verilines.Add(");");
            verilines.Add("");

            //process wires, first inputs with assignments
            int curNet = -1;
            foreach (DictionaryEntry id in componentLOT)
            {
                var c = (MRCS_Component)id.Value;

                if (c.Type.Equals("Input"))
                {
                    Dictionary<string, Node> fanoutLOT = new Dictionary<string, Node>();

                    foreach (var n in c.InputNodes)
                    {
                        //keep track of used input so that we can assign wires when fanout >1 
                        if (fanoutLOT.ContainsKey(n.Label_uid))
                        {
                            if (n.Radix == "Binary")
                                verilines.Add("wire " + n.Net + " = " + fanoutLOT[n.Label_uid].Net + ";");
                            else
                                verilines.Add("wire [1:0] " + n.Net + " = " + fanoutLOT[n.Label_uid].Net + ";");
                        }
                        else
                        {
                            fanoutLOT.Add(n.Label_uid, n);
                            var temp_uid = n.Label_uid.Split('_');

                            if (n.Radix == "Binary")
                                verilines.Add("wire " + n.Net + " = io_in[" + (int) portLookup[(string)(n.Port + "_" + n.ComponentId)] + "];");
                            else
                                verilines.Add("wire [1:0] " + n.Net + " = io_in[" +((int)portLookup[n.Port + "_" + n.ComponentId] +1) + ":" + (int)portLookup[n.Port + "_" + n.ComponentId] + "];");
                        }

                        curNet = int.Parse(n.Net.Substring(5));
                    }
                }
            }

            //process wires, logic gates
            verilines.Add("");

            Dictionary<int, string> tempLines = new Dictionary<int, string>();

            foreach (DictionaryEntry id in componentLOT)
            {
                var c = (MRCS_Component)id.Value;

                if (c.Type.Equals("LogicGate") || c.Type.Equals("SavedGate"))
                {
                    Dictionary<string, Node> fanoutLOT = new Dictionary<string, Node>();
                    foreach (var n in c.OutputNodes)
                    {
                        //keep track of used output so that we can assign wires when fanout >1 
                        if (fanoutLOT.ContainsKey(n.Label_uid))
                        {
                            if (n.Radix == "Binary")
                                tempLines.Add(int.Parse(n.Net.Substring(5)), "wire " + n.Net + " = " + fanoutLOT[n.Label_uid].Net + ";");
                            else
                                tempLines.Add(int.Parse(n.Net.Substring(5)), "wire [1:0] " + n.Net + " = " + fanoutLOT[n.Label_uid].Net + ";");
                        }
                        else
                        {
                            fanoutLOT.Add(n.Label_uid, n);

                            if (n.Radix == "Binary")
                                tempLines.Add(int.Parse(n.Net.Substring(5)), "wire " + n.Net + ";");
                            else
                                tempLines.Add(int.Parse(n.Net.Substring(5)), "wire [1:0] " + n.Net + ";");
                        }                        
                    }
                }
            }

            for (int i = curNet+1 ; i < (curNet+1+tempLines.Count); i++)
            {
                verilines.Add(tempLines[i]);
            }
            //process output assignments
            verilines.Add("");
            foreach (DictionaryEntry id in componentLOT)
            {
                var c = (MRCS_Component)id.Value;

                if (c.Type.Equals("Output"))
                {
                    foreach (var n in c.OutputNodes)
                    {
                        var temp_uid = n.Label_uid.Split('_');

                        if (n.Radix == "Binary")
                            verilines.Add("assign io_out[" + (int)portLookup[(string)(n.Port + "_" + n.ComponentId)] + "] = " + n.Net + ";");
                        else
                            verilines.Add("assign io_out[" + ((int)portLookup[(string)(n.Port + "_" + n.ComponentId)] + 1) + ":" + (int)portLookup[(string)(n.Port + "_" + n.ComponentId)] + "] = " + n.Net + ";");
                    }

                }
            }

            //process logic gates
            foreach (DictionaryEntry id in componentLOT)
            {
                var c = (MRCS_Component)id.Value;

                Dictionary<string, string> fanoutLOT = new Dictionary<string, string>();

                if (c.Type.Equals("LogicGate"))
                {
                    verilines.Add("");
                    var labelParts = c.Name.Split('_');

                    if (c.Radix == "Binary")
                        verilines.Add(labelParts[0] + "_" + labelParts[1] + " LogicGate_" + labelParts[2] + " (");
                    else
                        verilines.Add(labelParts[0] + "_" + labelParts[1] + "_bet"  + " LogicGate_" + labelParts[2] + " (");

                    //This is a annoying bug that is extra problematic in verilog, the arity affets the order of ports (originates from an old discussion between Halvor and me at the very start of MRCS)
                    //for arity 1 and 2 port 0 is 0 and port 1 is 1, but for arity 3 port 0 is 1, port 1 is 0 and port 2 is 2
                    //with port 0 being the lowest significant bit. With saved components the order does not have this weird order, but then port 0 is the MSB :(

                    //We can actually fix this more elegantly by using named input/outputs in a refactor, see SavedGates
                    int[] sortHelp = new int[c.InputNodes.Count];
                    for (int i = 0; i < c.InputNodes.Count; i++)
                    {
                        sortHelp[int.Parse(c.InputNodes[i].Port)] = i;
                    }

                    //note the gates are mapped slightly differently internally due to a legacy bug.
                    //Future implementation of both the spice netlist and verilog and both binary and ternary should have a straightforward implementation of port 0 being input A for both radii
                    if (c.Radix == "Binary")
                    {
                        switch (c.InputNodes.Count)
                        {
                            case 1:
                                verilines.Add(".in_0(" + c.InputNodes[sortHelp[0]].Net + "),"); //Input A
                                break;
                            case 2:
                                verilines.Add(".in_1(" + c.InputNodes[sortHelp[1]].Net + "),"); //Input B
                                verilines.Add(".in_0(" + c.InputNodes[sortHelp[0]].Net + "),"); //Input A
                                break;
                            case 3:
                                verilines.Add(".in_2(" + c.InputNodes[sortHelp[2]].Net + "),"); //Input C
                                verilines.Add(".in_1(" + c.InputNodes[sortHelp[0]].Net + "),"); //Input B
                                verilines.Add(".in_0(" + c.InputNodes[sortHelp[1]].Net + "),"); //Input A
                                break;
                        }
                    }
                    else //ternary
                    {
                        switch (c.InputNodes.Count)
                        {
                            case 1:
                                verilines.Add(".in_0(" + c.InputNodes[sortHelp[0]].Net + "),"); //Input A
                                break;
                            case 2:
                                verilines.Add(".in_1(" + c.InputNodes[sortHelp[0]].Net + "),"); //Input B
                                verilines.Add(".in_0(" + c.InputNodes[sortHelp[1]].Net + "),"); //Input A
                                break;
                            case 3:
                                verilines.Add(".in_2(" + c.InputNodes[sortHelp[2]].Net + "),"); //Input C
                                verilines.Add(".in_1(" + c.InputNodes[sortHelp[0]].Net + "),"); //Input B
                                verilines.Add(".in_0(" + c.InputNodes[sortHelp[1]].Net + "),"); //Input A
                                break;
                        }
                    }

                    //we do not need to order the outputs on the port since there is only one port
                    for (int i = 0; i < c.OutputNodes.Count; i++)
                    {
                       //only add outputs once if there fanout is larger than 1 (fanout assignment is fixed in wire section)
                        if (!fanoutLOT.ContainsKey(c.OutputNodes[i].Label_uid))
                        {
                            fanoutLOT.Add(c.OutputNodes[i].Label_uid, c.OutputNodes[i].Net);
                            verilines.Add(".out_0" + "(" + c.OutputNodes[i].Net + "),");
                        }
                    }

                    //we need to remove the last comma
                    templine = verilines[verilines.Count - 1];
                    templine = templine.Substring(0, templine.Length - 1); //remove the last symbol which is a comma
                    verilines[verilines.Count - 1] = templine;
                    verilines.Add(");"); //add semicolon
                }
            }


            //process saved gates, very similar to logic gates, only name change but ordered after logic gates.
            foreach (DictionaryEntry id in componentLOT)
            {
                var c = (MRCS_Component)id.Value;

                Dictionary<string, string> fanoutLOT = new Dictionary<string, string>();

                if (c.Type.Equals("SavedGate"))
                {
                    verilines.Add("");
                    var labelParts = c.Name.Split('_');

                    verilines.Add(labelParts[0] + "_" + labelParts[1] + " SavedGate_" + labelParts[2] + " (");

                    //instead of ordering the logic gates we use the named ports for both input and output)

                    //gate 
                    string orderedInputBitSlices = "";

                    int[] sortHelp = new int[c.InputNodes.Count];
                    for (int i = 0; i < c.InputNodes.Count; i++)
                    {
                        sortHelp[int.Parse(c.InputNodes[i].Port)] = i;
                    }


                    for (int i = c.InputNodes.Count -1; i >= 0 ; i--) //LSB first (highest port)
                    {
                        orderedInputBitSlices += c.InputNodes[sortHelp[i]].Net;
                        orderedInputBitSlices += ",";
                    }

                    //remove last ,
                    orderedInputBitSlices = orderedInputBitSlices.Substring(0, orderedInputBitSlices.Length - 1);

                    verilines.Add(".io_in({"+ orderedInputBitSlices + "}),");

                    string orderedOutputBitSlices = "";

                    sortHelp = new int[c.OutputNodes.Count];
                    for (int i = 0; i < c.OutputNodes.Count; i++)
                    {
                        //we need to subtract input ports
                        sortHelp[int.Parse(c.OutputNodes[i].Port) - c.InputNodes.Count] = i;
                    }

                    for (int i = c.OutputNodes.Count -1; i >= 0; i--) //LSB first
                    {
                        //only add outputs once if there fanout is larger than 1 (fanout assignment is fixed in wire section)
                        if (!fanoutLOT.ContainsKey(c.OutputNodes[sortHelp[i]].Label_uid))
                        {
                            fanoutLOT.Add(c.OutputNodes[sortHelp[i]].Label_uid, c.OutputNodes[sortHelp[i]].Net);

                            orderedOutputBitSlices += c.OutputNodes[sortHelp[i]].Net;
                            orderedOutputBitSlices += ",";
                        }
                    }

                    //remove last ,
                    orderedOutputBitSlices = orderedOutputBitSlices.Substring(0, orderedOutputBitSlices.Length - 1);

                    verilines.Add(".io_out({" + orderedOutputBitSlices + "})");
               
                    verilines.Add(");"); //add semicolon
                }
            }


            verilines.Add("");
            verilines.Add("endmodule");
            verilines.Add(""); //nice spacing between modules


            string dirPath = verilogPath + "Circuit/";
            if (!System.IO.Directory.Exists(dirPath))
                System.IO.Directory.CreateDirectory(dirPath);

            string filePath2 = dirPath + fileName + ".v";
            TruthtableFunctionHelper.CreateFile(filePath2, verilines.ToArray());

            //copy circuit and all its logic gates to circuits and logic gates folder
            foreach (var sg in savedCircuitNames)
            {
                var parts = sg.Split('_');
                var savedGatePathCircuit = Application.persistentDataPath + "/User/Generated/" + parts[1] + "/Verilog/Circuit/";
                var savedGatePathLG = Application.persistentDataPath + "/User/Generated/" + parts[1] + "/Verilog/LogicGates/";
                
                //move all .v files to path, remove/overwrite existing *.v files
                var sgFiles = System.IO.Directory.GetFiles(savedGatePathCircuit, "*.v", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < sgFiles.Length; i++)
                {
                    var filenameWithExtension = new DirectoryInfo(System.IO.Path.GetFileName(sgFiles[i]));
                    var targetFilePath = verilogPath + "Circuit/" + filenameWithExtension;
                    //first, delete target file if exists, as File.Move() does not support overwrite
                    if (File.Exists(targetFilePath))
                    {
                        File.Delete(targetFilePath);
                    }

                    System.IO.File.Copy(sgFiles[i], targetFilePath);
                }

                //check if folder for verilog/logicgates has been created
                string dirLgPath = verilogPath + "LogicGates/";
                if (!System.IO.Directory.Exists(dirLgPath))
                    System.IO.Directory.CreateDirectory(dirLgPath);

                sgFiles = System.IO.Directory.GetFiles(savedGatePathLG, "*.v", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < sgFiles.Length; i++)
                {
                    var filenameWithExtension = new DirectoryInfo(System.IO.Path.GetFileName(sgFiles[i]));
                    var targetFilePath = verilogPath + "LogicGates/" + filenameWithExtension;
                                        
                    //first, delete target file if exists, as File.Move() does not support overwrite
                    if (File.Exists(targetFilePath))
                    {
                        File.Delete(targetFilePath);
                    }

                    System.IO.File.Copy(sgFiles[i], targetFilePath);
                }
            }



            //create a single file output as well
            //read all circuit files starting with the current one in one List<string>
            var savedGatePathCircuitCurrent = verilogPath + "Circuit/";
            var sgFilesCur = System.IO.Directory.GetFiles(savedGatePathCircuitCurrent, "*.v", SearchOption.TopDirectoryOnly);
            List<string> singleVerilogFile = new List<string>();

            //first pass add main .v file first
            for (int i = 0; i < sgFilesCur.Length; i++)
            {
                if (sgFilesCur[i].Equals(filePath2))
                {
                    string[] linesTemp = System.IO.File.ReadAllLines(sgFilesCur[i]);
                    singleVerilogFile.AddRange(linesTemp);
                }
            }

            //second pass add other .v  files and skip main one
            for (int i = 0; i < sgFilesCur.Length; i++)
            {
                if (!sgFilesCur[i].Equals(filePath2))
                {
                    string[] linesTemp = System.IO.File.ReadAllLines(sgFilesCur[i]);
                    singleVerilogFile.AddRange(linesTemp);
                }
            }

            //read all logic gate files
            var savedGatePathLGcurrent = verilogPath + "LogicGates/";
            sgFilesCur = System.IO.Directory.GetFiles(savedGatePathLGcurrent, "*.v", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < sgFilesCur.Length; i++)
            {
                string[] linesTemp = System.IO.File.ReadAllLines(sgFilesCur[i]);
                singleVerilogFile.AddRange(linesTemp);
            }

            //write all lines to Verilog/c_F_singlefile.v
            string verilogSingleFilePath = (verilogPath + "" + fileName + "_singlefile.v");
            using(StreamWriter writer = new StreamWriter(verilogSingleFilePath))
            {
                foreach (var line in singleVerilogFile)
                    writer.WriteLine(line);
            }
        }
        else
        {
            stats.success = false;
        }

        return stats;

    }

    public Stats SaveComponent(string name)
    {
        Stats stats = GenerateCircuit(name + "/", "c_" + name);
        return stats;
    }
}

public class MRCS_Component
{
    public string Type;
    public string Id;
    public string Name;
    public string Radix;
    public List<Node> OutputNodes = new List<Node>();
    public List<Node> InputNodes = new List<Node>();

    public MRCS_Component(string id, string type, string name, string radix)
    {
        Id = id;
        Type = type;
        Name = name;
        Radix = radix;
    }
}

public class Node
{
    public string ComponentId;
    public string Port;
    public string Label_uid;
    public string Net;
    public string Radix;

    public Node(string componentId, string port, string label_uid, string net, string radix)
    {
        ComponentId = componentId;
        Port = port;
        Label_uid = label_uid;
        Net = net;
        Radix = radix;
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

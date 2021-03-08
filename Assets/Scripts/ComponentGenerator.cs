using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BtnInput;
using UnityEngine.UI;
using static SaveCircuit;
using System;
using System.IO;
using static ComponentGenerator;
using UnityEngine.UI.Extensions;

public class ComponentGenerator : MonoBehaviour
{
    public enum NetlistComponentType
    {
        savedcomponent,
        function,
        input,
        output
    }

    public GameObject terminalInputPrefab;
    public GameObject terminalOutputPrefab; //we can refactor and merge this with input to "terminal"
    public TextMeshProUGUI title;
    public RectTransform body;
    public GameObject infoBtn;
    public int Size; //actually height 
    Color _colorTernary = new Color(255, 0, 211); //we should define it in 1 place instead of 2, see btninput
    Color _colorBinary = new Color(0, 214, 255);//we should define it in 1 place instead of 2, see btninput
    public List<GameObject> Generate(SavedComponent s, Views level)
    {
        List<GameObject> ios = new List<GameObject>();

        title.text = name;

        int count = s.Inputs.Count > s.Outputs.Count ? s.Inputs.Count : s.Outputs.Count;
        Size = 30 + (count - 1) * 20;

        body.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Size);
        title.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Size + 10);

        if (level == Views.ComponentView)
        {
            infoBtn.transform.localPosition = new Vector3(0, Size / 2);

            if (s.ComponentNetlistPath != null)
                GenerateNextAbstractionLevel(s);
        }


        for (int i = 0; i < s.Inputs.Count; i++)
        {
            var input = GameObject.Instantiate(terminalInputPrefab);
            input.name = i+ "_saved";
            input.transform.SetParent(this.transform);
            input.transform.localScale = new Vector3(1, 1, 1);
            input.transform.localPosition = new Vector2(-65, (-10 * (s.Inputs.Count - 1)) + i * 20);
            input.GetComponent<BtnInput>()._portIndex = i;
            input.GetComponent<BtnInput>().isOutput = false;
            input.GetComponent<BtnInput>()._radix = s.Inputs[i].ToString();
            input.GetComponent<BtnInput>().Init(); //needed?
            ios.Add(input);

            if (s.Inputs[i] == RadixOptions.Binary)
            {
                input.transform.GetChild(0).GetComponent<Image>().color = _colorBinary;

                if (level == Views.MenuView)
                    input.GetComponent<BtnInput>().label.text = "B";
                else
                    input.GetComponent<BtnInput>().label.text = s.InputLabels[i];
            }
            else
            {
                input.transform.GetChild(0).GetComponent<Image>().color = _colorTernary;

                if (level == Views.MenuView)
                    input.GetComponent<BtnInput>().label.text = "T";
                else
                    input.GetComponent<BtnInput>().label.text = s.InputLabels[i];
            }
        }

        for (int i = 0; i < s.Outputs.Count; i++)
        {
            var output = GameObject.Instantiate(terminalOutputPrefab);
            output.name = i + "_saved";
            output.transform.SetParent(this.transform);
            output.transform.localScale = new Vector3(1, 1, 1);
            output.transform.localPosition = new Vector2(65, (-10 * (s.Outputs.Count - 1)) + i * 20);
            output.GetComponent<BtnInput>()._portIndex = s.Inputs.Count+ i;
            output.GetComponent<BtnInput>().isOutput = true;
            output.GetComponent<BtnInput>()._radix = s.Outputs[i].ToString();
            output.GetComponent<BtnInput>().Init(); //needed?
            ios.Add(output);

            if (s.Outputs[i] == RadixOptions.Binary)
            {
                output.transform.GetChild(1).GetComponent<Image>().color = _colorBinary;

                if (level == Views.MenuView)
                {
                    output.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "B";
                    output.GetComponent<BtnInput>().label.text = "B";
                }
                else
                {
                    output.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = s.OutputLabels[i];
                    output.GetComponent<BtnInput>().label.text = s.OutputLabels[i];
                }
            }
            else
            {
                output.transform.GetChild(1).GetComponent<Image>().color = _colorTernary;

                if (level == Views.MenuView)
                {
                    output.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "T";
                    output.GetComponent<BtnInput>().label.text = "T";
                }
                else
                {
                    output.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = s.OutputLabels[i];
                    output.GetComponent<BtnInput>().label.text = s.OutputLabels[i];
                }
            }
        }

        return ios;
    }


    private void GenerateNextAbstractionLevel(SavedComponent s)
    {
        GameObject root = new GameObject();
        root.AddComponent<RectTransform>();
        root.name = "Level: " + (s.Stats.abstractionLevelCount - 1).ToString();
        root.transform.SetParent(this.gameObject.transform.parent.transform);
        root.transform.localPosition = new Vector3(0, 0);
        root.transform.localScale = new Vector3(1, 1);
        root.SetActive(false);
        this.gameObject.transform.parent.GetComponent<DragDrop>().LowerAbstractionVersion = root;

        //read netlist
        var result = FindComponentsInNetlist(s.ComponentNetlistPath);
        Dictionary<string, InputController> componentList = new Dictionary<string, InputController>();

        //first pass instantiate components
        //very expensive call, we need to refactor using a singleton
        SaveCircuit saveCircuit = GameObject.FindObjectOfType<SaveCircuit>();
        foreach (var n in result.components)
        {
            //generate on screen
            if (n.type.Equals(NetlistComponentType.function))
            {
                var logicgate = GameObject.Instantiate(CircuitGenerator.LogicGatePrefab);
                logicgate.GetComponent<DragDrop>().MenuVersion.SetActive(false);
                logicgate.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false); //disable autodropdown due to init issues
                logicgate.GetComponent<DragDrop>().FullVersion.SetActive(true);
                logicgate.GetComponent<DragDrop>().SetVersion(true);
                logicgate.transform.SetParent(root.transform);
                var iclg = logicgate.GetComponentInChildren<InputControllerLogicGate>();
                var expandController = iclg.GetComponentInChildren<DragExpandTableComponent>();

                var radix = 3;
                if (n.radixType.Contains("Binary"))
                    radix = 2;

                var radixDropdown = iclg.transform.GetComponentInChildren<TMP_Dropdown>();

                expandController.SetPanelSize(radix, n.arity);

                for (int i = 0; i < radixDropdown.options.Count; i++)
                {
                    if (radixDropdown.options[i].text.Equals(n.radixType))
                        radixDropdown.value = i;
                }

                //order is important due to autocomplete dropwdown init issue
                logicgate.GetComponentInChildren<InputController>().Init();
                componentList.Add(n.name, logicgate.GetComponentInChildren<InputController>());
                logicgate.GetComponent<DragDrop>().FullVersion.SetActive(false);
                logicgate.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(true); //enable autodropdown d
                iclg._optimizedFunction = n.id;
                logicgate.transform.localPosition = n.pos2d;
            }

            if (n.type.Equals(NetlistComponentType.savedcomponent))
            {
                //create a saved component
                string path = Application.persistentDataPath + "/User/Generated/" + n.id.Substring(2) + "/" + n.id + ".sp";
                var sc = ReadHeaderFromNetlist(path, n.id.Substring(2));
                var go = saveCircuit.GenerateListItem(sc, root.transform, true);

                go.transform.localPosition = n.pos2d;
                go.transform.GetComponent<DragDrop>().FullVersion.GetComponent<InputController>().Init();
                componentList.Add(n.name, go.transform.GetComponent<DragDrop>().FullVersion.GetComponent<InputController>());              
            }

            if (n.type.Equals(NetlistComponentType.input))
            {
                var logicgate = GameObject.Instantiate(CircuitGenerator.InputPrefab);
                logicgate.GetComponent<DragDrop>().MenuVersion.SetActive(false);
                logicgate.GetComponent<DragDrop>().FullVersion.SetActive(true);
                logicgate.GetComponent<DragDrop>().SetVersion(true);
                logicgate.transform.SetParent(root.transform);

                var radixDropdown = logicgate.GetComponentInChildren<TMP_Dropdown>();
                for (int i = 0; i < radixDropdown.options.Count; i++)
                {
                    if (radixDropdown.options[i].text.Equals(n.radixType))
                        radixDropdown.value = i;
                }

                logicgate.GetComponentInChildren<DragExpand>().SetPanelSize(n.size);

                var buttons = logicgate.GetComponentsInChildren<BtnInput>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].GetComponentInChildren<TMP_InputField>().text = n.ioLabels[i];
                    buttons[i].Init();
                }

                logicgate.transform.localPosition = n.pos2d;
                logicgate.GetComponentInChildren<InputController>().Init();
                componentList.Add(n.name, logicgate.GetComponentInChildren<InputController>());
                logicgate.GetComponent<DragDrop>().FullVersion.SetActive(false);

            }

            if (n.type.Equals(NetlistComponentType.output))
            {
                var logicgate = GameObject.Instantiate(CircuitGenerator.OutputPrefab);
                logicgate.GetComponent<DragDrop>().MenuVersion.SetActive(false);
                logicgate.GetComponent<DragDrop>().FullVersion.SetActive(true);
                logicgate.GetComponent<DragDrop>().SetVersion(true);
                logicgate.transform.SetParent(root.transform);

                var radixDropdown = logicgate.GetComponentInChildren<TMP_Dropdown>();
                for (int i = 0; i < radixDropdown.options.Count; i++)
                {
                    if (radixDropdown.options[i].text.Equals(n.radixType))
                        radixDropdown.value = i;
                }

                logicgate.GetComponentInChildren<DragExpand>().SetPanelSize(n.size);

                var buttons = logicgate.GetComponentsInChildren<BtnInput>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].GetComponentInChildren<TMP_InputField>().text = n.ioLabels[i];
                    buttons[i].Init();
                }

                logicgate.transform.localPosition = n.pos2d;
                logicgate.GetComponentInChildren<InputController>().Init();
                componentList.Add(n.name, logicgate.GetComponentInChildren<InputController>());
                logicgate.GetComponent<DragDrop>().FullVersion.SetActive(false);
            }
        }

        //second pass instantiate connections
        var lm = GameObject.FindObjectOfType<LineManager>();
        foreach (var n in result.connections)
        {
            var parts = n.Split(' ');
            string inputId = parts[0];
            string outputId = parts[2];
            int inputPort = int.Parse(parts[1]);
            int outputPort = int.Parse(parts[3]);
            InputController input = componentList[inputId];
            InputController output = componentList[outputId];
            BtnInput startTerminal = input.Buttons[inputPort].GetComponent<BtnInput>();
            BtnInput endTerminal = output.Buttons[outputPort].GetComponent<BtnInput>();
            lm.NewConnection(startTerminal, endTerminal, root.transform);
        }
    }

    private (NetlistComponent[] components, List<string> connections) FindComponentsInNetlist(string path)
    {
        List<NetlistComponent> components = new List<NetlistComponent>();
        List<string> connectionList = new List<string>();
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("*** @f "))
                {
                    var partsId = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsName = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsRadixType = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsArity = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsPos2d = line.Split(' '); //index 2 index 3

                    NetlistComponent c = new NetlistComponent();
                    c.type = NetlistComponentType.function;
                    c.id = partsId[2];
                    c.name = partsName[2];
                    c.radixType = partsRadixType[2];
                    c.arity = int.Parse(partsArity[2]);
                    c.pos2d = new Vector2(float.Parse(partsPos2d[2]), float.Parse(partsPos2d[3]));

                    components.Add(c);
                }

                if (line.Contains("*** @s "))
                {
                    var partsId = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsName = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsPos2d = line.Split(' '); //index 2 index 3

                    NetlistComponent c = new NetlistComponent();
                    c.type = NetlistComponentType.savedcomponent;
                    c.id = partsId[2];
                    c.name = partsName[2];
                    c.pos2d = new Vector2(float.Parse(partsPos2d[2]), float.Parse(partsPos2d[3]));

                    components.Add(c);
                }

                if (line.Contains("*** @i "))
                {
                    var partsRadixType = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsName = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partSize = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsInputLbl = line.TrimEnd().Split(' '); //index 2 ... n
                    line = reader.ReadLine();
                    var partsPos2d = line.Split(' '); //index 2 index 3 

                    List<string> ioLbls = new List<string>();
                    for (int i = 2; i < partsInputLbl.Length; i++)
                    {
                        ioLbls.Add(partsInputLbl[i].Substring(2, partsInputLbl[i].Length - 4));
                    }

                    NetlistComponent c = new NetlistComponent();
                    c.radixType = partsRadixType[2];
                    c.name = partsName[2];
                    c.size = int.Parse(partSize[2]);
                    c.type = NetlistComponentType.input;
                    c.ioLabels = ioLbls.ToArray();
                    c.pos2d = new Vector2(float.Parse(partsPos2d[2]), float.Parse(partsPos2d[3]));

                    components.Add(c);
                }

                if (line.Contains("*** @o "))
                {
                    var partsRadixType = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsName = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partSize = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsInputLbl = line.TrimEnd().Split(' '); //index 2 ... n
                    line = reader.ReadLine();
                    var partsPos2d = line.Split(' '); //index 2 index 3 

                    List<string> ioLbls = new List<string>();
                    for (int i = 2; i < partsInputLbl.Length; i++)
                    {
                        ioLbls.Add(partsInputLbl[i].Substring(2, partsInputLbl[i].Length - 4));
                    }

                    NetlistComponent c = new NetlistComponent();
                    c.radixType = partsRadixType[2];
                    c.name = partsName[2];
                    c.size = int.Parse(partSize[2]);
                    c.type = NetlistComponentType.output;
                    c.ioLabels = ioLbls.ToArray();
                    c.pos2d = new Vector2(float.Parse(partsPos2d[2]), float.Parse(partsPos2d[3]));

                    components.Add(c);
                }

                if (line.Contains("*** @conn "))
                {
                    connectionList.Add(line.Substring(10));
                }
            }
        }

        return (components.ToArray(), connectionList);
    }

    //this function is basically importNetlist OutputRoutine, we need to refactor it
    private SavedComponent ReadHeaderFromNetlist(string path, string name)
    {
        //ADD preview
        List<string> netlistAttributes = new List<string>();
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            Stats stats = new Stats();
            List<RadixOptions> inputs = null;
            List<RadixOptions> outputs = null;
            List<string> inputLabels = new List<string>();
            List<string> outputLabels = new List<string>();

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("*** STATS"))
                {
                    line = reader.ReadLine();
                    var tcount = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var tlogicgates = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var ulogicgates = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var alevel = line.Split(' '); //index 2                   

                    stats.abstractionLevelCount = int.Parse(alevel[2]);
                    stats.transistorCount = int.Parse(tcount[2]);
                    stats.uniqueLogicGateCount = int.Parse(ulogicgates[2]);
                    stats.totalLogicGateCount = int.Parse(tlogicgates[2]);
                }

                if (line.Contains("*** SEMANTIC INTERFACE"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("*** @i "))
                        {
                            var partsRadixType = line.TrimEnd().Split(' '); //index 2
                            line = reader.ReadLine();
                            var partsName = line.Split(' '); //index 2
                            line = reader.ReadLine();
                            var partSize = line.Split(' '); //index 2
                            line = reader.ReadLine();
                            var partsInputLbl = line.TrimEnd().Split(' '); //index 2 ... n
                            line = reader.ReadLine();
                            var partsPos2d = line.Split(' '); //index 2 index 3 

                            List<string> ioLbls = new List<string>();
                            for (int i = 2; i < partsInputLbl.Length; i++)
                            {
                                ioLbls.Add(partsInputLbl[i].Substring(2, partsInputLbl[i].Length - 4));
                            }

                            List<RadixOptions> ioRadix = new List<RadixOptions>();
                            for (int i = 2; i < partsRadixType.Length; i++)
                            {
                                ioRadix.Add((RadixOptions)Enum.Parse(typeof(RadixOptions), partsRadixType[i]));
                            }

                            inputs = ioRadix;
                            inputLabels = ioLbls;
                        }

                        if (line.Contains("*** @o "))
                        {
                            var partsRadixType = line.Split(' '); //index 2
                            line = reader.ReadLine();
                            var partsName = line.Split(' '); //index 2
                            line = reader.ReadLine();
                            var partSize = line.Split(' '); //index 2
                            line = reader.ReadLine();
                            var partsInputLbl = line.TrimEnd().Split(' '); //index 2 ... n
                            line = reader.ReadLine();
                            var partsPos2d = line.Split(' '); //index 2 index 3 

                            List<string> ioLbls = new List<string>();
                            for (int i = 2; i < partsInputLbl.Length; i++)
                            {
                                ioLbls.Add(partsInputLbl[i].Substring(2, partsInputLbl[i].Length - 4));
                            }

                            List<RadixOptions> ioRadix = new List<RadixOptions>();
                            for (int i = 2; i < partsRadixType.Length; i++)
                            {
                                ioRadix.Add((RadixOptions)Enum.Parse(typeof(RadixOptions), partsRadixType[i]));
                            }

                            outputs = ioRadix;
                            outputLabels = ioLbls;
                        }
                    }
                }
            }

            SavedComponent c = new SavedComponent();
            c.ComponentName = name;
            c.ComponentNetlistPath = path;
            c.Stats = stats;
            c.Inputs = inputs;
            c.InputLabels = inputLabels;
            c.Outputs = outputs;
            c.OutputLabels = outputLabels;
            return c;
        }

    }
}

public class NetlistComponent
{
    public NetlistComponentType type;
    public string id;
    public string name;
    public int arity;
    public string radixType;
    public Vector2 pos2d;
    public int ios;
    public string[] ioLabels;
    public int size;
}



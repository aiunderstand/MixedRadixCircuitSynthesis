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

public class ComponentGenerator : MonoBehaviour
{
    public enum NetlistComponentType
    {
        savedcomponent,
        function
    }

    public GameObject terminalInputPrefab;
    public GameObject terminalOutputPrefab; //we can refactor and merge this with input to "terminal"
    public TextMeshProUGUI title;
    public RectTransform body;
    public GameObject infoBtn;
    public int Size; //actually height 
    Color _colorTernary = new Color(255, 0, 211); //we should define it in 1 place instead of 2, see btninput
    Color _colorBinary = new Color(0, 214, 255);//we should define it in 1 place instead of 2, see btninput
    public void Generate(SavedComponent s, Views level)
    {
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
            input.transform.SetParent(this.transform);
            input.transform.localScale = new Vector3(1, 1, 1);
            input.transform.localPosition = new Vector2(-65, (-10 * (s.Inputs.Count - 1)) + i * 20);
            input.GetComponent<BtnInput>()._portIndex = i;
            input.GetComponent<BtnInput>().isOutput = false;



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
            output.transform.SetParent(this.transform);
            output.transform.localScale = new Vector3(1, 1, 1);
            output.transform.localPosition = new Vector2(65, (-10 * (s.Outputs.Count - 1)) + i * 20);
            output.GetComponent<BtnInput>()._portIndex = i;
            output.GetComponent<BtnInput>().isOutput = true;

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
    }


    private void GenerateNextAbstractionLevel(SavedComponent s)
    {
        GameObject root = new GameObject();
        root.AddComponent<RectTransform>();
        root.name = "Level: " + (s.Stats.abstractionLevelCount - 1).ToString();
        root.transform.SetParent(this.gameObject.transform.parent.transform);
        root.transform.localPosition = new Vector3(0, 0);
        root.transform.localScale = new Vector3(1, 1);
        this.gameObject.transform.parent.GetComponent<DragDrop>().LowerAbstractionVersion = root;

        //read netlist
        NetlistComponent[] components = FindComponentsInNetlist(s.ComponentNetlistPath);

        //very expensive call, we need to refactor using a singleton
        SaveCircuit saveCircuit = GameObject.FindObjectOfType<SaveCircuit>();
        foreach (var n in components)
        {
            //generate on screen
            if (n.type.Equals(NetlistComponentType.function))
            {
                GameObject go = new GameObject();
                go.AddComponent<RectTransform>();
                go.name = n.id;
                go.transform.SetParent(root.transform);
                go.transform.localPosition = n.pos2d;
            }

            if (n.type.Equals(NetlistComponentType.savedcomponent))
            {
                //create a saved component
                string circuitName = n.id.Substring(2);
                string path = Application.persistentDataPath + "/User/Generated/" + circuitName + "/" + n.id + ".sp";
                var sc = ReadHeaderFromNetlist(path, circuitName);
                var go = saveCircuit.GenerateListItem(sc, root.transform, true);
                go.transform.localPosition = n.pos2d;
            }
        }
    }

    private NetlistComponent[] FindComponentsInNetlist(string path)
    {
        List<NetlistComponent> components = new List<NetlistComponent>();
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("*** @f "))
                {
                    var partsId = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsArity = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsPos2d = line.Split(' '); //index 2 index 3
                    line = reader.ReadLine();
                    var partsConn = line.Split(' '); //index 2 ... n
                    List<string> con = new List<string>();
                    for (int i = 2; i < partsConn.Length; i++)
                    {
                        con.Add(partsConn[i]);
                    }

                    NetlistComponent c = new NetlistComponent();
                    c.type = NetlistComponentType.function;
                    c.id = partsId[2];
                    c.arity = int.Parse(partsArity[2]);
                    c.pos2d = new Vector2(float.Parse(partsPos2d[2]), float.Parse(partsPos2d[3]));
                    c.connections = con.ToArray();

                    components.Add(c);
                }

                if (line.Contains("*** @s "))
                {
                    var partsId = line.Split(' '); //index 2
                    line = reader.ReadLine();
                    var partsPos2d = line.Split(' '); //index 2 index 3
                    line = reader.ReadLine();
                    var partsConn = line.Split(' '); //index 2 ... n
                    List<string> con = new List<string>();
                    for (int i = 2; i < partsConn.Length; i++)
                    {
                        con.Add(partsConn[i]);
                    }

                    NetlistComponent c = new NetlistComponent();
                    c.type = NetlistComponentType.savedcomponent;
                    c.id = partsId[2];
                    c.pos2d = new Vector2(float.Parse(partsPos2d[2]), float.Parse(partsPos2d[3]));
                    c.connections = con.ToArray();

                    components.Add(c);
                }
            }
        }

        return components.ToArray();
    }

    //this function is basically importNetlist OutputRoutine, we need to refactor it
    private SavedComponent ReadHeaderFromNetlist(string path, string name)
    {
         //ADD preview
        List<string> netlistAttributes = new List<string>();
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("*** @"))
                    netlistAttributes.Add(line);
            }
        }

        SavedComponent c =ImportNetlist.ParseIntoSavedComponent(netlistAttributes);
        c.ComponentName = name;
        c.ComponentNetlistPath = path;

        return c;
    }

}
public class NetlistComponent
{
    public NetlistComponentType type;
    public string id;
    public int arity;
    public Vector2 pos2d;
    public string[] connections;
}



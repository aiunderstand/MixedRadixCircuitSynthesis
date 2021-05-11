using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO.Compression;
using System.IO;
using System;
using static VisualTransistor;
using UnityEngine.UI;

public class StatisticsUI : MonoBehaviour
{
    public TextMeshProUGUI transistorCount;
    public TextMeshProUGUI uniqueLogicGateCount;
    public TextMeshProUGUI totalLogicGateCount;
    public TextMeshProUGUI abstractionLevelCount;
    public TextMeshProUGUI netlistName;
    public RectTransform Vdd;
    public RectTransform Gnd;
    public RectTransform Out;
    public RectTransform DivUpWire;
    public RectTransform DivDownWire;
    public RectTransform DivVertWire;

    public TextMeshProUGUI gateName;
    public GameObject TransistorPrefab;    
    public RectTransform Background;
    public Transform CircuitCanvas;
    
    int logicGateId;
    List<string> logicGatePaths = new List<string>();

    public enum ParsePhases
    {
        division = 0,
        pullup = 1,
        pulldown = 2,
        pullup_half = 3,
        pulldown_half = 4
    }

    public void Show(Stats stats, DragDrop dd)
    {
        transistorCount.text = stats.transistorCount.ToString();
        abstractionLevelCount.text = stats.abstractionLevelCount.ToString();
        totalLogicGateCount.text = stats.totalLogicGateCount.ToString();
        uniqueLogicGateCount.text = stats.uniqueLogicGateCount.ToString();
        netlistName.text = dd.SavedComponent.ComponentName;
        gameObject.SetActive(true);

        logicGateId = 0;

        logicGatePaths = GetLogicGatePaths(Path.GetDirectoryName(dd.SavedComponent.ComponentNetlistPath));

        DrawLogicGate(logicGateId);
        UpdateDrawingWithActiveTransistorPath(dd);
    }

    public void ShowSimple(Stats stats, SavedComponent sc) //without any active path
    {
        transistorCount.text = stats.transistorCount.ToString();
        abstractionLevelCount.text = stats.abstractionLevelCount.ToString();
        totalLogicGateCount.text = stats.totalLogicGateCount.ToString();
        uniqueLogicGateCount.text = stats.uniqueLogicGateCount.ToString();
        netlistName.text = sc.ComponentName;
        gameObject.SetActive(true);

        logicGateId = 0;

        logicGatePaths = GetLogicGatePaths(Path.GetDirectoryName(sc.ComponentNetlistPath));

        DrawLogicGate(logicGateId);
    }

    private void UpdateDrawingWithActiveTransistorPath(DragDrop dd)
    {
        //get all transistors
        var allTs = GameObject.FindGameObjectsWithTag("Transistor");

        //get all btnInputs from the logic gate. Add them in order (eg. C, B, A in arity 3)
        dd.LowerAbstractionVersion.gameObject.SetActive(true);
        List<BtnInput> inputs = new List<BtnInput>();
        foreach (Transform child in dd.LowerAbstractionVersion.gameObject.transform)
        {
            GameObject go = child.gameObject;
            if (go.name.Contains("LogicGate"))
            {
                if (go.GetComponent<DragDrop>()) //only drag drop components, not wires
                {
                    var iclg = go.GetComponent<DragDrop>().FullVersion.transform.GetChild(0).GetComponent<InputControllerLogicGate>();

                    foreach (var btn in iclg.activeIC.Buttons)
                    {
                        inputs.Add(btn.GetComponent<BtnInput>());
                    }
                }   
            }
        }

        dd.LowerAbstractionVersion.gameObject.SetActive(false);

        //update label of each transistor with: inverter (optional) bntInput label and value in format p:?n:?"" +label + [state]
        //check each row of each network and set its color ON (colored) or OFF (grayscalish).
        //blue NMOS/PMOS are OFF (HIGH) ON (MED) ON (LOW)
        //red  NMOS/PMOS are OFF (HIGH) OFF (MED) ON (LOW)

        foreach (var t in allTs)
        {
            VisualTransistor vt = t.GetComponent<VisualTransistor>();

            //parse label to find port, which is always the second symbol (eg. i0_p)
            if (vt.origLabel != "") //skip empty (eg. voltage dividers)
            {
                int port = int.Parse(vt.origLabel.Substring(1,1));
                var parsedLabel = vt.origLabel.Split('_');

                string value = inputs[port]._value.ToString();
                if (parsedLabel.Length == 2)
                {
                    if (parsedLabel[1].Equals("p"))
                        value = InvertValueP(value);
                    else
                        value = InvertValueN(value);
                }

                string lbl = inputs[port].label.text + " [" + value + "]";
                string newLabel = parsedLabel.Length == 2 ? parsedLabel[1] + ":" + lbl : lbl;
                vt.label.text = newLabel;

                //determine color based on if transistor is ON or OFF
                bool isOn = false;
                switch (value)
                {
                    case "-1":
                        if (vt.GetTransistorType().Equals(VisualTransistor.TransistorTypes.PMOS_BodyToSource))
                            isOn = true;
                       
                        if (vt.GetTransistorType().Equals(VisualTransistor.TransistorTypes.NMOS_BodyToSource))
                            isOn = false;
                        break;
                    case "0":
                        if (vt.GetTransistorType().Equals(VisualTransistor.TransistorTypes.PMOS_BodyToSource))
                        {
                            if (vt.GetDiameter().Equals(19))
                                isOn = true;
                            else
                                isOn = false;
                        }

                        if (vt.GetTransistorType().Equals(VisualTransistor.TransistorTypes.NMOS_BodyToSource))
                        {
                            if (vt.GetDiameter().Equals(19))
                                isOn = true;
                            else
                                isOn = false;
                        }
                        break;
                    case "1":
                        if (vt.GetTransistorType().Equals(VisualTransistor.TransistorTypes.PMOS_BodyToSource))
                            isOn = false;

                        if (vt.GetTransistorType().Equals(VisualTransistor.TransistorTypes.NMOS_BodyToSource))
                            isOn = true;
                        break;
                }

                vt.SetActivationLevel(isOn);
            }
        }
        
        

    }

    private string InvertValueP(string value)
    {
       string result = "";
       switch (value)
        {
            case "-1":
                result = "1";
                break;
            case "0":
                result = "1";
                break;
            case "1":
                result = "-1";
                break;
        }

        return result;
    }

    private string InvertValueN(string value)
    {
        string result = "";
        switch (value)
        {
            case "-1":
                result = "1";
                break;
            case "0":
                result = "-1";
                break;
            case "1":
                result = "-1";
                break;
        }

        return result;
    }
    private List<string> GetLogicGatePaths(string path)
    {
        var files = System.IO.Directory.GetFiles(path, "f_***.sp", SearchOption.TopDirectoryOnly);

        List<string> paths = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            paths.Add(files[i]);
        }
       
        return paths;
    }

    private void DrawLogicGate(int logicGateId)
    {
        //remove possible previous transistors
        var previousTs = GameObject.FindGameObjectsWithTag("Transistor");
        for (int i = 0; i < previousTs.Length; i++)
        {
            Destroy(previousTs[i]);
        }

        //reset scaling
        CircuitCanvas.transform.localScale = new Vector3(1f, 1f, 1);

        //parse logicgateFunction
        NetlistGate LogicGateAsTxt = ParseLogicGateNetlist(logicGatePaths[logicGateId]);

        //draw map from txt
        var gName = Path.GetFileNameWithoutExtension(logicGatePaths[logicGateId]);
        gateName.text = gName.Substring(2, gName.Length - 2);

        int offsetX = 80; //fixed 
        int offsetY = 40;
        int startY = 0;        
        bool addExtraRowForDividerUp = LogicGateAsTxt.PUN.Rows == LogicGateAsTxt.PU_halfN.Rows ? true : false;
        bool addExtraRowForDividerDown = LogicGateAsTxt.PDN.Rows == LogicGateAsTxt.PD_halfN.Rows ? true : false;

        //draw Divider Network (special case)
        if (LogicGateAsTxt.Dividers.Rows > 0)
        {
            startY = offsetY;
            int shiftRight = LogicGateAsTxt.PU_halfN.Columns > LogicGateAsTxt.PD_halfN.Columns ? LogicGateAsTxt.PU_halfN.Columns : LogicGateAsTxt.PD_halfN.Columns;

            Transistors t = LogicGateAsTxt.Dividers.Transistors[0][0];
            GameObject tGo;
            tGo = GameObject.Instantiate(TransistorPrefab);
            VisualTransistor vt = tGo.GetComponent<VisualTransistor>();

            vt.label.text = "";
            vt.origLabel = "";

            if (t.channel.Equals("PCNFET"))
                vt.SetTransistorTypeTo(TransistorTypes.PMOS_BodyToSource, t.diameter);
            else
                vt.SetTransistorTypeTo(TransistorTypes.NMOS_BodyToSource, t.diameter);
          
            tGo.transform.SetParent(CircuitCanvas);
            tGo.transform.localPosition = new Vector3((shiftRight - 0.5f) * -offsetX, 0.5f * offsetY, 0);


            t = LogicGateAsTxt.Dividers.Transistors[0][1];
            tGo = GameObject.Instantiate(TransistorPrefab);
            vt = tGo.GetComponent<VisualTransistor>();

            vt.label.text = "";
            vt.origLabel = "";

            if (t.channel.Equals("PCNFET"))
                vt.SetTransistorTypeTo(TransistorTypes.PMOS_BodyToSource, t.diameter);
            else
                vt.SetTransistorTypeTo(TransistorTypes.NMOS_BodyToSource, t.diameter);

            tGo.transform.SetParent(CircuitCanvas);
            tGo.transform.localPosition = new Vector3((shiftRight - 0.5f) * -offsetX, 0.5f * -offsetY, 0);

            //Edge case: draw connections to gnd or vdd if column of half network is zero but full network rows is larger than 1
            //check for up network
            if (LogicGateAsTxt.PU_halfN.Columns == 0 && LogicGateAsTxt.PUN.Rows > 1)
            {
                for (int i = 0; i < LogicGateAsTxt.PUN.Rows-1; i++)
                {
                    var emtpyTransGo = GameObject.Instantiate(TransistorPrefab);
                    var emptyVT = emtpyTransGo.GetComponent<VisualTransistor>();
                    emptyVT.SetTransistorTypeTo(TransistorTypes.Empty, 0);
                    emtpyTransGo.transform.SetParent(CircuitCanvas);
                    emtpyTransGo.transform.localPosition = new Vector3((shiftRight - 0.5f) * -offsetX, (i+1.5f) * offsetY, 0);
                }
            }

            //check for down network
            if (LogicGateAsTxt.PD_halfN.Columns == 0 && LogicGateAsTxt.PDN.Rows > 1)
            {
                for (int i = 0; i < LogicGateAsTxt.PDN.Rows - 1; i++)
                {
                    var emtpyTransGo = GameObject.Instantiate(TransistorPrefab);
                    var emptyVT = emtpyTransGo.GetComponent<VisualTransistor>();
                    emptyVT.SetTransistorTypeTo(TransistorTypes.Empty, 0);
                    emtpyTransGo.transform.SetParent(CircuitCanvas);
                    emtpyTransGo.transform.localPosition = new Vector3((shiftRight - 0.5f) * -offsetX, (i+1.5f) * -offsetY, 0);
                }
            }


            //draw Wires

            int divWireWidth = LogicGateAsTxt.PU_halfN.Columns > LogicGateAsTxt.PD_halfN.Columns ? LogicGateAsTxt.PU_halfN.Columns - 1 : LogicGateAsTxt.PD_halfN.Columns - 1;

            int divUpWireWidth = LogicGateAsTxt.PU_halfN.Columns == 0 ? 0 : divWireWidth;
            DivUpWire.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, divUpWireWidth * offsetX);
            DivUpWire.transform.localPosition = new Vector3(-26 +(shiftRight - 1) * -40f, offsetY, 0);
            DivUpWire.GetComponent<Image>().color = Color.green;

            int divDownWireWidth = LogicGateAsTxt.PD_halfN.Columns == 0 ? 0 : divWireWidth;
            DivDownWire.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, divDownWireWidth * offsetX);
            DivDownWire.transform.localPosition = new Vector3(-26 +(shiftRight - 1) * -40f, -offsetY, 0);
            DivDownWire.GetComponent<Image>().color = Color.green;

            DivVertWire.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offsetY);
            DivVertWire.transform.localPosition = new Vector3(-14 + (shiftRight - 0.5f) * -offsetX, 0, 0);
            DivVertWire.GetComponent<Image>().color = Color.green;

            DivUpWire.gameObject.SetActive(true);
            DivDownWire.gameObject.SetActive(true);
            DivVertWire.gameObject.SetActive(true);
        }
        else
        {
            DivUpWire.gameObject.SetActive(false);
            DivDownWire.gameObject.SetActive(false);
            DivVertWire.gameObject.SetActive(false);
        }


        //draw PU Network
        DrawNetwork(LogicGateAsTxt.PUN, 0, offsetX, offsetY, addExtraRowForDividerUp);

        //draw PD Network
        DrawNetwork(LogicGateAsTxt.PDN, 0, offsetX, -offsetY, addExtraRowForDividerDown);

        //draw PUhalf Network
        DrawNetwork(LogicGateAsTxt.PU_halfN, startY, -offsetX, offsetY, false);

        //draw PDhalf Network
        DrawNetwork(LogicGateAsTxt.PD_halfN, -startY, -offsetX, -offsetY, false);

        //resize background
        Background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.Columns+2) * offsetX);
        Background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (LogicGateAsTxt.Rows+2) * offsetY);

        //HACK: shift background based on imbalance of pu_half and pu networks, better is to compute start pos left corner

        float middle = ((float) LogicGateAsTxt.Columns) /2f;

        int largestLeftColumn = LogicGateAsTxt.PU_halfN.Columns > LogicGateAsTxt.PD_halfN.Columns ? LogicGateAsTxt.PU_halfN.Columns : LogicGateAsTxt.PD_halfN.Columns;
        int largestRightColumn = LogicGateAsTxt.PUN.Columns > LogicGateAsTxt.PDN.Columns ? LogicGateAsTxt.PUN.Columns : LogicGateAsTxt.PDN.Columns;
        int largestColumn = largestLeftColumn > largestRightColumn ? largestLeftColumn : largestRightColumn;
      
        int shiftOffset = largestLeftColumn > largestRightColumn ? offsetX : -offsetX;

        float shift = (-largestColumn + middle) * shiftOffset;
      
        Background.transform.localPosition = new Vector3(shift,0, 0);

        //add power lines
        float alignRight = (((LogicGateAsTxt.Columns - 0.2f) *80)/2) +8f;

        Vdd.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.Columns - 0.2f) * offsetX);
        Vdd.transform.localPosition = new Vector3(shift, (LogicGateAsTxt.MaxRowsTop ) * offsetY, 0);
        Vdd.transform.GetChild(0).transform.localPosition = new Vector3(alignRight,0,0);

        Gnd.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.Columns - 0.2f) * offsetX);
        Gnd.transform.localPosition = new Vector3(shift, (LogicGateAsTxt.MaxRowsBottom ) * -offsetY, 0);
        Gnd.transform.GetChild(0).transform.localPosition = new Vector3(alignRight, 0, 0);

        Out.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.Columns - 0.4f) * offsetX);
        Out.transform.localPosition = new Vector3(shift+10, 0, 0);
        Out.transform.GetChild(0).transform.localPosition = new Vector3(-8+ alignRight, 0, 0);


        //resize if to big
        if (LogicGateAsTxt.Columns > 16)
            CircuitCanvas.transform.localScale = new Vector3(0.8f, 0.8f, 1);
         

    }

    private void DrawNetwork(TransistorNetwork network, int startY, int offsetX, int offsetY, bool addExtraRowForDivider)
{
        int rows = network.Rows;
        if (addExtraRowForDivider)
            rows += 1;

    for (int c = 0; c < network.Columns; c++)
    {
            for (int r = 0; r < rows; r++)
            {
                GameObject tGo;
                tGo = GameObject.Instantiate(TransistorPrefab);
                VisualTransistor vt = tGo.GetComponent<VisualTransistor>();
                
                if (r < network.Transistors[c].Count) //check if transistor or empty transistor (just a filler with a line)
                {
                    var t = network.Transistors[c][r];
                    vt.label.text = t.gate;
                    vt.origLabel = t.gate;


                    if (t.channel.Equals("PCNFET"))
                        vt.SetTransistorTypeTo(TransistorTypes.PMOS_BodyToSource, t.diameter);
                    else
                        vt.SetTransistorTypeTo(TransistorTypes.NMOS_BodyToSource, t.diameter);
                }
                else
                {
                    vt.SetTransistorTypeTo(TransistorTypes.Empty, 0);
                    vt.label.text = "";
                    vt.origLabel = "";
                }
       
                tGo.transform.SetParent(CircuitCanvas);
                tGo.transform.localPosition = new Vector3((c+0.5f) * offsetX, startY +(r+0.5f)* offsetY, 0);                
            }
    }
}

    private NetlistGate ParseLogicGateNetlist(string path)
    {
        NetlistGate result = new NetlistGate();
        string line;
        
        using (StreamReader reader = new StreamReader(path))
        {
            //skip first 3 lines which are never empty
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();

            ParsePhases phase = ParsePhases.division; //order is division, pu, pd, puhalf, pdhalf

            List<Transistors> tempList = new List<Transistors>();
            TransistorNetwork tempNetwork = new TransistorNetwork();
            int maxRows = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("***pullup full") || line.Contains("***pulldown full") || line.Contains("***pullup half") || line.Contains("***pulldown half"))
                {
                    tempNetwork.Columns = tempNetwork.Transistors.Count;
                    tempNetwork.Rows = maxRows;
                    maxRows = 0;

                    switch (phase)
                    {
                        case ParsePhases.division:
                            result.Dividers = tempNetwork;                            
                            break;
                        case ParsePhases.pullup:
                            result.PUN = tempNetwork;
                            break;
                        case ParsePhases.pulldown:
                            result.PDN = tempNetwork;
                            break;
                        case ParsePhases.pullup_half:
                            result.PU_halfN = tempNetwork;
                            break;
                        case ParsePhases.pulldown_half:
                            result.PD_halfN = tempNetwork;
                            break;
                    }

                    int newPhase = Convert.ToInt32(phase);
                    phase = (ParsePhases) (newPhase+1);
                    reader.ReadLine(); //skip empty
                    reader.ReadLine(); //skip empty

                    tempList = new List<Transistors>();
                    tempNetwork = new TransistorNetwork();
                }
                else
                {
                    if (line.Contains("xn") || line.Contains("xp"))
                    {
                        var parts = line.Split(' ');
                        Transistors t = new Transistors();
                        t.drain = parts[1];
                        t.gate = parts[2];
                        t.source = parts[3];
                        t.substrate = parts[4];
                        t.nodeId = parts[0];
                        t.channel = parts[5];
                        
                        //the next line contains more info about the CNTFET incl. diameter
                        line = reader.ReadLine();
                        parts = line.Split(' ');
                        t.diameter = int.Parse(parts[33]);

                        tempList.Add(t);                        
                    }
                    else
                    {
                        if (line.Contains(""))
                        {
                            if (tempList.Count > 0)
                            {
                                if (tempList.Count > maxRows)
                                    maxRows = tempList.Count;

                                tempNetwork.Transistors.Add(tempList);
                                tempList = new List<Transistors>();
                            }
                        }
                    }
                }

                if (line.Contains(".ends"))
                {
                    tempNetwork.Columns = tempNetwork.Transistors.Count;
                    tempNetwork.Rows = maxRows;
                   
                    switch (phase)
                    {
                        case ParsePhases.division:
                            result.Dividers = tempNetwork;
                            break;
                        case ParsePhases.pullup:
                            result.PUN = tempNetwork;
                            break;
                        case ParsePhases.pulldown:
                            result.PDN = tempNetwork;
                            break;
                        case ParsePhases.pullup_half:
                            result.PU_halfN = tempNetwork;
                            break;
                        case ParsePhases.pulldown_half:
                            result.PD_halfN = tempNetwork;
                            break;
                    }
                }
            }
        }

        int maxTopRow;
        if (result.Dividers.Transistors.Count > 0)
            maxTopRow = result.PUN.Rows > result.PU_halfN.Rows+1 ? result.PUN.Rows : result.PU_halfN.Rows+1;
        else
            maxTopRow = result.PUN.Rows > result.PU_halfN.Rows ? result.PUN.Rows : result.PU_halfN.Rows;

        int maxBottomRow;
        if (result.Dividers.Transistors.Count > 0)
            maxBottomRow = result.PDN.Rows > result.PD_halfN.Rows + 1 ? result.PDN.Rows : result.PD_halfN.Rows + 1;
        else
            maxBottomRow = result.PDN.Rows > result.PD_halfN.Rows ? result.PDN.Rows : result.PD_halfN.Rows;

        result.MaxRowsTop = maxTopRow;
        result.MaxRowsBottom = maxBottomRow;
        result.Rows = maxTopRow + maxBottomRow;

        int maxRightCols = result.PUN.Columns > result.PDN.Columns ? result.PUN.Columns : result.PDN.Columns;
        int maxLeftCols = result.PU_halfN.Columns > result.PD_halfN.Columns ? result.PU_halfN.Columns : result.PD_halfN.Columns;
        
        result.Columns = maxLeftCols + maxRightCols;
        


        return result;
    }

    public void NextGate()
    {
        if (logicGateId + 1 < logicGatePaths.Count)
        {
            logicGateId++;
            DrawLogicGate(logicGateId);          
        }
    }

    public void PreviousGate()
    {
        if (logicGateId - 1 >= 0)
        {
            logicGateId--;
            DrawLogicGate(logicGateId);            
        }         
    }
}


public class NetlistGate
{
    public string GateName;
    public TransistorNetwork PUN;
    public TransistorNetwork PDN;
    public TransistorNetwork PU_halfN;
    public TransistorNetwork PD_halfN;
    public TransistorNetwork Dividers;
    public int Columns;
    public int Rows;
    public int MaxRowsTop;
    public int MaxRowsBottom;    

    public NetlistGate()
    {
        PUN = new TransistorNetwork();
        PDN = new TransistorNetwork();
        PU_halfN = new TransistorNetwork();
        PD_halfN = new TransistorNetwork();
        Dividers = new TransistorNetwork();
    }
}

public class TransistorNetwork
{
    public int Columns;
    public int Rows;
    public List<List<Transistors>> Transistors;
    
    public TransistorNetwork() 
    {
        Transistors = new List<List<Transistors>>();
    }
}
public class Transistors
{
    public string nodeId;
    public string drain;
    public string gate;
    public string source;
    public string substrate;
    public string channel;
    public int diameter;    
    public Transistors() { }
}


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

    public void Show(Stats stats, SavedComponent sc)
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

            Transistors t = LogicGateAsTxt.Dividers.Transistors[0][0];
            GameObject tGo;
            tGo = GameObject.Instantiate(TransistorPrefab);
            VisualTransistor vt = tGo.GetComponent<VisualTransistor>();

            //vt.label.text = t.gate;

            if (t.channel.Equals("PCNFET"))
                vt.SetTransistorTypeTo(TransistorTypes.PMOS_BodyToSource, t.diameter);
            else
                vt.SetTransistorTypeTo(TransistorTypes.NMOS_BodyToDrain, t.diameter);
          
            tGo.transform.SetParent(CircuitCanvas);
            tGo.transform.localPosition = new Vector3((LogicGateAsTxt.PU_halfN.Columns - 0.5f) * -offsetX, 0.5f * offsetY, 0);


            t = LogicGateAsTxt.Dividers.Transistors[0][1];
            tGo = GameObject.Instantiate(TransistorPrefab);
            vt = tGo.GetComponent<VisualTransistor>();

            //vt.label.text = t.gate;

            if (t.channel.Equals("PCNFET"))
                vt.SetTransistorTypeTo(TransistorTypes.PMOS_BodyToSource, t.diameter);
            else
                vt.SetTransistorTypeTo(TransistorTypes.NMOS_BodyToDrain, t.diameter);

            tGo.transform.SetParent(CircuitCanvas);
            tGo.transform.localPosition = new Vector3((LogicGateAsTxt.PD_halfN.Columns - 0.5f) * -offsetX, 0.5f * -offsetY, 0);


            //draw Wires
            DivUpWire.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.PU_halfN.Columns-1) * offsetX);
            DivUpWire.transform.localPosition = new Vector3(-26 +(LogicGateAsTxt.PU_halfN.Columns-1) * -40f, offsetY, 0);
            DivUpWire.GetComponent<Image>().color = Color.green;

            DivDownWire.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.PD_halfN.Columns-1) * offsetX);
            DivDownWire.transform.localPosition = new Vector3(-26 +(LogicGateAsTxt.PD_halfN.Columns-1) * -40f, -offsetY, 0);
            DivDownWire.GetComponent<Image>().color = Color.green;

            DivVertWire.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offsetY);
            DivVertWire.transform.localPosition = new Vector3(-14 + (LogicGateAsTxt.PD_halfN.Columns - 0.5f) * -offsetX, 0, 0);
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
        float shiftRight = (-LogicGateAsTxt.PU_halfN.Columns + middle) * offsetX;

        Background.transform.localPosition = new Vector3(shiftRight,0, 0);

        //add power lines
        float alignRight = (((LogicGateAsTxt.Columns - 0.2f) *80)/2) +8f;

        Vdd.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.Columns - 0.2f) * offsetX);
        Vdd.transform.localPosition = new Vector3(shiftRight, (LogicGateAsTxt.MaxRowsTop ) * offsetY, 0);
        Vdd.transform.GetChild(0).transform.localPosition = new Vector3(alignRight,0,0);

        Gnd.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.Columns - 0.2f) * offsetX);
        Gnd.transform.localPosition = new Vector3(shiftRight, (LogicGateAsTxt.MaxRowsBottom ) * -offsetY, 0);
        Gnd.transform.GetChild(0).transform.localPosition = new Vector3(alignRight, 0, 0);

        Out.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (LogicGateAsTxt.Columns - 0.4f) * offsetX);
        Out.transform.localPosition = new Vector3(shiftRight+10, 0, 0);
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

                    if (t.channel.Equals("PCNFET"))
                        vt.SetTransistorTypeTo(TransistorTypes.PMOS_BodyToSource, t.diameter);
                    else
                        vt.SetTransistorTypeTo(TransistorTypes.NMOS_BodyToDrain, t.diameter);
                }
                else
                {
                    vt.SetTransistorTypeTo(TransistorTypes.Empty, 0);
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
                if (line.Contains("***"))
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
        if (result.PUN.Rows == result.PU_halfN.Rows)
        {
            if (result.Dividers.Rows > 0)
                maxTopRow = result.PUN.Rows + 1; //1 for divider
            else
                maxTopRow = result.PUN.Rows;
        }
        else
        {
            maxTopRow = result.PUN.Rows > result.PU_halfN.Rows ? result.PUN.Rows : result.PU_halfN.Rows;
        }

        int maxBottomRow;
        if (result.PDN.Rows == result.PD_halfN.Rows)
        {
            if (result.Dividers.Rows > 0)
                maxBottomRow = result.PDN.Rows + 1; //1 for divider
            else
                maxBottomRow = result.PDN.Rows;
        }
        else
        {
            maxBottomRow = result.PDN.Rows > result.PD_halfN.Rows ? result.PDN.Rows : result.PD_halfN.Rows;
        }

        result.Rows = maxTopRow + maxBottomRow;

        int maxRightCols = result.PDN.Transistors.Count > result.PUN.Transistors.Count ? result.PDN.Transistors.Count : result.PDN.Transistors.Count;
        int maxLeftCols = result.PD_halfN.Transistors.Count > result.PU_halfN.Transistors.Count ? result.PD_halfN.Transistors.Count : result.PU_halfN.Transistors.Count;
        result.Columns = maxLeftCols + maxRightCols;

        result.MaxRowsTop = maxTopRow;
        result.MaxRowsBottom = maxBottomRow;
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


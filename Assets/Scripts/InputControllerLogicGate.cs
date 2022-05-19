using ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BtnInput;

public class InputControllerLogicGate : MonoBehaviour
{
    public InputField DropDownFunctionLabel;
    TextMeshProUGUI _radixTarget; //or source if it is linked to a output
    public Color panelColorDefault;
    public Color panelColorActive;
    public string _optimizedFunction; //field is filled after a save
                                      //internal values used for state stabilisation (if the output does not change do not propagate)
    int portA = -1;
    int portB = -1;
    int portC = -1;
    int portD = -1;
    int _prevOutput = -1; //initialize output at -1 (ground)
    int totalHeatmap = 0;
    int stateChangeCounter = 0;
    public static int stateChangeThreshold = 10;
    public InputController activeIC;
    RadixOptions _radix;

    public void resetStateChangeCounter()
    {
        stateChangeCounter = 0;
    }
    
    private void Awake()
    {
        if (this.name.Equals("LogicGate"))
        {
            GetComponentInParent<DragDrop>().name = ";LogicGate;" + GetInstanceID().ToString();
            activeIC = GetComponentInChildren<InputController>();
        }
    }

    private void Start()
    {
        if (_optimizedFunction != null)
        {
            DropDownFunctionLabel.text = _optimizedFunction;
        }

        ComputeTruthTableOutput();

        _prevOutput = portD;
    }

    public void UpdateInputController(InputController ic)
    {
        activeIC = ic;
        
        portA = 0;
        portB = 0;
        portC = 0;
        portD = 0;
    }

    public int GetArity()
    {
        return GetComponentInChildren<DragExpandTableComponent>().GetArity();
    }

    public int[] GetTruthTable()
    {
        //always return as 3x3 cells x arity, even when binary.
        return GetComponent<Matrix>().GetMatrixCells();
    }

    public RadixOptions GetRadix()
    {
        return _radix;
    }

    public void SetRadix(RadixOptions radix)
    {
        _radix = radix;
    }

    public async void ComputeTruthTableOutput()
    {
        bool stateChanged = false;
        RadixOptions radixTarget = GetRadix();

        bool allConnected = false;

        List<BtnInput> ports = new List<BtnInput>();
        foreach (var item in activeIC.Buttons) //potential bug? Is foreach always correctly ordered?
        {
            ports.Add(item.GetComponent<BtnInput>());
        }
        
        //Step 1: get input values of the logic gate
        Dictionary<int, int> inputs = new Dictionary<int, int>();

        if (applicationmanager.UseBigEndianForLogicGates())
        {
            foreach (var p in ports) 
            {
                switch (p.tag)
                {
                    case "PortA":
                        if (p.Connections.Count != 0)
                            inputs.Add(0, p.Connections[0].connection.startTerminal.GetValueAsIndex(radixTarget));
                        else
                        {
                            int zeroVolt = RadixHelper.ConvertRadixFromTo(RadixOptions.Unknown, radixTarget, -1);
                            inputs.Add(0, RadixHelper.GetValueAsIndex(radixTarget, zeroVolt)); //default to "no voltage" if not connected
                        }

                        if (inputs[0] != portA)
                        {
                            portA = inputs[0];
                        }


                        break;
                    case "PortB":
                        if (p.Connections.Count != 0)
                            inputs.Add(1, p.Connections[0].connection.startTerminal.GetValueAsIndex(radixTarget));
                        else
                        {
                            int zeroVolt = RadixHelper.ConvertRadixFromTo(RadixOptions.Unknown, radixTarget, -1);
                            inputs.Add(1, RadixHelper.GetValueAsIndex(radixTarget, zeroVolt)); //default to "no voltage" if not connected
                        }

                        if (inputs[1] != portB)
                        {
                            portB = inputs[1];
                        }

                        break;
                    case "PortC":
                        if (p.Connections.Count != 0)
                            inputs.Add(2, p.Connections[0].connection.startTerminal.GetValueAsIndex(radixTarget));
                        else
                        {
                            int zeroVolt = RadixHelper.ConvertRadixFromTo(RadixOptions.Unknown, radixTarget, -1);
                            inputs.Add(2, RadixHelper.GetValueAsIndex(radixTarget, zeroVolt)); //default to "no voltage" if not connected
                        }

                        if (inputs[2] != portC)
                        {
                            portC = inputs[2];
                        }
                        break;
                }
            }
        }
        else
        {
            foreach (var p in ports)
            {
                switch (p.tag)
                {
                    case "PortA":
                        inputs.Add(2, p.Connections[0].connection.startTerminal.GetValueAsIndex(radixTarget));

                        if (inputs[2] != portA)
                        {
                            portA = inputs[2];
                        }


                        break;
                    case "PortB":
                        inputs.Add(1, p.Connections[0].connection.startTerminal.GetValueAsIndex(radixTarget));

                        if (inputs[1] != portB)
                        {
                            portB = inputs[1];
                        }

                        break;
                    case "PortC":
                        inputs.Add(0, p.Connections[0].connection.startTerminal.GetValueAsIndex(radixTarget));

                        if (inputs[0] != portC)
                        {
                            portC = inputs[0];
                        }
                        break;
                }
            }
        }
        //Step 2: Get the correct matrix and the cell based on inputs
        //if we have arity 3, we first need to set the correct matrix based on the value
        switch (GetArity())
        {
            case 3:
                {
                    var dropdown = GetComponentInChildren<BtnInputTruthTableDropdown>().gameObject.GetComponent<TMP_Dropdown>();
                    dropdown.value = inputs[2];
                }
                break;
            case 2:
                {
                    inputs.Add(2, 0); //since we use a 3d lookup table we need to supply a depth = 0)
                }
                break;
            case 1:
                {
                    inputs.Add(1, 0); //since we use a 3d lookup table we need to supply a column = 0)
                    inputs.Add(2, 0); //since we use a 3d lookup table we need to supply a depth = 0)
                }
                break;
        }
          
        Matrix m = GetComponent<Matrix>();

        //index 0 = A(row), index 1 = B (column), index 2 (depth)
        string label = m.Truthtable[inputs[0], inputs[1], inputs[2]].label.text;


        //Step 3: Write cell value based on inputs to output port
        if (label.Equals("x"))
        {
            //this should never happen, 2do generate a warning message
            new NotImplementedException();
        }
        else
        {
            await Task.Yield(); //WAIT 1 FRAME AS EVERY COMPNENT IS 1 UNIT DELAY
            portD = int.Parse(label);
        }

        if (_prevOutput != portD)
        {
            stateChanged = true;
            stateChangeCounter++;

            if (SimulationManager.Instance.DebugIsEnabled)
                Debug.Log(this.name + " " + DropDownFunctionLabel.text + " changed output from: " + _prevOutput + " to: " + portD + ". Statechange counter: " + stateChangeCounter);
            
            _prevOutput = portD;
        }

        BtnInput outputPort = null;
        foreach (var p in ports)
        {
            if (p.isOutput)
            {
                outputPort = p;
                p.SetValueWithoutConversionAndCounter(portD);
            }
        }

        //Step 4: UI Stuff,  reset all backgrounds and highlight cell a
        for (int i = 0; i < m.Truthtable.GetLength(0); i++)
        {
            for (int j = 0; j < m.Truthtable.GetLength(1); j++)
            {
                for (int k = 0; k < m.Truthtable.GetLength(2); k++)
                {
                    m.Truthtable[i, j, k].label.color = panelColorDefault;
                }
            }
        }

        //highlight active cell (only works for unbalanced ternary ATM) and update heatmap
        m.Truthtable[inputs[0], inputs[1], inputs[2]].label.color = panelColorActive;
        totalHeatmap++;

        var freq = m.Heatmap[inputs[0], inputs[1], inputs[2]];

        m.Heatmap[inputs[0], inputs[1], inputs[2]] = ++freq;
        m.UpdateHeatmap(totalHeatmap);
           
        //Step 5: Propagate output to its connections
        if ((outputPort != null) && (outputPort.Connections.Count > 0))
        {
            foreach (var c in outputPort.Connections)
            {
                //Debug.Log("LinkId: " + c.connection.id);
                //determine if logic gate or output 
                if (c.connection.endTerminal.tag.Equals("Output"))
                {
                    var val = c.connection.endTerminal.SetValue(GetRadix(), portD, false);
                    c.connection.endTerminal.GetComponentInChildren<LEDtoggle>().SetLedColor(val);
                }
                else
                {
                    //only propagate when states have changed
                    if (stateChanged)
                    {
                        //Debug.Log("before: " + GetInstanceID());
                       
                        //Debug.Log("after:" + GetInstanceID());
                        if ((stateChangeCounter < stateChangeThreshold) && SimulationManager.Instance.SimulationIsEnabled)
                        {
                            if (c.connection.endTerminal.name.Contains("_saved"))
                            {
                                c.connection.endTerminal.SetValue(GetRadix(), portD, false);
                            }
                            else
                            {
                                c.connection.endTerminal.transform.parent.parent.GetComponent<InputControllerLogicGate>().ComputeTruthTableOutput();
                            }
                        }
                        else
                        {
                            if (SimulationManager.Instance.DebugIsEnabled)
                                Debug.Log("Simulation stopped: unstable circuit detected");

                            SimulationManager.Instance.SetSimulationTo(false);
                            MessageManager.Instance.Show("Error","Simulation stopped: circuit did not reach stable output state after 10 changes");
                        }
                    }
                }
            }
        }
    }
}
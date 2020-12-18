using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputControllerLogicGate : MonoBehaviour
{
    public TextMeshProUGUI DropDownFunctionLabel;
    public TextMeshProUGUI DropdownRadixLabel;
    public 
    int _radix = 0;
    public Connection[] Connections = new Connection[4]; //needed for data binding 
    public Color panelColorDefault;
    public Color panelColorActive;

    private void Awake()
    {
        for (int i = 0; i < Connections.Length; i++)
        {
            Connections[i] = new Connection();
        }
    }

    public void ComputeTruthTableOutput(int inputIndex, int value)
    {
        int _minValue = 0;
        int _maxValue = 0;
        int _value = 0;

        switch (DropdownRadixLabel.text)
        {
            case "Balanced Ternary":
                {
                    _minValue = -1;
                    _maxValue = 1;
                }
                break;
            case "Unbalanced Ternary":
                {
                    _minValue = 0;
                    _maxValue = 2;
                }
                break;
            case "Binary":
                {
                    _minValue = 0;
                    _maxValue = 1;
                }
                break;
            default:
                break;
        }

        _value = value;


        // this makes sure there are no errors when binary and ternary are connected. We should inform the user if this happens!
        //--------------------------
        if (_value > _maxValue)
            _value = _maxValue;

        if (_value < _minValue)
            _value = _minValue;
        //--------------------------


        //compute output based on inputs 
        //first check if all connections that are required are present

        //what is the arity
        var _arity = DropdownRadixLabel.GetComponentInParent<DragExpandTableComponent>().Arity;

        bool allConnected =false;
        switch (_arity)
        {
            case 1: //1 input and 1 output
                {
                    if ((Connections[0].endTerminal != null) && (Connections[3].endTerminal != null))
                        allConnected = true;
                }
                break;
            case 2: //1 input and 1 output
                {
                    if ((Connections[0].endTerminal != null) && (Connections[1].endTerminal != null) && (Connections[3].endTerminal != null))
                        allConnected = true;
                }
                break;
            case 3: //1 input and 1 output
                {
                    if ((Connections[0].endTerminal != null) && (Connections[1].endTerminal != null) && (Connections[2].endTerminal != null) && (Connections[3].endTerminal != null))
                        allConnected = true;
                }
                break;
        }

        if (allConnected)
        {
            //inputs
            int index0 = 0;
            int index1 = 0;
            int index2 = 0;
            int _output = 0;
            //get the correct matrix
            var matrices = GetComponentsInChildren<Matrix>();
         
            foreach (var m in matrices)
            {
                if (m.isActiveAndEnabled)
                {
                    _output = int.Parse(m.Truthtable[index0, index1, index2].label.text);

                    //reset all backgrounds
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

                    //highlight active cell
                    m.Truthtable[index0, index1, index2].label.color = panelColorActive;
                }
            }

            //if connection(s), propagate connection(s)
            if (Connections[3].endTerminal.Count > 0)
            {
                foreach (var c in Connections[3].endTerminal)
                {
                    //determine if logic gate or output 
                    if (tag.Equals("IO"))
                        c.GetComponentInParent<BtnInput>().SetValue(_output);
                    else
                    {
                        int index = 0;
                        switch (c.name)
                        {
                            case "A":
                                index = 0;
                                break;
                            case "B":
                                index = 1;
                                break;
                            case "C":
                                index = 2;
                                break;
                            default:
                                break;
                        }

                        c.GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput(index, _output);
                    }
                }
            }
        }
        else //report that one or more connections are missing
        { 
        
        }
    }
 
}

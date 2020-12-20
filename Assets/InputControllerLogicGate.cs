using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BtnInput;

public class InputControllerLogicGate : MonoBehaviour
{
    public TextMeshProUGUI DropDownFunctionLabel;
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

    public void ComputeTruthTableOutput(RadixOptions radixSource)
    {
        //check if all terminals are connected
        var _arity = GetComponentInChildren<DragExpandTableComponent>().Arity;
        bool allConnected =true;
        //switch (_arity)
        //{
        //    case 1: //1 input and 1 output
        //        {
        //            if ((Connections[0].endTerminal != null) && (Connections[3].endTerminal != null))
        //                allConnected = true;
        //        }
        //        break;
        //    case 2: //1 input and 1 output
        //        {
        //            if ((Connections[0].endTerminal != null) && (Connections[1].endTerminal != null) && (Connections[3].endTerminal != null))
        //                allConnected = true;
        //        }
        //        break;
        //    case 3: //1 input and 1 output
        //        {
        //            if ((Connections[0].endTerminal != null) && (Connections[1].endTerminal != null) && (Connections[2].endTerminal != null) && (Connections[3].endTerminal != null))
        //                allConnected = true;
        //        }
        //        break;
        //}

        if (allConnected)
        {
            //inputs converted to start at 0 since we use them as indices of matrices.
            int[] matrixIndices = new int[3];
            switch (radixSource)
            {
                case RadixOptions.Binary:
                case RadixOptions.UnbalancedTernary:
                    for (int i = 0; i < 3; i++)
                    {
                        if (i < _arity)
                            matrixIndices[i] = ProtectFromWrongInput(int.Parse(Connections[i].startTerminal.label.text));
                        else
                            matrixIndices[i] = 0;
                    }
                    break;
                case RadixOptions.BalancedTernary:
                    for (int i = 0; i < 3; i++)
                    {
                        if (i < _arity)
                            matrixIndices[i] = ConvertFromBalancedToUnbalanced(int.Parse(Connections[i].startTerminal.label.text));
                        else
                            matrixIndices[i] = 0;
                    }
                    break;
            }
          
            int _output = 0;
            //get the correct matrix
            //if we have arity 3, we first need to set the correct matrix based on the value
            if (_arity.Equals(3))
            {
                var dropdown = GetComponentInChildren<BtnInputTruthTableDropdown>().gameObject.GetComponent<TMP_Dropdown>();
                //set dropdown index based on found value;
                dropdown.value = matrixIndices[2];
            }

            var matrices = GetComponentsInChildren<Matrix>(); //refactor as it is alwasy 1 matrix now
         
            foreach (var m in matrices)
            {
                if (m.isActiveAndEnabled)
                {
                    var label = m.Truthtable[matrixIndices[0], matrixIndices[1], matrixIndices[2]].label.text;
                    if (label.Equals("x"))
                    {
                        //this should never happen, 2do generate a warning message
                        _output = 0;
                    }
                    else
                    {
                        _output = int.Parse(label);
                    }

                    //write output to hidden value, for spagetti linking to other logic gate (refactor)
                    var lcs = GetComponentsInChildren<BtnInput>();
                    lcs[lcs.Length - 1].label.text = _output.ToString();


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

                    //highlight active cell (only works for unbalanced ternary ATM)
                    m.Truthtable[matrixIndices[0], matrixIndices[1], matrixIndices[2]].label.color = panelColorActive;
                }
            }

            //if connection(s), propagate connection(s)
            if (Connections[3].endTerminal.Count > 0)
            {
                foreach (var c in Connections[3].endTerminal)
                {
                    //determine if logic gate or output 
                    if (c.tag.Equals("Output"))
                    {
                        c.GetComponentInParent<BtnInput>().SetValue(_output);
                    }
                    else
                    {
                        c.GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput(radixSource);
                    }
                }
            }
        }
        else //report that one or more connections are missing
        { 
        
        }
    }

    private int ConvertFromBalancedToUnbalanced(int v)
    {
        if (v.Equals("x"))
        {
            //this should never happen, 2do generate a warning message
            v = 0;
        }
       
        return v + 1;

    }

    private int ProtectFromWrongInput(int v)
    {
        if (v.Equals("x"))
        {
            //this should never happen, 2do generate a warning message
            v = 0;
        }

        return v;
    }
}

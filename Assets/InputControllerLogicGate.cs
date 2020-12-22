using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BtnInput;

public class InputControllerLogicGate : MonoBehaviour
{
    public TextMeshProUGUI DropDownFunctionLabel;
    TextMeshProUGUI _radixTarget; //or source if it is linked to a output
    public Connection[] Connections = new Connection[4]; //needed for data binding 
    public Color panelColorDefault;
    public Color panelColorActive;

    private void Awake()
    {
        for (int i = 0; i < Connections.Length; i++)
        {
            Connections[i] = new Connection();
        }

        _radixTarget = GetComponent<Matrix>().DropdownLabel;

    }

    public void ComputeTruthTableOutput()
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
            //For binary we use -1 and 1 IF and only if target radix is -1 and 1 then these are converted to 0 and 2. We should visuale this better
            int[] matrixIndices = new int[3];
          
            for (int i = 0; i < 3; i++)
            {
                if (i < _arity)
                    matrixIndices[i] = ConvertInputToTruthtableIndex(Connections[i].startTerminal);
                else
                    matrixIndices[i] = 0;
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
                        new NotImplementedException();
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
                        RadixOptions radixTarget = (RadixOptions)Enum.Parse(typeof(RadixOptions), _radixTarget.text, true);
                        c.GetComponentInParent<BtnInput>().SetValue(radixTarget, _output);
                    }
                    else
                    {
                        c.GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput();
                    }
                }
            }
        }
        else //report that one or more connections are missing
        {
            new NotImplementedException(); //should not happen
        }
    }

    private int ConvertInputToTruthtableIndex(BtnInput startTerminal)
    {
        //we could also do this whole conversation at btnInput and use _value which is an index. The only thing we need to catch are x's.
        string value = startTerminal.label.text;
        int output=0;
        RadixOptions radixSource = (RadixOptions)Enum.Parse(typeof(RadixOptions), startTerminal.DropdownLabel.text, true);
        RadixOptions radixTarget = (RadixOptions)Enum.Parse(typeof(RadixOptions), _radixTarget.text, true);

        if (value.Equals("x"))
        {
            new NotImplementedException();
        }
        else
        {
            output = int.Parse(value);

            switch (radixSource)
            {
                case RadixOptions.Binary:
                    {
                        if (radixTarget != RadixOptions.Binary) //from binary to binary we can use 0-1 range
                        {
                            if (output == 0)
                                output = 0;
                            else
                                output = 2;
                        }
                    }
                    break;
                case RadixOptions.UnbalancedTernary:
                    {
                        //values match table indices, no conversation needed
                    }
                    break;
                case RadixOptions.BalancedTernary:
                    {
                        output = output + 1;
                    }
                    break;
            }
        }

        return output;
    }

    private int ConvertFromBinaryToUnbalanced(string v)
    {
        int x = 0;
        if (_radixTarget.text.Equals("Binary"))
        {
            //do nothing source and target radix are binary
            if (v.Equals(x))
                new NotImplementedException();

            x = int.Parse(v);
        }
        else //convert 0's to -1
        {
            //do nothing source and target radix are binary
            if (v.Equals("x"))
                new NotImplementedException();

            x = int.Parse(v);

            if (x==1)
                x = 2;
            else
                x = 0;
        }
        
        return x;
    }

    private int ConvertFromBalancedToUnbalanced(string v)
    {
            int x = 0;
            if (v.Equals("x"))
                new NotImplementedException();
            else
                x = int.Parse(v) + 1;
        return x; //this is not real conversion from balanced to unbalanced ternary just a digit shift for the sake of using it as an index. Maybe use another system
    }

        private int CatchBadInput(string v)
        {
            int x = 0;
            if (v.Equals("x"))
                new NotImplementedException();
            else
                x = int.Parse(v);
            return x; //this is not real conversion from balanced to unbalanced ternary just a digit shift for the sake of using it as an index. Maybe use another system
        }
    }

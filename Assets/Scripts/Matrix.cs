using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Matrix : MonoBehaviour
{
    public BtnInputTruthTable[,,] Truthtable;
    public TextMeshProUGUI DropdownLabel;
    int _radix = 0;
   
    private void Awake()
    {
        ComputeEmptyTruthTable(2); //default arity is 2
    }

    public void ComputeEmptyTruthTable(int arity)
    {
        //fill truthtable
        switch (DropdownLabel.text)
        {
            case "BalancedTernary":
                {
                    _radix = 3;
                }
                break;
            case "UnbalancedTernary":
                {
                    _radix = 3;
                }
                break;
            case "Binary":
                {
                    _radix = 2;
                }
                break;
            default:
                break;
        }

        //Unity requires cells to be setActive to be found. So lets request current index, activate other indices, and after init, return to index;
        BtnInputTruthTableDropdown bittd;
        int currentIndex = 0;
        if (arity.Equals(3))
        {
            bittd = GetComponentInChildren<BtnInputTruthTableDropdown>();
            currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

            bittd.ActivateAll();
        }

        var cells = GetComponentsInChildren<BtnInputTruthTable>();
       
        switch (arity)
        {
            case 1: //radix,1-ary (r^1)
                {
                    CreateMatrix1ary(_radix, cells);
                }
                break;
            case 2: //radix,2-ary (r^2)
                {
                    CreateMatrix2ary(_radix, cells);
                }
                break;
            case 3: //radix,3-ary (r^3)
                {
                    CreateMatrix3ary(_radix, cells);
                }
                break;
            default:
                break;
        }

        if (arity.Equals(3))
        {
            bittd = GetComponentInChildren<BtnInputTruthTableDropdown>();
            bittd.DeActivateAll();
            bittd.Activate(currentIndex);
        }
    }

    public int ComputeArityFromTT()
    {
        int arity = 0;

        for (int i = 0; i < 3; i++)
        {
            if (Truthtable.GetLength(i) > 1)
            {
                arity++;
            }
        }

        return arity;
    }

    public int[] GetMatrixCells()
    {
        List<int> tt = new List<int>();
        var arity = ComputeArityFromTT();

        switch (arity)
        {
            case 1:
                {
                    for (int A = 0; A < _radix; A++)
                    {
                       tt.Add(Truthtable[A, 0, 0].GetValueAsMapped()); //note: we are map binary as 0:0 , 1:2 and bal. ternary as -1:0, 0:1, 1;2)
                    }
                }
                break;
            case 2:
                {
                    for (int A = 0; A < _radix; A++)
                    {
                        for (int B = 0; B < _radix; B++)
                        {
                            tt.Add(Truthtable[A, B, 0].GetValueAsMapped()); //note: we are map binary as 0:0 , 1:2 and bal. ternary as -1:0, 0:1, 1;2)
                        }
                    }
                }
                break;
            case 3:
                {
                    for (int A = 0; A < _radix; A++)
                    {
                        for (int B = 0; B < _radix; B++)
                        {
                            for (int C = 0; C < _radix; C++)
                            {
                                tt.Add(Truthtable[A, B, C].GetValueAsMapped()); //note: we are map binary as 0:0 , 1:2 and bal. ternary as -1:0, 0:1, 1;2)
                            }
                        }
                    }
                }
                break;
        }

        return tt.ToArray();
    }

    private void CreateMatrix1ary(int radix, BtnInputTruthTable[] cells)
    {
        Truthtable = new BtnInputTruthTable[radix, 1, 1];

        //A (row)is index 0
        for (int A = 0; A < radix; A++)
        {
            Truthtable[A, 0, 0] = cells[A];
        }
    }

    private void CreateMatrix2ary(int radix, BtnInputTruthTable[] cells)
    {
        Truthtable = new BtnInputTruthTable[radix, radix, 1];
        int counter = 0;

        //A (row) is index 0, B (column) is index 1
        for (int A = 0; A < radix; A++)
        {
            for (int B = 0; B < radix; B++)
            {
                Truthtable[A, B, 0] = cells[counter];
                counter++;
            }
        }
    }

    private void CreateMatrix3ary(int radix, BtnInputTruthTable[] cells)
    {
        Truthtable = new BtnInputTruthTable[radix, radix, radix];
        int counter = 0;

        //A (row) is index 0, B (column) is index 1, C (depth) is index 2
        for (int A = 0; A < radix; A++)
        {
            for (int B = 0; B < radix; B++)
            {
                for (int C = 0; C < radix; C++)
                {
                    Truthtable[A, B, C] = cells[counter];
                    counter++;
                }
            }
        }
    }
}

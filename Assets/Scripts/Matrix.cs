using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Matrix : MonoBehaviour
{
    public BtnInputTruthTable[,,] Truthtable;
    public TextMeshProUGUI DropdownLabel;

    private void Awake()
    {
        //get values from logicgate controller

        var arity = GetComponent<InputControllerLogicGate>().GetArity();

        var radix = 3;
        if (DropdownLabel.text.Contains("Binary"))
            radix = 2;
        
        ComputeEmptyTruthTable(arity, radix); //default arity is 2, radix 3
    }

    public void ComputeEmptyTruthTable(int arity, int radix)
    {
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
                    CreateMatrix1ary(radix, cells);
                }
                break;
            case 2: //radix,2-ary (r^2)
                {
                    CreateMatrix2ary(radix, cells);
                }
                break;
            case 3: //radix,3-ary (r^3)
                {
                    CreateMatrix3ary(radix, cells);
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
        //GET THE UNOPTIMIZED SO WITH DontCare X value

        //always return as 3x3 cells x arity, even when binary.
        List<int> tt = new List<int>();
        var arity = ComputeArityFromTT();

        bool isBinary = Truthtable.GetLength(0) == 2 ? true : false;

        if (isBinary)
        {
            if (arity == 3)
            {
                tt.Add(Truthtable[0, 0, 0].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(Truthtable[0, 1, 0].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(Truthtable[1, 0, 0].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(Truthtable[1, 1, 0].GetValueAsMapped());

                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x

            

                tt.Add(Truthtable[0, 0, 1].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(Truthtable[0, 1, 1].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(Truthtable[1, 0, 1].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(Truthtable[1, 1, 1].GetValueAsMapped());
            }

            if (arity == 2)
            {
                tt.Add(Truthtable[0, 0, 0].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(Truthtable[1, 0, 0].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(3); //x
                tt.Add(Truthtable[0, 1, 0].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(Truthtable[1, 1, 0].GetValueAsMapped());
            }

            if (arity == 1)
            {
                tt.Add(Truthtable[1, 0, 0].GetValueAsMapped());
                tt.Add(3); //x
                tt.Add(Truthtable[0, 0, 0].GetValueAsMapped());
            }
        }
        else
        {
            if (arity == 3)
            {
                tt.Add(Truthtable[0, 0, 0].GetValueAsMapped());
                tt.Add(Truthtable[0, 1, 0].GetValueAsMapped());
                tt.Add(Truthtable[0, 2, 0].GetValueAsMapped());
                tt.Add(Truthtable[1, 0, 0].GetValueAsMapped());
                tt.Add(Truthtable[1, 1, 0].GetValueAsMapped());
                tt.Add(Truthtable[1, 2, 0].GetValueAsMapped());
                tt.Add(Truthtable[2, 0, 0].GetValueAsMapped());
                tt.Add(Truthtable[2, 1, 0].GetValueAsMapped());
                tt.Add(Truthtable[2, 2, 0].GetValueAsMapped());

                tt.Add(Truthtable[0, 0, 1].GetValueAsMapped());
                tt.Add(Truthtable[0, 1, 1].GetValueAsMapped());
                tt.Add(Truthtable[0, 2, 1].GetValueAsMapped());
                tt.Add(Truthtable[1, 0, 1].GetValueAsMapped());
                tt.Add(Truthtable[1, 1, 1].GetValueAsMapped());
                tt.Add(Truthtable[1, 2, 1].GetValueAsMapped());
                tt.Add(Truthtable[2, 0, 1].GetValueAsMapped());
                tt.Add(Truthtable[2, 1, 1].GetValueAsMapped());
                tt.Add(Truthtable[2, 2, 1].GetValueAsMapped());

        

                tt.Add(Truthtable[0, 0, 2].GetValueAsMapped());
                tt.Add(Truthtable[0, 1, 2].GetValueAsMapped());
                tt.Add(Truthtable[0, 2, 2].GetValueAsMapped());
                tt.Add(Truthtable[1, 0, 2].GetValueAsMapped());
                tt.Add(Truthtable[1, 1, 2].GetValueAsMapped());
                tt.Add(Truthtable[1, 2, 2].GetValueAsMapped());
                tt.Add(Truthtable[2, 0, 2].GetValueAsMapped());
                tt.Add(Truthtable[2, 1, 2].GetValueAsMapped());
                tt.Add(Truthtable[2, 2, 2].GetValueAsMapped());

            }

            if (arity == 2)
            {
                tt.Add(Truthtable[0, 0, 0].GetValueAsMapped());
                tt.Add(Truthtable[0, 1, 0].GetValueAsMapped());
                tt.Add(Truthtable[0, 2, 0].GetValueAsMapped());
                tt.Add(Truthtable[1, 0, 0].GetValueAsMapped());
                tt.Add(Truthtable[1, 1, 0].GetValueAsMapped());
                tt.Add(Truthtable[1, 2, 0].GetValueAsMapped());
                tt.Add(Truthtable[2, 0, 0].GetValueAsMapped());
                tt.Add(Truthtable[2, 1, 0].GetValueAsMapped());
                tt.Add(Truthtable[2, 2, 0].GetValueAsMapped());
            }

            if (arity == 1)
            {
                tt.Add(Truthtable[2, 0, 0].GetValueAsMapped());
                tt.Add(Truthtable[1, 0, 0].GetValueAsMapped());
                tt.Add(Truthtable[0, 0, 0].GetValueAsMapped());
            }
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

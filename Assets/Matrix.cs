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
    int _nary = 0;
    private void Awake()
    {
        //fill truthtable
        switch (DropdownLabel.text)
        {
            case "Balanced Ternary":
                {
                    _radix = 3;
                }
                break;
            case "Unbalanced Ternary":
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

        var cells = GetComponentsInChildren<BtnInputTruthTable>();
        _nary = (int) Math.Log(cells.Length, _radix); 

        switch (_nary)
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
    }

    private void CreateMatrix1ary(int radix, BtnInputTruthTable[] cells)
    {
        Truthtable = new BtnInputTruthTable[radix, 1, 1];

        for (int i = 0; i < radix; i++)
        {
            Truthtable[i, 0, 0] = cells[i];
        }
    }

    private void CreateMatrix2ary(int radix, BtnInputTruthTable[] cells)
    {
        Truthtable = new BtnInputTruthTable[radix, radix, 1];
        int counter = 0;

        for (int i = 0; i < radix; i++)
        {
            for (int j = 0; j < radix; j++)
            {
                Truthtable[i, j, 0] = cells[counter];
                counter++;
            }
        }
    }

    private void CreateMatrix3ary(int radix, BtnInputTruthTable[] cells)
    {
        Truthtable = new BtnInputTruthTable[radix, radix, radix];
        int counter = 0;

        for (int i = 0; i < radix; i++)
        {
            for (int j = 0; j < radix; j++)
            {
                for (int k = 0; k < radix; k++)
                {
                    Truthtable[i, j, k] = cells[counter];
                    counter++;
                }
            }
        }
    }
}

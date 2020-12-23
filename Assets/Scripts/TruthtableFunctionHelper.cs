using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.InteropServices;

public class TruthtableFunctionHelper : MonoBehaviour
{
    TMP_Dropdown _Dropdown;
    DragExpandTableComponent _DETC;
    public enum TempFunctions { 
        sum00a,
        sum00b,
        carry00,
        sum10,
        sum11a,
        sum01,
        carry01,
        sum11b,
        carry11,
        carry21,
        sum21a,
        sum1a,
        sum1b,
        sum2a,
        sum2b,
        carry1,
        carry2,
        i8119,
    }

    [DllImport("FunctionGenerator")]
    public static extern int[] GetTableFromIndex(int tableIndex);


    public void Start()
    {
        //Fetch the Dropdown GameObject
        _Dropdown = GetComponent<TMP_Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        _Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(_Dropdown);
        });

        _DETC = transform.parent.GetComponentInChildren<DragExpandTableComponent>();
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        if (change.value != 0)
        {
            TempFunctions func = (TempFunctions)(change.value - 1);


            BtnInputTruthTable[] cells;
            BtnInputTruthTableDropdown bittd;
            int currentIndex;
            switch (func)
            {
                //we read a vector list row by row, so r by r

                
                case TempFunctions.sum00a:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "1";
                    cells[2].label.text = "1";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "1";
                    cells[6].label.text = "-1";
                    cells[7].label.text = "0";
                    cells[8].label.text = "0";
                    break;
                case TempFunctions.sum00b:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "-1";
                    cells[1].label.text = "0";
                    cells[2].label.text = "1";
                    cells[3].label.text = "0";
                    cells[4].label.text = "1";
                    cells[5].label.text = "-1";
                    cells[6].label.text = "0";
                    cells[7].label.text = "1";
                    cells[8].label.text = "-1";
                    break;
                case TempFunctions.sum01:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "-1";
                    cells[1].label.text = "0";
                    cells[2].label.text = "1";
                    cells[3].label.text = "-1";
                    cells[4].label.text = "0";
                    cells[5].label.text = "1";
                    cells[6].label.text = "1";
                    cells[7].label.text = "-1";
                    cells[8].label.text = "0";
                    break;
                case TempFunctions.carry01:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "0";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "0";
                    cells[6].label.text = "-1";
                    cells[7].label.text = "0";
                    cells[8].label.text = "0";
                    break;
                case TempFunctions.sum10:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "1";
                    cells[2].label.text = "1";
                    cells[3].label.text = "0";
                    cells[4].label.text = "1";
                    cells[5].label.text = "1";
                    cells[6].label.text = "1";
                    cells[7].label.text = "1";
                    cells[8].label.text = "-1";
                    break;
                case TempFunctions.sum11a:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "-1";
                    cells[1].label.text = "0";
                    cells[2].label.text = "1";
                    cells[3].label.text = "x";
                    cells[4].label.text = "x";
                    cells[5].label.text = "x";
                    cells[6].label.text = "0";
                    cells[7].label.text = "1";
                    cells[8].label.text = "-1";
                    break;
                case TempFunctions.sum11b:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "1";
                    cells[1].label.text = "-1";
                    cells[2].label.text = "0";
                    cells[3].label.text = "-1";
                    cells[4].label.text = "0";
                    cells[5].label.text = "1";
                    cells[6].label.text = "-1";
                    cells[7].label.text = "0";
                    cells[8].label.text = "1";
                    break;
                case TempFunctions.sum21a:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "-1";
                    cells[1].label.text = "x";
                    cells[2].label.text = "0";
                    cells[3].label.text = "0";
                    cells[4].label.text = "x";
                    cells[5].label.text = "+";
                    cells[6].label.text = "+";
                    cells[7].label.text = "x";
                    cells[8].label.text = "-1";
                    break;
                case TempFunctions.sum1a: //checked
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "-1";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "0";
                    cells[6].label.text = "1";
                    cells[7].label.text = "0";
                    cells[8].label.text = "0";
                    break;
                case TempFunctions.sum1b: //checked
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "-1";
                    cells[1].label.text = "0";
                    cells[2].label.text = "0";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "1";
                    cells[6].label.text = "1";
                    cells[7].label.text = "1";
                    cells[8].label.text = "-1";
                    break;
                case TempFunctions.sum2a: //checked
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "1";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "1";
                    cells[6].label.text = "1";
                    cells[7].label.text = "1";
                    cells[8].label.text = "-1";
                    break;
                case TempFunctions.sum2b: //checked
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "-1";
                    cells[1].label.text = "-1";
                    cells[2].label.text = "-1";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "-1";
                    cells[6].label.text = "1";
                    cells[7].label.text = "1";
                    cells[8].label.text = "-1";
                    break;
                case TempFunctions.carry21: //checked
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "0";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "0";
                    cells[6].label.text = "0";
                    cells[7].label.text = "0";
                    cells[8].label.text = "1";
                    break;
                case TempFunctions.carry00:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 3);

                    //get all the cells
                    bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                    currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                    bittd.ActivateAll();

                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "0";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "0";
                    cells[6].label.text = "0";
                    cells[7].label.text = "0";
                    cells[8].label.text = "0";
                    cells[9].label.text = "0";
                    cells[10].label.text = "1";
                    cells[11].label.text = "1";
                    cells[12].label.text = "0";
                    cells[13].label.text = "1";
                    cells[14].label.text = "1";
                    cells[15].label.text = "0";
                    cells[16].label.text = "0";
                    cells[17].label.text = "0";
                    cells[18].label.text = "0";
                    cells[19].label.text = "1";
                    cells[20].label.text = "1";
                    cells[21].label.text = "0";
                    cells[22].label.text = "1";
                    cells[23].label.text = "1";
                    cells[24].label.text = "0";
                    cells[25].label.text = "0";
                    cells[26].label.text = "0";

                    bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                    bittd.DeActivateAll();
                    bittd.Activate(currentIndex);

                    break;
                case TempFunctions.carry11:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 3);

                    //get all the cells
                    bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                    currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                    bittd.ActivateAll();

                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "-1";
                    cells[3].label.text = "0";
                    cells[6].label.text = "0";
                    cells[1].label.text = "x";
                    cells[4].label.text = "x";
                    cells[7].label.text = "x";
                    cells[2].label.text = "0";
                    cells[5].label.text = "0";
                    cells[8].label.text = "0";

                    cells[9].label.text = "0";
                    cells[12].label.text = "0";
                    cells[15].label.text = "0";
                    cells[10].label.text = "x";
                    cells[13].label.text = "x";
                    cells[16].label.text = "x";
                    cells[11].label.text = "0";
                    cells[14].label.text = "0";
                    cells[17].label.text = "1";

                    cells[18].label.text = "0";
                    cells[21].label.text = "0";
                    cells[24].label.text = "1";
                    cells[19].label.text = "x";
                    cells[22].label.text = "x";
                    cells[25].label.text = "x";
                    cells[20].label.text = "0";
                    cells[23].label.text = "1";
                    cells[26].label.text = "1";

                   

                    bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                    bittd.DeActivateAll();
                    bittd.Activate(currentIndex);
                    break;
                case TempFunctions.carry1:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 3);

                    //get all the cells
                    bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                    currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                    bittd.ActivateAll();

                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "0";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "0";
                    cells[6].label.text = "0";
                    cells[7].label.text = "0";
                    cells[8].label.text = "0";

                    cells[9].label.text = "0";
                    cells[10].label.text = "0";
                    cells[11].label.text = "0";
                    cells[12].label.text = "0";
                    cells[13].label.text = "0";
                    cells[14].label.text = "0";
                    cells[15].label.text = "0";
                    cells[16].label.text = "0";
                    cells[17].label.text = "0";

                    cells[18].label.text = "0";
                    cells[19].label.text = "0";
                    cells[20].label.text = "1";
                    cells[21].label.text = "0";
                    cells[22].label.text = "0";
                    cells[23].label.text = "0";
                    cells[24].label.text = "0";
                    cells[25].label.text = "0";
                    cells[26].label.text = "0";

                    bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                    bittd.DeActivateAll();
                    bittd.Activate(currentIndex);
                    break;
                case TempFunctions.carry2:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 3);

                    //get all the cells
                    bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                    currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                    bittd.ActivateAll();

                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "0";
                    cells[3].label.text = "0";
                    cells[4].label.text = "0";
                    cells[5].label.text = "0";
                    cells[6].label.text = "0";
                    cells[7].label.text = "0";
                    cells[8].label.text = "1";

                    cells[9].label.text = "0";
                    cells[10].label.text = "0";
                    cells[11].label.text = "0";
                    cells[12].label.text = "0";
                    cells[13].label.text = "0";
                    cells[14].label.text = "0";
                    cells[15].label.text = "0";
                    cells[16].label.text = "0";
                    cells[17].label.text = "1";

                    cells[18].label.text = "0";
                    cells[19].label.text = "0";
                    cells[20].label.text = "0";
                    cells[21].label.text = "0";
                    cells[22].label.text = "0";
                    cells[23].label.text = "0";
                    cells[24].label.text = "1";
                    cells[25].label.text = "1";
                    cells[26].label.text = "1";

                    bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                    bittd.DeActivateAll();
                    bittd.Activate(currentIndex);
                    break;

                //this is a new style of adding TTs using Halvors indexing function. We should refactor the older ones to this new style
                case TempFunctions.i8119: 
                    //Balanced Distance Compare(Engdal): 8119 [102010201]\nGives the distance between the two values. 1 is no distance. 0 is a distance of one. 2 is a distance of two. Designed for use with balanced ternary.
                    //Unbalanced Distance Compare: 3936[012101210]\nAn altered version of Engdal's compare function. Gives the distance between the two values. In unbalanced ternary, it outputs the distance value.
                    //Size Compare: 7153 [100210221]\nGives which input has the highest value. An output of 0 means the first input has the bigger value, 2 is the second input. Identical values gives 1.
                    //Max or Or: 19569 [222211210]\nOutputs the highest value of the two inputs.
                    //Min or And: 16362 [211110000]\nIf BOTH inputs are 2, output is 2. If ANY input is 0, output is 0. Else, output is 1.
                    //Antimax or Nor: 15633 [210110000]\nOutputs the lowest value of the two inputs.
                    //Antimin or Nand: 4049 [012112222]If BOTH inputs are 2, output is 0. If ANY input is 0, output is 2. Else, output is 1.
                    //Xor: 4017 [012111210]\nExcluding the middle value, this functions the same way as binary XOR. If any input is 1, output is 1. If the inputs are opposites, the output is 2. If they are identical, output is 0.
                    //Balanced Sum: 5681 [021210102]\nAdds the two balanced ternary inputs together.
                    //Unbalanced Sum: 8229 [102021210]\nAdds the two unbalanced ternary inputs together.
                    //Consensus: 16401 [211111110]\nIf both inputs are 0, output is 0. If both are 2, output is 2. Else, the output is 1.
                    //Accept Anything: 18801 [221210100]\n Similar to consensus, except that if one of the outputs are 1, the other will decide the output. If both are 1, output is 1.
                    //Equality comparison: 13286 [200020002]\n If the inputs are equal, output is 2. Else, output is 0.

                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //2d0: https://stackoverflow.com/questions/3776485/marshal-c-int-array-to-c-sharp
                    int[] ttMatrix = GetTableFromIndex(8119);
                    

                    //fill in the cells
                    for (int i = 0; i < 9; i++)
                    {
                        cells[i].label.text = ttMatrix[i].ToString();
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
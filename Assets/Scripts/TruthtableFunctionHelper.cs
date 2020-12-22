using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
        sum21a,
        sum1a,
        sum1b,
        sum2a,
        sum2b,
        carry1,
        carry2
    }

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
                    cells[8].label.text = "1";
                    break;
                case TempFunctions.sum11a:
                    //switch to correct size
                    _DETC.SetPanelSize(3, 2);

                    //get all the cells
                    cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                    //fill in the cells
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "1";
                    cells[3].label.text = "1";
                    cells[4].label.text = "1";
                    cells[5].label.text = "-1";
                    cells[6].label.text = "1";
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
                    cells[0].label.text = "0";
                    cells[1].label.text = "0";
                    cells[2].label.text = "0";
                    cells[3].label.text = "0";
                    cells[4].label.text = "1";
                    cells[5].label.text = "1";
                    cells[6].label.text = "1";
                    cells[7].label.text = "-1";
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
                    cells[0].label.text = "1";
                    cells[3].label.text = "2";
                    cells[6].label.text = "3";
                    cells[1].label.text = "4";
                    cells[4].label.text = "5";
                    cells[7].label.text = "6";
                    cells[2].label.text = "7";
                    cells[5].label.text = "8";
                    cells[8].label.text = "9";

                    cells[9].label.text = "10";
                    cells[12].label.text = "11";
                    cells[15].label.text = "12";
                    cells[10].label.text = "13";
                    cells[13].label.text = "14";
                    cells[16].label.text = "15";
                    cells[11].label.text = "16";
                    cells[14].label.text = "17";
                    cells[17].label.text = "18";

                    cells[18].label.text = "19";
                    cells[21].label.text = "20";
                    cells[24].label.text = "21";
                    cells[19].label.text = "22";
                    cells[22].label.text = "23";
                    cells[25].label.text = "24";
                    cells[20].label.text = "25";
                    cells[23].label.text = "26";
                    cells[26].label.text = "27";

                   

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
                default:
                    break;
            }
        }
    }

}

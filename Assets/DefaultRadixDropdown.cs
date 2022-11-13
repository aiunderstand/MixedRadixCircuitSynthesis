using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BtnInput;
using System;

public class DefaultRadixDropdown : MonoBehaviour
{
    TMP_Dropdown defaultRadixDropDown;

    public void Start()
    {
        defaultRadixDropDown = GetComponent<TMP_Dropdown>();
    }
    public void UpdateDefaultRadix()
    {
        //convert defaultRadixDropdown to RadixOptions
        RadixOptions defaultRadix = (RadixOptions)Enum.Parse(typeof(RadixOptions), defaultRadixDropDown.options[defaultRadixDropDown.value].text, true);

        //set new default in prefab
        CircuitGenerator.InputPrefab.GetComponent<DragDrop>().FullVersion.GetComponentInChildren<InputController>().DropdownLabel.transform.parent.GetComponent<TMP_Dropdown>().value = defaultRadixDropDown.value;
        CircuitGenerator.OutputPrefab.GetComponent<DragDrop>().FullVersion.GetComponentInChildren<InputController>().DropdownLabel.transform.parent.GetComponent<TMP_Dropdown>().value = defaultRadixDropDown.value;
        CircuitGenerator.ClockPrefab.GetComponent<DragDrop>().FullVersion.GetComponentInChildren<InputController>().DropdownLabel.transform.parent.GetComponent<TMP_Dropdown>().value = defaultRadixDropDown.value;
        CircuitGenerator.LogicGatePrefab.GetComponent<DragDrop>().FullVersion.GetComponentInChildren<TruthTableDropDown>().GetComponent<TMP_Dropdown>().value = defaultRadixDropDown.value;



        //remove prefabs in menu and regenerate menu items with new prefabs
        Settings Settings = FindObjectOfType<Settings>();
        Settings.GenerateMenuItems();

    }
    
}

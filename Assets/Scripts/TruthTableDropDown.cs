using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BtnInput;
using System;

public class TruthTableDropDown : MonoBehaviour
{
    TMP_Dropdown _Dropdown;
    InputControllerLogicGate iclg;
    DragExpandTableComponent expandController;
    // Start is called before the first frame update
    void Start()
    {
        iclg = transform.parent.GetComponent<InputControllerLogicGate>();
        expandController = iclg.GetComponentInChildren<DragExpandTableComponent>();

        //Fetch the Dropdown GameObject
        _Dropdown = GetComponent<TMP_Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        _Dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(_Dropdown);
        });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        string radixTarget = change.options[change.value].text;
        int radixSymbols = 3;

        if (radixTarget.Contains("Binary"))
            radixSymbols = 2;
       
        //reset the index dropdown to custom
        iclg.DropDownFunctionLabel.text = "";

        //DragExpandTableComponent to set the radix target
        expandController.SetPanelSize(radixSymbols, iclg.GetArity());

        //update labels if ternary and convert matrix values
        if (radixSymbols == 3)
        {
            if (radixTarget.Contains("Unbal"))
            {
                iclg.activeIC.UpdateLabels(RadixOptions.UnbalancedTernary, iclg.GetArity());
                iclg.activeIC.ConvertMatrix(iclg.GetRadix(), RadixOptions.UnbalancedTernary);
            }
            else
            {
                iclg.activeIC.UpdateLabels(RadixOptions.BalancedTernary, iclg.GetArity());
                iclg.activeIC.ConvertMatrix(iclg.GetRadix(), RadixOptions.BalancedTernary);
            }
        }

        //update radix in iclg
        iclg.SetRadix((RadixOptions)Enum.Parse(typeof(RadixOptions), radixTarget, true));
    }
}

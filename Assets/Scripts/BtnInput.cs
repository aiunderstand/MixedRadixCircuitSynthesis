using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static InputController;

public class BtnInput : MonoBehaviour
{
    int _value = 0;
    public TextMeshProUGUI DropdownLabel;
    int _minValue = 0;
    int _maxValue = 0;
    public TextMeshProUGUI label;
    public int _portIndex = 0;
    //refactors as Logic gate doesnt have any onclick events, only used for its references to value.

    public enum RadixOptions { 
        BalancedTernary,
        UnbalancedTernary,
        Binary
    }

    public void OnClick()
    {
        RadixOptions radixSource = (RadixOptions) Enum.Parse(typeof(RadixOptions), DropdownLabel.text, true);
        switch (radixSource)
        {
            case  RadixOptions.BalancedTernary:
                {
                    _minValue = -1;
                    _maxValue = 1;
                }
                break;
            case RadixOptions.UnbalancedTernary:
                {
                    _minValue = 0;
                    _maxValue = 2;
                }
                break;
            case RadixOptions.Binary:
                {
                    _minValue = 0;
                    _maxValue = 1;
                }
                break;
            default:
                break;
        }

        _value++;
        
        if (_value > _maxValue)
            _value = _minValue;

        label.text = _value.ToString();

        //report back to counter to recount, which is the parent of this object;
        var ic = gameObject.GetComponentInParent<InputController>();
        ic.ComputeCounter();

        //update connect
        //go over connections and update next component, this code is duplicated in inputcontrollerlogicgate
        int index = int.Parse(this.transform.parent.name);
        if (ic.Connections[index].endTerminal.Count > 0)
        {
            foreach (var c in ic.Connections[index].endTerminal)
            {
                //determine if logic gate or output 
                if (c.tag.Equals("Output"))
                {
                    c.GetComponentInParent<BtnInput>().SetValue(_value);
                }
                else
                {
                    c.GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput(radixSource);
                }
            }
        }
    } 

    public void SetValue(int value)
    {
        switch (DropdownLabel.text)
        {
            case "BalancedTernary":
                {
                    _minValue = -1;
                    _maxValue = 1;
                }
                break;
            case "UnbalancedTernary":
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

        label.text = _value.ToString();

        //report back to counter to recount, which is the parent of this object;
        var ic = gameObject.GetComponentInParent<InputController>();
        ic.ComputeCounter();
    }
}

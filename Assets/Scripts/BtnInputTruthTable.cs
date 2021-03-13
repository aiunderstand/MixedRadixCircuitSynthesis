using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BtnInput;
using static InputController;

public class BtnInputTruthTable : MonoBehaviour
{
    int _value = 0;
    public TextMeshProUGUI DropdownLabel;
    int _minValue = 0;
    int _maxValue = 0;
    public TextMeshProUGUI label;
   
    public void OnClick()
    {
        switch (DropdownLabel.text)
        {
            case "BalancedTernary":
                {
                    _minValue = -1;
                    _maxValue = 2;

                    _value++;

                    if (_value > _maxValue)
                        _value = _minValue;

                }
                break;
            case "UnbalancedTernary":
                {
                    _minValue = 0;
                    _maxValue = 3;

                    _value++;

                    if (_value > _maxValue)
                        _value = _minValue;

                }
                break;
            case "Binary":
            case "SignedBinary":
                {
                    _minValue = 0;
                    _maxValue = 3;

                    switch (_value) {
                        case 0:
                            _value = 1;
                            break;
                        case 1: 
                            _value = 3; // is x
                            break;
                        case 3:
                            _value = 0;
                            break;
                    }
                }
                break;
            default:
                break;
        }

       
        if (_value == _maxValue)
            label.text = "x";
        else
            label.text = _value.ToString();
    }

    public int GetValueAsMapped()
    {
        //NOTE: we want use Dont Care values (marked as "x"). We label them as value 3 and decode them in the c++ function. 
        //NOTE: targetradix is always unbalanced;
       
        int outputValue = 0;
        RadixOptions radixSource = (RadixOptions)Enum.Parse(typeof(RadixOptions), DropdownLabel.text, true);
        switch (radixSource)
        {
            case RadixOptions.BalancedTernary:
                {
                    if (label.text == "x")
                        outputValue = 3;
                    else
                        outputValue = int.Parse(label.text) + 1;
                }
                break;
            case RadixOptions.UnbalancedTernary: 
                {
                    if (label.text == "x")
                        outputValue = 3;
                    else
                        outputValue = int.Parse(label.text);
                }
                break;
            case RadixOptions.Binary:
            case RadixOptions.SignedBinary:
                {
                    if (label.text == "x")
                        outputValue = 3;
                  
                    if (int.Parse(label.text) == 1)
                        outputValue = 2; //this is the high value;
                    else
                        outputValue = 0;
                }
                break;
        }

        return outputValue;
    }
}

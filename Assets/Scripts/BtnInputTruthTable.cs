using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
                {
                    _minValue = 0;
                    _maxValue = 2;

                    switch (_value) {
                        case 0:
                            _value = 1;
                            break;
                        case 1: 
                            _value = 2;
                            break;
                        case 2:
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
}

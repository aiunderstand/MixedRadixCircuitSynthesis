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
            case "Balanced Ternary":
                {
                    _minValue = -1;
                    _maxValue = 2;
                }
                break;
            case "Unbalanced Ternary":
                {
                    _minValue = 0;
                    _maxValue = 3;
                }
                break;
            case "Binary":
                {
                    _minValue = 0;
                    _maxValue = 2;
                }
                break;
            default:
                break;
        }

        _value++;

        if (_value > _maxValue)
            _value = _minValue;

        if (_value == _maxValue)
            label.text = "x";
        else
            label.text = _value.ToString();
    }
}

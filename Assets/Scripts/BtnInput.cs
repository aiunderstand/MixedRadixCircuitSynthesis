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

    public void OnClick()
    {
        switch (DropdownLabel.text)
        {
            case "Balanced Ternary":
                {
                    _minValue = -1;
                    _maxValue = 1;
                }
                break;
            case "Unbalanced Ternary":
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

        _value++;
        
        if (_value > _maxValue)
            _value = _minValue;

        label.text = _value.ToString();

        //report back to counter to recount, which is the parent of this object;
        gameObject.GetComponentInParent<InputController>().ComputeCounter();
    }
}

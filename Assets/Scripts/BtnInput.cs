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
        var ic = gameObject.GetComponentInParent<InputController>();
        ic.ComputeCounter();

        //update connect
        //go over connections and update next component,
        if (ic.Connections[0].endTerminal.Count > 0)
        {
            foreach (var c in ic.Connections[0].endTerminal)
            {
                //determine if logic gate or output 
                if (c.tag.Equals("IO"))
                {
                    c.GetComponentInParent<BtnInput>().SetValue(_value);
                }
                else
                {
                    int index = 0;
                    switch (c.name)
                    {
                        case "A":
                            index = 0;
                            break;
                        case "B":
                            index = 1;
                            break;
                        case "C":
                            index = 2;
                            break;
                        default:
                            break;
                    }

                    c.GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput(index, _value);
                }
            }
        }
    }

    public void SetValue(int value)
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

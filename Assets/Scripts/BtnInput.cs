﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InputController;

public class BtnInput : MonoBehaviour
{
    int _value = 0;
    public TextMeshProUGUI DropdownLabel; //redundant as we also have a dropdown
    public Image wireColor; 
    Color _colorTernary = new Color(255,0,211); 
    Color _colorBinary = new Color(0, 214, 255);
    int _minValue = 0;
    int _maxValue = 0;
    public TextMeshProUGUI label;
    public int _portIndex = 0;
    TMP_Dropdown _Dropdown;
    //refactors as Logic gate doesnt have any onclick events, only used for its references to value.

    public enum RadixOptions { 
        BalancedTernary,
        UnbalancedTernary,
        Binary
    }

    public void Start()
    {
        if (DropdownLabel != null)
        {
            //Fetch the Dropdown GameObject
            _Dropdown = DropdownLabel.transform.parent.GetComponent<TMP_Dropdown>();
            //Add listener for when the value of the Dropdown changes, to take action
            _Dropdown.onValueChanged.AddListener(delegate
            {
                DropdownValueChanged(_Dropdown);
            });
        }
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        if (wireColor != null)
        {
            if (change.options[change.value].text.Contains("Ter"))
                wireColor.color = _colorTernary;
            else
                wireColor.color = _colorBinary;
        }
     }

    private void OnEnable()
    {
        SetWireColor();
    }


    public void SetWireColor()
    {
        if (DropdownLabel != null && wireColor != null)
        {
            if (
                DropdownLabel.text.Contains("Ter"))
                wireColor.color = _colorTernary;
            else
                wireColor.color = _colorBinary;
        }
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

                    _value++;

                    if (_value > _maxValue)
                        _value = _minValue;
                }
                break;
            case RadixOptions.UnbalancedTernary:
                {
                    _minValue = 0;
                    _maxValue = 2;

                    _value++;

                    if (_value > _maxValue)
                        _value = _minValue;
                }
                break;
            case RadixOptions.Binary:
                {
                    _minValue = 0;
                    _maxValue = 1;

                    if (_value == 0)
                        _value = 1;
                    else
                        _value = 0;
                }
                break;
            default:
                break;
        }

       

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
                    c.GetComponentInParent<BtnInput>().SetValue(radixSource, _value);
                }
                else
                {
                    c.GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput();
                }
            }
        }
    } 

    public void SetValue(RadixOptions radixSource, int value)
    {
        RadixOptions radixTarget = (RadixOptions)Enum.Parse(typeof(RadixOptions), DropdownLabel.text, true);
       
        switch (radixSource)
        {
            case RadixOptions.Binary:
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                //no conversation needed
                                _value = value;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                //convert binary 1 to unbalanced 2
                                if (value == 1)
                                    _value = 2;
                                else
                                    _value = value;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                //convert binary 0 to balanced -1
                                if (value == 0)
                                    _value = -1;
                                else
                                    _value = value;
                            }
                            break;
                    }
                }
                break;
            case RadixOptions.UnbalancedTernary:
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                //convert unbalanced 2 to binary 1
                                if (value == 2)
                                    _value = 1;
                                else
                                    _value = value;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                //do nothing
                                _value = value;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                //shift everything with -1
                                _value = value -1;
                            }
                            break;
                    }
                }
                break;
            case RadixOptions.BalancedTernary:
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                //convert balanced -1 to binary 0
                                if (value == -1)
                                    _value = 0;
                                else
                                    _value = value;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                //convert balanced -1 to unbalanced by shifting +1;
                                _value = value+1;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                //do nothing
                                _value = value;
                            }
                            break;
                    }
                }
                break;
        }

        label.text = _value.ToString();

        //report back to counter to recount, which is the parent of this object;
        var ic = gameObject.GetComponentInParent<InputController>();
        ic.ComputeCounter();
    }
}
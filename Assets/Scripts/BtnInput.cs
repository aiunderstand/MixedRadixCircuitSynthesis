using ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InputController;

//We should rename BtnInput to Port as it evolved more into that
public class BtnInput : MonoBehaviour
{
    int _value = 0;
    public string _radix = "";
    public TextMeshProUGUI DropdownLabel; //redundant as we also have a dropdown

    public void RemoveConnection(int id)
    {
        int index = 0;
        for (int i = 0; i < Connections.Count; i++)
        {
            if (Connections[i].connection.id == id)
                index = i;
        }

        Connections.RemoveAt(index);
    }

    public void RemoveAllConnections()
    {
        while (Connections.Count > 0)
            Connections[0].DestroyConnection();
    }

    public Image wireColor; 
    public static Color _colorTernary = new Color(255,0,211); 
    public static Color _colorBinary = new Color(0, 214, 255);
    int _minValue = 0;
    int _maxValue = 0;
    public TextMeshProUGUI label;
    public int _portIndex = 0;
    public TMP_Dropdown _Dropdown;
    public bool isOutput = false; //duplicate because we also set this in lineController, refactor
    public List<LineFunctions> Connections = new List<LineFunctions>(); 

    //refactors as Logic gate doesnt have any onclick events, only used for its references to value.
    
    public enum RadixOptions { 
        BalancedTernary,
        UnbalancedTernary,
        Binary
    }

    public void Start()
    {
        Init();
    }

    public void Init()
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

            SetWireColor();
        }
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        if (wireColor != null)
        {
            if (change.options[change.value].text.Contains("Ter"))
            {
                wireColor.color = _colorTernary;
                _radix = change.options[change.value].text;
                
                //only change line color if not output
                if (!tag.Equals("Output"))
                {
                    foreach (var c in Connections)
                    {
                        c.Redraw(_colorTernary, true);
                    }
                }                
            }
            else
            {
                wireColor.color = _colorBinary;
                _radix = _radix = change.options[change.value].text;

                //only change line color if not output
                if (!tag.Equals("Output"))
                {
                    foreach (var c in Connections)
                    {
                        c.Redraw(_colorBinary, true);
                    }
                }
            }            
        }

        if (tag.Equals("Output"))
        {
            //have a look at connection (there can be max 1 connection to it
            if (Connections.Count > 0)
            {
                var start = Connections[0].connection.startTerminal;
                RadixOptions radixSource = (RadixOptions)Enum.Parse(typeof(RadixOptions), start._radix, true);                
                SetValue(radixSource, start._value, false);
            }
            else
            {
                SetValue(RadixOptions.Binary, 0, false); //set everything to zero, radix source is irrelevant
            }
        }
        else
        {
            Reset();
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

    public int GetValueAsIndex(RadixOptions radixTarget)
    {
        int outputValue = 0;
        RadixOptions radixSource = (RadixOptions)Enum.Parse(typeof(RadixOptions), DropdownLabel.text, true);
        switch (radixSource)
        {
            case RadixOptions.BalancedTernary: //from -1,0,1 to radix 2 (0,1) or to radix 3(0,1,2)
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                if (_value  <1)
                                    outputValue = 0;
                                else
                                    outputValue = 1;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                outputValue = _value + 1;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                outputValue = _value + 1;
                            }
                            break;

                    }

                }
                break;
            case RadixOptions.UnbalancedTernary: //from -1,0,1 to radix 2 (0,1) or to radix 3(0,1,2)
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                if (_value < 2)
                                    outputValue = 0;
                                else
                                    outputValue = 1;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                outputValue = _value;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                outputValue = _value;
                            }
                            break;

                    }

                }
                break;
            case RadixOptions.Binary: //from -1,0,1 to radix 2 (0,1) or to radix 3(0,1,2)
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                outputValue = _value;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                if (_value == 0)
                                    outputValue = 0;
                                else
                                    outputValue = 2;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                if (_value == 0)
                                    outputValue = 0;
                                else
                                    outputValue = 2;
                            }
                            break;
                    }
                }
                break;
        }

        return outputValue;
    }

    public void SetValueWithoutConversionAndCounter(int output)
    {
        _value = output;
        label.text = _value.ToString();
    }

    public void Reset()
    {
        SetValue(RadixOptions.Binary, 0, true); //set everything to zero, radix source is irrelevant
    }
    public void OnClick(int amount)
    {
      
        RadixOptions radixSource = (RadixOptions) Enum.Parse(typeof(RadixOptions), DropdownLabel.text, true);

        //this function is not a conversion, but rather updates the value allowing cyclic behavior
        switch (radixSource)
        {
            case  RadixOptions.BalancedTernary:
                {
                    _minValue = -1;
                    _maxValue = 1;

                    _value+= amount;

                    if (_value > _maxValue)
                        _value = _minValue;

                    if (_value < _minValue)
                        _value = _maxValue;
                }
                break;
            case RadixOptions.UnbalancedTernary:
                {
                    _minValue = 0;
                    _maxValue = 2;

                    _value += amount;

                    if (_value > _maxValue)
                        _value = _minValue;

                    if (_value < _minValue)
                        _value = _maxValue;
                }
                break;
            case RadixOptions.Binary:
                {
                    _minValue = 0;
                    _maxValue = 1;

                    //no need to look at amount, just toggle
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
        if (Connections.Count > 0)
        {
            foreach (var c in Connections)
            {
                //determine if logic gate or output 
                if (c.connection.endTerminal.tag.Equals("Output"))
                {
                    var val = c.connection.endTerminal.GetComponentInParent<BtnInput>().SetValue(radixSource, _value,false);
                    c.connection.endTerminal.GetComponentInChildren<LEDtoggle>().SetLedColor(val);
                }
                else
                {
                    if (c.connection.endTerminal.name.Contains("_saved"))
                    {
                        c.connection.endTerminal.GetComponentInParent<InputController>().ComputeSavedComponentOutput();
                    }
                    else
                    {
                        c.connection.endTerminal.GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput();
                    }
                }
            }
        }
    } 

    //horrible api: set, transform and then get in one
    public int SetValue(RadixOptions radixSource, int value, bool withPropagation)
    {
        RadixOptions radixTarget = (RadixOptions)Enum.Parse(typeof(RadixOptions), DropdownLabel.text, true);

        _value = RadixHelper.ConvertRadixFromTo(radixSource, radixTarget, value);

        label.text = _value.ToString();

        //report back to counter to recount, which is the parent of this object;
        var ic = gameObject.GetComponentInParent<InputController>();
        if (ic != null)
        {
            ic.ComputeCounter();

            if (withPropagation)
            {
                //update connect
                //go over connections and update next component, this code is duplicated in inputcontrollerlogicgate
                int index = int.Parse(this.name);
                if (Connections.Count > 0)
                {
                    foreach (var c in Connections)
                    {
                        //determine if logic gate or output 
                        if (c.connection.endTerminal.tag.Equals("Output"))
                        {
                            c.connection.endTerminal.GetComponentInParent<BtnInput>().SetValue(radixSource, _value, false);
                        }
                        else
                        {
                            if (c.connection.endTerminal.name.Contains("_saved"))
                            {
                                c.connection.endTerminal.GetComponentInParent<InputController>().ComputeSavedComponentOutput();
                            }
                            else
                            {
                                c.connection.endTerminal.GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput();
                            }
                        }
                    }
                }
            }
        }
        return _value;
    }
}

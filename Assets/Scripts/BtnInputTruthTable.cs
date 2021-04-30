using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BtnInput;
using static InputController;
using UnityEngine.UI;

public class BtnInputTruthTable : MonoBehaviour
{
    int _value = 0;
    public TextMeshProUGUI DropdownLabel;
    int _minValue = 0;
    int _maxValue = 0;
    public TextMeshProUGUI label;
     Button _button;  

    public void Awake()
    {
        _button = GetComponent<Button>();
    }

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

        //propagate via inputcontrollerlogicgate
        this.transform.parent.parent.GetComponent<InputControllerLogicGate>().ComputeTruthTableOutput();
    }

    public void SetHeatmapColor(int frequency, int total)
    {
        if (frequency != 0)
        {
            //white R 255, G 255, B 255 = not used
            //yellow R 255, G 255, B 0 = medium used
            //red R 255, G 0, B 0 = heavy used
            float green = 255;
            float blue = 200;
            
            if (frequency > 1)
            {
                green = 255;
                blue = 150;
            }

            if (frequency > 2)
            {
                green = 255;
                blue = 100;
            }

            if (frequency > 4)
            {
                green = 255;
                blue = 50;
            }

            if (frequency > 8)
            {
                green = 255;
                blue = 0;
            }

            if (frequency > 16)
            {
                green = 200;
                blue = 0;
            }

            if (frequency > 32)
            {
                green = 150;
                blue = 0;
            }


            if (frequency > 64)
            {
                green = 100;
                blue = 0;
            }


            if (frequency > 128)
            {
                green = 50;
                blue = 0;
            }

            if (frequency > 256)
            {
                green = 0;
                blue = 0;
            }

            //float relativeF = (float)frequency / (float) total;

            //if (relativeF > 0.90)
            //{
            //    green = 0;
            //    blue = 0;
            //}

            //if (relativeF > 0.80)
            //{
            //    green = 50;
            //    blue = 0;
            //}

            //if (relativeF > 0.70)
            //{
            //    green = 100;
            //    blue = 0;
            //}

            //if (relativeF > 0.60)
            //{
            //    green = 150;
            //    blue = 0;
            //}

            //if (relativeF > 0.50)
            //{
            //    green = 200;
            //    blue = 0;
            //}

            //if (relativeF > 0.40)
            //{
            //    green = 255;
            //    blue = 0;
            //}

            //if (relativeF > 0.30)
            //{
            //    green = 255;
            //    blue = 50;
            //}

            //if (relativeF > 0.20)
            //{
            //    green = 255;
            //    blue = 100;
            //}

            //if (relativeF > 0.10)
            //{
            //    green = 255;
            //    blue = 150;
            //}

            //if (relativeF > 0.00)
            //{
            //    green = 255;
            //    blue = 200;
            //}

            //if (frequency + offset < 256)
            //{
            //    blue = 255 - frequency - offset;
            //}
            //else
            //{
            //    if (frequency + offset < 512)
            //    {
            //        blue = 0;
            //        green = 255 - (frequency - 255 - offset);
            //    }
            //    else
            //    {
            //        blue = 0;
            //        green = 0;
            //    }
            //}

            var c = _button.colors;
             c.normalColor = new Color(1, (float)green / 255f, (float)blue / 255f);
            _button.colors = c;
        }
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

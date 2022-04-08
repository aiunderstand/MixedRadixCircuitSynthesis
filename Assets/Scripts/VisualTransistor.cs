using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class VisualTransistor : MonoBehaviour
{
    public enum TransistorTypes
        {
        NMOS_BodyToSource,
        PMOS_BodyToSource,
        Empty
    }

    Color low = Color.red;     //diameter 10 = 0.783nm
    Color middle = Color.green; //diameter 13 = 1.018nm
    Color high = Color.blue;     //diameter 19 = 1.487nm
    public TextMeshProUGUI label;
    public string origLabel;
    int diameter;
    TransistorTypes type;

    public void SetTransistorTypeTo(TransistorTypes type, int diameter)
    {
        this.type = type;
        this.diameter = diameter;
        switch (type)
        {
            case TransistorTypes.Empty:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/EmptyTransistor");
                break;
            case TransistorTypes.PMOS_BodyToSource:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/PMOS_BodyToSource");
                break;
            case TransistorTypes.NMOS_BodyToSource:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NMOS_BodyToSource");
                break;
        }
        
        Color c = Color.white;
        switch (this.diameter)
        {
            case 10:
                c = low;
                break;
            case 13:
                c = middle;
                break;
            case 19:
                c = high;
                break;
            default:                
                break;
        }

        GetComponent<Image>().color = c;
    }


    public TransistorTypes GetTransistorType()
    {
        return type;
    }

    public int GetDiameter()
    {
        return diameter;
    }

    public void SetActivationLevel(bool isOn)
    {
        float alpha = .1f;
        if (isOn)
            alpha = 1f;

        Color c = Color.white;
        switch (this.diameter)
        {
            case 10:
                c = new Color(low.r, low.g, low.b, alpha);
                break;
            case 13:
                c = new Color(middle.r, middle.g, middle.b, alpha);
                break;
            case 19:
                c = new Color(high.r, high.g, high.b, alpha);
                break;
            default:
                break;
        }

        GetComponent<Image>().color = c;
    }
}

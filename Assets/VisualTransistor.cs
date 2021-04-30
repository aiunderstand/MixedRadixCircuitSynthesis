using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VisualTransistor : MonoBehaviour
{
    public enum TransistorTypes
        {
        NMOS_BodyToDrain,
        PMOS_BodyToSource,
        Empty
    }

    Color low = Color.red;     //diameter 10 = 0.783nm
    Color middle = Color.green; //diameter 13 = 1.018nm
    Color high = Color.blue;     //diameter 19 = 1.487nm
    public TextMeshProUGUI label;
    TransistorTypes type;

    public void SetTransistorTypeTo(TransistorTypes type, int diameter)
    {
        this.type = type;

        switch (type)
        {
            case TransistorTypes.Empty:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/EmptyTransistor");
                break;
            case TransistorTypes.PMOS_BodyToSource:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/PMOS_BodyToSource");
                break;
            case TransistorTypes.NMOS_BodyToDrain:
                GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NMOS_BodyToSource");
                break;
        }
        
        Color c = Color.white;
        switch (diameter)
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
}

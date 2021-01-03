using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputController : MonoBehaviour
{
   public enum RadixTypes
    {
        balanced_ternary,
        unbalanced_ternary,
        binary
    }

    public List<GameObject> Buttons = new List<GameObject>();
    public TextMeshProUGUI CounterLabel;
    public TextMeshProUGUI DropdownLabel;

    int _radix = 0;
    
    private void Awake()
    {
        if (this.name.Equals("Trit-Input"))
            GetComponentInParent<DragDrop>().name  = "Input (" + GetInstanceID().ToString()+")";

        if (this.name.Equals("Trit-Output"))
            GetComponentInParent<DragDrop>().name = "Output (" + GetInstanceID().ToString() + ")";
    }

    public void ComputeCounter()
    {
        //get radix from dropdown
        switch (DropdownLabel.text)
        {
            case "BalancedTernary":
                _radix = 3;
                break;
            case "UnbalancedTernary":
                _radix = 3;
                break;
            case "Binary":
                _radix = 2;
                break;
            default:
                break;
        }

        //go over each button, compute value, set value to label
        int sum = 0;
        int temp = 0;
        for (int i=0; i<Buttons.Count; i++)
        {
            temp =  Int32.Parse(Buttons[Buttons.Count-1-i].GetComponent<BtnInput>().label.text); //get and convert input Top is highest 
            sum += (int) (temp * Math.Pow(_radix,i));
        }

        CounterLabel.text = sum.ToString();
    }
}


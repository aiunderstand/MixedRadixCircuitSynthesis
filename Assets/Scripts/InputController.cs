using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public List<GameObject> Buttons = new List<GameObject>(); //buttons found in the Full Version View
    
    public TextMeshProUGUI CounterLabel;
    public TextMeshProUGUI DropdownLabel;
    public SavedComponent savedComponent; //only needed for saved components
    int _radix = 0;
    bool isInit = false;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (!isInit)
        {
            if (this.name.Equals("Trit-Input"))
                transform.parent.parent.GetComponent<DragDrop>().name = ";Input;" + GetInstanceID().ToString();

            if (this.name.Equals("Trit-Output"))
                transform.parent.parent.GetComponent<DragDrop>().name = ";Output;" + GetInstanceID().ToString();

            if (this.name.Contains("Level"))
                transform.parent.GetComponent<DragDrop>().name = ";SavedGate;" + GetInstanceID().ToString();

            isInit = true;

            foreach (var b in Buttons)
            {
                b.GetComponent<BtnInput>().Init();
            }
        }
    }

    public void ComputeCounter()
    {
        //this if statement is only here because lazy code. 
        //This controller is required with logic gates as they use inputBtn which are controlled by this class. In a refactor this dependency should be not there
        //note that all the logic is done in the logicgatecontroller class if this a used with a logicgate component
        bool signed = false;
        if (DropdownLabel != null) 
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
                case "SignedBinary":
                    _radix = 2;
                    signed = true;
                    break;
                default:
                    break;
            }

            //go over each button, compute value, set value to label
            int sum = 0;
            int temp = 0;
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (i == Buttons.Count-1 && signed)
                    temp = -1 * (Int32.Parse(Buttons[Buttons.Count - 1 - i].GetComponent<BtnInput>().label.text)); //get and convert input Top is highest 
                else
                    temp = Int32.Parse(Buttons[Buttons.Count - 1 - i].GetComponent<BtnInput>().label.text); //get and convert input Top is highest 

                sum += (int)(temp * Math.Pow(_radix, i));
            }

            CounterLabel.text = sum.ToString();
        }
    }

   
}


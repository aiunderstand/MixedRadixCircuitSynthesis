using ExtensionMethods;
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
    public TextMeshProUGUI[] A;
    public TextMeshProUGUI[] B;
    public TMP_Dropdown C;

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

    public void ConvertMatrix(BtnInput.RadixOptions source, BtnInput.RadixOptions target)
    {
        //if binary to binary or ternary to ternary 
        int newValue;
        var tt = transform.parent.GetComponentInChildren<Matrix>().Truthtable;
        
        foreach (var t in tt)
        {
            newValue = RadixHelper.ConvertRadixFromTo(source, target, t.GetValue());
            t.SetValue(newValue);
        }

        //if binary to ternary conversion or ternary to binary 

        //we need to assign both binary and ternary game objects at start and during pull down or pull up



        
    }

    public void UpdateLabels(BtnInput.RadixOptions radixType, int arity)
    {
        //if we change from radix A to radix B


       if (radixType.Equals(BtnInput.RadixOptions.BalancedTernary))
       {
            A[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-1";
            A[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
            A[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";

            if (B.Length >0)
            {
                B[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
                B[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
                B[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-1";
            }

            if (arity == 3)
            {
                //update labels of C
                C.options[0].text = "C: -1";
                C.options[1].text = "C: 0";
                C.options[2].text = "C: 1";

                C.captionText.text = C.options[C.value].text;
            }
       }

       if (radixType.Equals(BtnInput.RadixOptions.UnbalancedTernary))
        {
            A[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
            A[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
            A[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "2";

            if (B.Length > 0)
            {
                B[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "2";
                B[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
                B[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
            }

            if (arity == 3)
            {
                //update labels of C
                C.options[0].text = "C: 0";
                C.options[1].text = "C: 1";
                C.options[2].text = "C: 2";

                C.captionText.text = C.options[C.value].text;
            }
        }
    }
}


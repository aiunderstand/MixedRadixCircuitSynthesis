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
    public Connection[] Connections = new Connection[6]; //needed for data binding 
    private void Awake()
    {
        for (int i = 0; i < Connections.Length; i++)
        {
            Connections[i] = new Connection();
        }
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
            temp =  Int32.Parse(Buttons[i].GetComponent<BtnInput>().label.text); //get and convert input
            sum += (int) (temp * Math.Pow(_radix,i));
        }

        CounterLabel.text = sum.ToString();
    }
}

public class Connection {
    public List<BtnInput> endTerminal = new List<BtnInput>();
    public BtnInput startTerminal;
}
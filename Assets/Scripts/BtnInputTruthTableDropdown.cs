﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BtnInputTruthTableDropdown : MonoBehaviour
{
    public GameObject[] optionsC;
    TMP_Dropdown _Dropdown;
    public void Start()
    {
            //Fetch the Dropdown GameObject
            _Dropdown = GetComponent<TMP_Dropdown>();
            //Add listener for when the value of the Dropdown changes, to take action
            _Dropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(_Dropdown);
            });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        foreach (var o in optionsC)
            o.SetActive(false);

        if (optionsC.Length == 27) //ternary
        {
            for (int i = (change.value * 9); i < (change.value * 9) + 9; i++)
            {
                optionsC[i].SetActive(true);
            }
        }
        else // binary
        {
            for (int i = (change.value * 4); i < (change.value * 4) + 4; i++)
            {
                optionsC[i].SetActive(true);
            }
        }
    }

    public void ActivateAll()
    {
        foreach (var o in optionsC)
            o.SetActive(true);
    }

    public void DeActivateAll()
    {
        foreach (var o in optionsC)
            o.SetActive(false);
    }

    public void Activate(int index)
    {
        if (optionsC.Length == 27) //ternary
        {
            for (int i = (index * 9); i < (index * 9) + 9; i++)
            {
                optionsC[i].SetActive(true);
            }
        }
        else // binary
        {
            for (int i = (index * 4); i < (index * 4) + 4; i++)
            {
                optionsC[i].SetActive(true);
            }
        }
    }
}

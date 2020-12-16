using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        optionsC[change.value].SetActive(true);
    }
}

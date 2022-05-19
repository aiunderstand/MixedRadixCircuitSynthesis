using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using static TruthtableFunctionHelper;

public class DragDropSaved : MonoBehaviour
{
    public TempFunctions tempFunctions;
    public void LoadSavedState()
    {
        var functionDropDown = GetComponentInChildren<AutoCompleteComboBox>();
        functionDropDown._mainInput.text = tempFunctions.ToString().ToLower();
        functionDropDown.ShowDropdownPanel(false);
    }
}

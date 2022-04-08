using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class InputValidator : MonoBehaviour
{
    TMP_InputField label;
    public void Awake()
    {
        label = GetComponent<TMP_InputField>();
    }
    public void CheckIOLabel(string newLabel)
    {
        var pattern = @"[^0-9a-zA-Z-]+"; //no spaces or _
        var filteredName = Regex.Replace(newLabel, pattern, "");
        label.text = filteredName;
    }
}

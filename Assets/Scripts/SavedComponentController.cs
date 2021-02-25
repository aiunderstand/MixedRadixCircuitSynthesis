using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavedComponentController : MonoBehaviour
{
    public SavedComponent savedComponent;
    private void Awake()
    {
        GetComponentInParent<DragDrop>().name = ";SavedGate;" + GetInstanceID().ToString();        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SaveCircuit : MonoBehaviour
{
    CircuitGenerator cGen;
    public TMP_InputField Name;
    public GameObject previewPrefab;
    private void Awake()
    {
        cGen = FindObjectOfType<CircuitGenerator>();    
    }

    public void SaveComponentAs()
    {
        cGen.SaveComponent(Name.text);
    }

    private void generatePreview()
    { 
        
    
    }
}

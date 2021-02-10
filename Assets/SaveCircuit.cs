using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using static BtnInput;
using System;

public class SaveCircuit : MonoBehaviour
{
    CircuitGenerator cGen;
    public TMP_InputField Name;
    public GameObject previewPrefab;
    
    [HideInInspector]
    public GameObject tempComponent;
    private void Awake()
    {
        cGen = FindObjectOfType<CircuitGenerator>();    
    }

    public void SaveComponentAs()
    {
        //filter symbols and spaces from name
        var pattern = @"[^0-9a-zA-Z_]+";
        var filteredName = Regex.Replace(Name.text, pattern, "");
        cGen.SaveComponent(filteredName);

        //clear canvas and input
        Name.text = "";
        applicationmanager.ClearCanvas();
    }

    public void GeneratePreview()
    {
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");

        List<RadixOptions> inputs = new List<RadixOptions>();
        List<RadixOptions> outputs = new List<RadixOptions>();
        List<string> inputLabels = new List<string>();
        List<string> outputLabels = new List<string>();

        foreach (var c in components)
        {
            if (c.name.Contains("Input"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                string id = inputControler.GetInstanceID().ToString();

                foreach (var b in inputControler.Buttons)
                {
                    //double check if button is connected to something
                    var bi = b.GetComponent<BtnInput>();

                    if (bi.Connections.Count > 0)
                    {
                        RadixOptions radixSource = (RadixOptions)Enum.Parse(typeof(RadixOptions), bi.DropdownLabel.text, true);
                        inputs.Add(radixSource);
                        inputLabels.Add(bi.transform.GetChild(3).GetComponent<TMP_InputField>().text);
                    }
                }
            }

            if (c.name.Contains("Output"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                string id = inputControler.GetInstanceID().ToString();

                foreach (var b in inputControler.Buttons)
                {
                    //double check if button is connected to something
                    var bi = b.GetComponent<BtnInput>();

                    if (bi.Connections.Count > 0)
                    {
                        RadixOptions radixSource = (RadixOptions)Enum.Parse(typeof(RadixOptions), bi.DropdownLabel.text, true);
                        outputs.Add(radixSource);
                        outputLabels.Add(bi.transform.GetChild(2).GetComponent<TMP_InputField>().text);
                    }
                }
            }
        }
        
        tempComponent = GameObject.Instantiate(previewPrefab);
        tempComponent.transform.parent = this.transform.parent;
        tempComponent.transform.localScale = new Vector3(1, 1, 1);
        tempComponent.transform.localPosition = new Vector3(0, 120);
        tempComponent.GetComponent<ComponentGenerator>().Generate("", inputs, inputLabels, outputs, outputLabels);
    }

    public void UpdateName(string componentName)
    {
        tempComponent.GetComponent<ComponentGenerator>().title.text = componentName;
    }

    public void DeletePreview()
    {
        Destroy(tempComponent);
        tempComponent = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using static BtnInput;
using System;
using UnityEngine.UI;

public class SaveCircuit : MonoBehaviour
{
    public enum AbstractionLevel
        {
            MenuView = -1,
            LogicGateView = 0,
            ComponentView = 1
        }

    CircuitGenerator cGen;
    public TMP_InputField Name;
    public GameObject previewPrefab;
    bool fulfillsSaveConditions;
    public GameObject ContentContainer;
    public GameObject SaveCanvas;
    public static float MenuScale = 0.66f;
    public static float FullScale = 1f;
    public GameObject DragDropArea;
    public GameObject deleteBtnPrefab;

    [HideInInspector]
    public GameObject tempComponent;
    private void Awake()
    {
        cGen = FindObjectOfType<CircuitGenerator>();    
    }

    public void SaveComponentAs()
    {
        if (fulfillsSaveConditions)
        {
            //filter symbols and spaces from name
            var pattern = @"[^0-9a-zA-Z_]+";
            var filteredName = Regex.Replace(Name.text, pattern, "");
            bool success = cGen.SaveComponent(filteredName);

            if (success)
            {
                tempComponent.transform.SetParent(ContentContainer.transform);
                tempComponent.transform.localScale = new Vector3(MenuScale, MenuScale, MenuScale);
                UpdateName(Name.text);
                tempComponent.name = ";" + Name.text + ";" + tempComponent.gameObject.GetInstanceID();

                tempComponent.GetComponent<DragDrop>().FullVersion.SetActive(false);
                tempComponent.GetComponent<DragDrop>().MenuVersion.SetActive(true);

                //clear canvas and input
                tempComponent = null;
                Name.text = "";
                applicationmanager.ClearCanvas();
            }
        }
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

        tempComponent = new GameObject();
        tempComponent.AddComponent<RectTransform>();
        tempComponent.name = "SavedLogicGate";
        tempComponent.transform.SetParent(SaveCanvas.transform);
        tempComponent.transform.localScale = new Vector3(1, 1, 1);
        tempComponent.transform.localPosition = new Vector3(0, 120);
        
        GameObject menuView = GameObject.Instantiate(previewPrefab);
        menuView.name = AbstractionLevel.MenuView.ToString();
        menuView.GetComponent<ComponentGenerator>().Generate("", inputs, inputLabels, outputs, outputLabels, AbstractionLevel.MenuView);
        menuView.transform.SetParent(tempComponent.transform);
        menuView.transform.localPosition = new Vector3(0, 0);
       
        GameObject componentView = GameObject.Instantiate(previewPrefab);
        componentView.name = AbstractionLevel.ComponentView.ToString();
        componentView.GetComponent<ComponentGenerator>().Generate("", inputs, inputLabels, outputs, outputLabels, AbstractionLevel.ComponentView);
        componentView.transform.SetParent(tempComponent.transform);
        componentView.transform.localPosition = new Vector3(0, 0);
       
        float size = menuView.GetComponent<ComponentGenerator>().Size * MenuScale;
        tempComponent.AddComponent<LayoutElement>().minHeight = (size +5); //offset between menu items
        var dd = tempComponent.AddComponent<DragDrop>();
        dd.DragDropArea = DragDropArea;
        dd.FullVersion = componentView;
        dd.MenuVersion = menuView;
        dd.panelBg = componentView.transform.GetChild(0).GetComponent<Image>(); //first child is body. Hardcoded so not ideal

        if (inputs.Count == 0 || outputs.Count == 0)
            fulfillsSaveConditions = false;
        else
            fulfillsSaveConditions = true;

        UpdateName(Name.text);

        //add a delete btn
        GameObject deleteBtn = GameObject.Instantiate(deleteBtnPrefab);
        deleteBtn.transform.SetParent(menuView.transform);
        deleteBtn.transform.localPosition = new Vector3(-83.7f, 0);

        tempComponent.GetComponent<DragDrop>().MenuVersion.SetActive(false);
    }

    public void UpdateName(string componentName)
    {
         if (tempComponent != null)
        {
            tempComponent.GetComponent<DragDrop>().MenuVersion.SetActive(true);

            var cgs = tempComponent.GetComponentsInChildren<ComponentGenerator>();
            foreach (var item in cgs)
            {
                item.title.text = componentName;
            }

            tempComponent.name = componentName;
            tempComponent.GetComponent<DragDrop>().MenuVersion.SetActive(false);
        }

    }

    public void DeletePreview()
    {
        Destroy(tempComponent);
        tempComponent = null;
    }
}

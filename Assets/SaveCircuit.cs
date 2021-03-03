using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using static BtnInput;
using System;
using UnityEngine.UI;
using System.Linq;
using static SaveCircuit;

public class SaveCircuit : MonoBehaviour
{
    public enum Views
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
    SavedComponent tempComponentStructure;
    public StatisticsUI StatisticsScreen;
    public Toggle SaveAsLibraryComponent;
    public Color SelectionColor = Color.black;

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
            var pattern = @"[^0-9a-zA-Z_-]+";
            var filteredName = Regex.Replace(Name.text, pattern, "");
            Stats stats = cGen.SaveComponent(filteredName); //generate netlist

            if (stats.success)
            {
                if (SaveAsLibraryComponent.isOn)
                {
                    //we need to regenerate the component because it has children now
                    //save to settings file
                    tempComponentStructure.Stats = stats;
                    tempComponentStructure.ComponentName = Name.text;
                    tempComponentStructure.ComponentNetlistPath = Application.persistentDataPath + "/User/Generated/" + filteredName + "/" + "c_" + filteredName + ".sp";
                  
                    var go = GenerateListItem(tempComponentStructure, ContentContainer.transform, false);
                    go.GetComponent<DragDrop>().MenuVersion.SetActive(true);
                    go.GetComponent<DragDrop>().FullVersion.SetActive(false);
                    go.name = Name.text;

                    Settings.Save(tempComponentStructure);
                }

                //show statistics
                StatisticsScreen.Show(stats);
               
                //clear canvas, we clear the preview with a call in the unity btn handler
                applicationmanager.ClearCanvas();
                Name.text = "";
            }
        }
       
    }


    public void DestroyPreview()
    {
        Destroy(tempComponent);
        tempComponent = null;        
        applicationmanager.scrollEnabled = true;
    }

    public void GeneratePreview()
    {
        applicationmanager.scrollEnabled = false;
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");

        List<RadixOptions> inputs = new List<RadixOptions>();
        List<RadixOptions> outputs = new List<RadixOptions>();
        List<string> inputLabels = new List<string>();
        List<string> outputLabels = new List<string>();
        Dictionary<int, float> inputOrder = new Dictionary<int, float>(); //needed for ordering the labels from low to high (using transform.position.y)
        Dictionary<int, float> outputOrder = new Dictionary<int, float>();
        foreach (var c in components)
        {
            if (c.name.Contains("Input"))
            {
                var inputControler = c.GetComponentInChildren<InputController>();
                string id = inputControler.GetInstanceID().ToString();

                for (int i = 0; i <inputControler.Buttons.Count ; i++)
                {
                    var bi = inputControler.Buttons[i].GetComponent<BtnInput>();

                    if (bi.Connections.Count > 0)
                    {
                        inputOrder.Add(inputs.Count, bi.transform.position.y);
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

                for (int i = 0; i < inputControler.Buttons.Count; i++)
                {
                    var bi = inputControler.Buttons[i].GetComponent<BtnInput>();

                    if (bi.Connections.Count > 0)
                    {
                        outputOrder.Add(outputs.Count, bi.transform.position.y);
                        RadixOptions radixSource = (RadixOptions)Enum.Parse(typeof(RadixOptions), bi.DropdownLabel.text, true);
                        outputs.Add(radixSource);
                        outputLabels.Add(bi.transform.GetComponentInChildren<TMP_InputField>().text);
                    }
                }
            }
        }


        //reorder the labels
        List<string> tempLabels = new List<string>();
        List<RadixOptions> tempRadixSource = new List<RadixOptions>();

        //sort inputOrder
        var sorted = inputOrder.OrderBy(key => key.Value);

        //assign tempLabels in correct order
        foreach (var item in sorted)
        {
            tempLabels.Add(inputLabels[item.Key]);
            tempRadixSource.Add(inputs[item.Key]);
        }
        
        inputLabels = tempLabels;
        inputs = tempRadixSource;

        List<string> tempLabels1 = new List<string>();
        List<RadixOptions> tempRadixSource1 = new List<RadixOptions>();

        //do the same for outputLabels
        sorted = outputOrder.OrderBy(key => key.Value);

        //assign tempLabels in correct order
        foreach (var item in sorted)
        {
            tempLabels1.Add(outputLabels[item.Key]);
            tempRadixSource1.Add(outputs[item.Key]);
        }
        
        outputLabels = tempLabels1;
        outputs = tempRadixSource1;

        //create a temp saved component for preview/save screen
        tempComponentStructure = new SavedComponent(inputs, inputLabels, outputs, outputLabels);
        tempComponentStructure.Stats = new Stats();
        tempComponent = GenerateListItem(tempComponentStructure, SaveCanvas.transform, false);
        tempComponent.GetComponent<DragDrop>().FullVersion.SetActive(true);
        tempComponent.GetComponent<DragDrop>().FullVersion.GetComponent<ComponentGenerator>().infoBtn.SetActive(false);

        if (inputs.Count == 0 || outputs.Count == 0)
            fulfillsSaveConditions = false;
        else
            fulfillsSaveConditions = true;
    }

    public void UpdateName(string componentName) //event triggered by input text box from save screen
    {
         if (tempComponent != null)
        {
            var cgs = tempComponent.GetComponentsInChildren<ComponentGenerator>();
            foreach (var item in cgs)
            {
                item.title.text = componentName;
            }

            tempComponent.name = componentName;
        }
    }

    public void DeletePreview()
    {
        Destroy(tempComponent);
        tempComponent = null;
    }

    //this is aweful.This method does the same thing as preview only slightly different.We need to make time to do serious refactoring here
    public GameObject GenerateListItem(SavedComponent c, Transform parent, bool isDropped)
    {
        var ListItemObject = new GameObject();
        ListItemObject.AddComponent<RectTransform>();
        ListItemObject.transform.SetParent(parent);
        ListItemObject.transform.localScale = new Vector3(1, 1, 1);
        ListItemObject.transform.localPosition = new Vector3(0, 120);
        var dd = ListItemObject.AddComponent<DragDrop>();

        GameObject menuView = GameObject.Instantiate(previewPrefab);
        menuView.name = Views.MenuView.ToString();
        menuView.GetComponent<ComponentGenerator>().Generate(c, Views.MenuView);
        menuView.transform.SetParent(ListItemObject.transform);
        menuView.transform.localPosition = new Vector3(0, 0);
        menuView.transform.localScale = new Vector3(MenuScale, MenuScale);

        GameObject componentView = GameObject.Instantiate(previewPrefab);
        componentView.transform.SetParent(ListItemObject.transform);
        componentView.name = "Level: " + c.Stats.abstractionLevelCount;
        componentView.GetComponent<ComponentGenerator>().Generate(c, Views.ComponentView);
        componentView.transform.localPosition = new Vector3(0, 0);
        componentView.transform.localScale = new Vector3(FullScale, FullScale);
        componentView.SetActive(false);

        GameObject selectionBox = GameObject.Instantiate(componentView.GetComponent<ComponentGenerator>().body.gameObject);
        selectionBox.transform.SetParent(componentView.transform);
        selectionBox.transform.localPosition = new Vector3(0, 0);
        selectionBox.transform.localScale = new Vector3(FullScale, FullScale);
        selectionBox.transform.SetAsFirstSibling();
        selectionBox.GetComponent<Image>().color = SelectionColor;
        selectionBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 156); //6 pixels wider then body
        selectionBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, menuView.GetComponent<ComponentGenerator>().Size + 6); //6 pixels heigher then body
        selectionBox.SetActive(false);
     
        float size = menuView.GetComponent<ComponentGenerator>().Size * MenuScale;
        ListItemObject.AddComponent<LayoutElement>().minHeight = (size + 5); //offset between menu items
       
        dd.DragDropArea = DragDropArea;
        dd.FullVersion = componentView;
        dd.MenuVersion = menuView;
        dd.SelectionBox = selectionBox;
        
        dd.panelBg = componentView.GetComponent<ComponentGenerator>().body.gameObject.GetComponent<Image>(); //first child is body. Hardcoded so not ideal

        dd.MenuVersion.GetComponent<ComponentGenerator>().title.text = c.ComponentName;
        dd.FullVersion.GetComponent<ComponentGenerator>().title.text = c.ComponentName;
        menuView.SetActive(false);

        //add a delete btn
        GameObject deleteBtn = GameObject.Instantiate(deleteBtnPrefab);
        deleteBtn.transform.SetParent(menuView.transform);
        deleteBtn.transform.localPosition = new Vector3(-83.7f, 0);
        deleteBtn.transform.localScale = new Vector3(1, 1);

        //responsible for the name and interaction, needs to be last due to awake funtion
        ListItemObject.GetComponent<DragDrop>().Stats = c.Stats;
        ListItemObject.GetComponent<DragDrop>().FullVersion.AddComponent<SavedComponentController>();
        ListItemObject.GetComponent<DragDrop>().FullVersion.GetComponent<SavedComponentController>().savedComponent = c;

        if (isDropped)
        {
            ListItemObject.GetComponent<DragDrop>().SetVersion(true); //mimic it being already dropped
            ListItemObject.AddComponent<SelectionBehavior>();
        }
        return ListItemObject;
    }

  

}

public class SavedComponent
{
    public List<RadixOptions> Inputs;
    public List<RadixOptions> Outputs;
    public List<string> InputLabels;
    public List<string> OutputLabels;
    public string ComponentName;
    public string ComponentNetlistPath;
    public Stats Stats;

    public SavedComponent() { }

    public SavedComponent(List<RadixOptions> inputs, List<string> inputLabels, List<RadixOptions> outputs, List<string> outputLabels)
    {
        this.Inputs = inputs;
        this.InputLabels = inputLabels;
        this.Outputs = outputs;
        this.OutputLabels = outputLabels;
    }
}
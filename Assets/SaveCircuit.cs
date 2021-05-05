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
    public TMP_InputField Label;
    public GameObject previewPrefab;
    bool fulfillsSaveConditions;
    public GameObject ContentContainer;
    public GameObject SaveCanvas;
    public static float MenuScale = 0.66f;
    public static float FullScale = 1f;
    public GameObject _DragDropArea;
    public GameObject deleteBtnPrefab;
    SavedComponent tempComponentStructure;
    public StatisticsUI StatisticsScreen;
    public Toggle SaveAsLibraryComponent;
    public Color SelectionColor = Color.black;
    public static GameObject DragDropArea;

    [HideInInspector]
    public GameObject tempComponent;
    private void Awake()
    {
        cGen = FindObjectOfType<CircuitGenerator>();
        DragDropArea = _DragDropArea;
    }

    public void SaveComponentAs()
    {
        if (fulfillsSaveConditions)
        {
            Stats stats = cGen.SaveComponent(Label.text); //generate netlist

            if (stats.success)
            {
                if (SaveAsLibraryComponent.isOn)
                {
                    //we need to regenerate the component because it has children now
                    //save to settings file
                    tempComponentStructure.Stats = stats;
                    tempComponentStructure.ComponentName = Label.text;
                    tempComponentStructure.ComponentNetlistPath = Application.persistentDataPath + "/User/Generated/" + Label.text + "/" + "c_" + Label.text + ".sp";

                    var go = GenerateMenuItem(tempComponentStructure, ContentContainer.transform);
                    go.GetComponent<DragDrop>().MenuVersion.SetActive(true);
                    go.GetComponent<DragDrop>().SavedComponent = tempComponentStructure;
                    go.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

                    //Unity bug where it will auto default to wrong anchor position when part of layout group (sets it to top left). Probably due to some awake script. Set it to center here.
                    go.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                    go.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                    go.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

                    //go.name = Label.text;

                    Settings.Save(tempComponentStructure);
                }

                //show statistics
                StatisticsScreen.Show(stats, tempComponentStructure);
               
                //clear canvas, we clear the preview with a call in the unity btn handler
                applicationmanager.ClearCanvas();
                Label.text = "";
            }
        }
       
    }


    public void DestroyPreview()
    {
        Destroy(tempComponent);
        tempComponent = null;        
        applicationmanager.scrollEnabled = true;
    }

    //we should remove this preview generator and use the normal generator as there are some slight differences to ordering of inputs
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
            //only save top abstraction level at this moment, not the lower one if they have been changed
            if (c.transform.parent.transform == DragDropArea.transform)
            {
                if (c.name.Contains("Input"))
                {
                    var inputControler = c.GetComponentInChildren<InputController>();
                    string id = inputControler.GetInstanceID().ToString();

                    for (int i = 0; i < inputControler.Buttons.Count; i++)
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
        }


        ////reorder the labels
        //List<string> tempLabels = new List<string>();
        //List<RadixOptions> tempRadixSource = new List<RadixOptions>();

        ////sort inputOrder
        //var sorted = inputOrder.OrderBy(key => key.Value);

        ////assign tempLabels in correct order
        //foreach (var item in sorted)
        //{
        //    tempLabels.Add(inputLabels[item.Key]);
        //    tempRadixSource.Add(inputs[item.Key]);
        //}
        
        //inputLabels = tempLabels;
        //inputs = tempRadixSource;

        //List<string> tempLabels1 = new List<string>();
        //List<RadixOptions> tempRadixSource1 = new List<RadixOptions>();

        ////do the same for outputLabels
        //sorted = outputOrder.OrderBy(key => key.Value);

        ////assign tempLabels in correct order
        //foreach (var item in sorted)
        //{
        //    tempLabels1.Add(outputLabels[item.Key]);
        //    tempRadixSource1.Add(outputs[item.Key]);
        //}
        
        //outputLabels = tempLabels1;
        //outputs = tempRadixSource1;

        //create a temp saved component for preview/save screen
        tempComponentStructure = new SavedComponent(inputs, inputLabels, outputs, outputLabels);
        tempComponentStructure.Stats = new Stats();

        applicationmanager.InitHack = new List<GameObject>();
        tempComponent = GenerateItem(tempComponentStructure, SaveCanvas.transform, false);
        applicationmanager.InitHack.Clear();

        tempComponent.GetComponent<DragDrop>().FullVersion.SetActive(true);
        tempComponent.GetComponent<DragDrop>().FullVersion.GetComponent<ComponentGenerator>().infoBtn.SetActive(false);

        if (inputs.Count == 0 || outputs.Count == 0)
            fulfillsSaveConditions = false;
        else
            fulfillsSaveConditions = true;
    }

    public GameObject GenerateMenuItem(SavedComponent c, Transform parent)
    {
        var ListItemObject = new GameObject();
        ListItemObject.AddComponent<RectTransform>();
        ListItemObject.transform.SetParent(parent);
        ListItemObject.transform.localScale = new Vector3(1, 1, 1);
        ListItemObject.transform.localPosition = new Vector3(0, 120);
        var dd = ListItemObject.AddComponent<DragDrop>();

        GameObject menuView = GameObject.Instantiate(previewPrefab);
        menuView.name = Views.MenuView.ToString();
        menuView.transform.SetParent(ListItemObject.transform);
        menuView.transform.localPosition = new Vector3(0, 0);
        menuView.transform.localScale = new Vector3(MenuScale, MenuScale);
        dd.MenuVersion = menuView;
        menuView.GetComponent<ComponentGenerator>().Generate(c, Views.MenuView);

        float size = menuView.GetComponent<ComponentGenerator>().Size * MenuScale;
        ListItemObject.AddComponent<LayoutElement>().minHeight = (size + 5); //offset between menu items        
        dd.MenuVersion.GetComponent<ComponentGenerator>().title.text = c.ComponentName;
        dd.panelBg = menuView.GetComponent<ComponentGenerator>().body.gameObject.GetComponent<Image>(); //first child is body. Hardcoded so not ideal
        menuView.SetActive(false);

        //add a delete btn
        GameObject deleteBtn = GameObject.Instantiate(deleteBtnPrefab);
        deleteBtn.transform.SetParent(menuView.transform);
        deleteBtn.transform.localPosition = new Vector3(-83.7f, 0);
        deleteBtn.transform.localScale = new Vector3(1, 1);
        menuView.GetComponent<ComponentGenerator>().deleteBtn = deleteBtn;

        //responsible for the name and interaction, needs to be last due to awake funtion
        ListItemObject.GetComponent<DragDrop>().Stats = c.Stats;
        ListItemObject.GetComponent<DragDrop>().SavedComponent = c;
        menuView.SetActive(true);

        return ListItemObject;
    }


    public void UpdateName(string componentName) //event triggered by input text box from save screen
    {
        //hacky, this is the same pattern as the input validator
        var pattern = @"[^0-9a-zA-Z-]+"; //no spaces or _
        var filteredName = Regex.Replace(componentName, pattern, "");
        
        if (tempComponent != null)
        {
            var cgs = tempComponent.GetComponentsInChildren<ComponentGenerator>();
            foreach (var item in cgs)
            {
                item.title.text = filteredName;
            }

            tempComponent.name = filteredName;
        }
    }

    public void DeletePreview()
    {
        Destroy(tempComponent);
        tempComponent = null;
    }

    public GameObject GenerateItem(SavedComponent c, Transform parent, bool isDropped)
    {
        var ListItemObject = new GameObject();
        ListItemObject.AddComponent<RectTransform>();
        ListItemObject.transform.SetParent(parent);
        ListItemObject.transform.localScale = new Vector3(1, 1, 1);
        ListItemObject.transform.localPosition = new Vector3(0, 120);
        var dd = ListItemObject.AddComponent<DragDrop>();

        GameObject menuView = GameObject.Instantiate(previewPrefab);
        menuView.name = Views.MenuView.ToString();
        menuView.transform.SetParent(ListItemObject.transform);
        menuView.transform.localPosition = new Vector3(0, 0);
        menuView.transform.localScale = new Vector3(MenuScale, MenuScale);
        dd.MenuVersion = menuView;
        menuView.GetComponent<ComponentGenerator>().Generate(c, Views.MenuView);
       
        GameObject componentView = GameObject.Instantiate(previewPrefab);
        applicationmanager.InitHack.Add(componentView);
        componentView.transform.SetParent(ListItemObject.transform);
        componentView.transform.localPosition = new Vector3(0, 0);
        componentView.transform.localScale = new Vector3(FullScale, FullScale);
        componentView.name = "Level: " + c.Stats.abstractionLevelCount;
        dd.FullVersion = componentView;
        var buttons = componentView.GetComponent<ComponentGenerator>().Generate(c, Views.ComponentView);
        //componentView.SetActive(false);
       
        GameObject selectionBox = GameObject.Instantiate(componentView.GetComponent<ComponentGenerator>().body.gameObject);
        selectionBox.transform.SetParent(componentView.transform);
        selectionBox.transform.localPosition = new Vector3(0, 0);
        selectionBox.transform.localScale = new Vector3(FullScale, FullScale);
        selectionBox.transform.SetAsFirstSibling();
        selectionBox.GetComponent<Image>().color = SelectionColor;
        selectionBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 156); //6 pixels wider then body
        selectionBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, menuView.GetComponent<ComponentGenerator>().Size + 6); //6 pixels heigher then body
        selectionBox.SetActive(false);
        dd.SelectionBox = selectionBox;

        float size = menuView.GetComponent<ComponentGenerator>().Size * MenuScale;
        ListItemObject.AddComponent<LayoutElement>().minHeight = (size + 5); //offset between menu items        
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
        ListItemObject.GetComponent<DragDrop>().FullVersion.AddComponent<InputController>().Buttons = buttons;
        ListItemObject.GetComponent<DragDrop>().FullVersion.GetComponent<InputController>().savedComponent = c;
      
        if (isDropped)
        {
            ListItemObject.GetComponent<DragDrop>().SetVersion(true); //mimic it being already dropped
            ListItemObject.AddComponent<SelectionBehavior>();
        }

        //applicationmanager.InitHack.Add(ListItemObject.GetComponent<DragDrop>().LowerAbstractionVersion.gameObject);

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
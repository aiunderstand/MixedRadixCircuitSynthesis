using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI.Extensions;
using ExtensionMethods;
using static BtnInput;
using System.Collections.Specialized;
using System.IO;

//see this presentation about marshalling data from c++ to C# and inverse
//https://www.slideshare.net/unity3d/adding-love-to-an-api-or-how-to-expose-c-in-unity
// https://github.com/lazerfalcon/Unite2018_Native/tree/master/Assets
// https://www.jacksondunstan.com/articles/3938 
// https://blogs.unity3d.com/2017/01/19/low-level-plugins-in-unity-webgl/
public class TruthtableFunctionHelper : MonoBehaviour
{
    public enum HardwareMappingModes
    {
        variantA_woBody = 0,
        variantB_wBodyDividersOnly = 1,
        variantB_wBodyDividersTransistors = 2
    }

    AutoCompleteComboBox _Dropdown;
    DragExpandTableComponent _DETC;
    public enum TempFunctions { //special name must be start with s
        _AND,
        _OR,
        _NOT,
        _XOR,
        _Sum00a,
        _Sum00b,
        _Carry00,
        _Sum10,
        _Sum11a,
        _Sum01,
        _Carry01,
        _Sum11b,
        _Carry11,
        _Carry21,
        _Sum21a,
        _Sum1a,
        _Sum1b,
        _Sum2a,
        _Sum2b,
        _Carry1,
        _Carry2,
        _Bal_dist_measure
    }
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern IntPtr GetTableFromIndex(int tableIndex);

    [DllImport("__Internal")]
    public static extern void GetTableFromIndex_Release(IntPtr ptr);

    [DllImport("__Internal")]
    public static extern int GetTableFromIndexSingle(int tableIndex, int index);

    [DllImport("__Internal")]
    public static extern int CreateNetlist(int mode,string filePath, int[] array, int length,int arity);

    [DllImport("__Internal")]
    public static extern int TestSum(int[] array, int length);

    [DllImport("__Internal")]
    public static extern int CreateCircuit(
        string filePath, 
        string fileName,
        int inputs,
        string[] inputNames,
        int outputs,
        string[] outputNames, 
        int ttIndicesCount, 
        string[] ttIndices,
        int arityCount,
        int[] arityArray,
        int connectionCount,
        string[] connectionArray,
        int invCount,
        int[] invArray, 
        int positionCount,
        string[] positionArray, 
        int savedCircuitCount, 
        string[] savedCircuitNamesArray,
        int connectionIndexCount,
        int[] connectionIndexArray,
        string[] functionRadixTypeArray,
        int functionRadixTypeCount,
        int inputComponents,
        int outputComponents,
        string[] ioRadixTypeArray,
        int ioRadixTypeCount,
        int[] inputOutputSizeArray,
        int inputOutputSizeCount,
        string[] ioPositionArray,
        int ioPositionCount,
        string[] connectionPairArray,
        int connectionPairCount,
        string[] idArray,
        int idArrayCount
        );

    [DllImport("__Internal")]
    public static extern IntPtr GetOptimzedTT();

    [DllImport("__Internal")]
    public static extern void GetOptimzedTT_Release(IntPtr ptr);

    [DllImport("__Internal")]
    public static extern IntPtr GetInvArray();

    [DllImport("__Internal")]
    public static extern void GetInvArray_Release(IntPtr ptr);

    [DllImport("__Internal")]
    public static extern void SetMode(int newMode);
#else
    [DllImport("CircuitGenerator", EntryPoint = "GetTableFromIndex")]
    public static extern IntPtr GetTableFromIndex(int tableIndex);

    [DllImport("CircuitGenerator", EntryPoint = "GetTableFromIndex_Release")]
    public static extern void GetTableFromIndex_Release(IntPtr ptr);

    [DllImport("CircuitGenerator", EntryPoint = "GetTableFromIndexSingle")]
    public static extern int GetTableFromIndexSingle(int tableIndex, int index);

    [DllImport("CircuitGenerator", EntryPoint = "CreateNetlist")]
    public static extern int CreateNetlist(int mode,string filePath, int[] array, int length,int arity);

    [DllImport("CircuitGenerator", EntryPoint = "TestSum")]
    public static extern int TestSum(int[] array, int length);

    [DllImport("CircuitGenerator", EntryPoint = "CreateCircuit")]
    public static extern int CreateCircuit(
        string filePath, 
        string fileName,
        int inputs,
        string[] inputNames,
        int outputs,
        string[] outputNames, 
        int ttIndicesCount, 
        string[] ttIndices,
        int arityCount,
        int[] arityArray,
        int connectionCount,
        string[] connectionArray,
        int invCount,
        int[] invArray, 
        int positionCount,
        string[] positionArray, 
        int savedCircuitCount, 
        string[] savedCircuitNamesArray,
        int connectionIndexCount,
        int[] connectionIndexArray,
        string[] functionRadixTypeArray,
        int functionRadixTypeCount,
        int inputComponents,
        int outputComponents,
        string[] ioRadixTypeArray,
        int ioRadixTypeCount,
        int[] inputOutputSizeArray,
        int inputOutputSizeCount,
        string[] ioPositionArray,
        int ioPositionCount,
        string[] connectionPairArray,
        int connectionPairCount,
        string[] idArray,
        int idArrayCount
        );

    [DllImport("CircuitGenerator", EntryPoint = "GetOptimzedTT")]
    public static extern IntPtr GetOptimzedTT();

    [DllImport("CircuitGenerator", EntryPoint = "GetOptimzedTT_Release")]
    public static extern void GetOptimzedTT_Release(IntPtr ptr);

    [DllImport("CircuitGenerator", EntryPoint = "GetInvArray")]
    public static extern IntPtr GetInvArray();

    [DllImport("CircuitGenerator", EntryPoint = "GetInvArray_Release")]
    public static extern void GetInvArray_Release(IntPtr ptr);

    [DllImport("CircuitGenerator", EntryPoint = "SetMode")]
    public static extern void SetMode(int newMode);

#endif

    bool isInitialized = false;

    public static int[] GetAndConvertInvArrayFormat(int arity)
    {
        List<int> invArray = new List<int>();
        int tablesize = arity * 3;

        int[] tempInvArray = new int[tablesize];
        IntPtr srcPtr = GetInvArray(); //if we use this method, we have to release the pointer later!
        Marshal.Copy(srcPtr, tempInvArray, 0, tablesize);

        for (int j = 0; j < tablesize; j++)
        {
            invArray.Add(tempInvArray[j]);
        }

        GetInvArray_Release(srcPtr);

        if (arity == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                invArray.Add(0);
            }
        }

        if (arity == 1)
        {
            for (int i = 0; i < 6; i++)
            {
                invArray.Add(0);
            }
        }

        return invArray.ToArray();
    }

    public void Awake() //must init after autocompletebox hence at second possition in hierarchy
    {
        if (!isInitialized) //for manual init
        {
            Initialize(); 
        }
    }

    public void Initialize()
    {
        //Fetch the Dropdown GameObject
        _Dropdown = GetComponent<AutoCompleteComboBox>();
        //Add listener for when the value of the Dropdown changes, to take action
        _Dropdown.OnSelectionChanged.AddListener(delegate {
            DropdownValueChanged(_Dropdown);
        });

        _DETC = transform.parent.GetComponentInChildren<DragExpandTableComponent>();

        //fill the dropdown with options
        List<string> options = new List<string>();

        //add 19k arity 2 functions

        //add 27 arity 1 functions

        //add 16 binary functions

        //add special named functions
        var values = Enum.GetValues(typeof(TempFunctions));
        foreach (var item in values)
        {
            options.Add(item.ToString());
        }

        _Dropdown.SetAvailableOptions(options);

        isInitialized = true;
    }

    void DropdownValueChanged(AutoCompleteComboBox change)
    {
        var function = change.Text;
        if (function.Length > 0)
        {
            if (function[0].ToString().Equals("_"))
            {
                TempFunctions func;
                if (Enum.TryParse(function, true, out func))
                    if (Enum.IsDefined(typeof(TempFunctions), func))
                    {
                        BtnInputTruthTable[] cells;
                        BtnInputTruthTableDropdown bittd;
                        int currentIndex;
                        switch (func)
                        {
                            //we read a vector list row by row, so r by r
                            case TempFunctions._AND:
                                //switch to correct radix
                                _DETC.DropdownLabel.gameObject.GetComponentInParent<TMP_Dropdown>().value = 2;

                                //switch to correct size
                                _DETC.SetPanelSize(2, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "0";
                                cells[2].label.text = "0";
                                cells[3].label.text = "1";
                                break;
                            case TempFunctions._OR:
                                //switch to correct radix
                                _DETC.DropdownLabel.gameObject.GetComponentInParent<TMP_Dropdown>().value = 2;

                                //switch to correct size
                                _DETC.SetPanelSize(2, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "1";
                                cells[2].label.text = "1";
                                cells[3].label.text = "1";
                                break;
                            case TempFunctions._XOR:
                                //switch to correct radix
                                _DETC.DropdownLabel.gameObject.GetComponentInParent<TMP_Dropdown>().value = 2;

                                //switch to correct size
                                _DETC.SetPanelSize(2, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "1";
                                cells[2].label.text = "1";
                                cells[3].label.text = "0";
                                break;
                            case TempFunctions._NOT:
                                //switch to correct radix
                                _DETC.DropdownLabel.gameObject.GetComponentInParent<TMP_Dropdown>().value = 2;

                                //switch to correct size
                                _DETC.SetPanelSize(2, 1);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "1";
                                cells[1].label.text = "0";
                                break;
                            case TempFunctions._Sum00a:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "1";
                                cells[2].label.text = "1";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "1";
                                cells[6].label.text = "-1";
                                cells[7].label.text = "0";
                                cells[8].label.text = "0";
                                break;
                            case TempFunctions._Sum00b:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "-1";
                                cells[1].label.text = "0";
                                cells[2].label.text = "1";
                                cells[3].label.text = "0";
                                cells[4].label.text = "1";
                                cells[5].label.text = "-1";
                                cells[6].label.text = "0";
                                cells[7].label.text = "1";
                                cells[8].label.text = "-1";
                                break;
                            case TempFunctions._Sum01:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "-1";
                                cells[1].label.text = "0";
                                cells[2].label.text = "1";
                                cells[3].label.text = "-1";
                                cells[4].label.text = "0";
                                cells[5].label.text = "1";
                                cells[6].label.text = "1";
                                cells[7].label.text = "-1";
                                cells[8].label.text = "0";
                                break;
                            case TempFunctions._Carry01:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "0";
                                cells[2].label.text = "0";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "0";
                                cells[6].label.text = "-1";
                                cells[7].label.text = "0";
                                cells[8].label.text = "0";
                                break;
                            case TempFunctions._Sum10:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "1";
                                cells[2].label.text = "1";
                                cells[3].label.text = "0";
                                cells[4].label.text = "1";
                                cells[5].label.text = "1";
                                cells[6].label.text = "1";
                                cells[7].label.text = "1";
                                cells[8].label.text = "-1";
                                break;
                            case TempFunctions._Sum11a:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "-1";
                                cells[1].label.text = "0";
                                cells[2].label.text = "1";
                                cells[3].label.text = "x";
                                cells[4].label.text = "x";
                                cells[5].label.text = "x";
                                cells[6].label.text = "0";
                                cells[7].label.text = "1";
                                cells[8].label.text = "-1";
                                break;
                            case TempFunctions._Sum11b:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "1";
                                cells[1].label.text = "-1";
                                cells[2].label.text = "0";
                                cells[3].label.text = "-1";
                                cells[4].label.text = "0";
                                cells[5].label.text = "1";
                                cells[6].label.text = "-1";
                                cells[7].label.text = "0";
                                cells[8].label.text = "1";
                                break;
                            case TempFunctions._Sum21a:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "-1";
                                cells[1].label.text = "x";
                                cells[2].label.text = "0";
                                cells[3].label.text = "0";
                                cells[4].label.text = "x";
                                cells[5].label.text = "+";
                                cells[6].label.text = "+";
                                cells[7].label.text = "x";
                                cells[8].label.text = "-1";
                                break;
                            case TempFunctions._Sum1a: //checked
                                                       //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "0";
                                cells[2].label.text = "-1";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "0";
                                cells[6].label.text = "1";
                                cells[7].label.text = "0";
                                cells[8].label.text = "0";
                                break;
                            case TempFunctions._Sum1b: //checked
                                                       //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "-1";
                                cells[1].label.text = "0";
                                cells[2].label.text = "0";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "1";
                                cells[6].label.text = "1";
                                cells[7].label.text = "1";
                                cells[8].label.text = "-1";
                                break;
                            case TempFunctions._Sum2a: //checked
                                                       //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "0";
                                cells[2].label.text = "1";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "1";
                                cells[6].label.text = "1";
                                cells[7].label.text = "1";
                                cells[8].label.text = "-1";
                                break;
                            case TempFunctions._Sum2b: //checked
                                                       //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "-1";
                                cells[1].label.text = "-1";
                                cells[2].label.text = "-1";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "-1";
                                cells[6].label.text = "1";
                                cells[7].label.text = "1";
                                cells[8].label.text = "-1";
                                break;
                            case TempFunctions._Carry21: //checked
                                                         //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "0";
                                cells[2].label.text = "0";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "0";
                                cells[6].label.text = "0";
                                cells[7].label.text = "0";
                                cells[8].label.text = "1";
                                break;
                            case TempFunctions._Carry00:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 3);

                                //get all the cells
                                bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                                currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                                bittd.ActivateAll();

                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "0";
                                cells[2].label.text = "0";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "0";
                                cells[6].label.text = "0";
                                cells[7].label.text = "0";
                                cells[8].label.text = "0";
                                cells[9].label.text = "0";
                                cells[10].label.text = "1";
                                cells[11].label.text = "1";
                                cells[12].label.text = "0";
                                cells[13].label.text = "1";
                                cells[14].label.text = "1";
                                cells[15].label.text = "0";
                                cells[16].label.text = "0";
                                cells[17].label.text = "0";
                                cells[18].label.text = "0";
                                cells[19].label.text = "1";
                                cells[20].label.text = "1";
                                cells[21].label.text = "0";
                                cells[22].label.text = "1";
                                cells[23].label.text = "1";
                                cells[24].label.text = "0";
                                cells[25].label.text = "0";
                                cells[26].label.text = "0";

                                bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                                bittd.DeActivateAll();
                                bittd.Activate(currentIndex);

                                break;
                            case TempFunctions._Carry11:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 3);

                                //get all the cells
                                bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                                currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                                bittd.ActivateAll();

                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "-1";
                                cells[3].label.text = "0";
                                cells[6].label.text = "0";
                                cells[1].label.text = "x";
                                cells[4].label.text = "x";
                                cells[7].label.text = "x";
                                cells[2].label.text = "0";
                                cells[5].label.text = "0";
                                cells[8].label.text = "0";

                                cells[9].label.text = "0";
                                cells[12].label.text = "0";
                                cells[15].label.text = "0";
                                cells[10].label.text = "x";
                                cells[13].label.text = "x";
                                cells[16].label.text = "x";
                                cells[11].label.text = "0";
                                cells[14].label.text = "0";
                                cells[17].label.text = "1";

                                cells[18].label.text = "0";
                                cells[21].label.text = "0";
                                cells[24].label.text = "1";
                                cells[19].label.text = "x";
                                cells[22].label.text = "x";
                                cells[25].label.text = "x";
                                cells[20].label.text = "0";
                                cells[23].label.text = "1";
                                cells[26].label.text = "1";



                                bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                                bittd.DeActivateAll();
                                bittd.Activate(currentIndex);
                                break;
                            case TempFunctions._Carry1:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 3);

                                //get all the cells
                                bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                                currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                                bittd.ActivateAll();

                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "0";
                                cells[2].label.text = "0";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "0";
                                cells[6].label.text = "0";
                                cells[7].label.text = "0";
                                cells[8].label.text = "0";

                                cells[9].label.text = "0";
                                cells[10].label.text = "0";
                                cells[11].label.text = "0";
                                cells[12].label.text = "0";
                                cells[13].label.text = "0";
                                cells[14].label.text = "0";
                                cells[15].label.text = "0";
                                cells[16].label.text = "0";
                                cells[17].label.text = "0";

                                cells[18].label.text = "0";
                                cells[19].label.text = "0";
                                cells[20].label.text = "1";
                                cells[21].label.text = "0";
                                cells[22].label.text = "0";
                                cells[23].label.text = "0";
                                cells[24].label.text = "0";
                                cells[25].label.text = "0";
                                cells[26].label.text = "0";

                                bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                                bittd.DeActivateAll();
                                bittd.Activate(currentIndex);
                                break;
                            case TempFunctions._Carry2:
                                //switch to correct size
                                _DETC.SetPanelSize(3, 3);

                                //get all the cells
                                bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                                currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                                bittd.ActivateAll();

                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                //fill in the cells
                                cells[0].label.text = "0";
                                cells[1].label.text = "0";
                                cells[2].label.text = "0";
                                cells[3].label.text = "0";
                                cells[4].label.text = "0";
                                cells[5].label.text = "0";
                                cells[6].label.text = "0";
                                cells[7].label.text = "0";
                                cells[8].label.text = "1";

                                cells[9].label.text = "0";
                                cells[10].label.text = "0";
                                cells[11].label.text = "0";
                                cells[12].label.text = "0";
                                cells[13].label.text = "0";
                                cells[14].label.text = "0";
                                cells[15].label.text = "0";
                                cells[16].label.text = "0";
                                cells[17].label.text = "1";

                                cells[18].label.text = "0";
                                cells[19].label.text = "0";
                                cells[20].label.text = "0";
                                cells[21].label.text = "0";
                                cells[22].label.text = "0";
                                cells[23].label.text = "0";
                                cells[24].label.text = "1";
                                cells[25].label.text = "1";
                                cells[26].label.text = "1";

                                bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                                bittd.DeActivateAll();
                                bittd.Activate(currentIndex);
                                break;

                            //this is a new style of adding TTs using Halvors indexing function. We should refactor the older ones to this new style
                            case TempFunctions._Bal_dist_measure:
                                //Balanced Distance Compare(Engdal): 8119 [102010201]\nGives the distance between the two values. 1 is no distance. 0 is a distance of one. 2 is a distance of two. Designed for use with balanced ternary.
                                //Unbalanced Distance Compare: 3936[012101210]\nAn altered version of Engdal's compare function. Gives the distance between the two values. In unbalanced ternary, it outputs the distance value.
                                //Size Compare: 7153 [100210221]\nGives which input has the highest value. An output of 0 means the first input has the bigger value, 2 is the second input. Identical values gives 1.
                                //Max or Or: 19569 [222211210]\nOutputs the highest value of the two inputs.
                                //Min or And: 16362 [211110000]\nIf BOTH inputs are 2, output is 2. If ANY input is 0, output is 0. Else, output is 1.
                                //Antimax or Nor: 15633 [210110000]\nOutputs the lowest value of the two inputs.
                                //Antimin or Nand: 4049 [012112222]If BOTH inputs are 2, output is 0. If ANY input is 0, output is 2. Else, output is 1.
                                //Xor: 4017 [012111210]\nExcluding the middle value, this functions the same way as binary XOR. If any input is 1, output is 1. If the inputs are opposites, the output is 2. If they are identical, output is 0.
                                //Balanced Sum: 5681 [021210102]\nAdds the two balanced ternary inputs together.
                                //Unbalanced Sum: 8229 [102021210]\nAdds the two unbalanced ternary inputs together.
                                //Consensus: 16401 [211111110]\nIf both inputs are 0, output is 0. If both are 2, output is 2. Else, the output is 1.
                                //Accept Anything: 18801 [221210100]\n Similar to consensus, except that if one of the outputs are 1, the other will decide the output. If both are 1, output is 1.
                                //Equality comparison: 13286 [200020002]\n If the inputs are equal, output is 2. Else, output is 0.

                                //switch to correct size
                                _DETC.SetPanelSize(3, 2);

                                //get all the cells
                                cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                                int[] ttMatrix = new int[9];
                                IntPtr srcPtr = GetTableFromIndex(8119); //if we use this method, we have to release the pointer later!
                                Marshal.Copy(srcPtr, ttMatrix, 0, 9);

                                //fill in the cells
                                for (int i = 0; i < 9; i++)
                                {
                                    //cells[i].label.text = GetTableFromIndexSingle(8119, i).ToString(); No need creating and releasing pointers
                                    cells[i].label.text = ttMatrix[i].ToString();
                                }

                                GetTableFromIndex_Release(srcPtr);

                                break;
                            default:
                                break;
                        }
                    }
            }
            else
            {
                //parse into either 1 or 3 or 9 (arity 1, arity 2 or 3)
                if (function.Length == 1 || function.Length == 3 || function.Length == 9)
                {
                    RadixOptions targetRadix = GetComponentInParent<InputControllerLogicGate>().GetRadixHack();
                    int radix = 3;
                    if (targetRadix.ToString().Contains("Binary"))
                        radix = 2;

                    if (function.Length == 1)
                    {
                        //parse into table index
                        int tIndex = ConvertHeptavintimalEncodingToArity2TableIndex(function);

                        if (tIndex != -1)
                        {
                            //only change panel size if needed, otherwise all connections are reset
                            if (_DETC.GetArity() != 1)
                                _DETC.SetPanelSize(radix, 1);

                            //get all the cells
                            BtnInputTruthTable[] cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                            int[] ttMatrix = new int[9];
                            IntPtr srcPtr = GetTableFromIndex(tIndex); //if we use this method, we have to release the pointer later!
                            Marshal.Copy(srcPtr, ttMatrix, 0, 9);

                            //fill in the cells
                            if (targetRadix.ToString().Contains("Binary"))
                            {
                                cells[0].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[6]).ToString();
                                cells[1].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[8]).ToString();
                            }
                            else
                            {
                                cells[0].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[6]).ToString();
                                cells[1].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[7]).ToString();
                                cells[2].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[8]).ToString();
                            }

                            GetTableFromIndex_Release(srcPtr);
                        }
                    }

                    if (function.Length == 3)
                    {
                        //parse into table index
                        int tIndex = ConvertHeptavintimalEncodingToArity2TableIndex(function);

                        if (tIndex != -1)
                        {
                            //only change panel size if needed, otherwise all connections are reset
                            if (_DETC.GetArity() != 2)
                                _DETC.SetPanelSize(radix, 2);

                            //get all the cells
                            BtnInputTruthTable[] cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                            int[] ttMatrix = new int[9];
                            IntPtr srcPtr = GetTableFromIndex(tIndex); //if we use this method, we have to release the pointer later!
                            Marshal.Copy(srcPtr, ttMatrix, 0, 9);

                            //fill in the cells
                            if (targetRadix.ToString().Contains("Binary"))
                            {
                                cells[0].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[0]).ToString();
                                cells[1].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[6]).ToString();
                                cells[2].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[2]).ToString();
                                cells[3].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[8]).ToString();
                            }
                            else
                            {
                                for (int i = 0; i < 9; i++)
                                {
                                    cells[i].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[i]).ToString();
                                }
                            }
                            GetTableFromIndex_Release(srcPtr);
                        }
                    }

                    if (function.Length == 9)
                    {
                        bool isValid = function.isValidHeptCode();

                        if (isValid)
                        {
                            //only change panel size if needed, otherwise all connections are reset
                            if (_DETC.GetArity() != 3)
                                _DETC.SetPanelSize(radix, 3);

                            //get all the cells
                            BtnInputTruthTableDropdown bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                            int currentIndex = bittd.GetComponent<TMP_Dropdown>().value;

                            bittd.ActivateAll();

                            //get all the cells
                            BtnInputTruthTable[] cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                            for (int i = 0; i < 3; i++)
                            {
                                string functionPart = function.Substring(function.Length - (i + 1) * 3, 3);
                                //parse into table index
                                int tIndex = ConvertHeptavintimalEncodingToArity2TableIndex(functionPart);

                                int[] ttMatrix = new int[9];
                                IntPtr srcPtr = GetTableFromIndex(tIndex); //if we use this method, we have to release the pointer later!
                                Marshal.Copy(srcPtr, ttMatrix, 0, 9);

                                //fill in the cells
                                if (targetRadix.ToString().Contains("Binary"))
                                {
                                    if (i == 0)
                                    {
                                        //0 is top left, 1 is bottom left, 2 is top right, 3 is bottom rig+ht
                                        cells[0].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[0]).ToString();
                                        cells[4].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[6]).ToString();
                                        cells[2].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[2]).ToString();
                                        cells[6].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[8]).ToString();
                                    }


                                    //we skip the middle matrix with zeros in case of binary

                                    if (i == 2)
                                    {
                                        //0 is top left, 1 is bottom left, 2 is top right, 3 is bottom rig+ht
                                        cells[1].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[0]).ToString();
                                        cells[5].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[6]).ToString();
                                        cells[3].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[2]).ToString();
                                        cells[7].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[8]).ToString();
                                    }
                                }
                                else
                                {
                                    cells[i].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[0]).ToString();
                                    cells[i + 3].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[1]).ToString();
                                    cells[i + 6].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[2]).ToString();
                                    cells[i + 9].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[3]).ToString();
                                    cells[i + 12].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[4]).ToString();
                                    cells[i + 15].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[5]).ToString();
                                    cells[i + 18].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[6]).ToString();
                                    cells[i + 21].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[7]).ToString();
                                    cells[i + 24].label.text = RadixHelper.ConvertRadixFromTo(RadixOptions.UnbalancedTernary, targetRadix, ttMatrix[8]).ToString();
                                }
                                GetTableFromIndex_Release(srcPtr);

                            }

                            bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                            bittd.DeActivateAll();
                            bittd.Activate(currentIndex);
                        }
                    }

                    GetComponentInParent<InputControllerLogicGate>().ComputeTruthTableOutput();
                }
            }
        }
    }

    internal static void CreateVerilogCircuit(string path, string fileName, List<string> inputNames, List<string> outputNames, List<string> ttIndices, List<string> connectionArray, List<int> inputOutputSizeArray, List<string> connectionPairArray, List<string> idArray)
    {
        //throw new NotImplementedException();
    }

    public static void CreateFile(string path, string[] lines)
    {
        try
        {
            // Create the file, or overwrite if the file exists.
            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (var line in lines)
                    sw.WriteLine (line);
            }
        }

        catch (Exception ex)
        {
           Debug.Log(ex.ToString());
        }
    }


    //THIS METHOD CREATED BINARY VERILOG WHATEVER THE RADIX
    public static void CreateVerilogLogicGates(string path, string optimizedTTIndex, int[] optimizedTT, RadixOptions radix, int arity)
    {
        //check if folder for verilog/logicgates has been created
        string dirPath = path + "LogicGates/";
        if (!System.IO.Directory.Exists(dirPath))
            System.IO.Directory.CreateDirectory(dirPath);

        //create if logicgate exist, else create
        string filePath;
        if (radix == RadixOptions.Binary)
            filePath = dirPath + optimizedTTIndex + ".v";
        else
            filePath = dirPath + optimizedTTIndex + "_bet" + ".v";
        
        if (!File.Exists(filePath))
        {
            string[] lines = new string[0];
            if (radix == RadixOptions.Binary)
                lines = ConvertTTtoBinaryVerilogModule(optimizedTTIndex, optimizedTT, radix, arity);
             
            if (radix == RadixOptions.BalancedTernary || radix == RadixOptions.UnbalancedTernary)
                lines = ConvertTTtoTernaryVerilogModule(optimizedTTIndex, optimizedTT, radix, arity);
            
            CreateFile(filePath, lines);
        }        
    }

    private static string[] ConvertTTtoBinaryVerilogModule(string optimizedTTIndex, int[] optimizedTT, RadixOptions radix, int arity)
    {
        List<string> lines = new List<string>(); 

        lines.Add("module " + optimizedTTIndex + " (");

        //based on arity we have 1, 2, 3 input
        switch (arity) {
            case 1: 
                lines.Add("     input wire in_0,");
                lines.Add("     output wire out_0");
                lines.Add("     );");
                lines.Add("");
                break;
            case 2: 
                lines.Add("     input wire in_0,"); 
                lines.Add("     input wire in_1,"); 
                lines.Add("     output wire out_0");
                lines.Add("     );");
                lines.Add("");
                break;
            case 3: 
                lines.Add("     input wire in_0,"); 
                lines.Add("     input wire in_1,"); 
                lines.Add("     input wire in_2,"); 
                lines.Add("     output wire out_0");
                lines.Add("     );");
                lines.Add("");
                break;
        }

        //convert TT to SumOfProduct form
        lines.Add("     assign out_0 = ");
        var sop = Convert_TT_to_BinarySumOfProduct(radix, arity, optimizedTT);
        for (int i = 0; i < sop.Length; i++)
            lines.Add(sop[i]);
           
        lines.Add("endmodule");
        lines.Add(""); //spacing between modules

        return lines.ToArray();
    }

    private static string[] Convert_TT_to_BinarySumOfProduct(RadixOptions radix, int arity, int[] optimizedTT)
    {
        //gate_in_0 = A, gate_in_1 = B, gate_in_2 = C
        //Sum of Product are the conditions for when inputs are equal to 1 (= 2 since the general TT is unbal. ternary)
        List<string> SumOfProduct = new List<string>();

        switch (arity)
        {
            case 1:
                if (optimizedTT[0] == 2)
                {
                    SumOfProduct.Add("(in_0 == 0)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[2] == 2)
                {
                    SumOfProduct.Add("(in_0 == 1)");
                    SumOfProduct.Add(" | ");
                }
                break;
            case 2:
                if (optimizedTT[0] == 2)
                {
                    SumOfProduct.Add("(in_0 == 0 & in_1 == 0)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[2] == 2)
                {
                    SumOfProduct.Add("(in_0 == 0 & in_1 == 1)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[6] == 2)
                {
                    SumOfProduct.Add("(in_0 == 1 & in_1 == 0)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[8] == 2)
                {
                    SumOfProduct.Add("(in_0 == 1 & in_1 == 1)");
                    SumOfProduct.Add(" | ");
                }
                break;
            case 3:
                if (optimizedTT[0] == 2)
                {
                    SumOfProduct.Add("(in_0 == 0 & in_1 == 0 & in_2 == 0)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[2] == 2)
                {
                    SumOfProduct.Add("(in_0 == 0 & in_1 == 1 & in_2 == 0)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[6] == 2)
                {
                    SumOfProduct.Add("(in_0 == 1 & in_1 == 0 & in_2 == 0)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[8] == 2)
                {
                    SumOfProduct.Add("(in_0 == 1 & in_1 == 1 & in_2 == 0)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[18] == 2)
                {
                    SumOfProduct.Add("(in_0 == 0 & in_1 == 0 & in_2 == 1)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[20] == 2)
                {
                    SumOfProduct.Add("(in_0 == 0 & in_1 == 1 & in_2 == 1)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[24] == 2)
                {
                    SumOfProduct.Add("(in_0 == 1 & in_1 == 0 & in_2 == 1)");
                    SumOfProduct.Add(" | ");
                }

                if (optimizedTT[26] == 2)
                {
                    SumOfProduct.Add("(in_0 == 1 & in_1 == 1 & in_2 == 1)");
                    SumOfProduct.Add(" | ");
                }
                break;
        }

        if (SumOfProduct.Count == 0)
            SumOfProduct.Add("0;");
        else
            SumOfProduct[SumOfProduct.Count - 1] = ";"; //overwrite the last symbol (a piping symbol) with a ;

        return SumOfProduct.ToArray();
    }

    private static string[] ConvertTTtoTernaryVerilogModule(string optimizedTTIndex, int[] optimizedTT, RadixOptions radix, int arity)
    {
        List<string> lines = new List<string>();

        lines.Add("module " + optimizedTTIndex + "_bet (");

        //based on arity we have 1, 2, 3 input
        switch (arity)
        {
            case 1:
                lines.Add("     input wire[1:0] in_0,");
                lines.Add("     output wire[1:0] out_0");
                lines.Add("     );");
                lines.Add("");
                break;
            case 2:
                lines.Add("     input wire[1:0] in_0,");
                lines.Add("     input wire[1:0] in_1,");
                lines.Add("     output wire[1:0] out_0");
                lines.Add("     );");
                lines.Add("");
                break;
            case 3:
                lines.Add("     input wire[1:0] in_0,");
                lines.Add("     input wire[1:0] in_1,");
                lines.Add("     input wire[1:0] in_2,");
                lines.Add("     output wire[1:0] out_0");
                lines.Add("     );");
                lines.Add("");
                break;
        }

        //convert TT to SumOfProduct form
        lines.Add("     assign out_0 = ");
        var sop = Convert_TT_to_TernaryEncodedBinarySumOfProduct(radix, arity, optimizedTT);
        for (int i = 0; i < sop.Length; i++)
            lines.Add(sop[i]);

        lines.Add("endmodule");
        lines.Add(""); //spacing between modules

        return lines.ToArray();
    }

    private static string[] Convert_TT_to_TernaryEncodedBinarySumOfProduct(RadixOptions radix, int arity, int[] optimizedTT)
    {
        //gate_in_0 = A, gate_in_1 = B, gate_in_2 = C
        //Sum of Product are the conditions for when inputs are equal to 1
        List<string> SumOfProduct = new List<string>(); //REMOVE IS TEMP

        int cellIndex = 0;
        switch (arity)
        {
            case 1:
                for (int inputA = 0; inputA < 3; inputA++)
                {
                    SumOfProduct.Add("(in_0 == 2'b" + TerToBet(inputA) + ") ? 2'b" + TerToBet(optimizedTT[cellIndex]) + " :");
                    cellIndex++;
                }
                SumOfProduct.Add("2'b00;"); //add the error state as else case

                break;
            case 2:
                for (int inputA = 0; inputA < 3; inputA++) //in arity 2 input B iterate first over B then over A
                {
                    for (int inputB = 0; inputB < 3; inputB++)
                    {
                        SumOfProduct.Add("(in_0 == 2'b" + TerToBet(inputA) + ") & (in_1 == 2'b" + TerToBet(inputB) + ") ? 2'b" + TerToBet(optimizedTT[cellIndex]) + " :");
                        cellIndex++;
                    }
                }
                SumOfProduct.Add("2'b00;"); //add the error state as else case

                break;
            case 3:
                for (int inputC = 0; inputC < 3; inputC++) //in arity 2 input B iterate first over B then over A then C
                {
                    for (int inputA = 0; inputA < 3; inputA++)
                    {
                        for (int inputB = 0; inputB < 3; inputB++)
                        {
                            SumOfProduct.Add("(in_0 == 2'b" + TerToBet(inputA) + ") & (in_1 == 2'b" + TerToBet(inputB) + ") & (in_2 == 2'b" + TerToBet(inputC) + ") ? 2'b" + TerToBet(optimizedTT[cellIndex]) + " :");
                            cellIndex++;
                        }
                    }
                }
                SumOfProduct.Add("2'b00;"); //add the error state as else case
                break;
        }

        return SumOfProduct.ToArray();
    }

    private static string TerToBet(int i)
    {
        string result = "00";

        switch (i)
        {
            case 0:
                result = "01";
                break;
            case 1:
                result = "11";
                break;
            case 2:
                result = "10";
                break;
            default:            
                result = "00";
                break;
        }

        return result;
    }

    public int ConvertHeptavintimalEncodingToArity2TableIndex(string hepCode)
    {
        int tableIndex = 0;

        //heptiventimal is a base27 encoding. Its alphabet is 0123456789ABCDEFGHKMNPRTVXZ
        //which is very suitable for ternary. One sybmol can be expressed as three ternary values, eg.
        // 0= 000, 8 = 022, 9 = 100, A=101, Z=222, etc.
        //Each hepcode in this function must contain exactly 3 symbols. If an hepcode has eg 9 symbols (arity 3) it needs to be parsed before called.
        //each symbol has 3 ternary values, representing 3 tt cell values.
        //In this function we want to convert from 3 ternary values to a digit by concating them and converting to base 10 (decimal)
        OrderedDictionary heptiventimalAlphabet = new OrderedDictionary();
        heptiventimalAlphabet.Add("0", "000");
        heptiventimalAlphabet.Add("1", "001");
        heptiventimalAlphabet.Add("2", "002");
        heptiventimalAlphabet.Add("3", "010");
        heptiventimalAlphabet.Add("4", "011");
        heptiventimalAlphabet.Add("5", "012");
        heptiventimalAlphabet.Add("6", "020");
        heptiventimalAlphabet.Add("7", "021");
        heptiventimalAlphabet.Add("8", "022");
        heptiventimalAlphabet.Add("9", "100");
        heptiventimalAlphabet.Add("A", "101");
        heptiventimalAlphabet.Add("B", "102");
        heptiventimalAlphabet.Add("C", "110");
        heptiventimalAlphabet.Add("D", "111");
        heptiventimalAlphabet.Add("E", "112");
        heptiventimalAlphabet.Add("F", "120");
        heptiventimalAlphabet.Add("G", "121");
        heptiventimalAlphabet.Add("H", "122");
        heptiventimalAlphabet.Add("K", "200"); 
        heptiventimalAlphabet.Add("M", "201");
        heptiventimalAlphabet.Add("N", "202");
        heptiventimalAlphabet.Add("P", "210");
        heptiventimalAlphabet.Add("R", "211");
        heptiventimalAlphabet.Add("T", "212");
        heptiventimalAlphabet.Add("V", "220");
        heptiventimalAlphabet.Add("X", "221");
        heptiventimalAlphabet.Add("Z", "222");

        string ternaryString = "";
            if (hepCode.isValidHeptCode())
            {
                for (int i = 0; i < hepCode.Length; i++)
                {
                    ternaryString += heptiventimalAlphabet[hepCode[i].ToString()];
                }

                //convert to digit
                for (int i = 0; i < ternaryString.Length; i++)
                {
                    int v = int.Parse(ternaryString[i].ToString());
                    tableIndex += (int)(v * Mathf.Pow(3, i));
                }
            }
            else
                tableIndex = -1; //error, this is a special code

        return tableIndex;
    }

    public static string ConvertTTtoHeptEncoding(int[] tt)
    {
        OrderedDictionary heptiventimalAlphabet = new OrderedDictionary();
        heptiventimalAlphabet.Add("000", "0");
        heptiventimalAlphabet.Add("001", "1");
        heptiventimalAlphabet.Add("002", "2");
        heptiventimalAlphabet.Add("010", "3");
        heptiventimalAlphabet.Add("011", "4");
        heptiventimalAlphabet.Add("012", "5");
        heptiventimalAlphabet.Add("020", "6");
        heptiventimalAlphabet.Add("021", "7");
        heptiventimalAlphabet.Add("022", "8");
        heptiventimalAlphabet.Add("100", "9");
        heptiventimalAlphabet.Add("101", "A");
        heptiventimalAlphabet.Add("102", "B");
        heptiventimalAlphabet.Add("110", "C");
        heptiventimalAlphabet.Add("111", "D");
        heptiventimalAlphabet.Add("112", "E");
        heptiventimalAlphabet.Add("120", "F");
        heptiventimalAlphabet.Add("121", "G");
        heptiventimalAlphabet.Add("122", "H");
        heptiventimalAlphabet.Add("200", "K");
        heptiventimalAlphabet.Add("201", "M");
        heptiventimalAlphabet.Add("202", "N");
        heptiventimalAlphabet.Add("210", "P");
        heptiventimalAlphabet.Add("211", "R");
        heptiventimalAlphabet.Add("212", "T");
        heptiventimalAlphabet.Add("220", "V");
        heptiventimalAlphabet.Add("221", "X");
        heptiventimalAlphabet.Add("222", "Z");

        string index = "";

        for (int i = tt.Length -1; i >0; i -= 3)
        {
            string hept = tt[i].ToString() + tt[i-1].ToString() + tt[i-2].ToString();
            index += heptiventimalAlphabet[hept];
        }

        return index;
    }

    public static int CreateNetlist(int mode, string path, int[] tt, int arity)
    {
        //from unoptimized tt
        return CreateNetlist(mode, path, tt, tt.Length, arity);
       
    }

    public static int[] GetOptimizedTT(int arity)
    {
        List<int> tt = new List<int>();

        switch(arity)
        {
            case 3:
                {
                    int tablesize = 27;

                    int[] ttMatrix = new int[tablesize];
                    IntPtr srcPtr = GetOptimzedTT(); //if we use this method, we have to release the pointer later!
                    Marshal.Copy(srcPtr, ttMatrix, 0, tablesize);

                    for (int j = 0; j < tablesize; j++)
                    {
                        tt.Add(ttMatrix[j]);
                    }

                    GetOptimzedTT_Release(srcPtr);
                }
                break;
            case 2:
                {
                    int tablesize = 9;

                    int[] ttMatrix = new int[tablesize];
                    IntPtr srcPtr = GetOptimzedTT(); //if we use this method, we have to release the pointer later!
                    Marshal.Copy(srcPtr, ttMatrix, 0, tablesize);

                    for (int j = 0; j < tablesize; j++)
                    {
                        tt.Add(ttMatrix[j]);
                    }

                    GetOptimzedTT_Release(srcPtr);
                }
                break;
            case 1:
                {
                    int tablesize = 3;

                    int[] ttMatrix = new int[tablesize];
                    IntPtr srcPtr = GetOptimzedTT(); //if we use this method, we have to release the pointer later!
                    Marshal.Copy(srcPtr, ttMatrix, 0, tablesize);

                    for (int j = 0; j < tablesize; j++)
                    {
                        tt.Add(ttMatrix[j]);
                    }

                    GetOptimzedTT_Release(srcPtr);
                }
                break;
        }

        return tt.ToArray();
    }
}
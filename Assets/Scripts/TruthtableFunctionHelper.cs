using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI.Extensions;
using ExtensionMethods;
using static BtnInput;

//see this presentation about marshalling data from c++ to C# and inverse
//https://www.slideshare.net/unity3d/adding-love-to-an-api-or-how-to-expose-c-in-unity
// https://github.com/lazerfalcon/Unite2018_Native/tree/master/Assets
public class TruthtableFunctionHelper : MonoBehaviour
{
    AutoCompleteComboBox _Dropdown;
    DragExpandTableComponent _DETC;
    public enum TempFunctions { //special name must be start with s
        sSum00a,
        sSum00b,
        sCarry00,
        sSum10,
        sSum11a,
        sSum01,
        sCarry01,
        sSum11b,
        sCarry11,
        sCarry21,
        sSum21a,
        sSum1a,
        sSum1b,
        sSum2a,
        sSum2b,
        sCarry1,
        sCarry2,
        sBal_dist_measure
    }

    [DllImport("CircuitGenerator", EntryPoint = "GetTableFromIndex")]
    public static extern IntPtr GetTableFromIndex(int tableIndex);

    [DllImport("CircuitGenerator", EntryPoint = "GetTableFromIndex_Release")]
    public static extern void GetTableFromIndex_Release(IntPtr ptr);

    [DllImport("CircuitGenerator", EntryPoint = "GetTableFromIndexSingle")]
    public static extern int GetTableFromIndexSingle(int tableIndex, int index);

    [DllImport("CircuitGenerator", EntryPoint = "CreateNetlist")]
    public static extern void CreateNetlist(int[] array, int length,int arity);

    [DllImport("CircuitGenerator", EntryPoint = "TestSum")]
    public static extern int TestSum(int[] array, int length);

    [DllImport("CircuitGenerator", EntryPoint = "CreateCircuit")]
    public static extern int CreateCircuit(int inputs, int outputs, int[] ttIndices, int ttIndicesLength, int[] arityArray, int arityArrayLength, string[] connectionArray, int connectionLength, int[] invArray, int invArrayLength);
    
        public void Start()
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

    }

    void DropdownValueChanged(AutoCompleteComboBox change)
    {
        var function = change.Text;
        if (function.Length > 0)
        {
            if (function[0].ToString().Equals("s"))
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


                            case TempFunctions.sSum00a:
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
                            case TempFunctions.sSum00b:
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
                            case TempFunctions.sSum01:
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
                            case TempFunctions.sCarry01:
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
                            case TempFunctions.sSum10:
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
                            case TempFunctions.sSum11a:
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
                            case TempFunctions.sSum11b:
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
                            case TempFunctions.sSum21a:
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
                            case TempFunctions.sSum1a: //checked
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
                            case TempFunctions.sSum1b: //checked
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
                            case TempFunctions.sSum2a: //checked
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
                            case TempFunctions.sSum2b: //checked
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
                            case TempFunctions.sCarry21: //checked
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
                            case TempFunctions.sCarry00:
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
                            case TempFunctions.sCarry11:
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
                            case TempFunctions.sCarry1:
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
                            case TempFunctions.sCarry2:
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
                            case TempFunctions.sBal_dist_measure:
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
                //parse into either 3 or 9 (arity 2 or 3)
                if (function.Length == 3 || function.Length == 9)
                {
                    RadixOptions targetRadix = GetComponentInParent<InputControllerLogicGate>().GetRadixTarget();
                    if (function.Length == 3)
                    {
                        //parse into table index
                        int tIndex = ConvertHeptavintimalEncodingToArity2TableIndex(function);

                        if (tIndex != -1)
                        {
                            _DETC.SetPanelSize(3, 2);

                            //get all the cells
                            BtnInputTruthTable[] cells = transform.parent.GetComponentsInChildren<BtnInputTruthTable>();

                            int[] ttMatrix = new int[9];
                            IntPtr srcPtr = GetTableFromIndex(tIndex); //if we use this method, we have to release the pointer later!
                            Marshal.Copy(srcPtr, ttMatrix, 0, 9);

                            //fill in the cells
                            for (int i = 0; i < 9; i++)
                            {
                                cells[i].label.text = ConvertToCorrectRadix(ttMatrix[i], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                            }

                            GetTableFromIndex_Release(srcPtr);


                            //Convert to netlist file
                            //int[] ttMatrix2 = new int[9];
                            //IntPtr srcPtr2 = GetTableFromIndex(tIndex); //if we use this method, we have to release the pointer later!
                            //Marshal.Copy(srcPtr2, ttMatrix, 0, 9);

                            //ttMatrix2 = Convert(ttMatrix, ttMatrix.Length);

                            //int sum = TestSum(ttMatrix, ttMatrix.Length); //if we use this method, we have to release the pointer later!
                            //Debug.Log(sum);



                            //int test = CreateNetlist(ttMatrix, ttMatrix.Length, 2);
                        }
                    }
                    else
                    {
                        bool isValid = function.isValidHeptCode();

                        if (isValid)
                        {
                            _DETC.SetPanelSize(3, 3);

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
                                cells[i].label.text = ConvertToCorrectRadix(ttMatrix[0], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                                cells[i + 3].label.text = ConvertToCorrectRadix(ttMatrix[1], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                                cells[i + 6].label.text = ConvertToCorrectRadix(ttMatrix[2], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                                cells[i + 9].label.text = ConvertToCorrectRadix(ttMatrix[3], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                                cells[i + 12].label.text = ConvertToCorrectRadix(ttMatrix[4], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                                cells[i + 15].label.text = ConvertToCorrectRadix(ttMatrix[5], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                                cells[i + 18].label.text = ConvertToCorrectRadix(ttMatrix[6], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                                cells[i + 21].label.text = ConvertToCorrectRadix(ttMatrix[7], RadixOptions.UnbalancedTernary, targetRadix).ToString();
                                cells[i + 24].label.text = ConvertToCorrectRadix(ttMatrix[8], RadixOptions.UnbalancedTernary, targetRadix).ToString();

                                GetTableFromIndex_Release(srcPtr);

                            }

                            bittd = transform.parent.GetComponentInChildren<BtnInputTruthTableDropdown>();
                            bittd.DeActivateAll();
                            bittd.Activate(currentIndex);
                        }
                    }
                }
            }
        }
    }

    public int ConvertToCorrectRadix(int _value, RadixOptions radixSource, RadixOptions radixTarget)
    {
        int outputValue = 0;
        switch (radixSource)
        {
            case RadixOptions.BalancedTernary: 
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                if (_value < 1)
                                    outputValue = 0;
                                else
                                    outputValue = 1;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                outputValue = _value + 1;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                outputValue = _value;
                            }
                            break;

                    }

                }
                break;
            case RadixOptions.UnbalancedTernary: 
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                if (_value < 2)
                                    outputValue = 0;
                                else
                                    outputValue = 1;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                outputValue = _value;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                outputValue = _value -1;
                            }
                            break;

                    }

                }
                break;
            case RadixOptions.Binary:
                {
                    switch (radixTarget)
                    {
                        case RadixOptions.Binary:
                            {
                                outputValue = _value;
                            }
                            break;
                        case RadixOptions.UnbalancedTernary:
                            {
                                if (_value == 0)
                                    outputValue = -1;
                                else
                                    outputValue = 1;
                            }
                            break;
                        case RadixOptions.BalancedTernary:
                            {
                                if (_value == 0)
                                    outputValue = 0;
                                else
                                    outputValue = 2;
                            }
                            break;
                    }
                }
                break;
        }

        return outputValue;
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
        Hashtable heptiventimalAlphabet = new Hashtable();
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

    public static void CreateNetlist(string path, int[] tt, int arity)
    {
        //we need to reshuffle the tt order if and only if arity 3
        if(arity ==3)
        {
            List<int> ttList = new List<int>();
            
            ttList.Add(tt[0]);
            ttList.Add(tt[3]);
            ttList.Add(tt[6]);
            ttList.Add(tt[9]);
            ttList.Add(tt[12]);
            ttList.Add(tt[15]);
            ttList.Add(tt[18]);
            ttList.Add(tt[21]);
            ttList.Add(tt[24]);

            ttList.Add(tt[1]);
            ttList.Add(tt[4]);
            ttList.Add(tt[7]);
            ttList.Add(tt[10]);
            ttList.Add(tt[13]);
            ttList.Add(tt[16]);
            ttList.Add(tt[19]);
            ttList.Add(tt[22]);
            ttList.Add(tt[25]);

            ttList.Add(tt[2]);
            ttList.Add(tt[5]);
            ttList.Add(tt[8]);
            ttList.Add(tt[11]);
            ttList.Add(tt[14]);
            ttList.Add(tt[17]);
            ttList.Add(tt[20]);
            ttList.Add(tt[23]);
            ttList.Add(tt[26]);

            tt = ttList.ToArray();
        }
       
        CreateNetlist(tt, tt.Length, arity);
       
    }
}
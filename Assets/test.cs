using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Text text;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern IntPtr GetTableFromIndex(int tableIndex);

    [DllImport("__Internal")]
    public static extern void GetTableFromIndex_Release(IntPtr ptr);
#else
    [DllImport("CircuitGenerator", EntryPoint = "GetTableFromIndex")]
    public static extern IntPtr GetTableFromIndex(int tableIndex);

    [DllImport("CircuitGenerator", EntryPoint = "GetTableFromIndex_Release")]
    public static extern void GetTableFromIndex_Release(IntPtr ptr);
#endif

    void Start()
    {
        int[] ttMatrix = new int[9];
        IntPtr srcPtr = GetTableFromIndex(333); //if we use this method, we have to release the pointer later!
        Marshal.Copy(srcPtr, ttMatrix, 0, 9);
        GetTableFromIndex_Release(srcPtr);

        string m = "";
        for (int i = 0; i < ttMatrix.Length; i++)
        {
            m += ttMatrix[i].ToString()+" ";
        }

        text.text = m;
    }
}

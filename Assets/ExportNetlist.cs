using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using TMPro;
using System.Text;
using System;
using SFB;
using System.Runtime.InteropServices;

public class ExportNetlist : MonoBehaviour
{
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);
#endif

    public TMP_Dropdown selectedNetlist;

    public void OnEnable()
    {
        selectedNetlist.options.Clear();

        foreach (DictionaryEntry sc in Settings.savedComponents)
        {
            SavedComponent c = (SavedComponent)sc.Value;
            selectedNetlist.options.Add(new TMP_Dropdown.OptionData() { text = c.ComponentName });
        }        
    }

    public void ExportAll()
    {
        string netlistPath = Application.persistentDataPath + "/User/Generated/";
        string filePath = Application.persistentDataPath + "/User/Share/MRCS_ExportAll.zip";
        Export(netlistPath, filePath, false);
    }

    public void ExportSelected()
    {
        string folderName = selectedNetlist.options[selectedNetlist.value].text;
        string netlistPath = Application.persistentDataPath + "/User/Generated/" + folderName;
        string filePath = Application.persistentDataPath + "/User/Share/MRCS_" + folderName  + ".zip";
        Export(netlistPath, filePath, true);
    }

    public void Export(string sourcePath, string targetPath, bool topLevel)
    {
        if (File.Exists(targetPath))
        {
            // If file found, delete it    
            File.Delete(Path.Combine(targetPath));
        }

        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/User/Share/");
        ZipFile.CreateFromDirectory(sourcePath, targetPath, System.IO.Compression.CompressionLevel.Optimal, topLevel, Encoding.UTF8);

#if UNITY_WEBGL && !UNITY_EDITOR
            //we need to flush the result to disk
            //https://forum.unity.com/threads/how-does-saving-work-in-webgl.390385/
            //Application.ExternalEval("_JS_FileSystem_Sync();");
            
            var bytes = System.IO.File.ReadAllBytes(targetPath);
            var callBack = FindObjectOfType<applicationmanager>();
            DownloadFile(callBack.name, "OnFileDownload", "MRCS_export.zip", bytes, bytes.Length);
#endif
    }
}

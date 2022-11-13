using System.Collections;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
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
    public TMP_Dropdown selectedFormat;

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
        string netlistPath = Application.persistentDataPath + "/User/Generated/" ;
        string filePath = Application.persistentDataPath + "/User/Share/MRCS_Export_Library.zip";
        Export("Library", netlistPath, filePath, true);
    }

    public void ExportSelected()
    {
        string folderName = selectedNetlist.options[selectedNetlist.value].text;
        string netlistPath = Application.persistentDataPath + "/User/Generated/" + folderName;

        //create temp dir
        string tempFolder = Application.persistentDataPath + "/Temp/" + folderName;
        string tempPath = Application.persistentDataPath + "/Temp/";
        Directory.CreateDirectory(tempFolder);

        //copy selected folder
        var dir = new DirectoryInfo(netlistPath);
        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(tempFolder, file.Name);
            file.CopyTo(targetFilePath);
        }

        //export
        string filePath = Application.persistentDataPath + "/User/Share/MRCS_Export_" + folderName  + ".zip";
        Export(folderName, tempPath, filePath, true);

        //remove temp folder
        Directory.Delete(tempPath,true);
    }

    public void Export(string foldername, string sourcePath, string targetPath, bool recursive)
    {
        if (File.Exists(targetPath))
        {
            // If file found, delete it    
            File.Delete(Path.Combine(targetPath));
        }

        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/User/Share/");
        try
        {
            //we previously used system.io.compression, but since U2022 there seems to be a bug in the deflate zstream constructor
            FastZip fastZip = new FastZip();
            fastZip.CreateEmptyDirectories = false;
            fastZip.CreateZip(targetPath, sourcePath, true, "");
   }
        catch (IOException e)
        {
            Debug.LogException(e);
        }

#if UNITY_WEBGL && !UNITY_EDITOR
            //we need to flush the result to disk
            //https://forum.unity.com/threads/how-does-saving-work-in-webgl.390385/
            //Application.ExternalEval("_JS_FileSystem_Sync();");
            
            var bytes = System.IO.File.ReadAllBytes(targetPath);
            var callBack = FindObjectOfType<applicationmanager>();
            var filename = "MRCS_export_" + foldername + ".zip";
            DownloadFile(callBack.name, "OnFileDownload", filename, bytes, bytes.Length);
#endif
    }
}

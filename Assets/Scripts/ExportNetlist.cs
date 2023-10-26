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
    Dictionary<string, string> FoundCircuits = new Dictionary<string, string>();

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
        string filePath = Application.persistentDataPath + "/User/Share/MRCS_Export_Library.zip";
        Export("Library", netlistPath, filePath, true);
    }

    public void ExportSelected()
    {
        string folderName = selectedNetlist.options[selectedNetlist.value].text;
        string circuitPath = Application.persistentDataPath + "/User/Generated/" + folderName;
        string hspicePath = circuitPath + "/HSPICE/";        
        FoundCircuits.Clear();
        FoundCircuits.Add(folderName, folderName); //add the root circuit

        //create temp dir
        string tempPath = Application.persistentDataPath + "/Temp/";
        Directory.CreateDirectory(tempPath);

        //find the selected circuit and its dependencies using recursive search with global variable to prevent circular dependencies
        var dir = new DirectoryInfo(hspicePath);
        FindCircuitDependencies(dir);

        //copy folders
        foreach (var key in FoundCircuits)
        {
            string sourceFolder = Application.persistentDataPath + "/User/Generated/" + key.Value;
            string targetFolder = tempPath + key.Value;

            ExtensionMethods.FileHelpers.CopyDirectory(sourceFolder, targetFolder, true);
        }

        ////export
        string filePath = Application.persistentDataPath + "/User/Share/MRCS_Export_" + folderName + ".zip";
        Export(folderName, tempPath, filePath, true);

        //remove temp folder
        Directory.Delete(tempPath,true);
    }

    private void FindCircuitDependencies(DirectoryInfo dir)
    {
        List<string> circuits = new List<string>();
        foreach (FileInfo file in dir.GetFiles())
        {
            //find all circuits in directory
            if (file.Extension == ".sp" && file.Name[0] == 'c')
            {
                //if circuit not in list add to list and continue recursive search
                string c_raw = Path.GetFileNameWithoutExtension(file.FullName);
                string c = c_raw.Substring(2);
                if (FoundCircuits.ContainsKey(c) == false)
                {
                    FoundCircuits.Add(c, c);

                    dir = new DirectoryInfo(Application.persistentDataPath + "/User/Generated/" + c + "/HSPICE/");
                    FindCircuitDependencies(dir);
                }
            }   
        }
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

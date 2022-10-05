using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using static BtnInput;
using System;



[RequireComponent(typeof(Button))]
public class ImportNetlist : MonoBehaviour, IPointerDownHandler
{
#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".zip", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutine(url));
    }
#else

    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Select netlist file or library", "", "zip", false);
        if (paths.Length > 0)
        {
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }
#endif

    private IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);
        yield return loader;

        string extractPath = Application.persistentDataPath + "/User/Unzipped/";

#if UNITY_WEBGL && !UNITY_EDITOR

         //new directory
        Directory.CreateDirectory(extractPath);

        //save zip file
        System.IO.File.WriteAllBytes(extractPath + "upload.zip", loader.bytes);
                
        //extract
        FastZip fastZip = new FastZip();
        fastZip.ExtractZip(extractPath + "upload.zip", extractPath, "");
    
        //check amount of circuits found
        var directories = System.IO.Directory.GetDirectories(extractPath, "*", SearchOption.TopDirectoryOnly);
        Debug.Log("Found for import: " + directories.Length.ToString());

        for (int i = 0; i < directories.Length; i++)
        {
            var circuitName = new DirectoryInfo(System.IO.Path.GetDirectoryName(directories[i]+"/")).Name;
            
            //move folder to generated
            string generatedPath = Application.persistentDataPath + "/User/Generated/" + circuitName;
            string generatedFilePath = generatedPath + "/c_" + circuitName + ".sp";
            Debug.Log("Saved at: " + generatedFilePath);

            bool exists = System.IO.Directory.Exists(generatedPath);
            if (exists)
            {
                //remove 
                System.IO.Directory.Delete(generatedPath, true);
                Debug.Log("Overwritten folder!");
            }

            //needed?
            //Application.ExternalEval("_JS_FileSystem_Sync();");
            
            DirectoryCopy(directories[i], generatedPath, true);
         
            var result = ComponentGenerator.FindComponentsInNetlist(generatedFilePath, circuitName);                        
            
            result.savedComponent.ComponentName = circuitName;
            result.savedComponent.ComponentNetlistPath = generatedFilePath;            
            var sc = GameObject.FindObjectOfType<SaveCircuit>();

            sc.GenerateMenuItem(result.savedComponent, sc.ContentContainer.transform);             
            Settings.Save(result.savedComponent);
            Debug.Log("Import completed");
        }
#else

        //clear directory
        bool DirExists = System.IO.Directory.Exists(extractPath);
        if (DirExists)
            Directory.Delete(extractPath, true);
        
        //new directory
        Directory.CreateDirectory(extractPath);

        //save zip file
        System.IO.File.WriteAllBytes(extractPath + "upload.zip", loader.bytes);

        //extract
        FastZip fastZip = new FastZip();
        fastZip.ExtractZip(extractPath + "upload.zip", extractPath, "");
        
        //check amount of circuits found
        var directories = System.IO.Directory.GetDirectories(extractPath, "*", SearchOption.TopDirectoryOnly);

        //for every circuit update the UI list and copy to /generated
        for (int i = 0; i < directories.Length; i++)
        {
            var circuitName = new DirectoryInfo(System.IO.Path.GetDirectoryName(directories[i]+"/")).Name;
          
            //move folder to generated
            string generatedPath = Application.persistentDataPath + "/User/Generated/" + circuitName;
            string generatedFilePath = generatedPath + "/c_" + circuitName + ".sp";

            bool exists = System.IO.Directory.Exists(generatedPath);
            if (exists)
            {
                //remove 
                System.IO.Directory.Delete(generatedPath, true);
                Debug.Log("Overwritten folder!");
            }
                        
            Directory.Move(directories[i], generatedPath);
            var result = ComponentGenerator.FindComponentsInNetlist(generatedFilePath, circuitName);
           
            result.savedComponent.ComponentName = circuitName;
            result.savedComponent.ComponentNetlistPath = generatedFilePath;
            var sc = GameObject.FindObjectOfType<SaveCircuit>();

            sc.GenerateMenuItem(result.savedComponent, sc.ContentContainer.transform);
            Settings.Save(result.savedComponent);     
        }
#endif
    }

    //from: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
    //used as workaround instead of Directory.Move in WebGL
    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();

        // If the destination directory doesn't exist, create it.       
        Directory.CreateDirectory(destDirName);

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
            }
        }
    }
}

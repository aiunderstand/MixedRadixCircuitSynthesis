using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using System.IO;
using System.IO.Compression;
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
        UploadFile(gameObject.name, "OnFileUpload", ".txt", false);
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
        
        //clear directory
        bool DirExists = System.IO.Directory.Exists(extractPath);
        if (DirExists)
            Directory.Delete(extractPath, true);
        
        //new directory
        Directory.CreateDirectory(extractPath);

        //save zip file
        System.IO.File.WriteAllBytes(extractPath + "upload.zip", loader.bytes);

        //extract
        ZipFile.ExtractToDirectory(extractPath + "upload.zip", extractPath);

        //check amount of circuits found
        var directories = System.IO.Directory.GetDirectories(extractPath, "*", SearchOption.TopDirectoryOnly);

        //for every circuit update the UI list and copy to /generated
        applicationmanager.InitHack = new List<GameObject>();
        for (int i = 0; i < directories.Length; i++)
        {
            var circuitName = new DirectoryInfo(System.IO.Path.GetDirectoryName(directories[i]+"/")).Name;

            string unzipPath = Application.persistentDataPath + "/User/Unzipped/" + circuitName;
            string mainNetlistPath = unzipPath + "/" + "c_" + circuitName + ".sp";

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

            var go = sc.GenerateItem(result.savedComponent, sc.ContentContainer.transform, false);
            
            go.GetComponent<DragDrop>().MenuVersion.SetActive(true);
            //go.GetComponent<DragDrop>().FullVersion.SetActive(false);
            //go.name = circuitName;
            Settings.Save(result.savedComponent);
        }

        applicationmanager.clearInitHack = true;

    }
}

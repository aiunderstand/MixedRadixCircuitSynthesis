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
using static ReaderCSV;

[RequireComponent(typeof(Button))]
public class VerifyLogic : MonoBehaviour, IPointerDownHandler
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
        var paths = StandaloneFileBrowser.OpenFilePanel("Select test file", "", "csv", false);
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

        string extractPath = Application.persistentDataPath + "/User/Tests/";
        string fullPath = extractPath + "upload.csv";

        //clear directory
        bool DirExists = System.IO.Directory.Exists(extractPath);
        if (DirExists)
            Directory.Delete(extractPath, true);

        //new directory
        Directory.CreateDirectory(extractPath);

        System.IO.File.WriteAllBytes(fullPath, loader.bytes);
        ReaderCSV.ReadCSV(fullPath);
        GameObject.FindObjectOfType<LogicTestRunner>().RunTests();


    }
}

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
public class LoadNetlists : MonoBehaviour, IPointerDownHandler
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
        for (int i = 0; i < directories.Length; i++)
        {
            var circuitName = new DirectoryInfo(System.IO.Path.GetDirectoryName(directories[i]+"/")).Name;

            string unzipPath = Application.persistentDataPath + "/User/Unzipped/" + circuitName;
            string mainNetlistPath = unzipPath + "/" + circuitName + ".sp";

            //ADD preview
            List<string> netlistAttributes = new List<string>();
            string line;
            using (StreamReader reader = new StreamReader(mainNetlistPath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains ("*** @"))
                        netlistAttributes.Add(line);
                }
            }

            SavedComponent c;
            Stats s;
            List<LogicGate> lgs;
            ParseIntoSavedComponent(netlistAttributes, out c, out s, out lgs);
            c.ComponentName = circuitName;
            c.ComponentNetlistPath = mainNetlistPath;
            c.Stats = s;

            GameObject.FindObjectOfType<SaveCircuit>().GenerateListItem(c);

            //move folder to generated
            string generatedPath = Application.persistentDataPath + "/User/Generated/" + circuitName;
            Directory.Move(directories[i], generatedPath);

            Settings.Save(c);
        }
    }

    private void ParseIntoSavedComponent(List<string> netlistAttributes, out SavedComponent sc, out Stats s, out List<LogicGate> lgs)
    {
        sc = new SavedComponent();
        s = new Stats();
        lgs = new List<LogicGate>();

        sc.Inputs = new List<BtnInput.RadixOptions>();
        sc.InputLabels = new List<string>();
        sc.Outputs = new List<BtnInput.RadixOptions>();
        sc.OutputLabels = new List<string>();
       
        for (int i = 0; i < netlistAttributes.Count; i++)
        {
            var parts = netlistAttributes[i].TrimEnd(' ').Split(' ');
            switch (parts[1])
            {
                case "@tcount":
                    {
                        s.transistorCount = int.Parse(parts[2]);
                    }
                    break;
                case "@gcount":
                    {
                        s.totalLogicGateCount = int.Parse(parts[2]);
                    }
                    break;
                case "@ugcount":
                    {
                        s.uniqueLogicGateCount = int.Parse(parts[2]);
                    }
                    break;
                case "@abslvl":
                    {
                        s.abstractionLevelCount = int.Parse(parts[2]);
                    }
                    break;
                case "@inputs":
                    {
                        for (int j = 2; j < parts.Length; j++)
                        {
                            var x = (RadixOptions)Enum.Parse(typeof(RadixOptions), parts[j]);
                            sc.Inputs.Add(x);
                        }                        
                    }
                    break;
                case "@inputlbl":
                    {
                        for (int j = 2; j < parts.Length; j++)
                        {
                            var parsedNames = parts[j].Split('_');
                            sc.InputLabels.Add(parsedNames[0]);
                        }
                    }
                    break;
                case "@outputs":
                    {
                        for (int j = 2; j < parts.Length; j++)
                        {
                            var x = (RadixOptions)Enum.Parse(typeof(RadixOptions), parts[j]);
                            sc.Outputs.Add(x);
                        }
                    }
                    break;
                case "@outputlbl":
                    {
                        for (int j = 2; j < parts.Length; j++)
                        {
                            var parsedNames = parts[j].Split('_');
                            sc.OutputLabels.Add(parsedNames[0]);
                        }
                    }
                    break;
                case "@f":
                    {
                        LogicGate l = new LogicGate();
                        l.FunctionIndex = parts[2]; //function
                        i++;
                        parts = netlistAttributes[i].Split(' ');
                        l.Arity = int.Parse(parts[2]); //arity
                        i++;
                        parts = netlistAttributes[i].Split(' '); 
                        l.Position2d = new Vector2(float.Parse(parts[2]), float.Parse(parts[3])); //pos2d
                        i++;
                        parts = netlistAttributes[i].Split(' ');
                        l.Connections = new List<string>();
                        for (int j = 2; j < 6; j++)
                        {
                            l.Connections.Add(parts[j]); //connections
                        }
                    }
                    break;
            }
        }
    }
}

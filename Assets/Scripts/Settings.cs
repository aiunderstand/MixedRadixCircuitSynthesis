using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;
using static BtnInput;

public class Settings : MonoBehaviour
{
    public static bool loadingDone = false;
    public static string settingsPath;
    public static OrderedDictionary savedComponents = new OrderedDictionary();
    public GameObject ContentContainerMenuItems;

    [HideInInspector]
    public SaveCircuit saveCircuit;

    public void Start()
    {
        saveCircuit = GameObject.FindObjectOfType<SaveCircuit>();

        var settingsDirectory = Application.persistentDataPath + "/User/Settings/";
       
        bool DirExists = System.IO.Directory.Exists(settingsDirectory);
        if (!DirExists)
            System.IO.Directory.CreateDirectory(settingsDirectory);

        settingsPath += settingsDirectory + "library.csv";
        bool FileExist = System.IO.File.Exists(settingsPath);
        Debug.Log("Exist: " + FileExist);

        if (FileExist)
            LoadSavedComponents();
       
        loadingDone = true;

        GenerateMenuItems();
    }


    public void GenerateMenuItems()
    {
        ClearMenuItems();

        var input = GameObject.Instantiate(CircuitGenerator.InputPrefab);
        var output = GameObject.Instantiate(CircuitGenerator.OutputPrefab);
        var clock = GameObject.Instantiate(CircuitGenerator.ClockPrefab);
        var gate = GameObject.Instantiate(CircuitGenerator.LogicGatePrefab);

        input.transform.SetParent(ContentContainerMenuItems.transform);
        input.transform.localScale = new Vector3(1f, 1f, 1f);
        output.transform.SetParent(ContentContainerMenuItems.transform);
        output.transform.localScale = new Vector3(1f, 1f, 1f);
        clock.transform.SetParent(ContentContainerMenuItems.transform);
        clock.transform.localScale = new Vector3(1f, 1f, 1f);
        gate.transform.SetParent(ContentContainerMenuItems.transform);
        gate.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ClearMenuItems()
    {
        var gos = ContentContainerMenuItems.GetComponentsInChildren<DragDrop>();

        for (int i = 0; i < gos.Length; i++)
          {
            var go = gos[i].gameObject;
            Destroy(go);
        }
    }

    private void LoadSavedComponents()
    {
        string line;
        using (StreamReader reader = new StreamReader(settingsPath))
        {
            //read version
            var version = reader.ReadLine();

            if (version.Contains("1.0"))
            {
                //read library entries
                while ((line = reader.ReadLine()) != null)
                {

                    var parts = line.Split(';');
                    string name = parts[0];
                    Debug.Log("Load :" + name);

                    string generatedPath = Application.persistentDataPath + "/User/Generated/" + name;
                    string generatedFilePath = generatedPath + "/c_" + name + ".sp";

                    var result = ComponentGenerator.FindComponentsInNetlist(generatedFilePath, name);
                 
                    var go = saveCircuit.GenerateMenuItem(result.savedComponent, saveCircuit.ContentContainer.transform);
                    savedComponents.Add(result.savedComponent.ComponentName, result.savedComponent);
                    go.name = name;
                }
            }
        }
    }

    public static void Save(SavedComponent component)
    {
        //add to discitonary
        if (savedComponents.Contains(component.ComponentName))
            savedComponents[component.ComponentName] = component;
        else
            savedComponents.Add(component.ComponentName, component);

        SaveToDisk();
    }

    public static void Remove(string name)
    {
        //find component in hash
        savedComponents.Remove(name);

        SaveToDisk();
    }

    public static void SaveToDisk()
    {
        using (StreamWriter writer = new StreamWriter(settingsPath, false))
        {
            //version
            writer.WriteLine("Library version: 1.0;");

            //library entries
            foreach (DictionaryEntry sc in savedComponents)
            {
                SavedComponent c =(SavedComponent) sc.Value;
                writer.WriteLine(c.ComponentName + ";");
            }
        }

        #if UNITY_WEBGL && !UNITY_EDITOR
        //we need to flush the result to disk
        //https://forum.unity.com/threads/how-does-saving-work-in-webgl.390385/
        Application.ExternalEval("_JS_FileSystem_Sync();");
        #endif

    }
}

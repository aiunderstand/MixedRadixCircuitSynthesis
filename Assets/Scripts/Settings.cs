using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static BtnInput;

public class Settings : MonoBehaviour
{
    public static bool loadingDone = false;
    public static string settingsPath;
    public static Hashtable savedComponents;

    [HideInInspector]
    public SaveCircuit saveCircuit;

    public void Awake()
    {
        saveCircuit = GameObject.FindObjectOfType<SaveCircuit>();

        var settingsDirectory = Application.persistentDataPath + "/User/Settings/";

        bool DirExists = System.IO.Directory.Exists(settingsDirectory);
        if (!DirExists)
            System.IO.Directory.CreateDirectory(settingsDirectory);

        settingsPath += settingsDirectory + "savedcomponents.csv";
        bool FileExist = System.IO.File.Exists(settingsPath);       

        if (!FileExist)
        { 
            loadingDone = true;
            savedComponents = new Hashtable();
        }
        else
        {
            savedComponents = LoadSavedComponents();
            loadingDone = true;
        }
    }

    private Hashtable LoadSavedComponents()
    {
        Hashtable t = new Hashtable();

        string line;
        using (StreamReader reader = new StreamReader(settingsPath))
        {
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(';');
                int index = 0;
                string name = parts[index++];
                string netlistPath = parts[index++];

                int inputCount = int.Parse(parts[index++]);
                List<RadixOptions> inputs = new List<RadixOptions>();
                for (int i = 0; i < inputCount; i++)
                {
                    inputs.Add((RadixOptions) Enum.Parse(typeof(RadixOptions),parts[index++]));
                }

                int inputLabelCount = int.Parse(parts[index++]);
                List<string> inputLabels = new List<string>();
                for (int i = 0; i < inputLabelCount; i++)
                {
                    inputLabels.Add(parts[index++]);
                }
             
                int outputCount = int.Parse(parts[index++]);
                List<RadixOptions> outputs = new List<RadixOptions>();
                for (int i = 0; i < outputCount; i++)
                {
                    outputs.Add((RadixOptions)Enum.Parse(typeof(RadixOptions), parts[index++]));
                }

                int outputLabelCount = int.Parse(parts[index++]);
                List<string> outputLabels = new List<string>();
                for (int i = 0; i < outputLabelCount; i++)
                {
                    outputLabels.Add(parts[index++]);
                }

                SavedComponent component = new SavedComponent(inputs, inputLabels, outputs, outputLabels);
                component.ComponentName = name;
                component.ComponentNetlistPath = netlistPath;
                t.Add(name, component);
                saveCircuit.GenerateListItem(component);
            }

            return t;
        }
    }

    public static void Save(SavedComponent component)
    {
        //add to discitonary
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
            foreach (DictionaryEntry sc in savedComponents)
            {
                SavedComponent c =(SavedComponent) sc.Value;
                string line = "";
                line += (c.ComponentName + ";");
                line += (c.ComponentNetlistPath + ";");
                line += (c.Inputs.Count + ";");
                foreach (var i in c.Inputs)
                    line += i + ";";
                line += (c.InputLabels.Count + ";"); //not really neccesarry because alwasy the same
                foreach (var i in c.InputLabels)
                    line += i + ";";
                line += (c.Outputs.Count + ";");
                foreach (var i in c.Outputs)
                    line += i + ";";
                line += (c.OutputLabels.Count + ";");
                foreach (var i in c.OutputLabels)
                    line += i + ";";

                writer.WriteLine(line);
            }
        }
    }
}

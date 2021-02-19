using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO.Compression;

public class Statistics : MonoBehaviour
{
    public TextMeshProUGUI transistorCount;
    public TextMeshProUGUI uniqueLogicGateCount;
    public TextMeshProUGUI totalLogicGateCount;
    public TextMeshProUGUI abstractionLevelCount;

    [HideInInspector]
    public string NetlistPath;
    [HideInInspector]
    public string NetlistName;

    public void ExportCircuitAsZip()
    {

        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/User/Share/");
        ZipFile.CreateFromDirectory(NetlistPath, Application.persistentDataPath + "/User/Share/" + NetlistName + ".zip");  
    }
}

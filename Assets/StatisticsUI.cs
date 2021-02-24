using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO.Compression;
using System.IO;

public class StatisticsUI : MonoBehaviour
{
    public TextMeshProUGUI transistorCount;
    public TextMeshProUGUI uniqueLogicGateCount;
    public TextMeshProUGUI totalLogicGateCount;
    public TextMeshProUGUI abstractionLevelCount;
    public TextMeshProUGUI netlistName;

    public void Show(Stats stats)
    {
        transistorCount.text = stats.transistorCount.ToString();
        abstractionLevelCount.text = stats.abstractionLevelCount.ToString();
        totalLogicGateCount.text = stats.totalLogicGateCount.ToString();
        uniqueLogicGateCount.text = stats.uniqueLogicGateCount.ToString();
        netlistName.text = stats.netlistName;
        gameObject.SetActive(true);
    }
}

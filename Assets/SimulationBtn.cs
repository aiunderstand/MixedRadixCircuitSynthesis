using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SimulationBtn : MonoBehaviour
{
    public TextMeshProUGUI label;

    public void Start()
    {
        UpdateLabel();
    }

    public void UpdateLabel() {
        if (SimulationManager.Instance.State == SimulationManager.SimulationStates.IsRunning)
            label.text = "Stop Simulation";
        else
            label.text = "Start Simulation";
    }
   
}

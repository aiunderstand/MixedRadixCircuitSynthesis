using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationManager : Singleton<SimulationManager>
{
    public enum SimulationStates
    {
        IsRunning,
        HasStopped
    }

    public SimulationStates State = SimulationStates.IsRunning; //this should be a flag and the states should be IsRunning or HasStopped or HasStoppedWithError
    public bool EnableDebugConsoleOutput = false;
    [HideInInspector]
    public long FrameCounter = 0;
    public int stateChangeThreshold = 10;
    public bool AllowUnstableCircuits = false;
    bool once = false;
    public void ResetSimulator(bool clearHeatmap = false )
    {
        FrameCounter = 0;
        once = false;

        if (EnableDebugConsoleOutput)
            Debug.Log("------new sim------");

        //get all logic gates that are non-saved component and the saved components
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");
        foreach (var c in components)
        {
            if (c.name.Contains("LogicGate"))
            {
                c.GetComponent<DragDrop>().FullVersion.transform.GetChild(0).GetComponent<InputControllerLogicGate>().resetStateChangeCounter();

                if (clearHeatmap)
                    c.GetComponent<DragDrop>().FullVersion.transform.GetChild(0).GetComponent<InputControllerLogicGate>().ClearHeatmap();

            }

            if (c.name.Contains("SavedGate"))
            {
                var logicgateList = c.GetComponentsInChildren<InputControllerLogicGate>(true);

                foreach( var lg in logicgateList)
                {
                    lg.resetStateChangeCounter();

                    if (clearHeatmap)
                       lg.ClearHeatmap();
                }
            }
        }
    }

    public void SetSimulationTo(SimulationStates state)
    {
        State = state;
    }

    public void ToggleSimulation()
    {
        if (State == SimulationStates.IsRunning)
            State = SimulationStates.HasStopped;
        else
        {
            State = SimulationStates.IsRunning;
            ResetSimulator(true);
        }
    }

    //public void ShowDebugInfo(bool state)
    //{
    //    if (state)
    //    {
    //        var components = GameObject.FindGameObjectsWithTag("DnDComponent");
    //        foreach (var c in components)
    //        {
    //            if (c.name.Contains("Input"))
    //            {
    //                var inputControler = c.GetComponentInChildren<InputController>();
    //                string id = inputControler.GetInstanceID().ToString();

    //                for (int i = 0; i < inputControler.Buttons.Count; i++)
    //                {
    //                    var bi = inputControler.Buttons[i].GetComponent<BtnInput>();

    //                    //check if connected
    //                    if (bi.Connections.Count > 0)
    //                    {
    //                        string portLabel = inputControler.Buttons[i].GetComponentInChildren<TMP_InputField>().text;
    //                        string value = inputControler.Buttons[i].GetComponentInChildren<BtnInput>().label.text;
    //                        Debug.Log(portLabel + " : " + value);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public void LateUpdate()
    {
        FrameCounter++;
    }

    public bool CheckForUnstabilty(int stateChangeCounter)
    {
        bool canContinue = true;

        if (State == SimulationStates.IsRunning)
        {
            if (!AllowUnstableCircuits)
            {
                if (stateChangeCounter >= stateChangeThreshold)
                    canContinue = false;

                if (!canContinue && !once)
                {
                    once = true;
                    MessageManager.Instance.Show("Error", "Simulation stopped: circuit did not reach stable output state after 10 changes");
                }
            }
        }
        else
        {
            canContinue = false;
        }

        return canContinue;
    }
}

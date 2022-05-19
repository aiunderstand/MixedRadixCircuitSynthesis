using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationManager : Singleton<SimulationManager>
{
    public bool SimulationIsEnabled = true; //this should be a flag and the states should be IsRunning or HasStopped or HasStoppedWithError
    public bool DebugIsEnabled = false;
    public void ResetCounters()
    {
        if (DebugIsEnabled)
            Debug.Log("------new sim------");

        //get all logic gates that are non-saved component and the saved components
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");
        foreach (var c in components)
        {
            if (c.name.Contains("LogicGate"))
            {
                c.GetComponent<DragDrop>().FullVersion.transform.GetChild(0).GetComponent<InputControllerLogicGate>().resetStateChangeCounter();
            }

            if (c.name.Contains("SavedGate"))
            {
                var logicgateList = c.GetComponentsInChildren<InputControllerLogicGate>(true);

                foreach( var lg in logicgateList)
                {
                    lg.resetStateChangeCounter();
                }
            }
        }
    }

    public void SetSimulationTo(bool state)
    {
        SimulationIsEnabled = state;
    }

    public void ShowDebugInfo(bool state)
    {
        DebugIsEnabled = state;

        if (state)
        {
            var components = GameObject.FindGameObjectsWithTag("DnDComponent");
            foreach (var c in components)
            {
                if (c.name.Contains("Input"))
                {
                    var inputControler = c.GetComponentInChildren<InputController>();
                    string id = inputControler.GetInstanceID().ToString();

                    for (int i = 0; i < inputControler.Buttons.Count; i++)
                    {
                        var bi = inputControler.Buttons[i].GetComponent<BtnInput>();

                        //check if connected
                        if (bi.Connections.Count > 0)
                        {
                            string portLabel = inputControler.Buttons[i].GetComponentInChildren<TMP_InputField>().text;
                            string value = inputControler.Buttons[i].GetComponentInChildren<BtnInput>().label.text;
                            Debug.Log(portLabel + " : " + value);
                        }
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : Singleton<SimulationManager>
{
    public void ResetCounters()
    {
        //get all logic gates that are non-saved component and the saved components
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");
        foreach (var c in components)
        {
            if (c.name.Contains("LogicGate"))
            {
                c.GetComponentInChildren<InputControllerLogicGate>().resetStateChangeCounter();
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
}

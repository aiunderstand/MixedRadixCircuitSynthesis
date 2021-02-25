using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class applicationmanager : MonoBehaviour
{
    public bool useBigEndianForLogicGates = true;
    public static int abstractionLevel = 0;
    int minLevel = 0;
    int maxLevel = 0;
    public static GameObject curSelectedComponent;

    public static bool UseBigEndianForLogicGates()
    {
        var m = GameObject.FindObjectOfType<applicationmanager>();
        return m.useBigEndianForLogicGates;
    }

    public static void ClearCanvas()
    {
        //get all connections and truth tables
        var connections = GameObject.FindGameObjectsWithTag("Wire");
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");

        for (int i = 0; i < connections.Length; i++)
        {
            Destroy(connections[i]);
        }

        for (int i = 0; i < components.Length; i++)
        {
            Destroy(components[i]);
        }
    }

    public void Update()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        bool abstractionLevelChanged = false;
        if (scrollDelta > 0)
        {
            if (abstractionLevel < maxLevel)
            {
                abstractionLevel++;
                abstractionLevelChanged = true;
            }
        }

        if (scrollDelta < 0)
        {
            if (abstractionLevel > minLevel)
            {
                abstractionLevel--;
                abstractionLevelChanged = true;
            }
        }

        if (abstractionLevelChanged)
        {
            ////get all logic gates (we can optimize this by registering this call to a manager instead of searching for this every event)
            //var components = GameObject.FindGameObjectsWithTag("DnDComponent");
            //foreach (var c in components)
            //{
            //    if (c.name.Contains("LogicGate"))
            //        c.GetComponent<DragDrop>().SetAbstractionLevelTo(abstractionLevel);
            //}

        }
    }
}

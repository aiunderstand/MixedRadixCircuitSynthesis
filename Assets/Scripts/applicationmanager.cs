using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class applicationmanager : MonoBehaviour
{
    public bool useBigEndianForLogicGates = true;
    public static int abstractionLevel = 1;
    int minLevel = 1;
    int maxLevel = 3;
    public static bool UseBigEndianForLogicGates()
    {
        var m = GameObject.FindObjectOfType<applicationmanager>();
        return m.useBigEndianForLogicGates;
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

        if(abstractionLevelChanged)
        {
            //get all logic gates (we can optimize this by registering this call to a manager instead of searching for this every event)
            var components =GameObject.FindGameObjectsWithTag("DnDComponent");
            foreach (var c in components)
            {
                if (c.name.Contains("LogicGate"))
                    c.GetComponent<DragDrop>().SetAbstractionLevelTo(abstractionLevel);
            }

        }
    }
}
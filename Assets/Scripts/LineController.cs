using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class LineController : MonoBehaviour, IPointerUpHandler
{
    public bool isInput = false;
    
    public void OnPointerUp(PointerEventData eventData)
    {
        //this check enforces that a connection starts with input, and the second one is output
        if (checkValidConnectionRule())
        {
            ConnectionData data = new ConnectionData();
            data.IsInput = isInput;
            data.ConnectionTerminal = this.transform.parent.GetComponent<BtnInput>();

            EventParam eventParam = new EventParam();
            eventParam.ConnectionData = data;
            EventManager.TriggerEvent("ConnectionData", eventParam);
        }
    }

    private bool checkValidConnectionRule()
    {
        //rule 1: first terminal must be input 
        if (!LineManager.IsDrawing && isInput)
        {
            return true;
        }

        //rule 2: second terminal must be output 
        if (LineManager.IsDrawing && !isInput)
       {
            return true;
       }

        return false;
    }
}

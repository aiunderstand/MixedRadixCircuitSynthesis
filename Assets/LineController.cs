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
            ConnectionData data = new ConnectionData();
            data.IsInput = isInput;
            data.ConnectionTerminal = this.gameObject;

            EventParam eventParam = new EventParam();
            eventParam.ConnectionData = data;
            EventManager.TriggerEvent("ConnectionData", eventParam);
    }
}

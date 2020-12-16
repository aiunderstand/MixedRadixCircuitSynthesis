using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class DeleteZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color panelColorDefault;
    public Color panelColorActive;
    public Image panelColor;

    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        //highlight panel
        panelColor.color = panelColorActive;

        //let drag control know this is a delete drop zone
        EventParam eventParam = new EventParam();
        eventParam.IsDeleteDropZone = true;
        EventManager.TriggerEvent("IsDeleteDropZone", eventParam);
    }
        
    public void OnPointerExit(PointerEventData eventData)
    {
        //highlight panel
        panelColor.color = panelColorDefault;

        //let drag control know this is a delete drop zone
        EventParam eventParam = new EventParam();
        eventParam.IsDeleteDropZone = false;
        EventManager.TriggerEvent("IsDeleteDropZone", eventParam);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionBehavior : MonoBehaviour, IPointerDownHandler
{
    public bool isSelected = false;
    public void OnPointerDown(PointerEventData e)
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            applicationmanager.UpdateSelectedComponent(this.gameObject);
        }
        else
        {
            applicationmanager.UpdateSelectedComponent(this.gameObject);
        }
    }
}

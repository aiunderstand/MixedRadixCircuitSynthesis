using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnInputMouseHelper : MonoBehaviour, IPointerClickHandler
{
    BtnInput controller;
    public void Awake()
    {
        controller = GetComponentInParent<BtnInput>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            controller.OnClick(1);
        if (eventData.button == PointerEventData.InputButton.Right)
            controller.OnClick(-1);
    }
}

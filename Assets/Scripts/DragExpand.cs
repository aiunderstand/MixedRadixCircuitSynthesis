using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragExpand : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    DragDrop _ddScript;
    public RectTransform _panel;
    public GameObject BtnInputPrefab;
    int MaxInputs = 12;
    int _btnInputSpacing = 23;
    int _btnInputMinPos = 70;
    float _startMouseHeight;
    int _computedHeight;
    int _AmountOfInputs =1;
    int _computedAmountOfInputs;
    public InputController InputController;

    void Start()
    {
        _ddScript = GetComponentInParent<DragDrop>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ddScript.enabled = false;
        _startMouseHeight = _panel.rect.height + Input.mousePosition.y;
    }


    public void OnDrag(PointerEventData eventData)
    {
        _computedHeight = Mathf.FloorToInt(_startMouseHeight - Input.mousePosition.y);
        SetPanelHeight(_computedHeight);


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _ddScript.enabled = true;
    }

    void SetPanelHeight(int computedHeight)
    {
        //determine to add or remove prefabs
        _computedAmountOfInputs = Mathf.FloorToInt((_computedHeight - _btnInputMinPos) / _btnInputSpacing) + 1;

        if (_computedAmountOfInputs > MaxInputs)
            _computedAmountOfInputs = MaxInputs;

        if (_computedAmountOfInputs < 1)
            _computedAmountOfInputs = 1;

        int diff = Mathf.Abs(_AmountOfInputs - _computedAmountOfInputs);
        for (int i = 0; i < diff; i++)
        {
            if (_computedAmountOfInputs > _AmountOfInputs) //add prefab
            {
                _computedHeight = _btnInputMinPos + (_AmountOfInputs * _btnInputSpacing);
                _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _computedHeight);
                var go = GameObject.Instantiate(BtnInputPrefab);
                go.transform.SetParent(BtnInputPrefab.transform.parent, false);
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y - (_AmountOfInputs * _btnInputSpacing), go.transform.localPosition.z);

                //add to input controller
                _AmountOfInputs++;
                go.name = (_AmountOfInputs - 1).ToString(); //reuse name for index, refactor this is bad practise
                go.GetComponent<BtnInput>()._portIndex = _AmountOfInputs - 1;
                go.GetComponent<BtnInput>().Connections.Clear();
                go.GetComponent<BtnInput>().hasDownwardsLink = null;
                go.GetComponent<BtnInput>().hasUpwardsLink = null;

                if (InputController.name.Contains("Input"))
                    go.GetComponent<BtnInput>().isOutput = false;
                else
                    go.GetComponent<BtnInput>().isOutput = true;

                InputController.Buttons.Add(go);
                InputController.ComputeCounter();
            }
            else
            {
                if (_computedAmountOfInputs < _AmountOfInputs) //remove prefab
                {
                    _computedHeight = _btnInputMinPos + ((_AmountOfInputs - 2) * _btnInputSpacing);
                    _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _computedHeight);
                    var go = InputController.Buttons[_AmountOfInputs - 1];
                    if (go.GetComponent<BtnInput>().Connections.Count > 0)
                    {
                        go.GetComponent<BtnInput>().RemoveAllConnections();
                    }

                    InputController.Buttons.Remove(go);
                    DestroyImmediate(go);
                    _AmountOfInputs--;
                    InputController.ComputeCounter();
                }
            }

        }
    }

    public void SetPanelSize(int size)
    {
        _computedHeight = _btnInputMinPos + ((size-1) * _btnInputSpacing); //there is always minimum of 1
        
        SetPanelHeight(_computedHeight);
       
    }
}

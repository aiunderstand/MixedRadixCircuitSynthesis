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
    public int MaxInputs = 6;
    int _btnInputSpacing = 23;
    int _btnInputMinPos = 70;
    int _btnInputMaxPos;
    float _startMouseHeight;
    int _computedHeight;
    int _AmountOfInputs;
    int _computedAmountOfInputs;
    public InputController InputController;

    void Start()
    {
        _ddScript = GetComponentInParent<DragDrop>();
        _btnInputMaxPos = _btnInputMinPos + ((MaxInputs - 1) * _btnInputSpacing) + 5; //plus 5 some spacing to add last one
        _AmountOfInputs = 1;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ddScript.enabled = false;
        _startMouseHeight = _panel.rect.height + Input.mousePosition.y;
    }
    

    public void OnDrag(PointerEventData eventData)
    {
       
        _computedHeight = Mathf.FloorToInt(_startMouseHeight - Input.mousePosition.y);


        if (_computedHeight < _btnInputMinPos)
        {
            _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _btnInputMinPos);
           
        }
        else
        {
            if (_computedHeight > _btnInputMaxPos)
            {
                _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _btnInputMaxPos);
                
            }
            else
            {
                _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _computedHeight);

                //determine to add or remove prefabs
                _computedAmountOfInputs = Mathf.FloorToInt((_computedHeight - _btnInputMinPos) / _btnInputSpacing) +1;

                if (_computedAmountOfInputs > _AmountOfInputs) //add prefab
                {
                   var go = GameObject.Instantiate(BtnInputPrefab);
                    go.transform.SetParent(BtnInputPrefab.transform.parent, false);
                    go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y - (_AmountOfInputs *_btnInputSpacing), go.transform.localPosition.z);

                    //add to input controller
                   _AmountOfInputs++;
                    go.name = (_AmountOfInputs -1).ToString(); //reuse name for index, refactor this is bad practise
                    InputController.Buttons.Add(go);
                    InputController.ComputeCounter();




                }
                else
                {
                    if (_computedAmountOfInputs < _AmountOfInputs) //remove prefab
                    {
                        var go = InputController.Buttons[_AmountOfInputs-1];
                        InputController.Buttons.Remove(go);
                        Destroy(go);
                        _AmountOfInputs--;
                        InputController.ComputeCounter();
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _ddScript.enabled = true;
    }
}

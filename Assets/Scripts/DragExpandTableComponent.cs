using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragExpandTableComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    DragDrop _ddScript;
    public RectTransform _panel;
    public GameObject[] BinaryTruthtables;
    public GameObject[] TernaryTruthtables;
    public TextMeshProUGUI DropdownLabel;
    int _btnInputMinPos = 85;
    int _btnInputMaxPosBinary = 115;
    int _btnInputMaxPosTernary = 130;
    float _startMouseHeight;
    int _computedHeight;
    bool _isBinary = false;
    public int Arity = 2; //default 2
    
    void Start()
    {
        _ddScript = GetComponentInParent<DragDrop>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ddScript.enabled = false;
        _startMouseHeight = _panel.rect.height + Input.mousePosition.y;
        _isBinary = DropdownLabel.text.Contains("Binary");
    }


    public void OnDrag(PointerEventData eventData)
    {
        //height of panel in uunits
        //          | binary        | ternary
        // 1-ary       85               85
        // 2-ary       100              115
        // 3-ary       115              130

        _computedHeight = Mathf.FloorToInt(_startMouseHeight - Input.mousePosition.y);

        if (_isBinary)
        {
            if (_computedHeight < _btnInputMinPos)
            {
                SetPanelSize(2, 1);
            }
            else
            {
                if (_computedHeight > _btnInputMaxPosBinary)
                {
                    SetPanelSize(2, 3);
                }
                else
                {
                    SetPanelSize(2, 2);
                }
            }
        }
        else 
        {
            if (_computedHeight < _btnInputMinPos)
            {
                SetPanelSize(3, 1);
            }
            else
            {
                if (_computedHeight > _btnInputMaxPosTernary)
                {
                    SetPanelSize(3, 3);
                }
                else
                {
                    SetPanelSize(3, 2);
                }
            }
        }

    }

    

    public void OnEndDrag(PointerEventData eventData)
    {
        _ddScript.enabled = true;
    }

    public void SetPanelSize(int radix, int arity)
    {
        InputControllerLogicGate iclg = transform.parent.parent.GetComponent<InputControllerLogicGate>();


        //remove all possibl existing connections to it
        var allTerminals = transform.parent.transform.parent.GetComponentsInChildren<BtnInput>();

        foreach (var t in allTerminals)
        {
            t.RemoveAllConnections();
        }


        //height of panel in uunits
        //          | binary        | ternary
        // 1-ary       85               85
        // 2-ary       100              115
        // 3-ary       115              130
        if (radix.Equals(2))
        {
            switch (arity)
            {
                case 1:
                    {
                        _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _btnInputMinPos);

                        foreach (var tt in TernaryTruthtables)
                            tt.SetActive(false);

                        foreach (var tt in BinaryTruthtables)
                            tt.SetActive(false);

                        BinaryTruthtables[0].SetActive(true);
                        iclg.UpdateInputController(BinaryTruthtables[0].GetComponent<InputController>());
                        Arity = 1;
                    }
                    break;
                case 2: 
                    {
                        _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

                        foreach (var tt in TernaryTruthtables)
                            tt.SetActive(false);

                        foreach (var tt in BinaryTruthtables)
                            tt.SetActive(false);

                        BinaryTruthtables[1].SetActive(true);
                        iclg.UpdateInputController(BinaryTruthtables[1].GetComponent<InputController>());
                        Arity = 2;
                    }
                    break;
                case 3:
                    {
                        _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _btnInputMaxPosBinary);

                        foreach (var tt in TernaryTruthtables)
                            tt.SetActive(false);

                        foreach (var tt in BinaryTruthtables)
                            tt.SetActive(false);

                        BinaryTruthtables[2].SetActive(true);
                        iclg.UpdateInputController(BinaryTruthtables[2].GetComponent<InputController>());
                        Arity = 3;
                    }
                    break;
            }        
        }

        if (radix.Equals(3))
        {
            switch (arity)
            {
                case 1:
                    {
                        _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _btnInputMinPos);

                        foreach (var tt in TernaryTruthtables)
                            tt.SetActive(false);

                        foreach (var tt in BinaryTruthtables)
                            tt.SetActive(false);

                        TernaryTruthtables[0].SetActive(true);
                        iclg.UpdateInputController(TernaryTruthtables[0].GetComponent<InputController>());
                        Arity = 1;
                    }
                    break;
                case 2:
                    {
                        _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 115);

                        foreach (var tt in TernaryTruthtables)
                            tt.SetActive(false);

                        foreach (var tt in BinaryTruthtables)
                            tt.SetActive(false);

                        TernaryTruthtables[1].SetActive(true);
                        iclg.UpdateInputController(TernaryTruthtables[1].GetComponent<InputController>());
                        Arity = 2;
                    }
                    break;
                case 3:
                    {
                        _panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _btnInputMaxPosTernary);

                        foreach (var tt in TernaryTruthtables)
                            tt.SetActive(false);

                        foreach (var tt in BinaryTruthtables)
                            tt.SetActive(false);

                        TernaryTruthtables[2].SetActive(true);
                        iclg.UpdateInputController(TernaryTruthtables[2].GetComponent<InputController>());
                        Arity = 3;
                    }
                    break;
            }
        }

        this.transform.parent.transform.parent.GetComponentInChildren<Matrix>().ComputeEmptyTruthTable(Arity, radix);
    }
}

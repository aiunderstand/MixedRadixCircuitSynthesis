using ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

//Adapted from https://stackoverflow.com/questions/37473802/unity3d-ui-calculation-for-position-dragging-an-item/37473953#37473953
[RequireComponent(typeof(EventTrigger))]
public class DragDrop : MonoBehaviour,
               IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 _dragOffset = Vector2.zero;
    Vector2 _limits = Vector2.zero;
    bool _isDeleteDropZone = false;
    Action<EventParam> _DeleteDropZoneListener;
    public GameObject SelectionBox;
    public GameObject MenuVersion; //view 0
    public GameObject FullVersion; //view 1 
    public GameObject LowerAbstractionVersion; //view 2     
    public Stats Stats;
    public Vector3 storedPosition;
    Color panelColorDefault;
    Color panelColorActive = new Color(255/255,82/255,45/255);
    public Image panelBg;
    bool _isFullVersion = false;
 

    void Awake()
    {
        _DeleteDropZoneListener = new Action<EventParam>(UpdateDeleteDropZoneParam);
       

    }

    public void SetVersion(bool state)
    {
        _isFullVersion = state;

        //break principle of 1 task per method, refactor
        if (state == true)
        {
            panelColorDefault = panelBg.color;
            this.transform.tag = "DnDComponent";

            if (this.FullVersion.GetComponent<ComponentGenerator>() != null)
                this.FullVersion.GetComponent<ComponentGenerator>().infoBtn.SetActive(true);
        }
    }


    void OnEnable()
    {
        EventManager.StartListening("IsDeleteDropZone", UpdateDeleteDropZoneParam);
    }

    void OnDisable()
    {
        EventManager.StopListening("IsDeleteDropZone", UpdateDeleteDropZoneParam);
    }

    void UpdateDeleteDropZoneParam(EventParam eventParam)
    {
        _isDeleteDropZone = eventParam.IsDeleteDropZone;

        if (_isFullVersion)
        {
            if (_isDeleteDropZone)
                panelBg.color = panelColorActive;
            else
                panelBg.color = panelColorDefault;
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _limits = SaveCircuit.DragDropArea.transform.GetComponent<RectTransform>().rect.max;

        if (_isFullVersion)
        {
            _dragOffset = eventData.position - (Vector2)transform.position;
        }
        else
        {
            _dragOffset = eventData.position - (Vector2)transform.position;

            //instantiate for the dnd menu either a new standard component (without component generator) or with
            if (this.FullVersion.GetComponent<ComponentGenerator>() == null)
            {
               GameObject go = null;
               switch (this.name)
                {
                    case "Input":
                        go = GameObject.Instantiate(CircuitGenerator.InputPrefab);
                        break;
                    case "Output":
                        go = GameObject.Instantiate(CircuitGenerator.OutputPrefab);
                        break;
                    case "LogicGate":
                        go = GameObject.Instantiate(CircuitGenerator.LogicGatePrefab);
                        break;
                }

                go.transform.SetParent(this.transform.parent, false);
                go.name = this.name;
                go.transform.SetSiblingIndex(this.transform.GetSiblingIndex()); //place it on same index
                //    go.GetComponent<DragDrop>().Stats = this.Stats;
            }
            else
            {
                var saveCircuit = GameObject.FindObjectOfType<SaveCircuit>();
                var component = this.FullVersion.GetComponent<InputController>().savedComponent;
                
                applicationmanager.InitHack = new List<GameObject>();
                var go = saveCircuit.GenerateListItem(component, saveCircuit.ContentContainer.transform, false);
                applicationmanager.clearInitHack = true;


                go.GetComponent<DragDrop>().MenuVersion.SetActive(true);
                go.GetComponent<DragDrop>().FullVersion.SetActive(false);
                //go.name = component.ComponentName;
                go.GetComponent<DragDrop>().FullVersion.GetComponent<InputController>().savedComponent = component;
                go.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

                //Unity bug where it will auto default to wrong anchor position when part of layout group (sets it to top left). Probably due to some awake script. Set it to center here.
                this.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                this.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                this.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

                //addtional stuff for correct UI
                this.FullVersion.GetComponent<ComponentGenerator>().infoBtn.SetActive(true);
                this.transform.localScale = new Vector3(SaveCircuit.FullScale, SaveCircuit.FullScale, SaveCircuit.FullScale);
            }
            
          
            //update current drag drop component 
            this.transform.SetParent(SaveCircuit.DragDropArea.transform);
            this.transform.tag = "DnDComponent";
            this.transform.SetAsLastSibling();
           
            //show expanded version of control
            MenuVersion.SetActive(false);
            FullVersion.SetActive(true);
            panelColorDefault = panelBg.color;
        }
    }

    public void Select()
    {
        SelectionBox.SetActive(true);
    }

    public void DeSelect()
    {
        SelectionBox.SetActive(false );
    }

    public IEnumerator LoadSave ()
    {
        yield return WaitFor.Frames(1); // wait for 1 frames since various components need to init. Otherwise race condition
        GetComponent<DragDropSaved>().LoadSavedState();
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - _dragOffset;
        var p = transform.localPosition;
        if (p.x < -_limits.x) { p.x = -_limits.x; }
        if (p.x > _limits.x) { p.x = _limits.x; }
        if (p.y < -_limits.y) { p.y = -_limits.y; }
        if (p.y > _limits.y) { p.y = _limits.y; }
        transform.localPosition = p;

        if (IsDeleteDropZone(transform.position))
            panelBg.color = panelColorActive;
        else
            panelBg.color = panelColorDefault;

        //redraw connections
        var allTerminals = GetComponentsInChildren<BtnInput>();
        foreach (var t in allTerminals)
        {
            foreach (var c in t.Connections)
            {
                c.Redraw(Color.clear); //no color update
            }
        }
    }

    public bool IsDropZone(Vector3 position)
    {
        if (position.x > 140)
            return true;
        else
            return false;
    }

    public bool IsDeleteDropZone(Vector3 position)
    {
        if (position.x < 50)
            return true;
        else
            return false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isFullVersion)
        {
            _isFullVersion = true;

            //if it is a saved component add selection behavior
            if (this.FullVersion.GetComponent<ComponentGenerator>() != null)
            {
                this.gameObject.AddComponent<SelectionBehavior>();
            }

            applicationmanager.ActiveCanvasElementStack[applicationmanager.abstractionLevel].Add(gameObject);
        }

        if (IsDeleteDropZone(transform.position))
        {
            //remove connections first

            //get all connections
            var connections = GameObject.FindGameObjectsWithTag("Wire");

            //get all connections from this component
            var ioConns = gameObject.GetComponentsInChildren<BtnInput>();

            for (int i = 0; i < connections.Length; i++)
            {
                for (int j = 0; j < ioConns.Length; j++)
                {
                    for (int k = 0; k < ioConns[j].Connections.Count; k++)
                    {
                        if (connections[i].name.Contains(ioConns[j].Connections[k].gameObject.name))
                        {
                            var l = connections[i].GetComponent<LineFunctions>();
                            var index = l.connection.id;
                            var go = l.gameObject;
                            l.connection.startTerminal.RemoveConnection(index);
                            l.connection.endTerminal.RemoveConnection(index);
                            applicationmanager.ActiveCanvasElementStack[applicationmanager.abstractionLevel].Remove(go);
                            Destroy(go);
                        }

                    }
                }
            }

            //deselect if applicable
            applicationmanager.DeleteCascade(gameObject);
            
            Destroy(gameObject);
        }
        else
        {
            //check if dropped in drop zone
            if (!IsDropZone(transform.position))
            {
                //snap to right side
                if (this.transform.localPosition.x < -295)
                    this.transform.localPosition = new Vector3(-275, this.transform.localPosition.y, 0);
            }

            //redraw connections
            var allTerminals = GetComponentsInChildren<BtnInput>();
            foreach (var t in allTerminals)
            {
                foreach (var c in t.Connections)
                {
                    c.Redraw(Color.clear); //no color update
                }
            }
        }

        _dragOffset = Vector2.zero;
    }
}
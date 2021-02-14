using ExtensionMethods;
using System;
using System.Collections;
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
    public GameObject MenuVersion; //view 0
    public GameObject FullVersion; //view 1 
    public GameObject TextVersion; //view 2
    public GameObject SymbolVersion; //view 3
    Color panelColorDefault;
    Color panelColorActive = new Color(255/255,82/255,45/255);
    public Image panelBg;
    bool _isFullVersion = false;

    public GameObject DragDropArea; //ugly, refactor so this is unneeded

    void Awake()
    {
        _DeleteDropZoneListener = new Action<EventParam>(UpdateDeleteDropZoneParam);
       

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
        if (_isFullVersion)
        {
            _dragOffset = eventData.position - (Vector2)transform.position;
            _limits = transform.parent.GetComponent<RectTransform>().rect.max;
        }
        else
        {
            _dragOffset = eventData.position - (Vector2)transform.position;
            _limits = transform.parent.GetComponent<RectTransform>().rect.max;

            //instantiate new one for menu
            var go = GameObject.Instantiate(this.gameObject);
            go.transform.SetParent(this.transform.parent, false);
            go.name = this.name; //isnt this overwritten?
            go.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            //update current drag drop component 
            this.transform.SetParent(DragDropArea.transform);
            this.transform.tag = "DnDComponent";
            this.transform.SetAsLastSibling();
           
            //show expanded version of control
            MenuVersion.SetActive(false);
            FullVersion.SetActive(true);
            panelColorDefault = panelBg.color;
            _limits = transform.parent.GetComponent<RectTransform>().rect.max;

            //check if saved state
            if (GetComponent<DragDropSaved>() != null)
            {
                StartCoroutine("LoadSave");
            }
        }
    }

    public void SetAbstractionLevelTo(int abstractionLevel)
    {
        
        MenuVersion.SetActive(false);
        FullVersion.SetActive(false);
        TextVersion.SetActive(false);
        SymbolVersion.SetActive(false);

        switch (abstractionLevel)
        {
            case 0:
                MenuVersion.SetActive(true);
                break;
            case 1: FullVersion.SetActive(true);
                break;
            case 2: TextVersion.SetActive(true);
                break;
            case 3: SymbolVersion.SetActive(true);
                break;
        }
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

        if (IsDeleteDropZone(eventData))
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

    public bool IsDropZone(PointerEventData eventData)
    {
        if (eventData.position.x > 140)
            return true;
        else
            return false;
    }

    public bool IsDeleteDropZone(PointerEventData eventData)
    {
        if (eventData.position.x < 80)
            return true;
        else
            return false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isFullVersion)
        {
            _isFullVersion = true;
        }

        if (IsDeleteDropZone(eventData))
        {
            Destroy(gameObject);
        }
        else
        {
            //check if dropped in drop zone
            if (!IsDropZone(eventData))
            {
                //snap to right side
                if (this.transform.localPosition.x < -295)
                    this.transform.localPosition = new Vector3(-275, this.transform.localPosition.y, 0);
            }
        }

        _dragOffset = Vector2.zero;
    }
}
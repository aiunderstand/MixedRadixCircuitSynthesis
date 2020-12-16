using System;
using UnityEngine;
using UnityEngine.EventSystems;

//Adapted from https://stackoverflow.com/questions/37473802/unity3d-ui-calculation-for-position-dragging-an-item/37473953#37473953
[RequireComponent(typeof(EventTrigger))]
public class DragDrop : MonoBehaviour,
               IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 _dragOffset = Vector2.zero;
    Vector2 _limits = Vector2.zero;
    bool _isDeleteDropZone = false;
    Action<EventParam> _DeleteDropZoneListener;
    public GameObject MenuVersion;
    public GameObject FullVersion;
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

            //instantiate new one (below)
            var go = GameObject.Instantiate(this.gameObject);
            go.transform.SetParent(this.transform.parent, false);
            go.name = this.name;

            //change order to lowest (overlay on top)
            this.transform.SetAsLastSibling();

            //show expanded version of control
            MenuVersion.SetActive(false);
            FullVersion.SetActive(true);
        }
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDeleteDropZone)
            Destroy(gameObject);
        else
        {
            if (!_isFullVersion)
            {
                _isFullVersion = true;

                //change canvas from UI to dropzone
                this.transform.SetParent(DragDropArea.transform);
            }
               
            _dragOffset = Vector2.zero;

               

        }
    }
}
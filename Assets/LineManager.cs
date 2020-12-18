using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    Action<EventParam> _ConnectionDataListener;
    bool _isDrawing = false;
    GameObject _tempLine;
    public GameObject LinePrefab;
    float _offsetX = 25;

    void Awake()
    {
        _ConnectionDataListener = new Action<EventParam>(NewConnectionData);
    }

    void OnEnable()
    {
        EventManager.StartListening("ConnectionData", NewConnectionData);
    }

    void OnDisable()
    {
        EventManager.StopListening("ConnectionData", NewConnectionData);
    }

    void NewConnectionData(EventParam eventParam)
    {
        //use right mouse to cancel, use left mouse for possible waypoints in the future
        _isDrawing = !_isDrawing;

        if (_isDrawing) //first point found
        {
            //add line renderer
            _tempLine = Instantiate(LinePrefab);
            _tempLine.name = "Link:" + eventParam.ConnectionData.ConnectionTerminal.name;
            _tempLine.transform.parent = this.transform;
            _tempLine.transform.localScale = new Vector3(1, 1, 1);
            _tempLine.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            var lr = _tempLine.GetComponent<LineRenderer>();
            var positionObject = eventParam.ConnectionData.ConnectionTerminal.transform.position;
            lr.SetPosition(0, positionObject);
            lr.SetPosition(1, new Vector3(positionObject.x + _offsetX, positionObject.y,0));
            lr.SetPosition(2, new Vector3(positionObject.x + _offsetX, positionObject.y, 0));
        }
        //initial condition is notDrawing mode. With first drawing event, enable drawing mode, add line renderer
        //and enable the mouse to update the 3th point.

        //when second ConnectionData event, check if line is valid. Disconnect mouse drawing. Disable Drawing mode. Checks are: 
        //input-output connection, different components, 1 output has max 1 input. If fail, remove line renderer, else add 3th and 4th point






    }

    private void Update()
    {
        if (_isDrawing)
        {
            if (Input.GetMouseButton(1))
            {
                _isDrawing = false;
                Destroy(_tempLine);
            }
            else
            {
                var mPos = Input.mousePosition;
                _tempLine.GetComponent<LineRenderer>().SetPosition(2, new Vector3(mPos.x, mPos.y, 0));
            }
        }
    }
}

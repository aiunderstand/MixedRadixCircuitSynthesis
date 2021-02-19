using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineManager : MonoBehaviour
{
    Action<EventParam> _ConnectionDataListener;
    public static bool IsDrawing = false;
    GameObject _tempLine;
    BtnInput _tempStartTerminal;
    public GameObject LinePrefab;
    public static float OffsetX = 16;
    int connectionId = 0; //this is increasing with every connection and never decreasing.

    public int GetNewConnectionId()
    {
        var id = connectionId;
        connectionId++;

        return id;
    }

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
        IsDrawing = !IsDrawing;
        if (IsDrawing) //first point found
        {
            _tempStartTerminal = eventParam.ConnectionData.ConnectionTerminal;
            //add line renderer
            _tempLine = Instantiate(LinePrefab);

            _tempLine.name = _tempStartTerminal.GetComponentInParent<DragDrop>().name + ";" +
                             _tempStartTerminal._portIndex + 
                             "; --> ";

            _tempLine.transform.SetParent(this.transform, false);
            _tempLine.transform.localScale = new Vector3(1, 1, 1);
            _tempLine.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            var lr = _tempLine.GetComponent<LineRenderer>();

            lr.startColor = _tempStartTerminal.wireColor.color;

            var positionObject = _tempStartTerminal.transform.position;
            lr.SetPosition(0, positionObject);
            lr.SetPosition(1, new Vector3(positionObject.x + OffsetX, positionObject.y, 0));
            lr.SetPosition(2, new Vector3(positionObject.x + OffsetX, positionObject.y, 0));
        }
        else
        {
            //try finalize line renderer
            if (!eventParam.ConnectionData.IsInput) //check that the 2nd ports is input, since the first port is output and no input input connections are allowed)
            {
                if (!eventParam.ConnectionData.ConnectionTerminal.GetInstanceID().Equals(_tempStartTerminal.GetInstanceID())) //different components check
                {
                    if (checkOutputHasMaxOneInput(eventParam.ConnectionData.ConnectionTerminal)) //2do: 1 output has max 1 input check
                    {
                        var lr = _tempLine.GetComponent<LineRenderer>();
                        lr.positionCount = 4;
                        var positionObject = eventParam.ConnectionData.ConnectionTerminal.transform.position;
                        lr.SetPosition(2, new Vector3(positionObject.x - OffsetX, positionObject.y, 0));
                        lr.SetPosition(3, new Vector3(positionObject.x, positionObject.y, 0));
                        lr.endColor = lr.startColor;

                        //redraw the line render segments with clickable buttons, strechted and correctly positioned
                        for (int i = 0; i < 3; i++)
                        {
                            DrawButtonSegment(i, _tempLine);
                        }

                        //create a connection
                        var conn = new Connection();
                        conn.startTerminal = _tempStartTerminal;
                        conn.endTerminal = eventParam.ConnectionData.ConnectionTerminal;
                        conn.id = GetNewConnectionId();

                        _tempLine.name = conn.id.ToString() + " = " + _tempLine.name + 
                                         conn.endTerminal.GetComponentInParent<DragDrop>().name + ";" +
                                         conn.endTerminal._portIndex;
                      
                        _tempLine.GetComponent<LineFunctions>().connection = conn;

                        //add connection to start and end terminals (ports)
                        conn.startTerminal.Connections.Add(_tempLine.GetComponent<LineFunctions>());
                        conn.endTerminal.Connections.Add(_tempLine.GetComponent<LineFunctions>());

                        //finalize
                        _tempLine = null;
                        _tempStartTerminal = null;
                    }
                    else
                    {
                        Destroy(_tempLine);
                    }
                }
                else
                {
                    Destroy(_tempLine);
                }
            }
            else
            {
                Destroy(_tempLine);
            }
        }
    }

    private void DrawButtonSegment(int i, GameObject tempLine)
    {
        var lr = _tempLine.GetComponent<LineRenderer>();

        RectTransform button = _tempLine.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
        var middlepoint = new Vector3(Mathf.Abs(lr.GetPosition(i).x + lr.GetPosition(i+1).x) / 2,
                                        Mathf.Abs(lr.GetPosition(i).y + lr.GetPosition(i+1).y) / 2,
                                        0);
        var width = Vector2.Distance(lr.GetPosition(i), lr.GetPosition(i+1));

        button.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        button.transform.localPosition = middlepoint;
        var angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(lr.GetPosition(i).y - lr.GetPosition(i+1).y) /
                                                    Mathf.Abs(lr.GetPosition(i).x - lr.GetPosition(i+1).x));

        var deltaY = (lr.GetPosition(i).y - lr.GetPosition(i+1).y);
        var deltaX = (lr.GetPosition(i).x - lr.GetPosition(i+1).x);
        if (deltaY < 0 && deltaX > 0)
            angle = -angle;

        if (deltaY > 0 && deltaX < 0)
            angle = -angle;

        button.transform.rotation = Quaternion.Euler(0, 0, angle);

        //color the connection line
        button.GetComponent<Image>().color = lr.startColor;
    }

    private bool checkOutputHasMaxOneInput(BtnInput endTerminal)
    {
        if (endTerminal.Connections.Count == 0)
            return true;
        else
            return false;
    }

    private void Update()
    {
        if (IsDrawing)
        {
            //use right mouse aka GetMouseButton(1) to cancel, use left mouse for possible waypoints in the future
            if (Input.GetMouseButton(1))
            {
                IsDrawing = false;
                Destroy(_tempLine);
            }
            else
            {
                var mPos = Input.mousePosition;
                _tempLine.GetComponent<LineRenderer>().SetPosition(2, new Vector3(mPos.x, mPos.y, 0));

                for (int i = 0; i < 2; i++)
                {
                    DrawButtonSegment(i, _tempLine);
                }
            }
        }
    }

   
}

public class Connection
{
    public int id; //Get this from the linemanager.GetNewConnectionId();
    public BtnInput endTerminal;
    public BtnInput startTerminal;

}

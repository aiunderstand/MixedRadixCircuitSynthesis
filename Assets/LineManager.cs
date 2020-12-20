using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineManager : MonoBehaviour, IPointerUpHandler
{
    Action<EventParam> _ConnectionDataListener;
    bool _isDrawing = false;
    GameObject _tempLine;
    BtnInput _tempStartTerminal;
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
       _isDrawing = !_isDrawing;

        if (_isDrawing) //first point found
        {
            _tempStartTerminal = eventParam.ConnectionData.ConnectionTerminal;
            //add line renderer
            _tempLine = Instantiate(LinePrefab);
            _tempLine.name = _tempStartTerminal.name + "-";
            _tempLine.transform.SetParent(this.transform, false);
            _tempLine.transform.localScale = new Vector3(1, 1, 1);
            _tempLine.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            var lr = _tempLine.GetComponent<LineRenderer>();
            var positionObject = _tempStartTerminal.transform.position;
            lr.SetPosition(0, positionObject);
            lr.SetPosition(1, new Vector3(positionObject.x + _offsetX, positionObject.y, 0));
            lr.SetPosition(2, new Vector3(positionObject.x + _offsetX, positionObject.y, 0));
        }
        else
        {
            //try finalize line renderer
            if (!eventParam.ConnectionData.IsInput) //input-output check
            {
                if (!eventParam.ConnectionData.ConnectionTerminal.GetInstanceID().Equals(_tempStartTerminal.GetInstanceID())) //different components check
                {
                    if (true) //2do: 1 output has max 1 input check
                    {
                        var lr = _tempLine.GetComponent<LineRenderer>();
                        lr.positionCount = 4;
                        var positionObject = eventParam.ConnectionData.ConnectionTerminal.transform.position;
                        lr.SetPosition(2, new Vector3(positionObject.x - _offsetX, positionObject.y, 0));
                        lr.SetPosition(3, new Vector3(positionObject.x, positionObject.y, 0));

                        lr.endColor = lr.startColor;

                        //update input controller of link
                        
                        var conn = _tempStartTerminal.GetComponentInParent<InputController>().Connections[int.Parse(_tempStartTerminal.name)];
                        conn.startTerminal = _tempStartTerminal;
                        conn.endTerminal.Add(eventParam.ConnectionData.ConnectionTerminal);
                        _tempLine.name = _tempLine.name + eventParam.ConnectionData.ConnectionTerminal.name;

                        //if end terminal is an Output 
                        if (eventParam.ConnectionData.ConnectionTerminal.tag.Equals("Output"))
                        {
                            //if start terminal is a input we do nothing 
                            if (conn.startTerminal.tag.Equals("Input"))
                            {
                                
                            }
                            else //else we need to propoage the connection
                            { 
                                var icl = conn.startTerminal.transform.parent.transform.parent.GetComponent<InputControllerLogicGate>();
                                icl.Connections[3] = conn; //index 3 is always the output port
                            }
                        }
                        else //if end terminal is a logic gate we need to propagate the conneciton
                        {
                            if (conn.startTerminal.name.Equals("3") && !conn.startTerminal.tag.Contains("Input"))
                            {
                                var icl2 = conn.startTerminal.transform.parent.transform.parent.GetComponent<InputControllerLogicGate>();
                                icl2.Connections[3] = conn; //index 3 is always the output port
                            }

                            int index = int.Parse(eventParam.ConnectionData.ConnectionTerminal.name);
                            var icl = eventParam.ConnectionData.ConnectionTerminal.transform.parent.transform.parent.GetComponent<InputControllerLogicGate>();
                            icl.Connections[index] = conn;
                        }




                    //attempt clickable lines
                    //option 1
                    //var mc = _tempLine.AddComponent<MeshCollider>();

                    //Mesh mesh = new Mesh();
                    //lr.BakeMesh(mesh, true);
                    //mc.sharedMesh = mesh;

                    //option 2
                    //Vector3[] points = new Vector3[lr.positionCount];
                    //lr.GetPositions(points);

                    //var ec = _tempLine.GetComponent<EdgeCollider2D>();

                    //ec.points = points.Select(x =>
                    //{
                    //    var pos = ec.transform.InverseTransformPoint(x);
                    //    return new Vector2(pos.x, pos.y);
                    //    //return new Vector2(x.x, x.y);
                    //}).ToArray();

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

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("test");
    }

    public void OnMouseDown()
    {
        Debug.Log("test");
    }

    private void Update()
    {
        if (_isDrawing)
        {
            //use right mouse aka GetMouseButton(1) to cancel, use left mouse for possible waypoints in the future
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

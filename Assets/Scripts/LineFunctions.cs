using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineFunctions : MonoBehaviour
{
    public Connection connection; //should probably rename this because now we have connection.connection which is weird

    public void DestroyConnection()
    {
        connection.startTerminal.RemoveConnection(connection.id);
        connection.endTerminal.RemoveConnection(connection.id);
        Destroy(this.gameObject);
    }

    private void DrawButtonSegment(int i, Color color, bool updateColor)
    {
        var lr = GetComponent<LineRenderer>();

        RectTransform button = transform.GetChild(i).gameObject.GetComponent<RectTransform>();
        var middlepoint = new Vector3(Mathf.Abs(lr.GetPosition(i).x + lr.GetPosition(i + 1).x) / 2,
                                        Mathf.Abs(lr.GetPosition(i).y + lr.GetPosition(i + 1).y) / 2,
                                        0);
        var width = Vector2.Distance(lr.GetPosition(i), lr.GetPosition(i + 1));

        button.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        button.transform.localPosition = middlepoint;
        var angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(lr.GetPosition(i).y - lr.GetPosition(i + 1).y) /
                                                    Mathf.Abs(lr.GetPosition(i).x - lr.GetPosition(i + 1).x));

        var deltaY = (lr.GetPosition(i).y - lr.GetPosition(i + 1).y);
        var deltaX = (lr.GetPosition(i).x - lr.GetPosition(i + 1).x);
        if (deltaY < 0 && deltaX > 0)
            angle = -angle;

        if (deltaY > 0 && deltaX < 0)
            angle = -angle;

        button.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (updateColor)
            button.GetComponent<Image>().color = color;
    }

    public void Redraw(Color color, bool updateColor = false)
    {
        var lr = GetComponent<LineRenderer>();

        var positionObject = connection.startTerminal.transform.position;
        lr.SetPosition(0, positionObject);

        if (connection.startTerminal.isOutput)
            lr.SetPosition(1, new Vector3(positionObject.x + LineManager.OffsetXY, positionObject.y, 0));
        else
            lr.SetPosition(1, new Vector3(positionObject.x + LineManager.OffsetX + LineManager.OffsetXY, positionObject.y, 0));


        positionObject = connection.endTerminal.transform.position;
        lr.SetPosition(2, new Vector3(positionObject.x - LineManager.OffsetXY, positionObject.y, 0));
        lr.SetPosition(3, new Vector3(positionObject.x, positionObject.y, 0));

        for (int i = 0; i < 3; i++)
        {
            DrawButtonSegment(i, color, updateColor);
        }
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LEDtoggle : MonoBehaviour
{
    public bool isLed = false;
    public Sprite ledImage;
    public Sprite defaultImage;
    Image image;
    public GameObject label;
    Color tempColor;

    public void Awake()
    {
        image = GetComponent<Image>();
        this.gameObject.transform.SetAsFirstSibling();
        tempColor = Color.white; //default is 0
    }
    public void OnClick()
    {
        isLed = !isLed;

        if (isLed)
        {
            this.gameObject.transform.SetAsLastSibling();
            image.sprite = ledImage;
            image.color = tempColor;
        }
        else
        {
            this.gameObject.transform.SetAsFirstSibling();
            image.sprite = defaultImage;
            image.color = Color.white;
        }

    }

    public void SetLedColor(int val)
    {
        switch (val)
        {
            case 0:
                tempColor = Color.white;
                break;
            case 1:
                tempColor = Color.green;
                break;
            case 2:
                tempColor = Color.red;
                break;
            case -1:
                tempColor = Color.red;
                break;
        }

        if (isLed)
        {
            image.color = tempColor;
        }
    }
}

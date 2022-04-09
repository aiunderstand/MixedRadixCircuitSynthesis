using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    public TMP_Text Title;
    public TMP_Text Message;
    public void UpdateMessage(string title, string message)
    {
        Title.text = title;
        Message.text = message;
    }
}

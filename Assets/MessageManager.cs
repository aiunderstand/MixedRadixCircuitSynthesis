using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    public GameObject MessageBoxGO;

    // Start is called before the first frame update
    public void Show(string title, string message)
    {
        MessageBoxGO.GetComponentInChildren<MessageBox>().UpdateMessage(title, message);

        MessageBoxGO.SetActive(true);
    }
}

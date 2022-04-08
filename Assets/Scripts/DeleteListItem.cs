using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteListItem : MonoBehaviour
{
   public void OnClick()
    {
        var go = GetComponentInParent<DragDrop>().gameObject;
        Settings.Remove(go.name);
        Destroy(go);
    }
}

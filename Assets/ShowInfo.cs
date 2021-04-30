using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInfo : MonoBehaviour
{
    StatisticsUI panel;

    public void Awake()
    {
        panel = GameObject.FindObjectOfType<SaveCircuit>().StatisticsScreen;    
    }

    public void Show()
    {
        var dd = transform.GetComponentInParent<DragDrop>();
        Stats stats = dd.Stats;
        panel.Show(stats, dd.SavedComponent);        
    }
}

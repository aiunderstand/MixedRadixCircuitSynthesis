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
        Stats stats = transform.GetComponentInParent<DragDrop>().Stats;
        panel.Show(stats);
    }
}

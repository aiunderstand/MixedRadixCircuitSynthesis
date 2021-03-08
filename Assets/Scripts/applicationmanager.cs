using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class applicationmanager : MonoBehaviour
{
    public bool useBigEndianForLogicGates = true;
    public static int abstractionLevel = 0;
    public static Dictionary<int, List<GameObject>> ActiveCanvasElementStack = new Dictionary<int, List<GameObject>>();
    public static bool scrollEnabled = true;
    public static GameObject curSelectedComponent;
   

    public void Awake()
    {
        ActiveCanvasElementStack.Add(applicationmanager.ActiveCanvasElementStack.Count, new List<GameObject>());
   
    }

    public static bool UseBigEndianForLogicGates()
    {
        var m = GameObject.FindObjectOfType<applicationmanager>();
        return m.useBigEndianForLogicGates;
    }

    public static void UpdateSelectedComponent(GameObject newSelecteComponent)
    {
        //currently only allow savedcomponents to be selected. Logic gates will come later.

        if (newSelecteComponent.GetComponent<DragDrop>().LowerAbstractionVersion != null)
        {
            if (curSelectedComponent == newSelecteComponent) //deselect
            {
                curSelectedComponent.GetComponent<DragDrop>().DeSelect();
                TryRemoveOfAbstractionStack();
                curSelectedComponent = null;
            }
            else
            {
                if (curSelectedComponent != null) //deselect previous before selecting new one
                {
                    curSelectedComponent.GetComponent<DragDrop>().DeSelect();
                    TryRemoveOfAbstractionStack();
                }
                curSelectedComponent = newSelecteComponent;
                curSelectedComponent.GetComponent<DragDrop>().Select();

                TryAddToAbstractionStack();
            }
        }
    }


    public static void DeleteCascade(GameObject newSelecteComponent)
    {
        //check if it is selected 

        if (curSelectedComponent == newSelecteComponent)  //we need to remove the selection as well as from the stack
        {
            TryRemoveOfAbstractionStack();
            applicationmanager.ActiveCanvasElementStack[applicationmanager.abstractionLevel].Remove(newSelecteComponent);
            curSelectedComponent = null;
        }
        else //it is not yet on the stack (delete while not dropping) or it is not selected (should not be possible, but in future we might use double click to select)
        {
            applicationmanager.ActiveCanvasElementStack[applicationmanager.abstractionLevel].Remove(newSelecteComponent);
        }
    }

    private static void TryAddToAbstractionStack()
    {
        var lav = curSelectedComponent.GetComponent<DragDrop>().LowerAbstractionVersion;

        applicationmanager.ActiveCanvasElementStack.Add(applicationmanager.ActiveCanvasElementStack.Count, new List<GameObject>());
        
        for (int i = 0; i < lav.transform.childCount; i++)
        {
            applicationmanager.ActiveCanvasElementStack[applicationmanager.abstractionLevel+1].Add(lav.transform.GetChild(i).gameObject);
        }
    }

    private static void TryRemoveOfAbstractionStack()
    {
        if (applicationmanager.ActiveCanvasElementStack.ContainsKey(applicationmanager.abstractionLevel + 1))
            applicationmanager.ActiveCanvasElementStack.Remove(applicationmanager.abstractionLevel + 1);


    }

    public static void ClearCanvas()
    {
        //get all connections and truth tables
        var connections = GameObject.FindGameObjectsWithTag("Wire");
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");

        var dragdropArea = SaveCircuit.DragDropArea;

        for (int i = 0; i < connections.Length; i++)
        {
            Destroy(connections[i]);
        }

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].transform.parent == dragdropArea.transform)
                Destroy(components[i]);
        }

        //clear abstraction controller
        applicationmanager.ActiveCanvasElementStack.Clear();
        ActiveCanvasElementStack.Add(applicationmanager.ActiveCanvasElementStack.Count, new List<GameObject>());
        abstractionLevel = 0;
        curSelectedComponent = null;

    }

    

    public void Update()
    {
        if (scrollEnabled)
        {
            float scrollDelta = Input.mouseScrollDelta.y;

            if (scrollDelta > 0)
            {
                if (abstractionLevel < (ActiveCanvasElementStack.Count - 1)) //we are going one level deeper (so towards level where elementary logic gates are)
                {
                    //reset position for the coordinate system to work
                    curSelectedComponent.GetComponent<DragDrop>().storedPosition = curSelectedComponent.transform.localPosition;
                    curSelectedComponent.transform.localPosition = Vector3.zero;

                    //remove selection
                    curSelectedComponent.GetComponent<DragDrop>().DeSelect();
                    curSelectedComponent = null;

                    
                    //disable current elements
                    for (int i = 0; i < ActiveCanvasElementStack[abstractionLevel].Count; i++)
                    {
                        if (ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>() == null)
                        {
                            ActiveCanvasElementStack[abstractionLevel][i].gameObject.SetActive(false); //it is a connection
                        }
                        else
                        {
                            ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().FullVersion.SetActive(false); //it is a component


                            if (ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionVersion != null)
                            {
                                ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionVersion.SetActive(true);
                                for (int j = 0; j < ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections.Count; j++)
                                {
                                    ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections[j].gameObject.SetActive(true);
                                    ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections[j].Redraw();
                                }

                            }
                        }
                    }

                    //increase abstraction level index
                    abstractionLevel++;

                    //enable new elements
                    for (int i = 0; i < ActiveCanvasElementStack[abstractionLevel].Count; i++)
                    {
                        ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().FullVersion.SetActive(true);

                        if (ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionVersion != null)
                        {
                            ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionVersion.SetActive(false);
                            for (int j = 0; j < ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections.Count; j++)
                            {
                                ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections[j].gameObject.SetActive(false);
                                ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections[j].Redraw();
                            }
                        }
                    }
                }
               
            }

            if (scrollDelta < 0)
            {
                if (abstractionLevel > 0) //we are going one level higher (so towards level where highest abstraction is)
                {
                    //remove selection
                    if (curSelectedComponent != null)
                    {
                        curSelectedComponent.GetComponent<DragDrop>().DeSelect();
                        TryRemoveOfAbstractionStack();
                        curSelectedComponent = null;
                    }

                    //disable current elements
                    for (int i = 0; i < ActiveCanvasElementStack[abstractionLevel].Count; i++)
                    {
                        ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().FullVersion.SetActive(false);

                        if (ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionVersion != null)
                        {
                            ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionVersion.SetActive(true);
                            for (int j = 0; j < ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections.Count; j++)
                            {
                                ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections[j].gameObject.SetActive(true);
                                ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections[j].Redraw();
                            }
                        }
                    }

                    //remove elements from stack
                    ActiveCanvasElementStack.Remove(abstractionLevel);

                    //increase abstraction level index
                    abstractionLevel--;

                    //enable new elements
                    for (int i = 0; i < ActiveCanvasElementStack[abstractionLevel].Count; i++)
                    {
                        if (ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>() == null)
                        {
                            ActiveCanvasElementStack[abstractionLevel][i].gameObject.SetActive(true); //it is a connection
                        }
                        else
                        {
                            ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().FullVersion.SetActive(true);

                            if (ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionVersion != null)
                            {
                                ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionVersion.SetActive(false);
                                for (int j = 0; j < ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections.Count; j++)
                                {
                                    ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections[j].gameObject.SetActive(false);
                                    ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().LowerAbstractionConnections[j].Redraw();
                                }
                            }

                            if (ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().transform.localPosition == Vector3.zero)
                                ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().transform.localPosition = ActiveCanvasElementStack[abstractionLevel][i].GetComponent<DragDrop>().storedPosition;
                        }
                    }
                }
               
            }
        }
    }
}

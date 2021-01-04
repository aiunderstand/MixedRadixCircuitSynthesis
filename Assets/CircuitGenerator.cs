using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitGenerator : MonoBehaviour
{
    public string path = "";
  public void OnClick()
    {
        GenerateCircuit(path);
    }

    private void GenerateCircuit(string path)
    {
        path = Application.dataPath + "/GeneratedCircuits/" + path;
        //get all connections and truth tables
        var connections = GameObject.FindGameObjectsWithTag("Wire");
        var components = GameObject.FindGameObjectsWithTag("DnDComponent");

        //foreach tt, generate a netlist
        foreach (var c in components)
        {
            if (c.name.Contains("LogicGate"))
            {
                var controller = c.GetComponentInChildren<InputControllerLogicGate>();
                int[] tt = controller.GetTruthTable();
                int arity = controller.GetArity();

                //generate
                TruthtableFunctionHelper.CreateNetlist(path, tt, arity);
            }
        }

        //generate circuit file
        int input = 2;
        int output = 2;
        int[] ttIndices = {2};
        int[] arityArray = { 2 }; ;
        int[] invArray = { 2 }; ;
        string[] connectionArray = { "test" };

        //TruthtableFunctionHelper.CreateCircuit(input, output, ttIndices, ttIndices.Length, arityArray, arityArray.Length, connectionArray, connectionArray.Length, invArray, invArray.Length);
      
    }
}

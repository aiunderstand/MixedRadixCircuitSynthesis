using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate
{
    public string FunctionIndex;
    public Vector2 Position2d;
    public int Arity;
    public List<string> Connections; //note the first 3 are inputs, the 4th is output. If arity 2 the first 2 are inputs and the 4th is output
}

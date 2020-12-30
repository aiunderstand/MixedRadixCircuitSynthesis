using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LogicTestRunner))]
public class TestRunnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LogicTestRunner myScript = (LogicTestRunner)target;
        if (GUILayout.Button("Run Test"))
        {
            myScript.OnClickRun();
        }
    }
}
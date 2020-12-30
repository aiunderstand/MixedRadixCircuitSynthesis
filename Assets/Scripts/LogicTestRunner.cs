using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTestRunner : MonoBehaviour
{
    public string csvFile;

    public void OnClickRun()
    {
        ReaderCSV.ReadCSV(csvFile);
    }
}

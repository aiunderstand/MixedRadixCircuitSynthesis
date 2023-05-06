using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VerifyScreen : MonoBehaviour
{
    public TextMeshProUGUI totalTests;
    public TextMeshProUGUI totalFailed;
    public TextMeshProUGUI failedIds;

    public void Show(string tests, string failed, string ids)
    {
        totalTests.text = tests;
        totalFailed.text = failed;
        failedIds.text = ids;

        //show screen
        gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBehaviour : MonoBehaviour
{
    bool isRunning = false;
    float timer = 0;
    public float updateFreq = 1; //once per second
    public BtnInput clock;

    public void Toggle()
    {
        isRunning = !isRunning;
    }

    public void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;

            if (timer > updateFreq)
            {
                // Remove the recorded 2 seconds.
                timer = timer - updateFreq;

                //toggle input
                clock.OnClick(1);
            }
        }
    }
}

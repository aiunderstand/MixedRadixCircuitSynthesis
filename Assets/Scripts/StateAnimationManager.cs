using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateAnimationManager : MonoBehaviour
{
    public Sprite pauseIcon;
    public Sprite playIcon;
    public static bool pausePlayback = false;
    public float playbackSpeed = 1; // 1 segment per second
    public Color highlightColor = Color.red;

    public void TogglePlayback()
    {
        pausePlayback = !pausePlayback;

        if (pausePlayback)
            GetComponent<Image>().sprite = pauseIcon;
        else
            GetComponent<Image>().sprite = playIcon;
    }


}

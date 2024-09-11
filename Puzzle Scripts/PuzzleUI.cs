using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    public PuzzleGameTimer PuzzleGameTimer;

    public Text TimeText;
    

    public void ShowTime(string time)
    {
        TimeText.text = time;
    }

    void Update()
    {
        ShowTime(PuzzleGameTimer.GetTimeAsString());
    }
}


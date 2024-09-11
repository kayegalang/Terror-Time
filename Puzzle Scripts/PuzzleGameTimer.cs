using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGameTimer : MonoBehaviour
{
    public GameManager GameManager;
    
    private bool isStopped = true;
    private int timeRemaining = 0;
    
    public void StartTimer(int durationInSeconds)
    {
        if (isStopped)
        {
            isStopped = false;
            timeRemaining = durationInSeconds;
            StartCoroutine(TickOneSecond());
        }
    }

    public void StopTimer()
    {
        isStopped = true;
    }

    public string GetTimeAsString()
    {
        int minutes = timeRemaining / 60;
        int seconds = timeRemaining - (minutes * 60);
        string minutesAsString = String.Format("{0:00}", minutes);
        string secondsAsString = String.Format("{0:00}", seconds);
        return minutesAsString + ":" + secondsAsString;
    }

    IEnumerator TickOneSecond()
    {
        yield return new WaitForSeconds(1);
        if (!isStopped)
        {
            timeRemaining = timeRemaining - 1;
            if (timeRemaining > 0)
            {
                StartCoroutine(TickOneSecond());
            }
            else
            {
                timeRemaining = 0;
                isStopped = true;
                GameManager.LoseGame();
            }
        }
    }
}

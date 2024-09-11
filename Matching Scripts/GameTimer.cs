using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public GameControler GameControler;
    
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
                GameControler.LoseGame();
            }
        }
    }
}

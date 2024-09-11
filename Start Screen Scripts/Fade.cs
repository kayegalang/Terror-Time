using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public StartScreen StartScreen;

    [SerializeField] private CanvasGroup StartScreenCanvasGroup;

    [SerializeField] private bool fadeOut = false;
    
    public void HideUI()
    {
        fadeOut = true;
    }

    void Update()
    {
        if (fadeOut)
        {
            if (StartScreenCanvasGroup.alpha >= 0)
            {
                StartScreenCanvasGroup.alpha -= Time.deltaTime;
                if (StartScreenCanvasGroup.alpha == 0)
                {
                    fadeOut = false;
                    StartScreen.HideStartScreenCanvas();
                }
            }
        }
    }
}

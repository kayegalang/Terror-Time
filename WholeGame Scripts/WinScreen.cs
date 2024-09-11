using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WinScreen : MonoBehaviour
{
    public WinWholeGame WinWholeGame;
    public TerrorTimeManager TerrorTimeManager;
    
    public VideoPlayer VideoPlayer;
    [SerializeField] private string WinClip;
    
    public GameObject WinScreenPanel;
    public GameObject PreviousMinigame;
    public GameObject NextMinigame;
    public GameObject ThisWinScreen;

    public void PlayVideo(string videoFileName)
    {
        VideoPlayer videoPlayer = VideoPlayer;

        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
    public void OnButtonClick()
    {
        StartNextGame();
    }

    private void StartNextGame()
    {
        TerrorTimeManager.GetKey();
        PreviousMinigame.SetActive(false);
        NextMinigame.SetActive(true);
        
        if (TerrorTimeManager.GetKeyIndex() == 5)
            WinWholeGame.ShowWinGameScreen();
        
        ThisWinScreen.SetActive(false);
        
    }
    
    private void OnEnable()
    {
        WinScreenPanel.SetActive(false);
        ShowWinScreenWhenAnimationFinished();
    }

    private void ShowWinScreenWhenAnimationFinished()
    {
        PlayVideo(WinClip);
        StartCoroutine(WaitTilWinScreenAppears());    }

    IEnumerator WaitTilWinScreenAppears()
    {
        yield return new WaitForSeconds(1.3f);
        WinScreenPanel.SetActive(true);
    }
    
}

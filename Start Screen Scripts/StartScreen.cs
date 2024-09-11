using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class StartScreen : MonoBehaviour
{
    public GameObject WinWholeGameScreen;
    public GameObject RestartWholeGameScreen;
    
    public GameObject StartScreenCanvas;
    public GameObject BedroomCanvas;
    public GameObject Closet;
    public GameObject Bed;

    public GameObject[] ClosetMinigames;

    public GameObject[] BedMinigames;
    
    public Fade Fade;

    public AudioSource AudioSource;
    public AudioClip StartScreenMusic;

    private void Start()
    {
        PlayStartScreenMusic();
        HideBedroom();
        Reset();
    }

    public void Reset()
    {
        HideMinigames();
        HideEndgameScreens();
    }

    private void HideEndgameScreens()
    {
        WinWholeGameScreen.SetActive(false);
        RestartWholeGameScreen.SetActive(false);    }

    private void PlayStartScreenMusic()
    {
        AudioSource.clip = StartScreenMusic;
        AudioSource.Play();
    }

    private void HideBedroom()
    {
        BedroomCanvas.SetActive(false);
        Closet.SetActive(false);
        Bed.SetActive(false);    }

    private void HideMinigames()
    {
        foreach (GameObject Minigame in ClosetMinigames)
            Minigame.SetActive(false);
        
        foreach (GameObject Minigame in BedMinigames)
            Minigame.SetActive(false);
    }

    public void OnStartScreenClick()
    {
        Fade.HideUI();
        StartCoroutine(FadeAudioSource.StartFade(AudioSource, 2f, 1f));
        BedroomCanvas.SetActive(true);
    }

    public void HideStartScreenCanvas()
    {
        StartScreenCanvas.SetActive(false);
    }

}

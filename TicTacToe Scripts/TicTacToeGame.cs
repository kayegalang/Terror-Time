using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.Video;

public class TicTacToeGame : MonoBehaviour
{
    public GameObject WinScreen;

    public GameObject RawImage;
    public VideoPlayer VideoPlayer;
    [SerializeField] private string JumpscareClip;
    public AudioSource AudioSource;
    public AudioClip Scream;
    
    public Slots Slots;
    public TicTacToeResolver TicTacToeResolver;
    public WinnerDisplay WinnerDisplay;
    public Sounds Sounds;
    public DeathCounter DeathCounter;

    public GameObject YouDiedPanel;
    public GameObject TryAgainPanel;
    
    private int numberOfWins = 0;
    private MarkerType currentMarkerType;
    private MarkerType firstPlayerMarkerType;
    private int numberOfTurnsPlayed;
    private int numOfLosses = 0;


    private bool isWaitingForComputerToPlay;
    
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
    public void OnEnable()
    {
        Reset();
        numOfLosses = 0;
        numberOfWins = 0;
        DeathCounter.ResetSkull();
        WinnerDisplay.Reset();
    }

    public int GetNumberOfWins()
    {
        return numberOfWins;
    }

    public void OnSlotClicked(Slot slot)
    {
        if (!isWaitingForComputerToPlay)
        {
            PlaceMarkerInSlot(slot);
        }
    }

    public void Reset()
    {
        RawImage.SetActive(false);
        numberOfTurnsPlayed = 0;
        ResetSlots();
        ResetPlayers();
    }

    public void ChangeOpponent()
    {
        Reset();
    }
    
    private void ResetSlots()
    {
        Slots.Reset();
    }

    private void ResetPlayers()
    {
        TicTacToeResolver.Reset();
        RandomizePlayer();
        firstPlayerMarkerType = currentMarkerType;
        isWaitingForComputerToPlay = false;
    }
    

    private void PlaceMarkerInSlot(Slot slot)
    {
        if (GameNotOver())
        {
            UpdateSlotImage(slot);
            if (currentMarkerType == MarkerType.Knife)
                Sounds.PlayKnifeSound();
            else
                Sounds.PlayBloodSound();
            CheckForWinner();
            
            EndTurn();
        }

    }

    private bool GameNotOver()
    {
        return TicTacToeResolver.NoWinner();
    }

    private void CheckForWinner()
    {
        numberOfTurnsPlayed++;
        if (numberOfTurnsPlayed < 5)
            return;
        TicTacToeResolver.CheckForEndOfGame(Slots.SlotOccupants());
    }

    private void EndTurn()
    {
        if (GameNotOver())
            ChangePlayer();
        else if (TicTacToeResolver.Winner() == firstPlayerMarkerType && numberOfWins < 3)
        {
            AddWin();
        }
        else if (TicTacToeResolver.Winner() != firstPlayerMarkerType)
            LoseGame();
        
            
    }

    private bool CheckIfWholeGameWon()
    {
        if (numberOfWins == 3)
            return true;
        return false;
    }

    private void WinGame()
    {
        StopAllCoroutines();

        WinScreen.SetActive(true);
    }

    private void LoseGame()
    {
        numOfLosses++;
        if (numOfLosses == 1)
        {
            DeathCounter.OnFirstLoss();
            Reset();
        }
        if (numOfLosses == 2)
            LoseWholeGame();
    }

    private void LoseWholeGame()
    {
        AudioSource.clip = Scream;
        AudioSource.Play();
        
        RawImage.SetActive(true);
        PlayVideo(JumpscareClip);
        
        
        YouDiedPanel.SetActive(true);
        StartCoroutine(WaitTilYouDiedScreenAppears());
    }

    IEnumerator WaitTilYouDiedScreenAppears()
    {
        yield return new WaitForSeconds(1f);
        RawImage.SetActive(false);
        YouDiedPanel.SetActive(true);
        StartCoroutine(WaitTilTryAgainPanelAppears());
    }

    IEnumerator WaitTilTryAgainPanelAppears()
    {
        yield return new WaitForSeconds(2f);
        TryAgainPanel.SetActive(true);
    }

    private void AddWin()
    {
            numberOfWins++;
            WinnerDisplay.Show();
            if (CheckIfWholeGameWon())
                WinGame();
            Reset();
    }

    

    private void ChangePlayer()
    {
        if (currentMarkerType == MarkerType.Blood)
            currentMarkerType = MarkerType.Knife;
        else
            currentMarkerType = MarkerType.Blood;

        SeeIfComputerShouldPlay();
    }
    
    private void SeeIfComputerShouldPlay()
    {
        if (IsHumanTurn())
        {
            return;
        }
        PlayComputerTurn();
    }   



    private void PlayComputerTurn()
    {
        StartCoroutine(PauseForComputerPlayer());
    }

    IEnumerator PauseForComputerPlayer()
    {
        isWaitingForComputerToPlay = true;
        float secondsToWait = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(secondsToWait);
        isWaitingForComputerToPlay = false;
        PlayComputerTurnAfterPause();
    }
    
    private void PlayComputerTurnAfterPause()
    {
        PlayHardComputerMove();
    }

    private void PlayHardComputerMove()
    {
        // if can win
        bool hasWon = TryToWin();
        if (hasWon)
            return;
        // if can block
        bool hasBlocked = TryToBlock();
        if (hasBlocked)
            return;
        // random slot
        PlayMarkerInRandomSlot();
    }

    private bool TryToWin()
    {
        return TryToPlayBestMoveForPlayer(currentMarkerType);
    }

    private bool TryToBlock()
    {
        return TryToPlayBestMoveForPlayer(firstPlayerMarkerType);
    }

    private bool TryToPlayBestMoveForPlayer(MarkerType markerType)
    {
        int bestSlotIndex = TicTacToeResolver.FindBestSlotIndexForPlayer(Slots.SlotOccupants(), markerType);
        if (bestSlotIndex != -1)
        {
            PlaceMarkerInSlot(Slots.GetSlot(bestSlotIndex));
            return true;
        }

        return false;
    }

    private void PlayMarkerInRandomSlot()
    {
        // pick random free slot
        Slot slot = Slots.RandomFreeSlot();
        PlaceMarkerInSlot(slot);
    }
    

    private bool IsHumanTurn()
    {
        return currentMarkerType == firstPlayerMarkerType;
    }

    private void UpdateSlotImage(Slot slot)
    {
        Slots.UpdateSlot(slot, currentMarkerType);
    }

    private void RandomizePlayer()
    {
        int randomNumber = Random.Range(1, 3);
        if (randomNumber == 1)
            currentMarkerType = MarkerType.Knife;
        else
        {
            currentMarkerType = MarkerType.Blood;
        }
    }
}

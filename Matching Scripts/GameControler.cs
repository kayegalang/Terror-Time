using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class GameControler : MonoBehaviour
{
    public GameObject RawImage;
    public VideoPlayer VideoPlayer;
    [SerializeField] private string JumpscareClip;
    public AudioClip Scream;

    public GameObject WinScreen;

    public GameTimer GameTimer;
    public GameObject GameOverPanel;
    public GameObject TryAgainPanel;

    public AudioSource AudioSource;
    public AudioClip CardFlipClip;
    
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;
    public List<Sprite> gamepuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;
    
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;
    
    public Transform puzzleField;
    public GameObject btn;

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
    private void CreateCardButtons()
    {
        for(int i = 0; i < 12; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;
            button.transform.SetParent(puzzleField, false);
        }
    }

    private void Awake()
    {
        CreateCardButtons();
        puzzles = Resources.LoadAll<Sprite>("Art/Flipped");
        SetupButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamepuzzles);
        gameGuesses = gamepuzzles.Count / 2;
        
        RawImage.SetActive(false);
        
        firstGuess = false;
        secondGuess = false;
        
        countCorrectGuesses = 0;

        GameOverPanel.SetActive(false);
        TryAgainPanel.SetActive(false);
        
        ShowAllButtons();
        GameTimer.StartTimer(40);
    }

    public void Reset()
    {
        puzzles = Resources.LoadAll<Sprite>("Art/Flipped");
        
        Shuffle(gamepuzzles);
        gameGuesses = gamepuzzles.Count / 2;
        
        RawImage.SetActive(false);
        
        firstGuess = false;
        secondGuess = false;
        
        countCorrectGuesses = 0;

        GameOverPanel.SetActive(false);
        TryAgainPanel.SetActive(false);
        
        ShowAllButtons();
        GameTimer.StartTimer(40);
    }

    private void ShowAllButtons()
    {
        foreach (Button button in btns)
        {
            button.image.color = Color.white;
            button.interactable = true;
            button.image.sprite = bgImage;
        }
    }

    void SetupButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        btns = new List<Button>();
        
        for(int i=0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if(index == looper / 2)
            {
                index = 0;
            }
            gamepuzzles.Add(puzzles[index]);
            
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }
    public void PickAPuzzle()
    {
        AudioSource.clip = CardFlipClip;
        AudioSource.Play();
        
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstGuess)
        {
            firstGuess = true;

            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamepuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamepuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;

            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamepuzzles[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gamepuzzles[secondGuessIndex];
            

            StartCoroutine(CheckIfThePuzzlesMatch());

            
        }
    }

    IEnumerator CheckIfThePuzzlesMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex)
        {

            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);



            CheckIfTheGameIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(.3f);
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }

        yield return new WaitForSeconds(.5f);

        firstGuess = secondGuess = false;

    }
    void CheckIfTheGameIsFinished()
    {
        countCorrectGuesses++;

        if(countCorrectGuesses == gameGuesses)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Reset();
        
        WinScreen.SetActive(true);
        
        StopAllCoroutines();
        
        GameTimer.StopTimer();
        
    }

    void Shuffle(List<Sprite> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;

        }
    }

    public void LoseGame()
    {
        RawImage.SetActive(true);
        PlayVideo(JumpscareClip);
        AudioSource.clip = Scream;
        AudioSource.Play();
        
        StartCoroutine(WaitTilYouDiedScreenAppears());
    }
    
    IEnumerator WaitTilYouDiedScreenAppears()
    {
        yield return new WaitForSeconds(1f);
        RawImage.SetActive(false);
        GameOverPanel.SetActive(true);
        StartCoroutine(WaitTilTryAgainPanelAppears());
    }

    IEnumerator WaitTilTryAgainPanelAppears()
    {
        yield return new WaitForSeconds(2f);
        TryAgainPanel.SetActive(true);
    }

}

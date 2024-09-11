using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HangmanManager : MonoBehaviour
{
    public TerrorTimeManager TerrorTimeManager;
    public KeyboardResetter KeyboardResetter;
    public GameObject BedroomCanvas;
    public GameObject HangmanMinigame;
    public GameObject KeyHolderPanel;

    public AudioSource AudioSource;
    public AudioClip ManScreamingClip;
    
    public TextAsset CreepyWords;
    public Image LetterLine;
    public GameObject SecretWordHolder;
    public Text LetterText;
    public GameObject[] HangmanParts;

    public GameObject WinScreen;

    public GameObject YouDiedPanel;
    public GameObject TryAgainPanel;
    
    private string[] wordsToChooseFrom;
    private Vector3[] linePositions;

    private string secretWord;
    private string currentWordState = "";
    
    private int incorrectGuesses = 0;
    private int maxIncorrectGuesses = 7;

    public void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        currentWordState = "";
        
        ResetHangmanUI();
        
        incorrectGuesses = 0;
        
        foreach (Transform child in SecretWordHolder.transform) {
            GameObject.Destroy(child.gameObject);
        }

        YouDiedPanel.SetActive(false);
        TryAgainPanel.SetActive(false);
        
        ChooseWord();
        
        linePositions = new Vector3[secretWord.Length];

        for (int i = 0; i < secretWord.Length; i++)
        {
            currentWordState += i.ToString();
        }
            
        
        CreateStartUI();
    }

    private void ChooseWord()
    {
        wordsToChooseFrom = CreepyWords.text.Split( '\n' );
        secretWord = wordsToChooseFrom[Random.Range(0, wordsToChooseFrom.Length)];
    }

    public void UpdateUI(string letterToAdd, int indexToAdd)
    {
        LetterText.text = letterToAdd;
        Instantiate(LetterText, linePositions[indexToAdd], Quaternion.identity, SecretWordHolder.transform);
    }

    private void CreateStartUI()
    {
        int secretWordLength = secretWord.Length;

        if (secretWordLength % 2 != 0)
            MakeOddWordUI(secretWordLength);
        else
        {
            MakeEvenWordUI(secretWordLength);
        }
            
    }

    private void MakeEvenWordUI(int secretWordLength)
    {
        Vector3 middleLinePosition1 = new Vector3(1225f, 640f);
        Vector3 middleLinePosition2 = new Vector3(1335f, 640f);
        
        CreateLetterLine(middleLinePosition1);
        CreateLetterLine(middleLinePosition2);
        
        int middleOfWord1 = secretWordLength / 2;
        int middleOfWord2 = middleOfWord1++;

        linePositions[middleOfWord2 - 1] = middleLinePosition1;
        linePositions[middleOfWord1 - 1] = middleLinePosition2;

        for (int i = 2; i < middleOfWord1; i++)
        {
            double x = 1225 - ((middleOfWord1 - i) * 110);
            Vector3 position = new Vector3((float)x, 640f);
            CreateLetterLine(position);

            linePositions[i - 2] = position;
        }

        for (double i = secretWordLength - 1; i > middleOfWord2; i--)
        {
            double x = 1335 + ((i - middleOfWord2) * 110);
            Vector3 position = new Vector3((float)x, 640f);
            CreateLetterLine(position);

            linePositions[(int)i] = position;
        }
    }

    private void MakeOddWordUI(int secretWordLength)
    {
        Vector3 middleLinePosition = new Vector3(1280f, 640f);
        CreateLetterLine(middleLinePosition);
        double middleOfWord = (double) secretWordLength / 2 + 0.5;

        linePositions[(int)middleOfWord - 1] = middleLinePosition;

        for (int i = 1; i < middleOfWord; i++)
        {
            double x = 1280 - ((middleOfWord - i) * 110);
            Vector3 position = new Vector3((float)x, 640f);
            CreateLetterLine(position);

            linePositions[i - 1] = position;
        }

        for (double i = secretWordLength; i > middleOfWord; i--)
        {
            double x = 1280 + ((i - middleOfWord) * 110);
            Vector3 position = new Vector3((float)x, 640f);
            CreateLetterLine(position);

            linePositions[(int)i - 1] = position;
        }
    }

    private void CreateLetterLine(Vector2 position)
    {
        Instantiate(LetterLine, position, Quaternion.identity, SecretWordHolder.transform);
    }

    public string GetSecretWord()
    {
        return secretWord;
    }

    public void UpdateCurrentWordState(string letterToAdd)
    {
        for (int i = 0; i < secretWord.Length; i++)
        {
            if (char.Parse(letterToAdd) == secretWord[i])
            {
                currentWordState = currentWordState.Replace(i.ToString(), letterToAdd);
                UpdateUI(letterToAdd, i);
            }
        }
    }

    public void AddToIncorrectGuesses()
    {
        if (incorrectGuesses < maxIncorrectGuesses)
            incorrectGuesses++;
    }

    public void UpdateHangmanUI()
    {
        if (incorrectGuesses <= 6)
            HangmanParts[incorrectGuesses].SetActive(true);
    }

    private void ResetHangmanUI()
    {
        foreach (GameObject bodyPart in HangmanParts)
        {
            bodyPart.SetActive(false);
        }
    }

    public bool IsGameOver()
    {
        if (secretWord.Equals(currentWordState))
            return true;
        if (incorrectGuesses == maxIncorrectGuesses)
            return true;
        return false;

    }

    public  void LoseGame()
    {
        AudioSource.clip = ManScreamingClip;
        AudioSource.Play();
        
        YouDiedPanel.SetActive(true);
        StartCoroutine(WaitTilTryAgainScreenAppears());
    }

    IEnumerator WaitTilTryAgainScreenAppears()
    {
        yield return new WaitForSeconds(2f);
        TryAgainPanel.SetActive(true);
    }

    public void WinGame()
    {
        KeyboardResetter.Reset();
        WinScreen.SetActive(true);
        
        StopAllCoroutines();
        
        Reset();

        KeyHolderPanel.SetActive(true);
    }

    public bool CheckIfGameWon()
    {
        if (incorrectGuesses < maxIncorrectGuesses && currentWordState.Equals(secretWord))
            return true;
        return false;
    }

    public bool CheckIfGameLost()
    {
        if (incorrectGuesses == maxIncorrectGuesses)
            return true;
        return false;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip ClickAudioClip;
    
    public HangmanManager HangmanManager;
    public GameObject CrossOut;
    public void OnClick()
    {
        AudioSource.clip = ClickAudioClip;
        AudioSource.Play();
        
        CrossOut.SetActive(true);
        gameObject.GetComponent<Button>().interactable = false;
        
        string letterToCheck = gameObject.GetComponent<Button>().tag;
        
        if (CheckIfLetterMatches(char.Parse(letterToCheck)))
        {
            if (!HangmanManager.IsGameOver())
            {
                HangmanManager.UpdateCurrentWordState(letterToCheck);
                if (HangmanManager.CheckIfGameWon())
                    HangmanManager.WinGame();
            }
                
        }
        else
        {
            HangmanManager.UpdateHangmanUI();
            HangmanManager.AddToIncorrectGuesses();
            
            if (HangmanManager.CheckIfGameLost())
                HangmanManager.LoseGame();
        }
    }

    private bool CheckIfLetterMatches(char letterToCheck)
    {
        string secretWord = HangmanManager.GetSecretWord();
        for (int i = 0; i < secretWord.Length; i++)
        {
            char currentLetter = secretWord[i];
            if (letterToCheck == currentLetter)
                return true;
        }
        return false;
    }
    
}

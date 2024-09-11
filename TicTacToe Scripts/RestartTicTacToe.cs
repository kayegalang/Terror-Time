using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartTicTacToe : MonoBehaviour
{
    public TicTacToeGame TicTacToeGame;
    
    public GameObject YouDiedPanel;
    public GameObject TryAgainPanel;
    public void OnButtonClick()
    {
        TryAgainPanel.SetActive(false);
        YouDiedPanel.SetActive(false);
        TicTacToeGame.OnEnable();
    }
}

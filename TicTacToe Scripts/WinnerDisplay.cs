using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerDisplay : MonoBehaviour
{
    public TicTacToeGame TicTacToeGame;
    public Image WinnerImage;
    public Sprite Zero;
    public Sprite One;
    public Sprite Two;
    public Sprite Three;
    public void Reset()
    {
        Show();
    }
    public void Show()
    {
        int numberOfWins = TicTacToeGame.GetNumberOfWins();

        if (numberOfWins == 0)
            WinnerImage.sprite = Zero;
        else if (numberOfWins == 1)
            WinnerImage.sprite = One;
        else if (numberOfWins == 2)
            WinnerImage.sprite = Two;
        else if (numberOfWins == 3)
            WinnerImage.sprite = Three;
    }
}

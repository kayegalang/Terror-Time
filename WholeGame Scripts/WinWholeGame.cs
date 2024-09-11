using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinWholeGame : MonoBehaviour
{
    public GameObject WinWholeGameScreen;
    public GameObject RestartWholeGameScreen;

    public void ShowWinGameScreen()
    {
        WinWholeGameScreen.SetActive(true);
        StartCoroutine(WaitTilRestartScreenAppears());
    }
    
    IEnumerator WaitTilRestartScreenAppears()
    {
        yield return new WaitForSeconds(2f);
        RestartWholeGameScreen.SetActive(true);
    }
}

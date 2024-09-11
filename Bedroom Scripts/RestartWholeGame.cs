using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartWholeGame : MonoBehaviour
{
    public StartScreen StartScreen;
    public TerrorTimeManager TerrorTimeManager;
    public GameChooser GameChooser;
    public BedroomManager BedroomManager;

    public GameObject BedroomCanvas;

    public void OnButtonClick()
    {
        Reset();
    }

    private void Reset()
    {
        StartScreen.Reset();
        
        TerrorTimeManager.Reset();
        GameChooser.Start();
        BedroomManager.Start();
        
        BedroomCanvas.SetActive(true);
    }
}

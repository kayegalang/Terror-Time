using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject Minigame;
    public GameObject StartScreen;

    public void OnButtonClick()
    {
        Minigame.SetActive(true);
        StartScreen.SetActive(false);
    }
}

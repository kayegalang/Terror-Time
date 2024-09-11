using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMaze : MonoBehaviour
{
    public GameObject MazeMinigame;

    public void OnButtonClick()
    {
        MazeMinigame.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPuzzleGame : MonoBehaviour
{
    public GameManager GameManager;
    
    public void OnButtonClick()
    {
        GameManager.Reset();
        GameManager.OnEnable();
    }
}

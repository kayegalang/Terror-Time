using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMatchingGame : MonoBehaviour
{
    public GameControler GameControler;
    public void OnButtonClick()
    {
        GameControler.Reset();
    }
    
}

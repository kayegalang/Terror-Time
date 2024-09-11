using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardResetter : MonoBehaviour
{
    public GameObject KeyboardHolder;
    private Keyboard[] keyboards; 
    
    public void Reset()
    {
        keyboards = KeyboardHolder.GetComponentsInChildren<Keyboard>();
        foreach (Keyboard keyboard in keyboards)
        {
            keyboard.CrossOut.SetActive(false);
            keyboard.GetComponent<Button>().interactable = true;
        }
    }
}

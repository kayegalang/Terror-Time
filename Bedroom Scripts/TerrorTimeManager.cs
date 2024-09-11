using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TerrorTimeManager : MonoBehaviour
{
    public Sprite UnshadedKey;
    public Sprite GottenKey;
    
    public GameObject[] keys;
    private int keysIndex = 0;

    public void Reset()
    {
        keysIndex = 0;

        for (int i = 0; i < 5; i++)
        {
            Image keyImage = keys[i].GetComponent<Image>();
            var tempColor = keyImage.color;
            tempColor.a = 150f;
            keyImage.color = tempColor;

            keyImage.sprite = UnshadedKey;
        }
    }

    public void GetKey()
    {
        Image keyImage = keys[keysIndex].GetComponent<Image>();
        var tempColor = keyImage.color;
        tempColor.a = 255f;
        keyImage.color = tempColor;

        keyImage.sprite = GottenKey;
        keysIndex++;
    }

    public int GetKeyIndex()
    {
        return keysIndex;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Failures : MonoBehaviour
{
    public Notes Notes;
    public MouseInputProvider MouseInputProvider;

    private void OnBecameInvisible()
    {
        if (!Notes.GetIsGameRunning() && gameObject != null)
            Destroy(gameObject);
            
        if (MouseInputProvider.GetDidClickObject() == false)
        {
            Notes.AddToFails();
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            MouseInputProvider.SetDidClickObject(false);
        }
    }
}

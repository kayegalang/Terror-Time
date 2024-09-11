using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMusicNotes : MonoBehaviour
{
    public Notes Notes;

    public void OnButtonClick()
    {
        Notes.OnEnable();
    }
}

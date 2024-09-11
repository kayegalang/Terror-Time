using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    private float NoteSpeed = 5f;
    void Update()
    {
        transform.position += new Vector3(0f, NoteSpeed * Time.deltaTime, 0f);
    }
}

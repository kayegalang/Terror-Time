using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip KnifeClip;
    public AudioClip BloodClip;

    public void PlayKnifeSound()
    {
        AudioSource.clip = KnifeClip;
        AudioSource.Play();
    }

    public void PlayBloodSound()
    {
        AudioSource.clip = BloodClip;
        AudioSource.Play();
    }
}
    
    
    
    
    
    


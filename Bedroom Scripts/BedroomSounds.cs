using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BedroomSounds : MonoBehaviour
{
    public AudioSource MusicAudioSource;
    public AudioClip BedroomMusicClip;
    
    public AudioSource SoundAudioSource;
    public AudioClip EnterClosetClip;
    public AudioClip OpenClosetClip;
    public AudioClip CloseClosetClip;
    
    public AudioClip EnterBedClip;

    public void PlayEnterBedSound()
    {
        MusicAudioSource.loop = false;
        MusicAudioSource.clip = EnterBedClip;
        MusicAudioSource.Play();
    }
    public void PlayEnterClosetSound()
    {
        MusicAudioSource.loop = false;
        MusicAudioSource.clip = EnterClosetClip;
        MusicAudioSource.Play();
    }

    public void PlayBedroomMusic()
    {
        MusicAudioSource.loop = true;
        MusicAudioSource.clip = BedroomMusicClip;
        MusicAudioSource.Play();
    }

    public void PlayOpenClosetSound()
    {
        SoundAudioSource.clip = OpenClosetClip;
        SoundAudioSource.Play();
    }

    public void PlayCloseClosetSound()
    {
        SoundAudioSource.clip = CloseClosetClip;
        SoundAudioSource.Play();
    }

    public void StopBedroomMusic()
    {
        MusicAudioSource.Stop();
    }

    public AudioSource GetAudioSource()
    {
        return MusicAudioSource;
    }
    
    
}

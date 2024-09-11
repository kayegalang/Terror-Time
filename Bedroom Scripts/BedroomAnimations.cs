using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BedroomAnimations : MonoBehaviour
{
    public GameChooser GameChooser;
    public BedroomSounds BedroomSounds;

    [SerializeField] private string EnterClosetClip;
    [SerializeField] private string OpenClosetClip;
    [SerializeField] private string CloseClosetClip;
    
    [SerializeField] private string EnterBedClip;
    [SerializeField] private string MouseOnBedClip;
    [SerializeField] private string MouseStaysOnBedClip;
    [SerializeField] private string MouseExitsBedClip;

    void Start()
    {
        StopAllCoroutines();
    }
    
    public void PlayVideo(string videoFileName)
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }

    public void PlayEnterBedClip()
    {
        PlayVideo(EnterBedClip);
    }

    public void PlayMouseOnBedClip()
    {
        PlayVideo(MouseOnBedClip);

        StartCoroutine(WaitTilMouseStaysOnBedClip());
    }

    IEnumerator WaitTilMouseStaysOnBedClip()
    {
        yield return new WaitForSeconds(1f);
            
        PlayVideo(MouseStaysOnBedClip);
    }

    public void PlayMouseExitsBedClip()
    {
        StopAllCoroutines();
        
        PlayVideo(MouseExitsBedClip);
    }

    public void PlayEnterClosetClip()
    {
        PlayVideo(EnterClosetClip);
    }

    public void PlayOpenClosetClip()
    {
        GameChooser.HideBedroom();
        BedroomSounds.PlayOpenClosetSound();
        PlayVideo(OpenClosetClip);
    }

    public void PlayCloseClosetClip()
    {
        GameChooser.HideBedroom();
        BedroomSounds.PlayCloseClosetSound();
        PlayVideo(CloseClosetClip);
    }
    
}

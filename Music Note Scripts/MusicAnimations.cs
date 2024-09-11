using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MusicAnimations : MonoBehaviour
{
    public VideoPlayer VideoPlayer;

    [SerializeField] private string StageOneClip;
    [SerializeField] private string FailOneClip;
    
    [SerializeField] private string StageTwoClip;
    [SerializeField] private string FailTwoClip;
    
    [SerializeField] private string StageThreeClip;
    [SerializeField] private string FailThreeClip;

    public void PlayVideo(string videoFileName)
    {
        VideoPlayer videoPlayer = VideoPlayer;

        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
    public void PlayStageOneClip()
    {
        VideoPlayer.isLooping = true;
        PlayVideo(StageOneClip);
    }

    public void PlayFailOneClip()
    {
        VideoPlayer.isLooping = false;
        PlayVideo(FailOneClip);
    }

    public void PlayStageTwoClip()
    {
        VideoPlayer.isLooping = true;
        PlayVideo(StageTwoClip);
    }

    public void PlayFailTwoClip()
    {
        VideoPlayer.isLooping = false;
        PlayVideo(FailTwoClip);
    }

    public void PlayStageThreeClip()
    {
        VideoPlayer.isLooping = true;
        PlayVideo(StageThreeClip);
    }

    public void PlayFailThreeClip()
    {
        VideoPlayer.isLooping = false;
        PlayVideo(FailThreeClip);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameChooser : MonoBehaviour
{
    public GameObject Closet;
    public GameObject UnderBed;
    public GameObject BedroomPanel;
    public GameObject BedroomCanvas;

    public GameObject FirstClosetMinigame;
    public GameObject FirstBedMinigame;

    public GameObject KeyHolderPanel;

    public BedroomAnimations BedroomAnimations;
    public BedroomSounds BedroomSounds;

    private int closetClicks;
    private int bedClicks;
    
    public BedroomManager BedroomManager;
    public enum GameState { FreeRoam, Dialogue }

    private GameState state;
    
    [SerializeField] private Dialogue closetFinishedDialogue;
    [SerializeField] private Dialogue bedFinishedDialogue;

    public void Start()
    {
        closetClicks = 0;
        bedClicks = 0;
    }
    public void OnClosetClick()
    {
        BedroomManager.StopAllCoroutines();

        if (closetClicks == 0)
        {
            EnterCloset();
        }

        if (closetClicks > 0)
        {
            PlayClosetDoneDialogue();
        }
    }

    private void PlayClosetDoneDialogue()
    {
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.FreeRoam;
        };
        
        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (state == GameState.Dialogue)
                state = GameState.Dialogue;
        };
        
        StartCoroutine(DialogueManager.Instance.ShowDialogue(closetFinishedDialogue));    }

    private void EnterCloset()
    {
        KeyHolderPanel.SetActive(false);
        HideBedroom();
        BedroomAnimations.PlayEnterClosetClip();
        BedroomSounds.PlayEnterClosetSound();

        StartCoroutine(FadeAudioSource.StartFade(BedroomSounds.GetAudioSource(), 5f, 0f));
        StartCoroutine(WaitTilClosetGamesStart());    }

    IEnumerator WaitTilClosetGamesStart()
    {
        yield return new WaitForSeconds(2f);
        HideWholeBedroom();
        Closet.SetActive(true);
        FirstClosetMinigame.SetActive(true);
        closetClicks++;
    }

    public void OnUnderBedClick()
    {
        BedroomManager.StopAllCoroutines();
        if (bedClicks == 0)
        {
            BedroomAnimations.StopAllCoroutines();
            KeyHolderPanel.SetActive(false);
            HideBedroom();
            BedroomAnimations.PlayEnterBedClip();
            BedroomSounds.PlayEnterBedSound();

            StartCoroutine(FadeAudioSource.StartFade(BedroomSounds.GetAudioSource(), 5f, 0f));
            StartCoroutine(WaitTilBedGamesStart());
        }

        if (bedClicks > 0)
        {
            DialogueManager.Instance.OnShowDialogue += () =>
            {
                state = GameState.FreeRoam;
            };
        
            DialogueManager.Instance.OnCloseDialogue += () =>
            {
                if (state == GameState.Dialogue)
                    state = GameState.Dialogue;
            };
        
            StartCoroutine(DialogueManager.Instance.ShowDialogue(bedFinishedDialogue));
        }

        
    }

    public void HideBedroom()
    {
        BedroomPanel.SetActive(false);
    }

    private void HideWholeBedroom()
    {
        BedroomCanvas.SetActive(false);
    }
    
    IEnumerator WaitTilBedGamesStart()
    {
        yield return new WaitForSeconds(2f);
        
        KeyHolderPanel.SetActive(false);
        HideWholeBedroom();
        UnderBed.SetActive(true);
        FirstBedMinigame.SetActive(true);
        bedClicks++;
    }

}

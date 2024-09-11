using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomManager : MonoBehaviour
{
    public BedroomSounds BedroomSounds;
    public enum GameState { FreeRoam, Dialogue }

    private GameState state;
    private bool isHintOn;


    [SerializeField] private Dialogue bedroomDialogue;
    
    [SerializeField] private Dialogue closetHintDialogue;
    [SerializeField] private Dialogue bedHintDialogue;
    
    [SerializeField] private Dialogue clockDialogue;
    [SerializeField] private Dialogue paintingDialogue;

    public void Start()
    {
        isHintOn = false;
        PlayStartGameDialogue();
    }

    void Update()
    {
        if (state == GameState.Dialogue)
            DialogueManager.Instance.OnButtonClick();
        
        if (state == GameState.FreeRoam && isHintOn == false) 
            PlayClosetHintAfterTenSeconds();
    }
    public void OnClockClick()
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
        
        StartCoroutine(DialogueManager.Instance.ShowDialogue(clockDialogue));
    }

    public void OnPaintingClick()
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
        
        StartCoroutine(DialogueManager.Instance.ShowDialogue(paintingDialogue));
    }

    private void PlayStartGameDialogue()
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
        
        StartCoroutine(DialogueManager.Instance.ShowDialogue(bedroomDialogue));    }

    private void PlayClosetHintAfterTenSeconds()
    {
        isHintOn = true;
        StartCoroutine(WaitTilClosetHintAppears()); 
    }

    private void OnEnable()
    {
        BedroomSounds.PlayBedroomMusic();
    }
    
    IEnumerator WaitTilClosetHintAppears()
    {
        yield return new WaitForSeconds(10f);

        DialogueManager.Instance.OnShowDialogue += () => { state = GameState.FreeRoam; };

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (state == GameState.Dialogue)
            {
                state = GameState.Dialogue;
            }
        };

        StartCoroutine(DialogueManager.Instance.ShowDialogue(closetHintDialogue));

        if (state == GameState.FreeRoam)
            StartCoroutine(WaitTilBedHintAppears());
    }
    
    IEnumerator WaitTilBedHintAppears()
    {
        yield return new WaitForSeconds(10f);

        DialogueManager.Instance.OnShowDialogue += () => { state = GameState.FreeRoam; };

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (state == GameState.Dialogue)
            {
                state = GameState.Dialogue;
            }
        };

        StartCoroutine(DialogueManager.Instance.ShowDialogue(bedHintDialogue));

        isHintOn = false;

    }
}

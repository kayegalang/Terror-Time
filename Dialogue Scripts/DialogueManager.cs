using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Text dialogueText;
    [SerializeField] private int lettersPerSecond;
    
    public static DialogueManager Instance { get; private set; }

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;

    public GameObject NextLineButton;

    private void Awake()
    {
        Instance = this;
    }
    
    public void OnButtonClick()
    {
        if (!isTyping)
        {
            currentLine++;
            if (currentLine < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                NextLineButton.SetActive(false);
                dialogueBox.SetActive(false);
                OnCloseDialogue?.Invoke();
            }
        }
    }

    private Dialogue dialogue;
    private int currentLine;
    private bool isTyping; 
    
    public IEnumerator ShowDialogue(Dialogue dialogue)
    {
        yield return new WaitForEndOfFrame();
        
        OnShowDialogue?.Invoke();
        
        this.dialogue = dialogue;
        
        NextLineButton.SetActive(true);
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }

    public IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isTyping = false;
    }
}

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Notes : MonoBehaviour
{
    public GameObject WinScreen;

    public AudioSource AudioSource;
    public AudioSource SoundEffectsSource;
    public AudioClip CreepyMusicClip;
    public AudioClip PuppetLaugh;
    public AudioClip Scream;
    
    public MusicAnimations MusicAnimations;
    

    public GameObject[] NoteObjects = new GameObject[4];
    public float bpm;

    public GameObject GameOverPanel;
    public GameObject TryAgainPanel;

    private GameObject newObject;
    
    private float lastTime, deltaTime, timer;
    private int lastNumber;

    private int numberOfFails;

    private bool isGameRunning = true;
    private bool didWinGame = false;

    public void OnEnable()
    {
        numberOfFails = 0;
        Reset();
    }

    private void Reset()
    {
        isGameRunning = true;

        AudioSource.clip = CreepyMusicClip;
        AudioSource.Play();
        
        GameOverPanel.SetActive(false);
        TryAgainPanel.SetActive(false);
        
        MusicAnimations.PlayStageOneClip();
        
        lastTime = 0f;
        deltaTime = 0f;
        timer = 0f;
        numberOfFails = 0;
    }

    public void Update()
    {
        if (isGameRunning)
        {
            int randomNoteNumber = GetRandomNoteNumber();

            deltaTime = GetComponent<AudioSource>().time - lastTime;
            timer += deltaTime;

            if (timer >= (60f / bpm))
            {
                Place(randomNoteNumber);

                timer -= (60f / bpm);
            }

            lastTime = GetComponent<AudioSource>().time;
            
            if (numberOfFails < 3 && !AudioSource.isPlaying)
                WinGame();
        }
    }

    private void WinGame()
    {
        StopAllCoroutines();
        
        Destroy(newObject);
        
        isGameRunning = false;

        WinScreen.SetActive(true);
    }

    private int GetRandomNoteNumber()
    {
        int rand = Random.Range(0, 4);

        while (rand == lastNumber)
            rand = Random.Range(0, 4);

        lastNumber = rand;

        return lastNumber;
    }

    private void Place(int randomNoteNumber)
    {
        Vector3 position = SpriteTools.RandomTopOfScreenLocationWorldSpace(); 
        newObject = Instantiate(NoteObjects[randomNoteNumber], position, Quaternion.identity);
        Failures failures = newObject.GetComponent<Failures>();
        failures.Notes = this;
    }

    public void AddToFails()
    {
        if (!didWinGame)
        {
            numberOfFails++;
            if (numberOfFails == 1)
            {
                SoundEffectsSource.clip = PuppetLaugh;
                SoundEffectsSource.Play();
                
                MusicAnimations.PlayFailOneClip();
                StartCoroutine(WaitTilStageTwo());
            }

            if (numberOfFails == 2)
            {
                MusicAnimations.PlayFailTwoClip();
                StartCoroutine(WaitTilStageThree());
            }

            if (numberOfFails == 3)
            {
                MusicAnimations.PlayFailThreeClip();
                StartCoroutine(WaitTilScream());
                StartCoroutine(WaitTilLoseGame());
            }
        }
        
    }

    IEnumerator WaitTilScream()
    {
        yield return new WaitForSeconds(1.5f);
        AudioSource.clip = Scream;
        AudioSource.Play();
    }

    IEnumerator WaitTilStageTwo()
    {
        yield return new WaitForSeconds(1f);
        MusicAnimations.PlayStageTwoClip();
    }
    
    IEnumerator WaitTilStageThree()
    {
        yield return new WaitForSeconds(1f);
        MusicAnimations.PlayStageThreeClip();
    }

    IEnumerator WaitTilLoseGame()
    {
        yield return new WaitForSeconds(2f);
        LoseGame();
    }

    private void LoseGame()
    {
        isGameRunning = false;
        AudioSource.Stop();
        GameOverPanel.SetActive(true);
        StartCoroutine(WaitTilTryAgainPanelAppears());
    }
    
    public IEnumerator WaitTilTryAgainPanelAppears()
    {
        yield return new WaitForSeconds(2f);
        TryAgainPanel.SetActive(true);
    }

    public bool GetIsGameRunning()
    {
        return isGameRunning;
    }
}

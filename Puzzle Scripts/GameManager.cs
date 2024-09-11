using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
  public GameObject RawImage;
  public VideoPlayer VideoPlayer;
  [SerializeField] private string JumpscareClip;
  public AudioSource JumpscareAudioSource;
  public AudioClip ScreamClip;

  public GameObject WinScreen;
  
  public GameObject KeyHolderPanel;
  public GameObject BedroomPanel;

  public AudioSource AudioSource;
  public AudioClip SlideClip;

  public GameObject GameOverPanel;
  public GameObject TryAgainPanel;

  public PuzzleGameTimer PuzzleGameTimer;
  
  [SerializeField] private Transform gameTransform;
  [SerializeField] private Transform piecePrefab;

  private List<Transform> pieces;
  private int emptyLocation;
  private int size;
  private bool shuffling = false;
  private bool isRunning = false;
  private bool shuffled = false;

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
  private void CreateGamePieces(float gapThickness)
  {
    // Width of each tile
    float width = 1 / (float)size;
    for (int row = 0; row < size; row++)
    {
      for (int col = 0; col < size; col++)
      {
        Transform piece = Instantiate(piecePrefab, gameTransform);
        pieces.Add(piece);
        // Pieces from -1 to +1
        piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
          +1 - (2 * width * row) - width,
          0);
        piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
        piece.name = $"{(row * size) + col}";
        // Empty space in the bottom right
        if ((row == size - 1) && (col == size - 1))
        {
          emptyLocation = (size * size) - 1;
          piece.gameObject.SetActive(false);
        }
        else
        {
          // UV coordinates, they are 0->1
          float gap = gapThickness / 2;
          Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
          Vector2[] uv = new Vector2[4];
          // UV coord order (0, 1), (1, 1), (0, 0), (1, 0)
          uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
          uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
          uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
          uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
          // Assign new UVs to the mesh
          mesh.uv = uv;
        }
      }
    }
  }

  public void OnEnable()
  {
    RawImage.SetActive(false);
    
    shuffled = false;
    isRunning = true;

    GameOverPanel.SetActive(false);
    TryAgainPanel.SetActive(false);

    pieces = new List<Transform>();
    size = 4;
    CreateGamePieces(0.01f);
    
    // Check for completion
    if (!shuffling && CheckCompletion())
    {
      shuffling = true;
      StartCoroutine(WaitShuffle(3f));
    }
  }

  public void Reset()
  {
      foreach (Transform piece in pieces)
        Destroy(piece.gameObject);
  }

  void Update()
  {
    if (isRunning && shuffled && CheckCompletion())
    {
      isRunning = false;
      WinGame();
    }
    
    
    if (isRunning)
    {
      // See if we click a piece
      if (Input.GetMouseButtonDown(0))
      {
        AudioSource.clip = SlideClip;
        AudioSource.Play();
        
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
          // Go through the list to see position
          for (int i = 0; i < pieces.Count; i++)
          {
            if (pieces[i] == hit.transform)
            {
              // Check each direction to see if valid move
              if (SwapIfValid(i, -size, size))
              {
                break;
              }

              if (SwapIfValid(i, +size, size))
              {
                break;
              }

              if (SwapIfValid(i, -1, 0))
              {
                break;
              }

              if (SwapIfValid(i, +1, size - 1))
              {
                break;
              }
            }
          }
        }
      }
    }
    
  }

  private void WinGame()
  {
    WinScreen.SetActive(true);
    
    StopAllCoroutines();
    
    PuzzleGameTimer.StopTimer();
    
    Reset();
    
    KeyHolderPanel.SetActive(true);
    BedroomPanel.SetActive(true);
  }

  public void LoseGame()
  {
    RawImage.SetActive(true);
    PlayVideo(JumpscareClip);
    JumpscareAudioSource.clip = ScreamClip;
    JumpscareAudioSource.Play();
    
    StartCoroutine(WaitTilYouDiedScreenAppears());
  }
  
  IEnumerator WaitTilYouDiedScreenAppears()
  {
    yield return new WaitForSeconds(1f);
    RawImage.SetActive(false);
    GameOverPanel.SetActive(true);
    StartCoroutine(WaitTilTryAgainPanelAppears());
  }

  IEnumerator WaitTilTryAgainPanelAppears()
  {
    yield return new WaitForSeconds(2f);
    TryAgainPanel.SetActive(true);
  }

  // Stop horizontal moves wrapping
  private bool SwapIfValid(int i, int offset, int colCheck)
  {
    if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
    {
      // Swap them in game state
      (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
      // Swap their transforms
      (pieces[i].localPosition, pieces[i + offset].localPosition) =
        ((pieces[i + offset].localPosition, pieces[i].localPosition));
      // Update empty location
      emptyLocation = i;
      return true;
    }

    return false;
  }

  // check completion
  private bool CheckCompletion()
  {
    for (int i = 0; i < pieces.Count; i++)
    {
      if (pieces[i].name != $"{i}")
      {
        return false;
      }
    }

    return true;
  }

  private IEnumerator WaitShuffle(float duration)
  {
    yield return new WaitForSeconds(duration);
    PuzzleGameTimer.StartTimer(180);
    Shuffle();
    shuffling = false;
    shuffled = true;
  }

  // Shuffling
  private void Shuffle()
  {
    int count = 0;
    int last = 0;
    while (count < (size * size * size))
    {
      // Pick a random location.
      int rnd = Random.Range(0, size * size);
      // forbid undoing the last move
      if (rnd == last)
      {
        continue;
      }

      last = emptyLocation;
      // Try surrounding spaces looking for valid move
      if (SwapIfValid(rnd, -size, size))
      {
        count++;
      }
      else if (SwapIfValid(rnd, +size, size))
      {
        count++;
      }
      else if (SwapIfValid(rnd, -1, 0))
      {
        count++;
      }
      else if (SwapIfValid(rnd, +1, size - 1))
      {
        count++;
      }
    }
  }
}
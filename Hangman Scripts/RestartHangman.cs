using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartHangman : MonoBehaviour
{
   public HangmanManager HangmanManager;
   public KeyboardResetter KeyboardResetter;
   
   public void OnButtonClick()
   {
      HangmanManager.OnEnable();
      KeyboardResetter.Reset();
   }
}

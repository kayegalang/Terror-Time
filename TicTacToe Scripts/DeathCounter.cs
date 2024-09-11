using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    public Image DeathCounterImage;
    public Sprite ShadedSkullSprite;
    public Sprite UnshadedSkullSprite;

    public void OnFirstLoss()
    {
        DeathCounterImage.sprite = ShadedSkullSprite; 
        
    }

    public void ResetSkull()
    {
        DeathCounterImage.sprite = UnshadedSkullSprite;
    }

}

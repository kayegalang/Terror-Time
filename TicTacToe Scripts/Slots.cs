using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Slots : MonoBehaviour
{
    public TicTacToeGame TicTacToeGame;
    public List<Slot> SlotsList;
    public Sprite BloodSprite;
    public Sprite KnifeSprite;
    public Sprite BlankSprite;

    private List<MarkerType> slotOccupants;
    public void OnSlotClicked(Slot slot)
    {
        TicTacToeGame.OnSlotClicked(slot);
    }

    public void UpdateSlot(Slot slot, MarkerType markerType)
    {
        SetSlotImage(slot, markerType);
        SetSlotOccupant(slot, markerType);
    }

    public List<MarkerType> SlotOccupants()
    {
        return slotOccupants;
    }
    
    public void ResetSlotOccupants()
    {
        slotOccupants = new List<MarkerType>();
        for(int i = 0; i < 9; i++)
            slotOccupants.Add(MarkerType.None);
    }
    
    public void Reset()
    {
        ResetSlotOccupants();
        ResetSlotImages();
    }

    public Slot GetSlot(int slotIndex)
    {
        return SlotsList[slotIndex];
    }

    public Slot RandomFreeSlot()
    {
        // find list of all empty slots
        List<int> emptySlotIndices = FindEmptySlotIndices();
        
        // pick random slot from list
        int randomIndex = Random.Range(0, emptySlotIndices.Count);

        int randomSlotIndex = emptySlotIndices[randomIndex];

        return SlotsList[randomSlotIndex];
    }

    private List<int> FindEmptySlotIndices()
    {
        List<int> emptySlotIndices = new List<int>();

        for (int i = 0; i < SlotsList.Count; i++)
        {
            if (SlotsList[i].IsEmpty())
                emptySlotIndices.Add(i);
        }
        
        return emptySlotIndices;
    }
    private void SetSlotOccupant(Slot slot, MarkerType markerType)
    {
        int slotIndex = slot.SlotNumber - 1;
        slotOccupants[slotIndex] = markerType;
    }

    private void SetSlotImage(Slot slot, MarkerType markerType)
    {
        if (markerType == MarkerType.Knife)
            slot.Mark(KnifeSprite);
        else
            slot.Mark(BloodSprite);
    }
    
    private void ResetSlotImages()
    {
        foreach (Slot slot in SlotsList)
            slot.Reset(BlankSprite);
    }

  
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<ItemEntry> items = new List<ItemEntry>();
    public PlayerBrain player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerBrain>();
    }

    public void AddItem(ItemType itemToAdd, int amount)
    {
        ItemEntry entry = items.Find(x => x.item == itemToAdd);

        if (entry == null)
        {
            entry = new ItemEntry
            {
                item = itemToAdd,
                amount = 0
            };

            items.Add(entry);
        }

        entry.amount += amount;
    }
}


public enum ItemType
{
    RedKey,
    BlueKey
}

[System.Serializable]
public class ItemEntry
{
    public ItemType item;
    public int amount = 0;
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Event triggered ONLY when the inventory changes
    public static event Action OnInventoryUpdated;

    public List<ItemEntry> items = new List<ItemEntry>();
    public PlayerBrain player;
    
    [Header("Inventory Settings")]
    public int maxSlots = 2;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerBrain>();
    }

    public bool AddItem(ItemType itemToAdd, int amount)
    {
        ItemEntry entry = items.Find(x => x.item == itemToAdd);

        if (entry == null)
        {
            if (items.Count >= maxSlots)
            {
                Debug.Log("Inventory full!");
                return false;
            }

            entry = new ItemEntry
            {
                item = itemToAdd,
                amount = 0,
            };

            items.Add(entry);
        }

        entry.amount += amount;
        OnInventoryUpdated?.Invoke();
       

        return true;
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
    public Sprite icon;
}
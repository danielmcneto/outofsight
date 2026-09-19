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
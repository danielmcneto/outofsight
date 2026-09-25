using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public RawImage itemicon;
    private ItemEntry item;
    public TextMeshProUGUI count;
    public int itemnumber;
    public GameController gc;

    private void Start()
    {
        //find gamecontroller to fetch from
        gc = FindAnyObjectByType<GameController>();
        item = gc.items[itemnumber];
        
        //set initial icon and count
        itemicon.texture = item.icon;
        count.text = item.amount.ToString();
    }

    private void Update()
    {
        //if item is ran out, remove the icon and display no count
        if (item.amount <= 0)
        {
            count.text = "";
            itemicon.enabled = false;
        }
        else if(item.amount > 0)
        {
            //set the itementry thats looked on to the one according to itemnumber and then set the icon and count
            itemicon.enabled = true;
            itemicon.texture = item.icon;
            count.text = item.amount.ToString();
        }
    }
}

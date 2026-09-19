using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DoorActivator : MonoBehaviour
{
    public Door door;
    public GameController gc;
    [Space]
    public bool useKey;
    public ItemType requiredItem;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (useKey)
            {
                ItemEntry entry = gc.items.Find(x => x.item == requiredItem);
                if (entry != null && entry.amount > 0)
                {
                    Debug.Log("has one of item " + entry.item + ", opening door.");
                    //open
                    door.AttemptUnlock(gc.player.transform);
                    //deduct 1 from the amount of said requireditem
                    entry.amount--;
                    //since door has been unlocked, stop requiring a key
                    useKey = false;
                }
            }
            else
            {
                //open
                door.AttemptUnlock(gc.player.transform);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //close
            door.Close();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}

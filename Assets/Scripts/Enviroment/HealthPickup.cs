using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int amount = 20;
    private void OnTriggerEnter(Collider other)
    {
        //heal the player
        PlayerBrain player = FindObjectOfType<PlayerBrain>();
        player.TakeHealth(amount);
        
        StartCoroutine(Pickup());
    }
    
    IEnumerator Pickup()
    {
        float duration = 0.05f;
        float elapsed = 0f;

        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * 1.5f;

        while (elapsed < duration)
        {
            //move the object upwards as a form of "pick up animation"
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}

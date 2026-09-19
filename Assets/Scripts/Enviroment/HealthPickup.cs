using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int amount = 20;
    private void OnTriggerEnter(Collider other)
    {
        PlayerBrain player = FindObjectOfType<PlayerBrain>();
        player.TakeHealth(amount);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    public GameController gc;
    public ItemType itemToGive;
    public int amount = 1;
    // Start is called before the first frame update
    private void Start()
    {
        gc = FindFirstObjectByType<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        gc.AddItem(itemToGive, amount);

        // Prevent collecting the same item repeatedly.
        Destroy(this.gameObject);
    }
}

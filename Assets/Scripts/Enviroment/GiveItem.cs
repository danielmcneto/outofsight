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
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}

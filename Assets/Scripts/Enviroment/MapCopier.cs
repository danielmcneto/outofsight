using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCopier : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 900, 0);
    private bool instantiated = false;
    public GameObject duplicate;
    
    void Start()
    {
        //duplicate and apply offset
        duplicate = Instantiate(gameObject, transform.parent);
        duplicate.transform.position = transform.position + offset;
        //set layer and disable mapcopier component to prevent further more copies
        duplicate.layer = 3;
        duplicate.GetComponent<MapCopier>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        //create empty material and apply it to all mesh renderer objects in parent and children to show as a solid green in the minimap
        var m = new Material(Shader.Find("Standard")); foreach (var r in duplicate.GetComponentsInChildren<Renderer>()) r.sharedMaterials = r.sharedMaterials.Select(_ => m).ToArray();
        duplicate.GetComponent<MapCopier>().enabled = false;
    }
}

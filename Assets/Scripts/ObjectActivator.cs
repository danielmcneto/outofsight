using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectActivator : MonoBehaviour
{
    public bool oneTime;
    public bool dontInvokeAutomatically;
    public UnityEvent onInvoked;
    public List<GameObject> objectsToEnable = new List<GameObject>();
    public List<GameObject> objectsToDisable = new List<GameObject>();
    
    private bool jobdone;

    private void Start()
    {
        if (oneTime && !jobdone && !dontInvokeAutomatically)
        {
            Execute();
            jobdone = true;
            gameObject.SetActive(false);
        }
        else
        {
            if (!dontInvokeAutomatically)
            {
                Execute();
            }
        }
    }

    public void Execute()
    {
        onInvoked.Invoke();
        foreach (GameObject o in objectsToEnable)
        {
            o.SetActive(true);
        }
        foreach (GameObject o in objectsToDisable)
        {
            o.SetActive(false);
        }
    }
}

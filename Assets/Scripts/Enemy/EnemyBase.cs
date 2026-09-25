using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public NavMeshAgent agent;
    public float updatedelay = 0.25f;
    
    void Start()
    {
        //get navmesh agent component
        agent = GetComponent<NavMeshAgent>();
    }
}

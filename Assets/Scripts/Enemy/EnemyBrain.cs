using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public Transform target;
    private EnemyBase enemybase;
    private float stopdistance;
    private float pathupdatefinishline;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemybase = GetComponent<EnemyBase>();
        stopdistance = enemybase.agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            bool inenemyrange = Vector3.Distance(transform.position, target.position) <= stopdistance;
            if (inenemyrange)
            {
                LookAtTarget();
            }
            else
            {
                UpdatePath();
            }
        }
    }
    
    private void LookAtTarget()
    {
        Vector3 look = target.position - transform.position;
        look.y = 0;
        Quaternion rotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath()
    {
        if (Time.time >= pathupdatefinishline)
        {
            //update enemy path
            pathupdatefinishline = Time.time + enemybase.updatedelay;
            enemybase.agent.SetDestination(target.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private float damageInterval = 0.3f;
    [SerializeField] private int damageAmount = 1;
    private float nextDamageTime;
    private bool wasPlayerDetected;
    public Transform player;
    public Transform[] targets;
    private EnemyBase enemybase;
    public float stopdistance;
    private float pathupdatefinishline;
    private int currentTarget = 0;
    private DetectPlayer detectPlayer;
    public MeshCollider visionMesh;
    
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemybase = GetComponent<EnemyBase>();
        stopdistance = enemybase.agent.stoppingDistance;
        detectPlayer = visionMesh.GetComponent<DetectPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool detected = detectPlayer != null && detectPlayer.PlayerInVision;

        if (detected)
        {
            if (!wasPlayerDetected)
                nextDamageTime = Time.time + damageInterval;

            wasPlayerDetected = true;

            if (Time.time >= nextDamageTime)
            {
                player.GetComponent<PlayerBrain>().TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval;
            }

            UpdatePath(player);
            return;
        }
        else
        {
            wasPlayerDetected = false;
        }


        if (player != null)
        {
            
            bool inenemyrange = Vector3.Distance(transform.position, player.position) <= stopdistance;
            if (inenemyrange)
            {
                LookAtTarget(targets[currentTarget]);
                return;
            }

            var agent = enemybase.agent;
            if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentTarget = (currentTarget + 1) % targets.Length;
                UpdatePath(targets[currentTarget]);
            }
            
        }
    }
    
    private void LookAtTarget(Transform target)
    {
        Vector3 look = target.position - transform.position;
        look.y = 0;
        Quaternion rotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath(Transform target)
    {
        if (Time.time >= pathupdatefinishline)
        {
            //update enemy path
            pathupdatefinishline = Time.time + enemybase.updatedelay;
            enemybase.agent.SetDestination(target.position);
        }
    }
}

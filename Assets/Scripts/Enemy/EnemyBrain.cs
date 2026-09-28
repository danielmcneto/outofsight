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
    private float maxHealth;
    private float health;
    private LayerMask playerLayer;
    private float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemybase = GetComponent<EnemyBase>(); 
        playerLayer = enemybase.playerLayer;
        stopdistance = enemybase.agent.stoppingDistance;
        speed = enemybase.agent.speed;
        maxHealth = enemybase.health;
        health = maxHealth;
        detectPlayer = visionMesh.GetComponent<DetectPlayer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool detected = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10, playerLayer))
        {
            detected = hit.collider.CompareTag("Player");
        }
        //bool detected = Physics.Raycast(transform.position, transform.forward, out hit, 10, playerLayer);
        
        if (detected)
        {
            enemybase.agent.speed = 0;
            if (!wasPlayerDetected)
                nextDamageTime = Time.time + damageInterval;

            wasPlayerDetected = true;
            LookAtTarget(player);

            if (Time.time >= nextDamageTime)
            {
                Shoot();
            }

            UpdatePath(player);
            return;
        }
        else
        {
            enemybase.agent.speed = speed;
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

    private void Shoot()
    {
        Debug.Log("enter shoot");
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("hitplayer");
                player.GetComponent<PlayerBrain>().TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval;
            }
            else
            {
                UpdatePath(player);
            }
            
        }

        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Take damage:" + damage);
        if(health <= 0)
        {
            Debug.Log("Im dead");
            health = 0;
        }
    }
}

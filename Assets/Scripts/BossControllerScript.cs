using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossControllerScript : MonoBehaviour
{
    public float shootingInterval = 2f;          // Interval between each projectile shot
    public GameObject projectile;             // Prefab of the projectile
    public Transform playerTransform;            // Reference to the player's transform

    [SerializeField]
    private bool isInRange = false;              // Flag to indicate if enemy is within shootissng range
    private NavMeshAgent navMeshAgent;           // Reference to the NavMeshAgent component
    public float portalSpawnInterval = 5f;
    public GameObject portalPrefab;
    public GameStats gameStats;
    private Animator anim;
    private float health;
    private bool isDead;
    public bool isWarping;
    private Vector3 posLastFrame;

    void Start()
    {
        StartCoroutine(Shoot());
        //StartCoroutine(SpawnPortals());

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        navMeshAgent.speed = gameStats.boss.BossSpeed;
        health = gameStats.boss.BossMaxHealth;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            Die();
        }

        if (gameStats.player.PlayerHealth > 0 && !isDead)
        {
            // Move towards the player using NavMeshAgent
            navMeshAgent.SetDestination(playerTransform.position);

            if (playerTransform.position.x > transform.position.x)
                transform.eulerAngles = new Vector3(0, 0, 0);
            if (playerTransform.position.x < transform.position.x)
                transform.eulerAngles = new Vector3(0, 180, 0);

            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Check if the enemy is within the shooting range
            if (distanceToPlayer <= gameStats.boss.BossMinDistance)
            {
                isInRange = true;
                navMeshAgent.speed = gameStats.boss.BossSlowSpeed;
            }
            else
            {
                isInRange = false;
                navMeshAgent.speed = gameStats.boss.BossSpeed;
            }

            if (distanceToPlayer > 10)
            {
                Vector3 teleportPosition;
                // Teleport to the left or right side of the player based on relative positions
                if (transform.position.x < playerTransform.position.x)
                {
                    // Teleport to the right side of the player
                    teleportPosition = playerTransform.position + Vector3.right * gameStats.boss.BossMinDistance;
                }
                else
                {
                    // Teleport to the left side of the player
                    teleportPosition = playerTransform.position + Vector3.left * gameStats.boss.BossMinDistance;
                }
                navMeshAgent.Warp(teleportPosition);
                anim.SetTrigger("warp");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    void Die()
    {

    }

    IEnumerator Shoot()
    {
        if(isInRange)
        {
            Debug.Log("Shooting");
            GameObject projectileInstance = Instantiate(projectile, transform.position + Vector3.forward, Quaternion.identity);
            projectile.GetComponent<ProjectileController>().playerTransform = playerTransform;
            anim.SetTrigger("shoot");
        }

        yield return new WaitForSeconds(shootingInterval);
        StartCoroutine(Shoot());
    }
}

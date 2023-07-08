using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossControllerScript : MonoBehaviour
{
    public float shootingInterval = 2f;          // Interval between each projectile shot
    public GameObject invokerPrefab;             // Prefab of the projectile
    public Transform playerTransform;            // Reference to the player's transform

    private bool isInRange = false;              // Flag to indicate if enemy is within shootissng range
    private NavMeshAgent navMeshAgent;           // Reference to the NavMeshAgent component
    public float portalSpawnInterval = 5f;
    public GameObject portalPrefab;
    public GameStats gameStats;

    void Start()
    {
        StartCoroutine(Shoot());
        //StartCoroutine(SpawnPortals());
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        navMeshAgent.speed = gameStats.boss.BossSpeed;        
    }

    void Update()
    {
        // Move towards the player using NavMeshAgent
        navMeshAgent.SetDestination(playerTransform.position);

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
            // Teleport to the left or right side of the player based on relative positions
            if (transform.position.x < playerTransform.position.x)
            {
                // Teleport to the right side of the player
                Vector3 teleportPosition = playerTransform.position + Vector3.right * gameStats.boss.BossMinDistance;
                navMeshAgent.Warp(teleportPosition);
            }
            else
            {
                // Teleport to the left side of the player
                Vector3 teleportPosition = playerTransform.position + Vector3.left * gameStats.boss.BossMinDistance;
                navMeshAgent.Warp(teleportPosition);
            }
        }

    }

    IEnumerator Shoot()
    {
        if (isInRange)
        {
            Instantiate(invokerPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(shootingInterval);
    }

    /*IEnumerator SpawnPortals()
    {
        SpawnPortal(playerTransform.position + Vector3.up * gameStats.boss.BossMinDistance);
        SpawnPortal(playerTransform.position + Vector3.left * gameStats.boss.BossMinDistance);
        SpawnPortal(playerTransform.position + Vector3.right * gameStats.boss.BossMinDistance);
        SpawnPortal(playerTransform.position + Vector3.down * gameStats.boss.BossMinDistance);
        yield return new WaitForSeconds(portalSpawnInterval);
    }

    void SpawnPortal(Vector3 position)
    {
        // Instantiate a portal at the specified position
        GameObject portal = Instantiate(portalPrefab, position, Quaternion.identity);
        portal.GetComponent<PortalScript>().gameStats = gameStats;

    }*/
}

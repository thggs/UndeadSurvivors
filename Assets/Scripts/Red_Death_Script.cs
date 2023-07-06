using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Red_Death_Script : MonoBehaviour
{
    public float moveSpeed = 3f;                 // Speed at which the enemy moves towards the player
    public float shootingInterval = 2f;          // Interval between each projectile shot
    public GameObject invokerPrefab;             // Prefab of the projectile
    public Transform playerTransform;            // Reference to the player's transform
    public float minDistanceToPlayer = 5f;       // Minimum distance to maintain from the player
    public float slowMoveSpeed = 1f;             // Speed at which the enemy moves slowly in shooting range

    private float shootingTimer = 0f;            // Timer to track the shooting interval
    private bool isInRange = false;              // Flag to indicate if enemy is within shootissng range
    private NavMeshAgent navMeshAgent;           // Reference to the NavMeshAgent component
    public float portalSpawnInterval = 5f;
    private float portalSpawnTimer = 0f;
    public GameObject portalPrefab;
    public GameStats gameStats;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        // Move towards the player using NavMeshAgent
        navMeshAgent.SetDestination(playerTransform.position);

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if the enemy is within the shooting range
        if (distanceToPlayer <= minDistanceToPlayer)
        {
            isInRange = true;
            navMeshAgent.speed = slowMoveSpeed;
        }
        else
        {
            isInRange = false;
            navMeshAgent.speed = moveSpeed;
        }

        // Check if the enemy is within the shooting range and shoot projectile
        if (isInRange)
        {
            shootingTimer += Time.deltaTime;
            if (shootingTimer >= shootingInterval)
            {
                ShootProjectile();
                shootingTimer = 0f;
                Teleport();
            }
        }
        // Spawn portals around the player
        portalSpawnTimer += Time.deltaTime;
        if (portalSpawnTimer >= portalSpawnInterval)
        {
            SpawnPortals();
            portalSpawnTimer = 0f;
        }

    }

    void ShootProjectile()
    {
        // Instantiate a the invoker
        Instantiate(invokerPrefab, transform.position, Quaternion.identity);
    }

    void SpawnPortals()
    {
        // Spawn portals around the player
        SpawnPortal(playerTransform.position + Vector3.up);
        SpawnPortal(playerTransform.position + Vector3.left);
        SpawnPortal(playerTransform.position + Vector3.right);
        SpawnPortal(playerTransform.position + Vector3.down);
    }

    void SpawnPortal(Vector3 position)
    {
        // Instantiate a portal at the specified position
        GameObject portal = Instantiate(portalPrefab, position, Quaternion.identity);
        portal.GetComponent<PortalScript>().gameStats = gameStats;
        
    }

    void Teleport()
    {
        // Teleport to the left or right side of the player based on relative positions
        if (transform.position.x < playerTransform.position.x)
        {
            // Teleport to the right side of the player
            Vector3 teleportPosition = playerTransform.position + Vector3.right * minDistanceToPlayer;
            navMeshAgent.Warp(teleportPosition);
        }
        else
        {
            // Teleport to the left side of the player
            Vector3 teleportPosition = playerTransform.position + Vector3.left * minDistanceToPlayer;
            navMeshAgent.Warp(teleportPosition);
        }
    }
}

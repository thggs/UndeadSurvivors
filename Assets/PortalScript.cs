using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public GameObject zombiePrefab;            // Prefab of the zombie enemy
    public float enemySpawnInterval = 1f;      // Interval between spawning each zombie enemy
    public float spawnDuration = 4f;           // Duration of enemy spawning

    private Animator animator;                 // Reference to the animator component
    private float spawnTimer;                  // Timer to track enemy spawning
    private float closingTimer;                // Timer to track closing animation
    private bool isOpening;                    // Flag to indicate if portal is opening
    public GameStats gameStats;
    private bool isSpawning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Portal_Open");
    }

    void Update()
    {
        // Check if enemy spawning is in progress

        if (isSpawning)
        {
            // Spawn enemy at the specified interval
            spawnTimer += Time.deltaTime;
            // Check if enemy spawning duration is reached
            if (spawnTimer >= spawnDuration)
            {
                StopEnemySpawning();
                StartClosingAnimation();
            }
        }

    }

    void StartEnemySpawning()
    {
        // Start spawning enemies
        spawnTimer = 0f;
        isSpawning = true;
        animator.SetTrigger("Portal_Idle");
        InvokeRepeating("SpawnEnemy", 0f, enemySpawnInterval);
    }

    void SpawnEnemy()
    {
        // Instantiate a zombie enemy at the portal's position
        GameObject enemy = Instantiate(zombiePrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyControllerScript>().gameStats = gameStats;
    }

    void StopEnemySpawning()
    {
        // Stop spawning enemies
        isSpawning = false;
        CancelInvoke("SpawnEnemy");
    }

    void StartClosingAnimation()
    {
        // Start the closing animation
        animator.SetTrigger("Portal_Close");
    }
    void DestroyPortal()
    {
        Destroy(gameObject);
    }
}

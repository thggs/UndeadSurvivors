using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossControllerScript : MonoBehaviour
{
    public float shootingInterval = 10f;          // Interval between each projectile shot
    public GameObject projectile;             // Prefab of the projectile
    public Transform playerTransform;            // Reference to the player's transform

    [SerializeField]
    private bool isInRange = false;              // Flag to indicate if enemy is within shootissng range
    private NavMeshAgent navMeshAgent;           // Reference to the NavMeshAgent component
    public GameStats gameStats;
    private Animator anim;
    [SerializeField]
    private float health;
    private bool isDead;
    public bool isWarping;
    private Vector3 posLastFrame;
    private UI_game_manager ui;
    [SerializeField]
    private AudioClip winFanfare, evilLaugh;
    private AudioSource audioSource;
    private SpriteRenderer sprite;

    void Start()
    {
        StartCoroutine(Shoot());

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        ui = GameObject.FindGameObjectWithTag("UIToolkit").GetComponent<UI_game_manager>();
        audioSource = GetComponent<AudioSource>();
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        navMeshAgent.speed = gameStats.boss.BossSpeed;
        health = gameStats.boss.BossMaxHealth;

        anim = GetComponent<Animator>();

        audioSource.clip = evilLaugh;
        audioSource.Play();
    }

    void Update()
    {
        audioSource.volume = PlayerPrefs.GetFloat("EffectsVolume");
        if (health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(Die());
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
                StartCoroutine(WarpChangeColor());
                anim.SetTrigger("warp");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(DamageFrames());
    }

    IEnumerator DamageFrames()
    {
        sprite.color = new Color(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1, 1, 1, 1);
    }

    IEnumerator WarpChangeColor()
    {
        sprite.color = new Color(0.69492f, 0.33491f, 1, 1);
        yield return new WaitForSeconds(0.46f);
        sprite.color = new Color(1, 1, 1, 1);
    }

    IEnumerator Die()
    {
        
        anim.SetTrigger("die");
        audioSource.clip = winFanfare;
        audioSource.Play();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 2);
        ui.EndGame(true);
    }

    IEnumerator Shoot()
    {
        Vector3 offset = new Vector3(1.76f, 0.57f, 0);
        if(isInRange && !isDead)
        {
            GameObject projectileInstance = Instantiate(projectile, transform.position + offset, Quaternion.identity);
            projectile.GetComponent<ProjectileController>().playerTransform = playerTransform;
            projectile.GetComponent<ProjectileController>().damage = gameStats.boss.BossProjectileDamage;
            anim.SetTrigger("shoot");
        }

        yield return new WaitForSeconds(shootingInterval);
        StartCoroutine(Shoot());
    }
}

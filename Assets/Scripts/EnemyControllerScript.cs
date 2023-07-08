using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerScript : MonoBehaviour
{
    [SerializeField]
    private int enemyMaxDistance;
    public GameStats gameStats;
    public float damage;
    private float currentHealth;
    private float oldHealth;
    private Transform playerTransform;
    public GameObject experience;
    public GameObject health;
    private float healthProbability = 0.05f;

    private bool isDead = false;

    private SpriteRenderer sprite;
    private Animator animator;

    private Vector3 posLastFrame;
    private Vector2 posThisFrame;

    NavMeshAgent agent;

    void Start()
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        enemyMaxDistance = 25;
        if (gameStats.player.PlayerHealth > 0) { playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); }
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
        GetHealthAndDamage();
    }

    void GetHealthAndDamage()
    {
        switch (gameObject.name)
        {
            case "Zombie(Clone)":
            case "Zombie2(Clone)":
                currentHealth = gameStats.zombie.MaxHealth;
                damage = gameStats.zombie.Damage;
                break;
            case "Bat(Clone)":
                currentHealth = gameStats.bat.MaxHealth;
                damage = gameStats.bat.Damage;
                break;
            case "Skeleton(Clone)":
                currentHealth = gameStats.skeleton.MaxHealth;
                damage = gameStats.skeleton.Damage;
                break;
            case "Crawler(Clone)":
                currentHealth = gameStats.crawler.MaxHealth;
                damage = gameStats.crawler.Damage;
                break;
            case "Wraith(Clone)":
                currentHealth = gameStats.wraith.MaxHealth;
                damage = gameStats.wraith.Damage;
                break;
            case "Red_Death":
                currentHealth = gameStats.wraith.MaxHealth;
                damage = gameStats.wraith.Damage;
                break;
        }
    }

    void Update()
    {
        if (gameStats.player.PlayerHealth > 0)
        {
            if (!isDead)
            {
                if (Vector3.Distance(playerTransform.position, transform.position) > enemyMaxDistance)
                {
                    Destroy(gameObject);
                }
                if (currentHealth <= 0)
                {
                    isDead = true;
                    Die();
                }
            }
        }

    }

    void Die()
    {
        switch (gameObject.name)
        {
            case "Zombie(Clone)":
            case "Zombie2(Clone)":
                gameStats.enemiesKilled.zombies++;
                break;
            case "Bat(Clone)":
                gameStats.enemiesKilled.bats++;
                break;
            case "Skeleton(Clone)":
                gameStats.enemiesKilled.skeletons++;
                break;
            case "Crawler(Clone)":
                gameStats.enemiesKilled.crawlers++;
                break;
            case "Wraith(Clone)":
                gameStats.enemiesKilled.wraiths++;
                break;
            default: break;
        }

        if (Random.value <= healthProbability)
        {
            Instantiate(health, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(experience, transform.position, transform.rotation);
        }
        animator.SetTrigger("die");
        Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    void FixedUpdate()
    {
        if (oldHealth > currentHealth)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
        oldHealth = currentHealth;

        if (gameStats.player.PlayerHealth > 0)
        {
            if (!isDead)
            {
                agent.destination = playerTransform.position;

                posLastFrame = posThisFrame;

                posThisFrame = transform.position;


                if (posThisFrame.x > posLastFrame.x)
                    transform.eulerAngles = new Vector3(0, 0, 0);
                if (posThisFrame.x < posLastFrame.x)
                    transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}

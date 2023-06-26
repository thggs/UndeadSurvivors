using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerScript : MonoBehaviour
{
    [SerializeField]
    private int enemyMaxDistance;
    public GameStats gameStats;
    public int damage;
    public int currentHealth;
    private Transform playerTransform;
    public GameObject experience;

    private SpriteRenderer sprite;
    private Animator animator;

    private Vector3 posLastFrame;
    private Vector2 posThisFrame;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        enemyMaxDistance = 30;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
            case "Zombie (Clone)":
            case "Zombie2 (Clone)":
                currentHealth = gameStats.zombie.MaxHealth;
                damage = gameStats.zombie.Damage;
                break;
            case "Bat (Clone)":
                currentHealth = gameStats.bat.MaxHealth;
                damage = gameStats.bat.Damage;
                break;
            case "Skeleton (Clone)":
                currentHealth = gameStats.skeleton.MaxHealth;
                damage = gameStats.skeleton.Damage;
                break;
            case "Crawler (Clone)":
                currentHealth = gameStats.crawler.MaxHealth;
                damage = gameStats.crawler.Damage;
                break;
            case "Wraith (Clone)":
                currentHealth = gameStats.wraith.MaxHealth;
                damage = gameStats.wraith.Damage;
                break;
        }
    }

    void Update()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) > enemyMaxDistance)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.destination = playerTransform.position;

        posLastFrame = posThisFrame;

        posThisFrame = transform.position;

        animator.SetTrigger("walk");
        if (posThisFrame.x > posLastFrame.x)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (posThisFrame.x < posLastFrame.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
        {
            animator.SetTrigger("stop_walk");
        }

    }
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Weapon")
        {
            Instantiate(experience, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerContollerScript : MonoBehaviour
{
    [SerializeField]
    private GameStats gameStats;

    public float invulnerabilityDuration = 2f; // Duration of invulnerability after taking damage
    private bool isInvulnerable = false; // Flag to track invulnerability state
    private float invulnerabilityTimer = 0f; // Timer for tracking invulnerability duration

    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private Vector3 input;

    public bool hasWhip = false;
    public bool hasBible = false;
    public bool hasHolyWater = false;
    public bool hasThrowingKnife = false;
    public int healthBoost = 25;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        gameStats.player.PlayerHealth = gameStats.player.PlayerMaxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f).normalized;
        // Update the invulnerability timer if the player is currently invulnerable
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer <= 0f)
            {
                //invulnerabilityTimer = invulnerabilityDuration;
                isInvulnerable = false;
            }
        }
        else
        {
            sprite.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        if(gameStats.player.PlayerHealth > gameStats.player.PlayerMaxHealth)
        {
            gameStats.player.PlayerHealth = gameStats.player.PlayerMaxHealth;
        }

        gameObject.transform.GetChild(0).gameObject.SetActive(hasWhip);
        gameObject.transform.GetChild(1).gameObject.SetActive(hasBible);
        gameObject.transform.GetChild(2).gameObject.SetActive(hasHolyWater);
        gameObject.transform.GetChild(3).gameObject.SetActive(hasThrowingKnife);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + input * Time.deltaTime * gameStats.player.PlayerSpeed);

        // Character animations and sprite flipping
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetTrigger("player_walk");
            if (Input.GetAxisRaw("Horizontal") > 0)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else if (Input.GetAxisRaw("Horizontal") < 0)
                transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            animator.SetTrigger("player_idle");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameStats.player.PlayerHealth -= collision.GetComponent<EnemyControllerScript>().damage;

            if (gameStats.player.PlayerHealth <= 0)
            {
                Die();
            }
            else
            {
                StartInvulnerability();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Experience")
        {
            gameStats.player.PlayerXP++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Health")
        {
            // if hp after boost is over limit set hp to limit
            if(gameStats.player.PlayerHealth + healthBoost > gameStats.player.PlayerMaxHealth){
                gameStats.player.PlayerHealth = gameStats.player.PlayerMaxHealth;
            }else{
                gameStats.player.PlayerHealth += healthBoost;
            } 
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "ExitDoor")
        {
            SceneManager.LoadScene("MenuScene",LoadSceneMode.Single);
        }
        if (collision.gameObject.tag == "EnterDoor")
        {
            SceneManager.LoadScene("HouseScene",LoadSceneMode.Single);
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
        Debug.Log("Player is invulnerable for " + invulnerabilityDuration + " seconds.");
        sprite.color = new Vector4(1.0f, 1.0f, 1.0f, 0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContollerScript : MonoBehaviour
{
    
    public int maxHealth = 100; // Maximum health value
    public int currentHealth; // Current health value
    public float invulnerabilityDuration = 2f; // Duration of invulnerability after taking damage
    private bool isInvulnerable = false; // Flag to track invulnerability state
    private float invulnerabilityTimer = 0f; // Timer for tracking invulnerability duration
    
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private Vector3 input;

    public bool hasWhip = false;
    public int xp;
    public int playerLevel;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // Set the initial health to maximum
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
        else{
            sprite.color = new Vector4(1.0f,1.0f,1.0f,1.0f);
        }

        if(hasWhip){
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    void PlayerLevel(){
        if(xp >= playerLevel*10){
            xp = 0;
            Debug.Log("LEVEL UP! \nCurrent Level: " + playerLevel);
            playerLevel++;
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + input * Time.deltaTime * speed);
        
        // Character animations and sprite flipping
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            animator.SetTrigger("player_walk");
            if(Input.GetAxisRaw("Horizontal") > 0)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else if(Input.GetAxisRaw("Horizontal") < 0)
                transform.eulerAngles = new Vector3(0, 180, 0);
        }else{
            animator.SetTrigger("player_idle");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvulnerable)
        {
            int damage = 10; // Amount of damage taken
            TakeDamage(damage);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartInvulnerability();
            }
        }
        if(collision.gameObject.tag == "Experience"){
            xp++;
            PlayerLevel();
            Destroy(collision.gameObject);
        } 
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Health: " + currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        gameObject.SetActive(false);
    }

    private void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
        Debug.Log("Player is invulnerable for " + invulnerabilityDuration + " seconds.");
        sprite.color = new Vector4(1.0f,1.0f,1.0f,0.5f);
    }
}

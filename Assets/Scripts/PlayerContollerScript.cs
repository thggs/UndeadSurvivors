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

    private SpriteRenderer sprite;


    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // Set the initial health to maximum
    }

    void Update()
    {
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0.0f);
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

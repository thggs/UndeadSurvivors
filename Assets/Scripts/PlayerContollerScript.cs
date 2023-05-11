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



    public float speed;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
        // Additional code for handling player death, such as restarting the level or showing a game over screen.
    }

    private void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
        Debug.Log("Player is invulnerable for " + invulnerabilityDuration + " seconds.");
        // Additional code to provide visual/audio feedback to indicate invulnerability.
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpPickupController : MonoBehaviour
{
    private CircleCollider2D col;
    [SerializeField]
    private GameStats gameStats;
    private AudioSource playerAudioSource;

    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        playerAudioSource = transform.parent.GetComponent<AudioSource>();
    }

    void Update()
    {
        col.radius = gameStats.player.PlayerPickupRadius;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        float distanceToPlayer = Vector3.Distance(other.transform.position, transform.position);
        if (other.tag == "Experience")
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 moveDirection = (transform.position - other.gameObject.transform.position).normalized;

            rb.AddForce(moveDirection * 10, ForceMode2D.Impulse);

            if (distanceToPlayer < 0.5f)
            {
                gameStats.player.PlayerXP++;
                Destroy(other.gameObject);
                playerAudioSource.Play();
            }
        }
        if (other.tag == "Health" && distanceToPlayer < 0.5f)
        {
            if (gameStats.player.PlayerHealth + gameStats.healingStones.HealAmount > gameStats.player.PlayerMaxHealth)
            {
                gameStats.player.PlayerHealth = gameStats.player.PlayerMaxHealth;
            }
            else
            {
                gameStats.player.PlayerHealth += gameStats.healingStones.HealAmount;
            }
            Destroy(other.gameObject);
            playerAudioSource.Play();
        }
    }
}

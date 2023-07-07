using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpPickupController : MonoBehaviour
{
    private CircleCollider2D col;
    [SerializeField]
    private GameStats gameStats;

    void Start() {
        col = GetComponent<CircleCollider2D>();    
    }

    void Update()
    {
        col.radius = gameStats.player.PlayerPickupRadius;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Experience")
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 moveDirection = (transform.position - other.gameObject.transform.position).normalized;

            rb.AddForce(moveDirection * 10, ForceMode2D.Impulse);
        }
    }
}

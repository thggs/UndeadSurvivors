using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatControllerScript : MonoBehaviour
{

    const float ForcePower = 10f;
    Transform playerTransform;  
    Rigidbody2D rb2D;
    public float speed;
    public float force;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    
    void FixedUpdate() {
        direction = (playerTransform.position - transform.position).normalized;
        var desiredVelocity = direction * speed;
        var deltaVelocity = desiredVelocity - rb2D.velocity;
        Vector3 moveForce = deltaVelocity * (force * ForcePower * Time.fixedDeltaTime);
        rb2D.AddForce(moveForce);
    }
}

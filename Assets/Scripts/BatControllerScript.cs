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
    public GameObject experience;

    private Vector2 direction;
    private Animator animator;
       
    private SpriteRenderer sprite;
    Vector2 posLastFrame;
    Vector2 posThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        
    }
    
    void FixedUpdate() {
        direction = (playerTransform.position - transform.position).normalized;
        var desiredVelocity = direction * speed;
        var deltaVelocity = desiredVelocity - rb2D.velocity;
        Vector3 moveForce = deltaVelocity * (force * ForcePower * Time.fixedDeltaTime);
        rb2D.AddForce(moveForce);

        posLastFrame = posThisFrame;
 
        posThisFrame = transform.position;

        animator.SetTrigger("walk");
        if (posThisFrame.x > posLastFrame.x)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (posThisFrame.x < posLastFrame.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        
            
    }
    private void OnTriggerEnter2D(Collider2D trigger){
        if(trigger.gameObject.tag == "Player"){
            //enemy died :(
            // spawn xp ball
            GameObject ricardo = Instantiate(experience, transform.position, transform.rotation);
            Destroy(this.gameObject);
        } 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieControllerScript : MonoBehaviour
{
   public int maxHealth = 5;
   public int currentHealth;
   Transform playerTransform;  
   public float speed;
   public GameObject experience;

   private SpriteRenderer sprite;
   private Animator animator;

    Vector2 posLastFrame;
    Vector2 posThisFrame;

   NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.destination = playerTransform.position;
              
        /*transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed*Time.deltaTime);
        Vector3 direction = transform.position - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/
        posLastFrame = posThisFrame;
 
        posThisFrame = transform.position;

        animator.SetTrigger("walk");
        if (posThisFrame.x > posLastFrame.x)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (posThisFrame.x < posLastFrame.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else{
            animator.SetTrigger("stop_walk");
        }
       
    }
    private void OnTriggerEnter2D(Collider2D trigger){
        if(trigger.gameObject.tag == "Player"){
            //enemy died :(
            // spawn xp ball
            Instantiate(experience, transform.position, transform.rotation);
            Destroy(this.gameObject);
        } 
    }
}

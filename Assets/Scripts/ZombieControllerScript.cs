using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieControllerScript : MonoBehaviour
{
   Transform playerTransform;  
   public float speed;

   NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.destination = playerTransform.position;
        /*transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed*Time.deltaTime);
        Vector3 direction = transform.position - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/
       
    }
}

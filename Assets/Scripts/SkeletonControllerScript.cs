using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonControllerScript : MonoBehaviour
{
   Transform playerTransform;  
   public float followSpeed;
   public float closeInSpeed;
   public float longDistance;
   public float shortDistance;

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
        Vector3 toPlayer = playerTransform.position - transform.position;
        if(Vector3.Distance(transform.position, playerTransform.position) > longDistance)
        {
            agent.destination = playerTransform.position;
            Debug.Log("Closing Distance...");
            
        }
        else if(Vector3.Distance(transform.position, playerTransform.position) < shortDistance)
        {
            Vector3 destination = toPlayer.normalized * -shortDistance;
            agent.destination = destination;
            Debug.Log("Stepping away to " + destination);
        }
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/
       
    }
}

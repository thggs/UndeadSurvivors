using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonControllerScript : MonoBehaviour
{
   Transform playerTransform;  
   public float speed;
   public float shootingDistance;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) > shootingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed*Time.deltaTime);
            Vector3 direction = transform.position - playerTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
       
    }
}

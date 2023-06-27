using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneControllerScript : MonoBehaviour
{
    public Transform player;
    float offset = 0;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationSpeed = 500f;
        float rotationAngle = Time.deltaTime * rotationSpeed;
        position = new Vector3(player.position.x + offset, player.position.y + offset, transform.position.z);
        transform.RotateAround(position, Vector3.forward, rotationAngle);
    }
}
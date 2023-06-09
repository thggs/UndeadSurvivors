using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipControllerScript : MonoBehaviour
{

    private float whipTimer;

    [SerializeField]
    private GameObject whipSlash;
    private Transform player;
    private Vector3 initialSpawnPosition;
    public float whipCooldown = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        initialSpawnPosition = new Vector3(player.position.x + 3.5f, player.position.y,0.0f);
        whipTimer -= Time.deltaTime;
        if(whipTimer <= 0f){
            Instantiate(whipSlash, initialSpawnPosition, player.rotation);
            whipTimer = whipCooldown;
        }
    }
}

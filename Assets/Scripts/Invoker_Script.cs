using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invoker_Script : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

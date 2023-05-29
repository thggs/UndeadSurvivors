using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManagerScript : MonoBehaviour
{
    public GameObject[] enemyList;
    public new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        float randomX = Random.Range(1f, 1.5f);
        float randomY = Random.Range(1f, 1.5f);
        if(Random.Range(0f,1f) > 0.5)
        {
            randomX = -(randomX - 1);
        }
        if(Random.Range(0f,1f) > 0.5)
        {
            randomY = -(randomY - 1);
        }
        Vector3 spawnPosition = camera.ViewportToWorldPoint(new Vector3(randomX, randomY, camera.nearClipPlane));
        NavMeshHit hit;
        NavMesh.SamplePosition(spawnPosition, out hit,Mathf.Infinity, NavMesh.AllAreas);
        Instantiate(enemyList[0],hit.position,Quaternion.identity);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
Tecnologia de Jogos Digitais

David Ferreira      57693
Filipe Pereira      57838
Joao Maron          58268
Joao Goulao         52»»»»

May 2023
*/

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    private bool spawnEnemies = false;
    public GameObject[] enemyList;
    public new Camera camera;
    public float timeBetweenWaves;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWave(timeBetweenWaves));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnWave(float timeBetweenWaves)
    {
        if(spawnEnemies){
            // Pick random value outside camera view
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
            // Convert camera coordinates to world coordinates
            Vector3 spawnPosition = camera.ViewportToWorldPoint(new Vector3(randomX, randomY, camera.nearClipPlane));
            // Get closest coordinates in NavMesh
            NavMeshHit hit;
            NavMesh.SamplePosition(spawnPosition, out hit,Mathf.Infinity, NavMesh.AllAreas);
            // Spawn random enemy
            int enemy = Random.Range(0,3);
            Instantiate(enemyList[enemy],hit.position,Quaternion.identity);
        }
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(timeBetweenWaves));
    }
}

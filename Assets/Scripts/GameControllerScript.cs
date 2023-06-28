using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameControllerScript : MonoBehaviour
{
    public GameStats gameStats;
    public GameObject[] enemyList;
    public new Camera camera;
    public bool spawnEnemies;
    public float timeBetweenSpawns;
    private WaitForSeconds timeBetweenSpawnsWFS;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenSpawnsWFS = new WaitForSeconds(timeBetweenSpawns);
        StartCoroutine(Spawn());
    }

    void Update()
    {
        
        if (gameStats.player.PlayerXP >= gameStats.player.PlayerLevel * 10)
        {
            gameStats.player.PlayerXP = 0;
            gameStats.player.PlayerLevel++;
        }
    }

    private IEnumerator Spawn()
    {
        Debug.Log(gameStats.player.PlayerHealth);
        if (spawnEnemies)
        {
            float randomX = Random.Range(-0.1f, 0.1f);
            float randomY = Random.Range(-0.1f, 0.1f);
            if (randomX >= 0)
            {
                randomX += 1;
            }
            if (randomY >= 0)
            {
                randomY += 1;
            }
            
            Vector3 spawnPosition = camera.ViewportToWorldPoint(new Vector3(randomX, randomY, camera.nearClipPlane));
            NavMeshHit hit;
            NavMesh.SamplePosition(spawnPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);

            int index = Random.Range(0, enemyList.Length);
            GameObject enemy = Instantiate(enemyList[index], hit.position, Quaternion.identity);
            enemy.GetComponent<EnemyControllerScript>().gameStats = gameStats;            
        }
        yield return timeBetweenSpawnsWFS;
        StartCoroutine(Spawn());
    }
}
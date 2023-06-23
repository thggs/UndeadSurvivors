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

    void Awake()
    {
        gameStats.player.PlayerMaxHealth = 1000;
        gameStats.player.PlayerXP = 0;
        gameStats.player.PlayerLevel = 1;
        gameStats.zombie.MaxHealth = 5;
        gameStats.zombie.Damage = 1;
    }

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
            float randomX = Random.Range(1f, 1.1f);
            float randomY = Random.Range(1f, 1.1f);
            if (Random.Range(0f, 1f) > 0.5)
            {
                randomX = -(randomX - 1);
            }
            if (Random.Range(0f, 1f) > 0.5)
            {
                randomY = -(randomY - 1);
            }
            Vector3 spawnPosition = camera.ViewportToWorldPoint(new Vector3(randomX, randomY, camera.nearClipPlane));
            NavMeshHit hit;
            NavMesh.SamplePosition(spawnPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
            GameObject enemy = Instantiate(enemyList[Random.Range(0,enemyList.Length)], hit.position, Quaternion.identity);
            enemy.GetComponent<ZombieControllerScript>().gameStats = gameStats;
        }
        yield return timeBetweenSpawnsWFS;
        StartCoroutine(Spawn());
    }
}
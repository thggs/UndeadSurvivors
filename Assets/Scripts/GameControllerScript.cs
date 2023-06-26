using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField]
    private int playerMaxHealth, playerXP, playerLevel, zombieMaxHealth, zombieDamage, batMaxHealth, batDamage;

    public GameStats gameStats;
    public GameObject[] enemyList;
    public new Camera camera;
    public bool spawnEnemies;
    public float timeBetweenSpawns;
    private WaitForSeconds timeBetweenSpawnsWFS;

    void Awake()
    {
        gameStats.player.PlayerMaxHealth = playerMaxHealth;
        gameStats.player.PlayerXP = playerXP;
        gameStats.player.PlayerLevel = playerLevel;

        gameStats.zombie.MaxHealth = zombieMaxHealth;
        gameStats.zombie.Damage = zombieDamage;
        
        gameStats.bat.MaxHealth = batMaxHealth;
        gameStats.bat.Damage = batDamage;
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
            switch(index){
                case 0:
                case 1:
                    enemy.GetComponent<EnemyControllerScript>().damage = gameStats.zombie.Damage;
                    enemy.GetComponent<EnemyControllerScript>().currentHealth = gameStats.zombie.MaxHealth;
                    break;
                case 2:
                    enemy.GetComponent<EnemyControllerScript>().damage = gameStats.bat.Damage;
                    enemy.GetComponent<EnemyControllerScript>().currentHealth = gameStats.bat.MaxHealth;
                    break;
                    
            }
            
        }
        yield return timeBetweenSpawnsWFS;
        StartCoroutine(Spawn());
    }
}
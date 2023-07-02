using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider, xpSlider;
    [SerializeField]
    private GameObject upgradePanel;
    public GameStats gameStats;
    public WaveStats waveStats;
    public GameObject[] enemyList;
    public new Camera camera;
    public bool spawnEnemies;
    public float timeBetweenSpawns;
    private WaitForSeconds timeBetweenSpawnsWFS;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    void Update()
    {

        ManageHealth();
        ManageXP();
    }

    void ManageHealth()
    {
        if (gameStats.player.PlayerHealth != gameStats.player.PlayerMaxHealth)
        {
            healthSlider.gameObject.SetActive(true);
        }
        else
        {
            healthSlider.gameObject.SetActive(false);
        }

        healthSlider.maxValue = gameStats.player.PlayerMaxHealth;
        healthSlider.value = gameStats.player.PlayerHealth;
    }

    void ManageXP()
    {
        xpSlider.maxValue = gameStats.player.PlayerLevel * 10;
        xpSlider.value = gameStats.player.PlayerXP;

        if(gameStats.player.PlayerXP >= gameStats.player.PlayerLevel * 10)
        {
            gameStats.player.PlayerLevel++;
            gameStats.player.PlayerXP = 0;
            upgradePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private IEnumerator Spawn()
    {
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

            int index = Random.Range(0, waveStats.wave1.Length);
            GameObject enemy = Instantiate(waveStats.wave1[index], hit.position, Quaternion.identity);
            enemy.GetComponent<EnemyControllerScript>().gameStats = gameStats;
        }
        yield return new WaitForSeconds(timeBetweenSpawns);
        StartCoroutine(Spawn());
    }
}
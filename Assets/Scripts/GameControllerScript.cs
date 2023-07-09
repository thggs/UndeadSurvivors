using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameControllerScript : MonoBehaviour
{
    private float sceneTime;
    [SerializeField]
    private Slider healthSlider, xpSlider;
    [SerializeField]
    private GameStats gameStats;
    [SerializeField]
    private WaveStats waveStats;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private bool spawnEnemies;
    [SerializeField]
    private float timeBetweenSpawns;
    [SerializeField]
    private GameObject boss;
    private bool bossSpawned;

    void Awake()
    {
        gameStats.whip.WhipLevel = 1;
        gameStats.whip.WhipCooldown = 1;
        gameStats.whip.WhipDamage = 5;
        gameStats.whip.WhipProjectiles = 1;
        gameStats.whip.WhipDelay = 0.1f;

        gameStats.bible.BibleLevel = 0;
        gameStats.bible.BibleProjectiles = 0;
        gameStats.bible.BibleDamage = 5;
        gameStats.bible.BibleCooldown = 5;
        gameStats.bible.BibleLifetime = 3;

        gameStats.holyWater.WaterLevel = 0;
        gameStats.holyWater.WaterProjectiles = 0;
        gameStats.holyWater.WaterDamage = 0.1f;
        gameStats.holyWater.WaterCooldown = 8;
        gameStats.holyWater.WaterLifetime = 4;

        gameStats.throwingKnife.KnifeLevel = 0;
        gameStats.throwingKnife.KnifeDamage = 5;
        gameStats.throwingKnife.KnifeCooldown = 4;
        gameStats.throwingKnife.KnifeDelay = 0.1f;
        gameStats.throwingKnife.KnifeLifetime = 2;
        gameStats.throwingKnife.KnifeDurability = 2;
        gameStats.throwingKnife.KnifeProjectiles = 0;

        gameStats.healingStones.HealAmount = 50;
        gameStats.healingStones.HealLevel = 1;

        gameStats.player.PlayerLevel = 1;
        gameStats.player.PlayerSpeedLevel = 1;
        gameStats.player.PlayerMaxHealthLevel = 1;
        gameStats.player.PlayerPickupLevel = 1;
        gameStats.player.PlayerHealth = 1000;
        gameStats.player.PlayerMaxHealth = 1000;
        gameStats.player.PlayerXP = 0;
        gameStats.player.PlayerSpeed = 5;
        gameStats.player.PlayerPickupRadius = 0.5f;

        gameStats.enemiesKilled.zombies = 0;
        gameStats.enemiesKilled.bats = 0;
        gameStats.enemiesKilled.skeletons = 0;
        gameStats.enemiesKilled.crawlers = 0;
        gameStats.enemiesKilled.wraiths = 0;
        gameStats.enemiesKilled.flyingEyes = 0;

        gameStats.zombie.Damage = 1;
        gameStats.zombie.MaxHealth = 5;
        gameStats.zombie.Speed = 1;

        gameStats.bat.Damage = 1;
        gameStats.bat.MaxHealth = 5;
        gameStats.bat.Speed = 2.25f;

        gameStats.skeleton.Damage = 2;
        gameStats.skeleton.MaxHealth = 10;
        gameStats.skeleton.Speed = 1.5f;

        gameStats.crawler.Damage = 1;
        gameStats.crawler.MaxHealth = 5;
        gameStats.crawler.Speed = 3;

        gameStats.wraith.Damage = 3;
        gameStats.wraith.MaxHealth = 10;
        gameStats.wraith.Speed = 2;

        gameStats.flyingEye.Damage = 2;
        gameStats.flyingEye.MaxHealth = 15;
        gameStats.flyingEye.Speed = 2;

        gameStats.boss.BossMaxHealth = 500;
        gameStats.boss.BossDamage = 10;
        gameStats.boss.BossProjectileDamage = 50;
        gameStats.boss.BossSpeed = 3;
        gameStats.boss.BossSlowSpeed = 0;
        gameStats.boss.BossMinDistance = 5;

        // WAVE STATS
        waveStats.wave1Time = 120.0f;
        waveStats.wave2Time = 240.0f;
        waveStats.wave3Time = 240.0f;
        waveStats.wave4Time = 300.0f;

        bossSpawned = false; 
    }

    // Start is called before the first frame update
    void Start()
    {
        //Dictionaries();
        //upgradeButton1.onClick.RemoveAllListeners();

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
    }

    private GameObject[] SelectWave()
    {
        float currentTime = Time.timeSinceLevelLoad;
        if (currentTime <= waveStats.wave1Time)
        {
            return waveStats.wave1;
        }
        else if (currentTime <= (waveStats.wave1Time + waveStats.wave2Time))
        {
            return waveStats.wave2;
        }
        else if (currentTime <= (waveStats.wave1Time + waveStats.wave2Time + waveStats.wave3Time))
        {
            return waveStats.wave3;
        }
        else if (currentTime <= (waveStats.wave1Time + waveStats.wave2Time + waveStats.wave3Time + waveStats.wave4Time))
        {
            return waveStats.wave4;
        }
        else 
        {
            SpawnBoss();
            return waveStats.wave5;
        }
    }

    private IEnumerator Spawn()
    {
        if (spawnEnemies)
        {
            GameObject[] wave = SelectWave();

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

            Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(randomX, randomY, mainCamera.nearClipPlane));
            NavMeshHit hit;
            NavMesh.SamplePosition(spawnPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
            // select one random enemy and instantiate it from list of wave
            int index = Random.Range(0, wave.Length);
            Instantiate(wave[index], hit.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(timeBetweenSpawns);
        StartCoroutine(Spawn());
    }

    void SpawnBoss()
    {
        if(!bossSpawned){
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
            Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(randomX, randomY, mainCamera.nearClipPlane));
            NavMeshHit hit;
            NavMesh.SamplePosition(spawnPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
            GameObject bossInstance = Instantiate(boss, hit.position, Quaternion.identity);
            bossSpawned = true;
        }  
    }
}
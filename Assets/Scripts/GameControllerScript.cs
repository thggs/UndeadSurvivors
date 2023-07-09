using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameControllerScript : MonoBehaviour
{
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
        gameStats.throwingKnife.KnifeCooldown = 6;
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

        //health.SetSize(gameStats.player.PlayerHealth/gameStats.player.PlayerMaxHealth);
    }

    void ManageXP()
    {
        xpSlider.maxValue = gameStats.player.PlayerLevel * 10;
        xpSlider.value = gameStats.player.PlayerXP;

        //int maxValue = gameStats.player.PlayerLevel * 10;
        //xp.SetSize(gameStats.player.PlayerXP/maxValue);

        /*if (gameStats.player.PlayerXP >= gameStats.player.PlayerLevel * 10)
        {
            gameStats.player.PlayerLevel++;
            gameStats.player.PlayerXP = 0;

            // Activate the level up panel and pause the game
            upgradePanel.SetActive(true);
            Time.timeScale = 0;

            // Delete previous functions associated with the buttons
            upgradeButton1.onClick.RemoveAllListeners();
            upgradeButton2.onClick.RemoveAllListeners();
            upgradeButton3.onClick.RemoveAllListeners();

            // List of posible upgrades
            List<int> upgrades = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };

            // Remove from list if max level
            if (gameStats.player.PlayerMaxHealthLevel == 7)
                upgrades.Remove(0);
            if (gameStats.healingStones.HealLevel == 7)
                upgrades.Remove(1);
            if (gameStats.player.PlayerSpeedLevel == 7)
                upgrades.Remove(2);
            if (gameStats.whip.WhipLevel == 9)
                upgrades.Remove(4);
            if (gameStats.bible.BibleLevel == 9)
                upgrades.Remove(5);
            if (gameStats.holyWater.WaterLevel == 9)
                upgrades.Remove(6);
            if (gameStats.throwingKnife.KnifeLevel == 9)
                upgrades.Remove(7);

            // Choose three different upgrades
            int[] selectedInts = upgrades.OrderBy(x => Random.value).Take(3).ToArray();

            // need to add option to only put 2 or 1 buttons when upgrades.size() < 3 !!!

            // Add corresponding text to buttons
            string option1 = dictionariesList[selectedInts[0]][selectLevel(selectedInts[0])];
            string option2 = dictionariesList[selectedInts[1]][selectLevel(selectedInts[1])];
            string option3 = dictionariesList[selectedInts[2]][selectLevel(selectedInts[2])];
            upgradeButton1.gameObject.GetComponentInChildren<TMP_Text>().text = option1;
            upgradeButton2.gameObject.GetComponentInChildren<TMP_Text>().text = option2;
            upgradeButton3.gameObject.GetComponentInChildren<TMP_Text>().text = option3;

            // Add corresponding images to button
            upgradeButton1.transform.GetChild(1).GetComponent<Image>().sprite = SelectImage(selectedInts[0]);
            upgradeButton2.transform.GetChild(1).GetComponent<Image>().sprite = SelectImage(selectedInts[1]);
            upgradeButton3.transform.GetChild(1).GetComponent<Image>().sprite = SelectImage(selectedInts[2]);

            // Apply corresponding functions
            upgradeButton1.onClick.AddListener(() => LevelUp(selectedInts[0]));
            upgradeButton2.onClick.AddListener(() => LevelUp(selectedInts[1]));
            upgradeButton3.onClick.AddListener(() => LevelUp(selectedInts[2]));
        }*/

    }

    private GameObject[] SelectWave()
    {
        float currentTime = Time.time;

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
            // select one enemy and instantiate it from list of wave
            int index = Random.Range(0, wave.Length);
            Instantiate(wave[index], hit.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(timeBetweenSpawns);
        StartCoroutine(Spawn());
    }

    void SpawnBoss()
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
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(randomX, randomY, mainCamera.nearClipPlane));
        NavMeshHit hit;
        NavMesh.SamplePosition(spawnPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
        GameObject bossInstance = Instantiate(boss, hit.position, Quaternion.identity);
    }

    /*public int selectLevel(int option)
    {
        switch (option)
        {
            case 0: return (gameStats.player.PlayerMaxHealthLevel + 1);
            case 1: return (gameStats.healingStones.HealLevel + 1);
            case 2: return (gameStats.player.PlayerSpeedLevel + 1);
            case 3: return (gameStats.player.PlayerPickupLevel + 1); 
            case 4: return (gameStats.whip.WhipLevel + 1);
            case 5: return (gameStats.bible.BibleLevel + 1);
            case 6: return (gameStats.holyWater.WaterLevel + 1);
            case 7: return (gameStats.throwingKnife.KnifeLevel + 1);
            default: return 0;
        }
    }*/

    /*public void LevelUp(int option)
    {
        switch (option)
        {
            // Max Health
            case 0:
                gameStats.player.PlayerMaxHealth += 100;
                gameStats.player.PlayerMaxHealthLevel += 1;
                gameStats.player.PlayerHealth += 100;
                Debug.Log("Level Up Max Health");
                break;

            // Healing Stones
            case 1:
                gameStats.healingStones.HealLevel += 1;
                gameStats.healingStones.HealAmount += 25;
                Debug.Log("Level Up Healing Stones");
                break;
                
            // Player Speed
            case 2:
                gameStats.player.PlayerSpeedLevel += 1;
                gameStats.player.PlayerSpeed += 0.5f;
                Debug.Log("Level Up Player Speed");
                break;

            // XP Radius
            case 3:
                gameStats.player.PlayerPickupLevel += 1;
                gameStats.player.PlayerPickupRadius += 0.5f;
                break;

            // Whip
            case 4:
                gameStats.whip.WhipLevel += 1;
                switch (gameStats.whip.WhipLevel)
                {
                    case 2: gameStats.whip.WhipDamage = 6.25f; break;
                    case 3: gameStats.whip.WhipCooldown = 0.8f; break;
                    case 4: gameStats.whip.WhipDamage = 8.0f; gameStats.whip.WhipProjectiles = 2; break;
                    case 5: gameStats.whip.WhipCooldown = 0.7f; break;
                    case 6: gameStats.whip.WhipDamage = 10.0f; break;
                    case 7: gameStats.whip.WhipCooldown = 0.6f; break;
                    case 8: gameStats.whip.WhipDamage = 12.5f; break;
                    case 9: gameStats.whip.WhipDamage = 15.0f; gameStats.whip.WhipCooldown = 0.6f; break;
                }
                Debug.Log("Level Up Whip");
                break;

            // Bible
            case 5:
                gameStats.bible.BibleLevel += 1;
                switch (gameStats.bible.BibleLevel)
                {
                    case 1: gameStats.bible.BibleProjectiles = 1; break;
                    case 2: gameStats.bible.BibleDamage = 6.25f; break;
                    case 3: gameStats.bible.BibleProjectiles = 2; gameStats.bible.BibleLifetime = 4.0f; break;
                    case 4: gameStats.bible.BibleDamage = 8.0f; break;
                    case 5: gameStats.bible.BibleProjectiles = 3; gameStats.bible.BibleCooldown = 4.0f; break;
                    case 6: gameStats.bible.BibleDamage = 10.0f; break;
                    case 7: gameStats.bible.BibleProjectiles = 4; gameStats.bible.BibleLifetime = 5.0f; break;
                    case 8: gameStats.bible.BibleDamage = 12.5f; break;
                    case 9: gameStats.bible.BibleDamage = 16.0f; break;
                }
                Debug.Log("Level Up Bible");
                break;

            // Holy Water
            case 6:
                gameStats.holyWater.WaterLevel += 1;
                switch (gameStats.holyWater.WaterLevel)
                {
                    case 1: gameStats.holyWater.WaterProjectiles = 1; break;
                    case 2: gameStats.holyWater.WaterProjectiles = 2; gameStats.holyWater.WaterDamage = 0.125f; break;
                    case 3: gameStats.holyWater.WaterDamage = 0.15f; gameStats.holyWater.WaterLifetime = 5; break;
                    case 4: gameStats.holyWater.WaterProjectiles = 3; gameStats.holyWater.WaterCooldown = 7; break;
                    case 5: gameStats.holyWater.WaterDamage = 0.2f; break;
                    case 6: gameStats.holyWater.WaterProjectiles = 4; gameStats.holyWater.WaterLifetime = 6; break;
                    case 7: gameStats.holyWater.WaterProjectiles = 5; gameStats.holyWater.WaterCooldown = 6; break;
                    case 8: gameStats.holyWater.WaterProjectiles = 6; gameStats.holyWater.WaterDamage = 0.25f; break;
                    case 9: gameStats.holyWater.WaterProjectiles = 6; gameStats.holyWater.WaterDamage = 0.3f; break;
                }
                Debug.Log("Level Up Holy Water");
                break;

            // Knife
            case 7:
                gameStats.throwingKnife.KnifeLevel += 1;
                switch (gameStats.throwingKnife.KnifeLevel)
                {
                    case 1: gameStats.throwingKnife.KnifeProjectiles = 1; break;
                    case 2: gameStats.throwingKnife.KnifeProjectiles = 2; gameStats.throwingKnife.KnifeDamage = 6.25f; break;
                    case 3: gameStats.throwingKnife.KnifeCooldown = 5; gameStats.throwingKnife.KnifeDurability = 2; break;
                    case 4: gameStats.throwingKnife.KnifeProjectiles = 3; gameStats.throwingKnife.KnifeDamage = 8; break;
                    case 5: gameStats.throwingKnife.KnifeProjectiles = 4; gameStats.throwingKnife.KnifeCooldown = 4; break;
                    case 6: gameStats.throwingKnife.KnifeDamage = 10.0f; break;
                    case 7: gameStats.throwingKnife.KnifeProjectiles = 5; gameStats.throwingKnife.KnifeDamage = 12.5f; break;
                    case 8: gameStats.throwingKnife.KnifeCooldown = 3; gameStats.throwingKnife.KnifeDurability = 3; break;
                    case 9: gameStats.throwingKnife.KnifeProjectiles = 6; gameStats.throwingKnife.KnifeDamage = 16.0f; break;
                }
                Debug.Log("Level Up Knife");
                break;
        }

        // Remove level up panel and resume game
        upgradePanel.SetActive(false);
        Time.timeScale = 1;
    }*/

    /*public Sprite SelectImage(int option)
    {
        switch (option)
        {
            case 0: Sprite maxHealthImage = Resources.Load<Sprite>("Images/maxHealth"); return maxHealthImage;
            case 1: Sprite healthImage = Resources.Load<Sprite>("Images/health"); return healthImage;
            case 2: Sprite speedImage = Resources.Load<Sprite>("Images/speed"); return speedImage;
            case 3: Sprite xpRadiusImage = Resources.Load<Sprite>("Images/xpRadius"); return xpRadiusImage;
            case 4: Sprite whipSprite = Resources.Load<Sprite>("Images/whip"); return whipSprite;
            case 5: Sprite bibleSprite = Resources.Load<Sprite>("Images/bible"); return bibleSprite;
            case 6: Sprite holyWaterSprite = Resources.Load<Sprite>("Images/holyWater"); return holyWaterSprite;
            case 7: Sprite knifeSprite = Resources.Load<Sprite>("Images/knife"); return knifeSprite;
            default: Sprite maxHealthImagea = Resources.Load<Sprite>("Images/maxHealth"); return maxHealthImagea;
        }
    }*/

    /*public void Dictionaries()
    {
        // Create dictionaries
        Dictionary<int, string> maxHealthDictionary = new Dictionary<int, string>();
        Dictionary<int, string> healthPickupsDictionary = new Dictionary<int, string>();
        Dictionary<int, string> speedDictionary = new Dictionary<int, string>();
        Dictionary<int, string> xpRadiusDictionary = new Dictionary<int, string>();
        Dictionary<int, string> whipDictionary = new Dictionary<int, string>();
        Dictionary<int, string> bibleDictionary = new Dictionary<int, string>();
        Dictionary<int, string> holyWaterDictionary = new Dictionary<int, string>();
        Dictionary<int, string> knifeDictionary = new Dictionary<int, string>();

        // Assign Values - Player
        // Max Health -> 0
        maxHealthDictionary.Add(2, "<b>Level 2:</b> Increase Max Health by 10%");
        maxHealthDictionary.Add(3, "<b>Level 3:</b> Increase Max Health by 10%");
        maxHealthDictionary.Add(4, "<b>Level 4:</b> Increase Max Health by 10%");
        maxHealthDictionary.Add(5, "<b>Level 5:</b> Increase Max Health by 10%");
        maxHealthDictionary.Add(6, "<b>Level 6:</b> Increase Max Health by 10%");
        maxHealthDictionary.Add(7, "<b>Level 7:</b> Increase Max Health by 10%");

        // Health Pickups -> 1
        healthPickupsDictionary.Add(2, "<b>Level 2:</b> Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(3, "<b>Level 3:</b> Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(4, "<b>Level 4:</b> Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(5, "<b>Level 5:</b> Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(6, "<b>Level 6:</b> Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(7, "<b>Level 7:</b> Increase health restored by Pickups 50%");

        // Player Speed -> 2
        speedDictionary.Add(2, "<b>Level 2:</b> Increase Player Speed 10%");
        speedDictionary.Add(3, "<b>Level 3:</b> Increase Player Speed 10%");
        speedDictionary.Add(4, "<b>Level 4:</b> Increase Player Speed 10%");
        speedDictionary.Add(5, "<b>Level 5:</b> Increase Player Speed 10%");
        speedDictionary.Add(6, "<b>Level 6:</b> Increase Player Speed 10%");
        speedDictionary.Add(7, "<b>Level 7:</b> Increase Player Speed 10%");

        // XP Radius -> 3
        xpRadiusDictionary.Add(2, "<b>Level 2:</b> Increase XP Radius to 1");
        xpRadiusDictionary.Add(3, "<b>Level 3:</b> Increase XP Radius to 1.5");
        xpRadiusDictionary.Add(4, "<b>Level 4:</b> Increase XP Radius to 2");
        xpRadiusDictionary.Add(5, "<b>Level 5:</b> Increase XP Radius to 2.5");
        xpRadiusDictionary.Add(6, "<b>Level 6:</b> Increase XP Radius to 3");
        xpRadiusDictionary.Add(7, "<b>Level 7:</b> Increase XP Radius to 3.5");

        // Assign Values - Weapons
        // Whip -> 4
        whipDictionary.Add(2, "<b>Level 2:</b> Increase Whip Damage by 25%");
        whipDictionary.Add(3, "<b>Level 3:</b> Reduce Whip Cooldown by 20%");
        whipDictionary.Add(4, "<b>Level 4:</b> Increase Whip Projectiles to 2 and Damage by 25%");
        whipDictionary.Add(5, "<b>Level 5:</b> Reduce Whip Cooldown by 10%");
        whipDictionary.Add(6, "<b>Level 6:</b> Increase Whip Damage by 25%");
        whipDictionary.Add(7, "<b>Level 7:</b> Reduce Whip Cooldown by 10%");
        whipDictionary.Add(8, "<b>Level 8:</b> Increase Whip Damage by 25%");
        whipDictionary.Add(9, "<b>Level 9:</b> Increase Whip Damage by 25% and Reduce Whip Cooldown by 10%");

        // Bible -> 5
        bibleDictionary.Add(1, "<b>Level 1:</b> Unlock Bible Weapon");
        bibleDictionary.Add(2, "<b>Level 2:</b> Increase Bible Damage by 25%");
        bibleDictionary.Add(3, "<b>Level 3:</b> Increase Amount of Bibles to 2 and Lifetime by 1s");
        bibleDictionary.Add(4, "<b>Level 4:</b> Increase Bible Damage by 25%");
        bibleDictionary.Add(5, "<b>Level 5:</b> Increase Amount of Bibles to 3 and Reduce Cooldown by 1s");
        bibleDictionary.Add(6, "<b>Level 6:</b> Increase Bible Damage by 25%");
        bibleDictionary.Add(7, "<b>Level 7:</b> Increase Amount of Bibles to 4 and Lifetime by 1s");
        bibleDictionary.Add(8, "<b>Level 8:</b> Increase Bible Damage by 25%");
        bibleDictionary.Add(9, "<b>Level 9:</b> Increase Bible Damage by 25%");

        // Holy Water -> 6
        holyWaterDictionary.Add(1, "<b>Level 1:</b> Unlock Holy Water Weapon");
        holyWaterDictionary.Add(2, "<b>Level 2:</b> Increase Amount of Holy Waters to 2 and Damage by 25%");
        holyWaterDictionary.Add(3, "<b>Level 3:</b> Increase Holy Water Damage by 25% and Lifetime by 1s");
        holyWaterDictionary.Add(4, "<b>Level 4:</b> Increase Amount of Holy Waters to 3 and Reduce Cooldown by 1s");
        holyWaterDictionary.Add(5, "<b>Level 5:</b> Increase Holy Water Damage by 25%");
        holyWaterDictionary.Add(6, "<b>Level 6:</b> Increase Amount of Holy Waters to 4 and Lifetime by 1s");
        holyWaterDictionary.Add(7, "<b>Level 7:</b> Increase Amount of Holy Waters to 5 and Reduce Cooldown by 1s");
        holyWaterDictionary.Add(8, "<b>Level 8:</b> Increase Amount of Holy Waters to 6 and Damage by 25%");
        holyWaterDictionary.Add(9, "<b>Level 9:</b> Increase Amount of Holy Waters to 7 and Damage by 25%");

        // Knife -> 7
        knifeDictionary.Add(1, "<b>Level 1:</b> Unlock Throwing Knifes Weapon");
        knifeDictionary.Add(2, "<b>Level 2:</b> Increase Amount of Knifes to 2 and Damage by 25%");
        knifeDictionary.Add(3, "<b>Level 3:</b> Reduce Knifes Cooldown by 1s and Increase Durability to 2");
        knifeDictionary.Add(4, "<b>Level 4:</b> Increase Amount of Knifes to 3 and Damage by 25%");
        knifeDictionary.Add(5, "<b>Level 5:</b> Increase Amount of Knifes to 4 Reduce Cooldown by 1s");
        knifeDictionary.Add(6, "<b>Level 6:</b> Increase Knifes Damage by 25%");
        knifeDictionary.Add(7, "<b>Level 7:</b> Increase Amount of Knifes to 5 and Damage by 25%");
        knifeDictionary.Add(8, "<b>Level 8:</b> Reduce Knifes Cooldown by 1s and Increase Durability to 3");
        knifeDictionary.Add(9, "<b>Level 9:</b> Increase Amount of Knifes to 6 and Damage by 25%");

        // Add Dictionaries to list of dictionaries
        dictionariesList.Add(maxHealthDictionary);              //  -> 0
        dictionariesList.Add(healthPickupsDictionary);          //  -> 1
        dictionariesList.Add(speedDictionary);                  //  -> 2
        dictionariesList.Add(xpRadiusDictionary);               //  -> 3
        dictionariesList.Add(whipDictionary);                   //  -> 4
        dictionariesList.Add(bibleDictionary);                  //  -> 5
        dictionariesList.Add(holyWaterDictionary);              //  -> 6
        dictionariesList.Add(knifeDictionary);                  //  -> 7
    }*/
}
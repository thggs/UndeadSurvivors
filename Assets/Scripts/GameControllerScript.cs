using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

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

    public HealthBar health;
    public XpBar xp;
    public Button upgradeButton1;
    public Button upgradeButton2;
    public Button upgradeButton3;

    public List<Dictionary<int, string>> dictionariesList = new List<Dictionary<int, string>>();


    // Start is called before the first frame update
    void Start()
    {
        Dictionaries();
        upgradeButton1.onClick.RemoveAllListeners();
        int[] upgrades = new int[6] {0,2,4,5,6,7};
        int[] selectedInts = upgrades.OrderBy(x => Random.value).Take(3).ToArray();
        string option1 = dictionariesList[selectedInts[0]][selectLevel(selectedInts[0])];
        string option2 = dictionariesList[selectedInts[1]][selectLevel(selectedInts[1])];
        string option3 = dictionariesList[selectedInts[2]][selectLevel(selectedInts[2])];
        upgradeButton1.gameObject.GetComponentInChildren<TMP_Text>().text = option1;
        upgradeButton2.gameObject.GetComponentInChildren<TMP_Text>().text = option2;
        upgradeButton3.gameObject.GetComponentInChildren<TMP_Text>().text = option3;

        //upgradeButton1.onClick.AddListener(Ricardo);
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

        if(gameStats.player.PlayerXP >= gameStats.player.PlayerLevel * 10)
        {
            gameStats.player.PlayerLevel++;
            gameStats.player.PlayerXP = 0;

            upgradePanel.SetActive(true);
            Time.timeScale = 0;
            
            int[] upgrades = new int[6] {0,2,4,5,6,7};
            int[] selectedInts = upgrades.OrderBy(x => Random.value).Take(3).ToArray();

            string option1 = dictionariesList[selectedInts[0]][selectLevel(selectedInts[0])];
            string option2 = dictionariesList[selectedInts[1]][selectLevel(selectedInts[1])];
            string option3 = dictionariesList[selectedInts[2]][selectLevel(selectedInts[2])];
            upgradeButton1.gameObject.GetComponentInChildren<TMP_Text>().text = option1;
            upgradeButton2.gameObject.GetComponentInChildren<TMP_Text>().text = option2;
            upgradeButton3.gameObject.GetComponentInChildren<TMP_Text>().text = option3;

            //upgradeButton1.gameObject.GetComponentInChildren(Text).text = "testing";


            //upgradePanel.SetActive(false);
            //Time.timeScale = 1;
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

    public int selectLevel(int option){
        switch(option){
            case 0: return(gameStats.player.PlayerMaxHealthLevel + 1);
            //case 1: return  HP Pickups break;
            case 2: return(gameStats.player.PlayerSpeedLevel + 1); 
            //case 3: break; XP Radius; break;
            case 4: return(gameStats.whip.WhipLevel + 1); 
            case 5: return(gameStats.bible.BibleLevel + 1); 
            case 6: return(gameStats.holyWater.WaterLevel + 1); 
            case 7: return(gameStats.throwingKnife.KnifeLevel + 1); 
            default: return 0;
        }   
    }

    public void Dictionaries()
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
        maxHealthDictionary.Add(2, "Increase Max Health by 10%");
        maxHealthDictionary.Add(3, "Increase Max Health by 10%");
        maxHealthDictionary.Add(4, "Increase Max Health by 10%");
        maxHealthDictionary.Add(5, "Increase Max Health by 10%");
        maxHealthDictionary.Add(6, "Increase Max Health by 10%");
        maxHealthDictionary.Add(7, "Increase Max Health by 10%");

        // Health Pickups -> 1
        healthPickupsDictionary.Add(2, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(3, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(4, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(5, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(6, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(7, "Increase health restored by Pickups 50%");
        
        // Player Speed -> 2
        speedDictionary.Add(2, "Increase Player Speed 10%");
        speedDictionary.Add(3, "Increase Player Speed 10%");
        speedDictionary.Add(4, "Increase Player Speed 10%");
        speedDictionary.Add(5, "Increase Player Speed 10%");
        speedDictionary.Add(6, "Increase Player Speed 10%");
        speedDictionary.Add(7, "Increase Player Speed 10%");

        // XP Radius -> 3
        xpRadiusDictionary.Add(2, "Increase XP Radius to 1");
        xpRadiusDictionary.Add(3, "Increase XP Radius to 1.5");
        xpRadiusDictionary.Add(4, "Increase XP Radius to 2");
        xpRadiusDictionary.Add(5, "Increase XP Radius to 2.5");
        xpRadiusDictionary.Add(6, "Increase XP Radius to 3");
        xpRadiusDictionary.Add(7, "Increase XP Radius to 3.5");
        
        // Assign Values - Weapons
        // Whip -> 4
        whipDictionary.Add(2, "Increase Whip Damage by 25%");
        whipDictionary.Add(3, "Reduce Whip Cooldown by 20%");
        whipDictionary.Add(4, "Increase Whip Damage by 25%");
        whipDictionary.Add(5, "Reduce Whip Cooldown by 10%");
        whipDictionary.Add(6, "Increase Whip Damage by 25%");
        whipDictionary.Add(7, "Reduce Whip Cooldown by 10%");
        whipDictionary.Add(8, "Increase Whip Damage by 25%");
        whipDictionary.Add(9, "Increase Whip Damage by 25% and Reduce Whip Cooldown by 10%");

        // Bible -> 5
        bibleDictionary.Add(2, "Increase Bible Damage by 25%");
        bibleDictionary.Add(3, "Increase Amount of Bibles to 2 and Lifetime by 1s");
        bibleDictionary.Add(4, "Increase Bible Damage by 25%");
        bibleDictionary.Add(5, "Increase Amount of Bibles to 3 and Reduce Cooldown by 1s");
        bibleDictionary.Add(6, "Increase Bible Damage by 25%");
        bibleDictionary.Add(7, "Increase Amount of Bibles to 4 and Lifetime by 1s");
        bibleDictionary.Add(8, "Increase Bible Damage by 25%");
        bibleDictionary.Add(9, "Increase Bible Damage by 25%");

        // Holy Water -> 6
        holyWaterDictionary.Add(2, "Increase Amount of Holy Waters to 2 and Damage by 25%");
        holyWaterDictionary.Add(3, "Increase Holy Water Damage by 25% and Lifetime by 1s");
        holyWaterDictionary.Add(4, "Increase Amount of Holy Waters to 3 and Reduce Cooldown by 1s");
        holyWaterDictionary.Add(5, "Increase Holy Water Damage by 25%");
        holyWaterDictionary.Add(6, "Increase Amount of Holy Waters to 4 and Lifetime by 1s");
        holyWaterDictionary.Add(7, "Increase Amount of Holy Waters to 5 and Reduce Cooldown by 1s");
        holyWaterDictionary.Add(8, "Increase Amount of Holy Waters to 6 and Damage by 25%");
        holyWaterDictionary.Add(9, "Increase Amount of Holy Waters to 7 and Damage by 25%");

        // Knife -> 7
        knifeDictionary.Add(2, "Increase Amount of Knifes to 2 and Damage by 25%");
        knifeDictionary.Add(3, "Reduce Knifes Cooldown by 1s");
        knifeDictionary.Add(4, "Increase Amount of Knifes to 3 and Damage by 25%");
        knifeDictionary.Add(5, "Increase Amount of Knifes to 4 Reduce Cooldown by 1s");
        knifeDictionary.Add(6, "Increase Knifes Damage by 25%");
        knifeDictionary.Add(7, "Increase Amount of Knifes to 5 and Damage by 25%");
        knifeDictionary.Add(8, "Reduce Knifes Cooldown by 1s");
        knifeDictionary.Add(9, "Increase Amount of Knifes to 6 and Damage by 25%");

        // Add Dictionaries to list of dictionaries
        dictionariesList.Add(maxHealthDictionary);              //  -> 0
        dictionariesList.Add(healthPickupsDictionary);          //  -> 1
        dictionariesList.Add(speedDictionary);                  //  -> 2
        dictionariesList.Add(xpRadiusDictionary);               //  -> 3
        dictionariesList.Add(whipDictionary);                   //  -> 4
        dictionariesList.Add(bibleDictionary);                  //  -> 5
        dictionariesList.Add(holyWaterDictionary);              //  -> 6
        dictionariesList.Add(knifeDictionary);                  //  -> 7

        // Exemplo de obtencao de valores (depois de passar a lista para outro lado ou aceder diretamente daqui)
        //string value1 = dictionariesList[0][1]; // primeiro dicionario nivel 1: maxHealth nivel 1
        //string value2 = dictionariesList[1][2]; // segundo dicionario nivel 2: healthPickups nivel 2
    }
}
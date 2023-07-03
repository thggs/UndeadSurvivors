using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibleControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bible;
    [SerializeField]
    private GameStats gameStats;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Bible());
    }

    public void LevelUp()
    {
        
    }

    IEnumerator Bible()
    {
        GameObject[] bibleSpawns = new GameObject[gameStats.bible.BibleLevel];

        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i <= gameStats.bible.BibleLevel; i++)
        {
            // Create object that holds the bible projectiles and set it to follow the player
            bibleSpawns[i-1] = new GameObject("BibleSpawn" + i);
            bibleSpawns[i-1].transform.position = transform.position;
            bibleSpawns[i-1].transform.rotation = rotation;
            bibleSpawns[i-1].AddComponent<CameraControllerScript>();
            bibleSpawns[i-1].GetComponent<CameraControllerScript>().player = transform.parent;

            // Instantiate Bible as child object of BibleSpawn
            GameObject instance = Instantiate(bible, Vector3.zero, Quaternion.identity, bibleSpawns[i-1].transform);
            instance.GetComponent<SingleDamageScript>().damage = gameStats.bible.BibleDamage;

            // Rotate Bible around
            rotation.eulerAngles += new Vector3(0, 0, (360 / gameStats.bible.BibleLevel));
        }

        yield return new WaitForSeconds(gameStats.bible.BibleLifetime);

        for (int i = 1; i <= gameStats.bible.BibleLevel; i++)
        {
            Animator anim = bibleSpawns[i-1].GetComponentInChildren<Animator>();
            anim.SetTrigger("disappear");
        }
        yield return new WaitForSeconds(gameStats.bible.BibleCooldown);
        StartCoroutine(Bible());
    }
}

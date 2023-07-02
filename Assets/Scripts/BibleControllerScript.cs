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

    IEnumerator Bible()
    {
        GameObject[] bibleSpawns = new GameObject[gameStats.bible.BibleLevel];

        Quaternion rotation = Quaternion.identity;

        for (int i = 0; i < gameStats.bible.BibleLevel; i++)
        {
            // Create object that holds the bible projectiles and set it to follow the player
            bibleSpawns[i] = new GameObject("BibleSpawn" + (i+1));
            bibleSpawns[i].transform.position = transform.position;
            bibleSpawns[i].transform.rotation = rotation;
            bibleSpawns[i].AddComponent<CameraControllerScript>();
            bibleSpawns[i].GetComponent<CameraControllerScript>().player = transform.parent;

            // Instantiate Bible as child object of BibleSpawn
            GameObject instance = Instantiate(bible, Vector3.zero, Quaternion.identity, bibleSpawns[i].transform);
            instance.GetComponent<SingleDamageScript>().damage = gameStats.bible.BibleDamage;
            // Rotate Bible around
            rotation.eulerAngles += new Vector3(0, 0, (360 / gameStats.bible.BibleLevel));
        }

        yield return new WaitForSeconds(gameStats.bible.BibleLifetime);

        for (int i = 0; i < gameStats.bible.BibleLevel; i++)
        {
            Animator anim = bibleSpawns[i].GetComponentInChildren<Animator>();
            anim.SetTrigger("disappear");
        }
        yield return new WaitForSeconds(gameStats.bible.BibleCooldown);
        StartCoroutine(Bible());
    }
}

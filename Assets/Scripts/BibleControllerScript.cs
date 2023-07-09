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
        int bibleProjectiles = gameStats.bible.BibleProjectiles;
        float bibleCooldown = gameStats.bible.BibleCooldown;
        float bibleLifetime = gameStats.bible.BibleLifetime;
        float bibleDamage = gameStats.bible.BibleDamage;
        GameObject[] bibleSpawns = new GameObject[bibleProjectiles];

        Quaternion rotation = Quaternion.identity;
        if (bibleProjectiles > 0)
        {
            for (int i = 1; i <= bibleProjectiles; i++)
            {
                // Create object that holds the bible projectiles and set it to follow the player
                bibleSpawns[i - 1] = new GameObject("BibleSpawn" + i);
                bibleSpawns[i - 1].transform.position = transform.position;
                bibleSpawns[i - 1].transform.rotation = rotation;
                bibleSpawns[i - 1].AddComponent<CameraControllerScript>();
                bibleSpawns[i - 1].GetComponent<CameraControllerScript>().player = transform.parent;

                // Instantiate Bible as child object of BibleSpawn
                GameObject instance = Instantiate(bible, Vector3.zero, Quaternion.identity, bibleSpawns[i - 1].transform);
                instance.GetComponent<SingleDamageScript>().damage = bibleDamage;

                // Rotate Bible around
                rotation.eulerAngles += new Vector3(0, 0, (360 / bibleProjectiles));
            }

            yield return new WaitForSeconds(bibleLifetime);

            for (int i = 1; i <= bibleProjectiles; i++)
            {
                Animator anim = bibleSpawns[i - 1].GetComponentInChildren<Animator>();
                anim.SetTrigger("disappear");
            }
        }
        yield return new WaitForSeconds(bibleCooldown);
        StartCoroutine(Bible());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject whipSlash;
    [SerializeField]
    private GameStats gameStats;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Whip());
    }

    IEnumerator Whip()
    {
        int whipProjectiles = gameStats.whip.WhipProjectiles;
        float whipDamage = gameStats.whip.WhipDamage;
        float whipDelay = gameStats.whip.WhipDelay;
        float whipCooldown = gameStats.whip.WhipCooldown;

        Vector3 offset = new Vector3(transform.parent.right.x * 2f, 0, 0);
        Quaternion rotation = transform.rotation;

        for (int i = 1; i <= whipProjectiles; i++)
        {

            // Spawn WhipSpawn object to allow WhipSlash to do its animation whithout problems
            GameObject whipSpawn = new GameObject("WhipSpawn" + i);
            whipSpawn.transform.position = transform.position + offset;
            whipSpawn.transform.rotation = rotation;
            whipSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0);

            // Instantiate WhipSlash as child object of WhipSpawn
            GameObject whip = Instantiate(whipSlash, Vector3.zero, rotation, whipSpawn.transform);
            whip.GetComponent<SingleDamageScript>().damage = whipDamage;
            whip.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EffectsVolume");
            whip.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);

            // Rotate WhipSpawn around
            rotation.eulerAngles += new Vector3(0, 180, 0);

            // Offset position of WhipSpawn
            if (i % 2 == 0)
            {
                offset.x = transform.parent.right.x * 2f;
                offset.y -= 0.5f;
            }
            else
            {
                offset.x = transform.parent.right.x * -2f;
            }

            Destroy(whipSpawn, whip.GetComponent<AudioSource>().clip.length);
            yield return new WaitForSeconds(whipDelay);
        }
        yield return new WaitForSeconds(whipCooldown);
        StartCoroutine(Whip());
    }
}

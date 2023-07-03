using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject knife;
    [SerializeField]
    private GameStats gameStats;
    public int knifeLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Knife());
    }

    IEnumerator Knife() {

        Vector3 offset = new Vector3(transform.right.x * 2f, transform.up.y, 0);
        Quaternion rotation = transform.rotation;

        for(int i = 1; i <= knifeLevel; i++){

            // Instantiate WhipSlash as child object of WhipSpawn
            GameObject knifeObject = Instantiate(knife, transform.position+(transform.right*1), transform.rotation);
            Rigidbody2D rb = knifeObject.GetComponent<Rigidbody2D>();
            rb.AddForce(rb.transform.right * 20, ForceMode2D.Impulse);
            
            yield return new WaitForSeconds(gameStats.throwingKnife.KnifeDelay);
        }
        yield return new WaitForSeconds(gameStats.throwingKnife.KnifeCooldown);
        StartCoroutine(Knife());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject knife;
    [SerializeField]
    private GameStats gameStats;
    [SerializeField]
    private Vector3 throwDirection;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Knife());
        throwDirection = new Vector3(1,0,0);
    }

    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            throwDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        }
            
    }

    IEnumerator Knife() {

        int knifeProjectiles = gameStats.throwingKnife.KnifeProjectiles;
        int knifeDurability = gameStats.throwingKnife.KnifeDurability;
        float knifeDamage = gameStats.throwingKnife.KnifeDamage;
        float knifeLifetime = gameStats.throwingKnife.KnifeLifetime;
        float knifeDelay = gameStats.throwingKnife.KnifeDelay;
        float knifeCooldown = gameStats.throwingKnife.KnifeCooldown;

        for(int i = 1; i <= knifeProjectiles; i++){

            // Instantiate WhipSlash as child object of WhipSpawn
            GameObject knifeObject = Instantiate(knife, throwDirection + transform.position, Quaternion.FromToRotation(new Vector3(1,0,0), throwDirection));
            
            SingleDamageScript knifeObjectScript = knifeObject.GetComponent<SingleDamageScript>();
            knifeObjectScript.hasDurability = true;
            knifeObjectScript.durability = knifeDurability;
            knifeObjectScript.damage = knifeDamage;

            Rigidbody2D rb = knifeObject.GetComponent<Rigidbody2D>();
            rb.AddForce(rb.transform.right * 20, ForceMode2D.Impulse);

            Destroy(knifeObject, knifeLifetime);
            
            yield return new WaitForSeconds(knifeDelay);
        }
        yield return new WaitForSeconds(knifeCooldown);
        StartCoroutine(Knife());
    }
}

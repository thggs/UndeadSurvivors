using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject knife;
    [SerializeField]
    private GameStats gameStats;
    private Vector3 throwDirection;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Knife());
        throwDirection = transform.right;
    }

    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Vertical") > 0){
            throwDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        }
            
    }

    IEnumerator Knife() {

        for(int i = 1; i <= gameStats.throwingKnife.KnifeProjectiles; i++){

            // Instantiate WhipSlash as child object of WhipSpawn
            GameObject knifeObject = Instantiate(knife, throwDirection + transform.position, Quaternion.FromToRotation(transform.right, throwDirection));
            
            SingleDamageScript knifeObjectScript = knifeObject.GetComponent<SingleDamageScript>();
            knifeObjectScript.hasDurability = true;
            knifeObjectScript.durability = gameStats.throwingKnife.KnifeDurability;
            knifeObjectScript.damage = gameStats.throwingKnife.KnifeDamage;

            Rigidbody2D rb = knifeObject.GetComponent<Rigidbody2D>();
            rb.AddForce(rb.transform.right * 20, ForceMode2D.Impulse);

            Destroy(knifeObject, gameStats.throwingKnife.KnifeLifetime);
            
            yield return new WaitForSeconds(gameStats.throwingKnife.KnifeDelay);
        }
        yield return new WaitForSeconds(gameStats.throwingKnife.KnifeCooldown);
        StartCoroutine(Knife());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDamageScript : MonoBehaviour
{
    public float damage;
    public bool hasDurability = false;
    public int durability;
    
    private void Update() {
        if(durability <= 0 && hasDurability)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject otherGameObject = other.gameObject;
        if(other.tag == "Enemy")
        {
            otherGameObject.GetComponent<EnemyControllerScript>().TakeDamage(damage);
            if(hasDurability)
            {
                durability--;
            }
        }else if(other.tag == "Boss")
        {
            otherGameObject.GetComponent<BossControllerScript>().TakeDamage(damage);
            if(hasDurability)
            {
                durability--;
            }
        } 
        else if(other.tag == "Untagged" && hasDurability)
        {
            Destroy(gameObject);
        }
    }
}

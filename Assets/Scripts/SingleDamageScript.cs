using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDamageScript : MonoBehaviour
{
    public float damage;
    public bool hasDurability = false;
    public int durability = 0;
    
    private void Update() {
        if(durability <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject enemy = other.gameObject;
        if(other.tag == "Enemy")
        {
            enemy.GetComponent<EnemyControllerScript>().TakeDamage(damage);
            if(hasDurability)
            {
                durability--;
            }
        }
    }
}

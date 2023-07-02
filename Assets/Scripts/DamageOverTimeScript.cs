using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeScript : MonoBehaviour
{
    public float damage;
    private void OnTriggerStay2D(Collider2D other) {
        GameObject enemy = other.gameObject;
        if(other.tag == "Enemy")
        {
            enemy.GetComponent<EnemyControllerScript>().TakeDamage(damage);
        }
    }
}

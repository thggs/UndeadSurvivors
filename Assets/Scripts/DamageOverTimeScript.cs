using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeScript : MonoBehaviour
{
    public float damage;
    private void OnTriggerStay2D(Collider2D other) {
        GameObject otherGameObject = other.gameObject;
        if(other.tag == "Enemy")
        {
            otherGameObject.GetComponent<EnemyControllerScript>().TakeDamage(damage);
        }
        else if(other.tag == "Boss")
        {
            otherGameObject.GetComponent<BossControllerScript>().TakeDamage(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knifeColliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 3);
    }
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : MonoBehaviour
{
    public bool destroyObject = false;
    void Update(){
        if(destroyObject)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }else
            {
                Destroy(gameObject);
            }
        }
    }
}

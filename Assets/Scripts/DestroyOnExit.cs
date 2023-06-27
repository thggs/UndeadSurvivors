using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : MonoBehaviour
{
    void Update(){
        if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Exit"))
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

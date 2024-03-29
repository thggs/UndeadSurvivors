using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private Transform bar;
    public GameStats gameStats;

    // Start is called before the first frame update
    private void Start()
    {
        bar = transform.Find("Bar");
        //bar.localScale = new Vector3( .4f , 1f, 1f);
    }

    public void SetSize (float sizeNormalized){
        bar.localScale = (new Vector3(sizeNormalized, 1f, 1f));
    }

    void Update() {
        if(gameStats.player.PlayerHealth <= 0){
            this.gameObject.SetActive(false);
        }
        
    }
}

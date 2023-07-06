using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("TimerSettings")]
    public float currentTime;
    //public bool countUp;

    
    
    private bool stop; 

    
    // Start is called before the first frame update
    void Start()
    {
        stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stop){

        currentTime += Time.deltaTime ;

        int minutes = Mathf.FloorToInt(currentTime/60);
        int seconds = Mathf.FloorToInt(currentTime%60);

        timerText.text = minutes.ToString("00") + ":"+ seconds.ToString("00");
        }
    }

    public void stopTimer (bool setStop){

        stop = setStop; 
    }

    public string GetTime (){
        int minutes = Mathf.FloorToInt(currentTime/60);
        int seconds = Mathf.FloorToInt(currentTime%60);
        string gameTime = minutes.ToString("00") + ":"+ seconds.ToString("00");
        return gameTime;
    }
}

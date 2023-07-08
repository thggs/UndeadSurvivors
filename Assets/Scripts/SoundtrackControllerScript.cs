using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameStats gameStats;
    [SerializeField]
    private AudioClip[] trackList;
    [SerializeField]
    private List<AudioClip> trackQueue = new List<AudioClip>();
    private AudioSource source;
    private int lastPlayerLevel;


    void Start()
    {
        source = GetComponent<AudioSource>();

        trackQueue.Add(trackList[0]);
        trackQueue.Add(trackList[1]);

        StartCoroutine(Jukebox());
    }

    void Update()
    {
        int currentPlayerLevel = gameStats.player.PlayerLevel;
        if (lastPlayerLevel != currentPlayerLevel)
        {
            if (gameStats.player.PlayerLevel == 2)
            {
                trackQueue.Add(trackList[2]);
                trackQueue.Add(trackList[3]);
            }
            if (gameStats.player.PlayerLevel == 3)
            {
                trackQueue.Add(trackList[4]);
                trackQueue.Add(trackList[5]);
            }
            if (gameStats.player.PlayerLevel == 4)
            {
                trackQueue.Add(trackList[6]);
                trackQueue.Add(trackList[7]);
            }
            if (gameStats.player.PlayerLevel == 5)
            {
                trackQueue.Add(trackList[8]);
                trackQueue.Add(trackList[9]);
            }
        }
        lastPlayerLevel = currentPlayerLevel;

        
    }

    IEnumerator Jukebox()
    {
        yield return new WaitForSecondsRealtime(source.clip.length);
        if (trackQueue.Count != 1)
        {
            source.clip = trackQueue[1];
            source.Play();
            trackQueue.RemoveAt(0);
        }
        StartCoroutine(Jukebox());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveStats", menuName = "Scriptables/WaveStats", order = 0)]
public class WaveStats : ScriptableObject 
{
    [SerializeField]
    public GameObject[] wave1;

    [SerializeField]
    public GameObject[] wave2;

    [SerializeField]
    public GameObject[] wave3;
    
    [SerializeField]
    public GameObject[] wave4;

    [SerializeField]
    public GameObject[] wave5;
}
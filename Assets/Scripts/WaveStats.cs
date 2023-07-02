using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveStats", menuName = "Scriptables/WaveStats", order = 0)]
public class WaveStats : ScriptableObject 
{
    [SerializeField]
    public GameObject[] wave1;
    [SerializeField]
    public float wave1Time;

    [SerializeField]
    public GameObject[] wave2;
    [SerializeField]
    public float wave2Time;

    [SerializeField]
    public GameObject[] wave3;
    [SerializeField]
    public float wave3Time;
    
    [SerializeField]
    public GameObject[] wave4;
    [SerializeField]
    public float wave4Time;

    [SerializeField]
    public GameObject[] wave5;
    [SerializeField]
    public float wave5Time;
}
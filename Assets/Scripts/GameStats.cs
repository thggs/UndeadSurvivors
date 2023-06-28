using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameStats", menuName = "Scriptables/GameStats")]
public class GameStats : ScriptableObject
{
    [System.Serializable]
    public class Whip
    {
        public float WhipLevel;
        public float WhipDamage;
        public float WhipCooldown;
        public float WhipDelay;
    }
    public Whip whip = new Whip();

    [System.Serializable]
    public class Bible
    {
        public int BibleLevel;
        public int BibleDamage;
        public float BibleCooldown;
        public float BibleLifetime;
    }
    public Bible bible = new Bible();


    [System.Serializable]
    public class Player
    {
        public int PlayerHealth;
        public int PlayerMaxHealth;
        public int PlayerXP;
        public int PlayerLevel;
    }
    public Player player = new Player();


    [System.Serializable]
    public class Enemy
    {
        public int Damage;
        public int MaxHealth;
    }
    public Enemy zombie = new Enemy();
    public Enemy bat = new Enemy();
    public Enemy skeleton = new Enemy();
    public Enemy crawler = new Enemy();
    public Enemy wraith = new Enemy();
    public Enemy flyingEye = new Enemy();
}

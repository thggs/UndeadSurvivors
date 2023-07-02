using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameStats", menuName = "Scriptables/GameStats")]
public class GameStats : ScriptableObject
{
    [System.Serializable]
    public class Whip
    {
        public int WhipLevel;
        public float WhipDamage;
        public float WhipCooldown;
        public float WhipDelay;
    }
    public Whip whip = new Whip();

    [System.Serializable]
    public class Bible
    {
        public int BibleLevel;
        public float BibleDamage;
        public float BibleCooldown;
        public float BibleLifetime;
    }
    public Bible bible = new Bible();

    [System.Serializable]
    public class HolyWater
    {
        public int WaterLevel;
        public float WaterDamage;
        public float WaterCooldown;
        public float WaterLifetime;
    }
    public HolyWater holyWater = new HolyWater();

    [System.Serializable]
    public class Player
    {
        public float PlayerHealth;
        public float PlayerMaxHealth;
        public float PlayerXP;
        public float PlayerLevel;
        public float PlayerSpeed;
    }
    public Player player = new Player();


    [System.Serializable]
    public class Enemy
    {
        public float Damage;
        public float MaxHealth;
    }
    public Enemy zombie = new Enemy();
    public Enemy bat = new Enemy();
    public Enemy skeleton = new Enemy();
    public Enemy crawler = new Enemy();
    public Enemy wraith = new Enemy();
    public Enemy flyingEye = new Enemy();
}

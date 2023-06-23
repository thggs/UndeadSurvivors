using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptables/GameStats")]
public class GameStats : ScriptableObject
{
    [System.Serializable]
    public class Whip
    {
        public int WhipDamage { get; set; }
    }
    public Whip whip = new Whip();


    [System.Serializable]
    public class Player
    {
        public int PlayerHealth { get; set; }
        public int PlayerMaxHealth { get; set; }
        public int PlayerXP { get; set; }
        public int PlayerLevel { get; set; }
    }
    public Player player = new Player();


    [System.Serializable]
    public class Zombie
    {
        public int Damage { get; set; }
        public int MaxHealth { get; set; }
    }
    public Zombie zombie = new Zombie();
}

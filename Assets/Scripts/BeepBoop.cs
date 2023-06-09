using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptables/Beep")]
public class BeepBoop : ScriptableObject
{
   [field: SerializeField]
   public int Beep {get; private set;}
}

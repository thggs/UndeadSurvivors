using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    void Update()
    {
        if(!PlayerPrefs.HasKey("FoundEasterEgg"))
        {
            PlayerPrefs.SetString("FoundEasterEgg", "NotFound");
        }

        if(PlayerPrefs.GetString("FoundEasterEgg") == "NotFound")
        {
            gameObject.SetActive(false);
        }
    }
}

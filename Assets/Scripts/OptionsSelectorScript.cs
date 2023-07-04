using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSelectorScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // Create list
        List<Dictionary<int, string>> dictionariesList = new List<Dictionary<int, string>>();

        // Create dictionaries
        Dictionary<int, string> maxHealthDictionary = new Dictionary<int, string>();
        Dictionary<int, string> healthPickupsDictionary = new Dictionary<int, string>();
        Dictionary<int, string> speedDictionary = new Dictionary<int, string>();
        Dictionary<int, string> xpRadiusDictionary = new Dictionary<int, string>();
        Dictionary<int, string> whipDictionary = new Dictionary<int, string>();
        Dictionary<int, string> bibleDictionary = new Dictionary<int, string>();
        Dictionary<int, string> holyWaterDictionary = new Dictionary<int, string>();
        Dictionary<int, string> knifeDictionary = new Dictionary<int, string>();

        // Assign Values - Player
        // Max Health -> 0
        maxHealthDictionary.Add(1, "Increase Max Health by 10%");
        maxHealthDictionary.Add(2, "Increase Max Health by 10%");
        maxHealthDictionary.Add(3, "Increase Max Health by 10%");
        maxHealthDictionary.Add(4, "Increase Max Health by 10%");
        maxHealthDictionary.Add(5, "Increase Max Health by 10%");
        maxHealthDictionary.Add(6, "Increase Max Health by 10%");

        // Health Pickups -> 1
        healthPickupsDictionary.Add(1, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(2, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(3, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(4, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(5, "Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(6, "Increase health restored by Pickups 50%");
        
        // Player Speed -> 2
        speedDictionary.Add(1, "Increase Player Speed 10%");
        speedDictionary.Add(2, "Increase Player Speed 10%");
        speedDictionary.Add(3, "Increase Player Speed 10%");
        speedDictionary.Add(4, "Increase Player Speed 10%");
        speedDictionary.Add(5, "Increase Player Speed 10%");
        speedDictionary.Add(6, "Increase Player Speed 10%");

        // XP Radius -> 3
        xpRadiusDictionary.Add(1, "Increase XP Radius to 1");
        xpRadiusDictionary.Add(2, "Increase XP Radius to 1.5");
        xpRadiusDictionary.Add(3, "Increase XP Radius to 2");
        xpRadiusDictionary.Add(4, "Increase XP Radius to 2.5");
        xpRadiusDictionary.Add(5, "Increase XP Radius to 3");
        xpRadiusDictionary.Add(6, "Increase XP Radius to 3.5");
        
        // Assign Values - Weapons
        // Whip -> 4
        whipDictionary.Add(1, "Increase Whip Damage by 25%");
        whipDictionary.Add(2, "Reduce Whip Cooldown by 20%");
        whipDictionary.Add(3, "Increase Whip Damage by 25%");
        whipDictionary.Add(4, "Reduce Whip Cooldown by 10%");
        whipDictionary.Add(5, "Increase Whip Damage by 25%");
        whipDictionary.Add(6, "Reduce Whip Cooldown by 10%");
        whipDictionary.Add(7, "Increase Whip Damage by 25%");
        whipDictionary.Add(8, "Increase Whip Damage by 25% and Reduce Whip Cooldown by 10%");

        // Bible -> 5
        bibleDictionary.Add(1, "Increase Bible Damage by 25%");
        bibleDictionary.Add(2, "Increase Amount of Bibles to 2 and Lifetime by 1s");
        bibleDictionary.Add(3, "Increase Bible Damage by 25%");
        bibleDictionary.Add(4, "Increase Amount of Bibles to 3 and Reduce Cooldown by 1s");
        bibleDictionary.Add(5, "Increase Bible Damage by 25%");
        bibleDictionary.Add(6, "Increase Amount of Bibles to 4 and Lifetime by 1s");
        bibleDictionary.Add(7, "Increase Bible Damage by 25%");
        bibleDictionary.Add(8, "Increase Bible Damage by 25%");

        // Holy Water -> 6
        holyWaterDictionary.Add(1, "Increase Amount of Holy Waters to 2 and Damage by 25%");
        holyWaterDictionary.Add(2, "Increase Holy Water Damage by 25% and Lifetime by 1s");
        holyWaterDictionary.Add(3, "Increase Amount of Holy Waters to 3 and Reduce Cooldown by 1s");
        holyWaterDictionary.Add(4, "Increase Holy Water Damage by 25%");
        holyWaterDictionary.Add(5, "Increase Amount of Holy Waters to 4 and Lifetime by 1s");
        holyWaterDictionary.Add(6, "Increase Amount of Holy Waters to 5 and Reduce Cooldown by 1s");
        holyWaterDictionary.Add(7, "Increase Amount of Holy Waters to 6 and Damage by 25%");
        holyWaterDictionary.Add(8, "Increase Amount of Holy Waters to 7 and Damage by 25%");

        // Knife -> 7
        knifeDictionary.Add(1, "Increase Amount of Knifes to 2 and Damage by 25%");
        knifeDictionary.Add(2, "Reduce Knifes Cooldown by 1s");
        knifeDictionary.Add(3, "Increase Amount of Knifes to 3 and Damage by 25%");
        knifeDictionary.Add(4, "Increase Amount of Knifes to 4 Reduce Cooldown by 1s");
        knifeDictionary.Add(5, "Increase Knifes Damage by 25%");
        knifeDictionary.Add(6, "Increase Amount of Knifes to 5 and Damage by 25%");
        knifeDictionary.Add(7, "Reduce Knifes Cooldown by 1s");
        knifeDictionary.Add(8, "Increase Amount of Knifes to 6 and Damage by 25%");

        // Add Dictionaries to list of dictionaries
        dictionariesList.Add(maxHealthDictionary);              //  -> 0
        dictionariesList.Add(healthPickupsDictionary);          //  -> 1
        dictionariesList.Add(speedDictionary);                  //  -> 2
        dictionariesList.Add(xpRadiusDictionary);               //  -> 3
        dictionariesList.Add(whipDictionary);                   //  -> 4
        dictionariesList.Add(bibleDictionary);                  //  -> 5
        dictionariesList.Add(holyWaterDictionary);              //  -> 6
        dictionariesList.Add(knifeDictionary);                  //  -> 7

        // Exemplo de obtencao de valores (depois de passar a lista para outro lado ou aceder diretamente daqui)
        //string value1 = dictionariesList[0][1]; // primeiro dicionario nivel 1: maxHealth nivel 1
        //string value2 = dictionariesList[1][2]; // segundo dicionario nivel 2: healthPickups nivel 2
    }
}

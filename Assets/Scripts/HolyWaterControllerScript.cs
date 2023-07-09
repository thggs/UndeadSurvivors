using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterControllerScript : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameStats gameStats;
    [SerializeField]
    private GameObject holyWater;

    void Start()
    {
        StartCoroutine(HolyWater());
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(mainCamera.ViewportToWorldPoint(new Vector3(0,1,mainCamera.nearClipPlane)), 0.1f);
    }

    IEnumerator HolyWater()
    {
        int waterProjectiles = gameStats.holyWater.WaterProjectiles;
        float waterDamage = gameStats.holyWater.WaterDamage;
        float WaterCooldown = gameStats.holyWater.WaterCooldown;
        float waterLifetime = gameStats.holyWater.WaterLifetime;
        for(int i = 1; i <= waterProjectiles; i++)
        {
            Quaternion rotation = Quaternion.identity;

            float RandomX = Random.Range(0.1f, 0.9f);
            float RandomY = Random.Range(0.3f, 0.7f);

            if(RandomX < 0.5f)
            {
                rotation.eulerAngles = new Vector3(0,180,0);
            }

            Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(RandomX, RandomY, mainCamera.nearClipPlane));
            
            GameObject instance = Instantiate(holyWater, spawnPosition, rotation);
            instance.GetComponentInChildren<DamageOverTimeScript>(true).damage = waterDamage;
            instance.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EffectsVolume");
            Destroy(instance, waterLifetime);
        }
        yield return new WaitForSeconds(WaterCooldown);
        StartCoroutine(HolyWater());
    }
}

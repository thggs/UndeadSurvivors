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
        for(int i = 1; i <= gameStats.holyWater.WaterProjectiles; i++)
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
            instance.GetComponentInChildren<DamageOverTimeScript>(true).damage = gameStats.holyWater.WaterDamage;
            Destroy(instance, gameStats.holyWater.WaterLifetime);
        }
        yield return new WaitForSeconds(gameStats.holyWater.WaterCooldown);
        StartCoroutine(HolyWater());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject whipSlash;
    public float whipCooldown = 1.0f;
    public float betweenWhips = 0.1f;
    private WaitForSeconds whipCooldownWFS;
    private WaitForSeconds betweenWhipsWFS = new WaitForSeconds(0.1f);
    public int whipLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        whipCooldownWFS = new WaitForSeconds(whipCooldown);
        betweenWhipsWFS = new WaitForSeconds(betweenWhips);
        StartCoroutine(Whip());
    }

    IEnumerator Whip() {

        Vector3 offset = Vector3.zero;
        Quaternion rotation = transform.rotation;

        for(int i = 1; i <= whipLevel; i++){

            // Spawn WhipSpawn object to allow WhipSlash to do its animation whithout problems
            GameObject whipSpawn = new GameObject("WhipSpawn" + i);
            whipSpawn.transform.position = transform.position + offset;
            whipSpawn.transform.rotation = rotation;
            whipSpawn.transform.localScale = new Vector3(0.5f,0.5f,0);

            // Instantiate WhipSlash as child object of WhipSpawn
            GameObject whip = Instantiate(whipSlash, Vector3.zero, rotation, whipSpawn.transform);

            // Rotate WhipSpawn around
            rotation.eulerAngles += new Vector3(0, 180 , 0);

            // Offset position of WhipSpawn
            if(i % 2 == 1){
                offset += new Vector3(-3.5f,0,0);
            }
            else{
                offset += new Vector3(3.5f,-0.5f,0);
            }

            Destroy(whipSpawn, whip.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            yield return betweenWhipsWFS;
        }
        yield return whipCooldownWFS;
        StartCoroutine(Whip());
    }
}

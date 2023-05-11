using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
    public Transform player;

    float offset = 0;

    void FixedUpdate()
    {
        // Follow player, offset varies with distance to world border
        transform.position = new Vector3(player.position.x + offset, player.position.y + offset, transform.position.z);
    }
}

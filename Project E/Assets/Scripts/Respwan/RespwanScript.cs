using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public GameObject RespawnPoint;
    public float threshold;
    public void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            Respawn();
        }
    }
    public void Respawn()
    {
        transform.position = RespawnPoint.transform.position;
    }
}

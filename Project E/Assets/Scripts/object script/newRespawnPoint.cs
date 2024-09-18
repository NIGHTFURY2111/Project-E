using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewRespawnPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject.FindWithTag("Player").GetComponent<Movement>().RespawnPoint = gameObject;
    }
}

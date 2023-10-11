using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public Transform Destination;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<Movement>().currentTeleporter = gameObject;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameObject.FindWithTag("Player").GetComponent<Movement>().currentTeleporter == gameObject)
        {
            GameObject.FindWithTag("Player").GetComponent<Movement>().currentTeleporter = null;
        }
    }
}

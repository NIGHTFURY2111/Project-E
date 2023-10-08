using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleport : MonoBehaviour
{
    private GameObject currentTeleport;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentTeleport != null)
            {
                transform.position = currentTeleport.GetComponent<TeleportScript>().GetDestination().position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TeleportScript"))
        {
            currentTeleport = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TeleportScript"))
        {
            if (collision.gameObject == currentTeleport)
            {
                currentTeleport = null;
            }
        }
    }

}

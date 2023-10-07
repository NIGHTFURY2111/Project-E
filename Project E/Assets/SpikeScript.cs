using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{   public RespawnScript respawnScript;
    // Start is called before the first frame update 
    private void Start()
    {
        respawnScript = GameObject.FindWithTag("Player").GetComponent<RespawnScript>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        respawnScript.Respawn(); 

    }
}

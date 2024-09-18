using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{   /*public Movement PlayerScript;*/
    // Start is called before the first frame update 
    private void Start()
    {
        //PlayerScript = GameObject.FindWithTag("Player").GetComponent<Movement>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //PlayerScript.Respawn(); 

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private GameObject currentTeleporter;
    [SerializeField] private Transform destination;

    public Transform GetDestination()
    {
        return destination;
    }
} 

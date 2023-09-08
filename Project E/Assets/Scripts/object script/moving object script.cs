using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingobjectscript : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int waypointsIndex;
    [SerializeField] private float speed = 2f;
    void Update()
    {
        if (Vector2.Distance(waypoints[waypointsIndex].transform.position, transform.position) < .1f)
        {
            waypointsIndex++;
            if (waypointsIndex >= waypoints.Length)
            {
                waypointsIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointsIndex].transform.position, Time.deltaTime * speed);
    }
}

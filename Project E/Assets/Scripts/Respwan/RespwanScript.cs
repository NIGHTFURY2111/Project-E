using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float threshold;
    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            transform.position = new Vector2(-2.4097f, 2.577633f);
        }
    }
}

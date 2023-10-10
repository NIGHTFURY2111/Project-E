using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    void Start()
    {
        VirtualCamera.Follow = GameObject.FindWithTag("Player").transform;
    }


}

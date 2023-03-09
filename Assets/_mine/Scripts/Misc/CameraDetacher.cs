using System;
using UnityEngine;
using Cinemachine;


public class CameraDetacher : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camera;
    
    private void Awake()
    {
        KillZone.OnPlayerFell += TakeAwayCamera;
    }

    public void TakeAwayCamera()
    {
        camera.Follow = null;
    }
}

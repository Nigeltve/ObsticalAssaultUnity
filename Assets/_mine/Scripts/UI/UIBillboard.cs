using System;
using Unity.VisualScripting;
using UnityEngine;


public class UIBillboard : MonoBehaviour
{
    private Camera _camera;
    private Transform _cameraTrans;

    private bool _hasCamera = true;
    private void Awake()
    {
        if (Camera.main == null)
        {
            _hasCamera = false;
            return;
        }
        
        _camera = Camera.main;
        _cameraTrans = _camera.transform;
    }

    private void Update()
    {
        if (!_hasCamera) return; 
        
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _cameraTrans.rotation * Vector3.up);
    }
}

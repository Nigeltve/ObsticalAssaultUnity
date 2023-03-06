using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static Action OnCoinCollected;
    
    private static int _keyCollected = 0;
    private Transform _transform;
    
    private void Awake()
    {
        _transform = this.transform;
    }

    private void Update()
    {
        _transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _keyCollected++;
            
            Debug.Log($"[key] collected {_keyCollected}");
            OnCoinCollected?.Invoke();
            Destroy(gameObject);
        }
    }

    public static int GetNumKeysCollected()
    {
        return _keyCollected;
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public static Action OnPlayerFell;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerFell?.Invoke();
        }
    }
}

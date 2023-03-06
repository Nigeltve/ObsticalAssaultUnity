using System;
using System.Collections.Specialized;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    public static Action OnLevelCompleted;
    [SerializeField] private int keysToEnd = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Coin.GetNumKeysCollected() >= keysToEnd)
        {
            Debug.Log($"[EndZone] Level Completed");
            OnLevelCompleted?.Invoke();
        }
    }
}

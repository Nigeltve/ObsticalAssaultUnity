using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using MyBox;
using TMPro;

public class UIPlay : MonoBehaviour
{
    [Separator("UI Elements")]
    [SerializeField] private GameObject _inner;
    [SerializeField] private TMP_Text _errorText;
    
    [Separator("Events")]
    [SerializeField] private UnityEvent _onLogoutSuccess;
    [SerializeField] private UnityEvent _onLogoutError;
    
    public void PlayGame()
    {
        Loader.Load(Loader.Scenes.Level3);
    }

    
    
    public void Logout()
    {
        LockerManager.Instance.LogoutUser(() =>
            {
                _onLogoutSuccess?.Invoke();
            },
            (errorMessage) =>
            {
                _errorText.text = errorMessage;
                _onLogoutError?.Invoke();
            });
    }
    
}

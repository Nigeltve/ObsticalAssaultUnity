using System;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class UIMenu : MonoBehaviour
{
    [Separator("ui elements")]
    [SerializeField] private GameObject _innerMenu;

    [SerializeField] private GameObject _innerNoInternet;
    
    
    [Separator("Events")]
    [SerializeField] private UnityEvent _onAlreadyLoggedin;
    [SerializeField] private UnityEvent _onNotLoggedin;
    private void Awake()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            _innerNoInternet.SetActive(true);
            _innerMenu.SetActive(false);
            return;
        }

        LockerManager.Instance.CheckValidSession(() =>
            {
                _onAlreadyLoggedin?.Invoke();
            },
            () =>
            {
                LockerManager.Instance.ClearWhiteLabelPlayerPrefs();
                _onNotLoggedin?.Invoke();
            });
    }

    public void CloseMenuPage()
    {
        _innerMenu.SetActive(false);
        _innerNoInternet.SetActive(false);
    } 
    
    
}

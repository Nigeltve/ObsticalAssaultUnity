using System;
using UnityEngine;

public class UIInternetAvalable : MonoBehaviour
{
    public GameObject _noInternetPage;
    public GameObject _menuPage; 
    
    private void Awake()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            _noInternetPage.SetActive(true);
            _menuPage.SetActive(false);
        }
        else
        {
            _noInternetPage.SetActive(false);
            _menuPage.SetActive(true);
        }
            
    }
        
}

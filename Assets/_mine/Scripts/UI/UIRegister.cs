using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class UIRegister : MonoBehaviour
{
    [Separator("ui elements")]
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordRetypeInputField;
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private TMP_Text _errorText;
    
    [Separator("Events")]
    [SerializeField] private UnityEvent _onSuccess;
    [SerializeField] private UnityEvent _onError;

    private void OnEnable()
    {
        ClearPage();
    }

    private void OnDisable()
    {
        ClearPage();
    }

    private void ClearPage()
    {
        _emailInputField.text = "";
        _passwordInputField.text = "";
        _passwordRetypeInputField.text = "";
        _usernameInputField.text = "";
        _errorText.text = "";
    }

    public void RegisterUser()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;
        string passwordRetype = _passwordRetypeInputField.text;
        string username = _usernameInputField.text;
        
        PlayfabManager.Instance.RegisterUser(email, password, username, passwordRetype, 
            () =>
            {
                //successful
                _onSuccess.Invoke();
            },
            (errorMessage) =>
            {
                //error
                _errorText.text = errorMessage;
                _onError.Invoke();
            });
        
    }
}

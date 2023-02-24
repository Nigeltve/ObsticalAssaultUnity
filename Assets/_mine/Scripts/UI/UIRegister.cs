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
    [SerializeField] private TMP_Text _errorText;
    
    [Separator("Events")]
    [SerializeField] private UnityEvent _onSuccess;
    [SerializeField] private UnityEvent _onError;

    public void RegisterUser()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;
        string passwordRetype = _passwordRetypeInputField.text;
        
        LootLockerManager.Instance.RegisterUser(email, password, passwordRetype, 
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

using System;
using UnityEngine;
using MyBox;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace _mine.Scripts.UI
{
    public class UILogin : MonoBehaviour
    {
        [Separator("ui elements")] [SerializeField]
        private GameObject _inner;
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Toggle _rememberToggle;
        [SerializeField] private TMP_Text _errorText;
    
        [Separator("Events")]
        [SerializeField] private UnityEvent _onSuccessWithName;
        [SerializeField] private UnityEvent _onSuccessWithoutName;
        [SerializeField] private UnityEvent _onError;

        public void OnEnable()
        {
            ClearPage();
        }

        public void OnDisable()
        {
            ClearPage();
        }

        public void OpenLoginPage()
        {
            ClearPage();
            _inner.SetActive(true);
        }

        public void CloseLoginPage()
        {
            ClearPage();
            _inner.SetActive(false);
        }
        
        private void ClearPage()
        {
            _emailInputField.text = "";
            _passwordInputField.text = "";
            _errorText.text = "";
        }

        public void LoginUser()
        {
            string email = _emailInputField.text;
            string password = _passwordInputField.text;
            bool remember = _rememberToggle.isOn;

            LockerManager.Instance.LoginUser(email, password, remember,
                (hasName) =>
                {
                    //successful
                    if(hasName)
                        _onSuccessWithName.Invoke();
                    else
                        _onSuccessWithoutName.Invoke();

                    if (!remember)
                    {
                        LockerManager.Instance.ClearWhiteLabelPlayerPrefs();
                    }
                },
                (errorMessage) =>
                {
                    _errorText.text = errorMessage;
                    _onError.Invoke();
                });
        }
    }
}
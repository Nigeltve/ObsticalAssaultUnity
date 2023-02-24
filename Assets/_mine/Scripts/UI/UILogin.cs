using UnityEngine;
using MyBox;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace _mine.Scripts.UI
{
    public class UILogin : MonoBehaviour
    {
        [Separator("ui elements")]
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_Text _errorText;
    
        [Separator("Events")]
        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onError;

        public void LoginUser()
        {
            string email = _emailInputField.text;
            string password = _passwordInputField.text;
            
            LootLockerManager.Instance.LoginUser(email, password,
                () =>
                {
                    //successful
                    _onSuccess.Invoke();
                },
                (errorMessage) =>
                {
                    _errorText.text = errorMessage;
                    _onError.Invoke();
                });
            
            
        }
    }
}
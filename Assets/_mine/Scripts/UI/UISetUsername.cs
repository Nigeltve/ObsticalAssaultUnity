using UnityEngine;
using UnityEngine.Events;
using MyBox;
using TMPro;

public class UISetUsername : MonoBehaviour
{
    [Separator("UI Elements")]
    [SerializeField] private GameObject _inner;
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TMP_Text _errorText;
    
    [Separator("Events")]
    [SerializeField] private UnityEvent _onSuccess;
    [SerializeField] private UnityEvent _onError;
    
    public void OpenUsernamePage()
    {
        ClearPage();
        _inner.SetActive(true);
    }

    public void CloseUsernamePage()
    {
        ClearPage();
        _inner.SetActive(false);
    }
    
    private void ClearPage()
    {
        _usernameInput.text = "";
        _errorText.text = "";
    }

    public void SetUsername()
    {
        string username = _usernameInput.text;
        
        LockerManager.Instance.ChangeUsername(username,
            () =>
            {
                _onSuccess.Invoke();
            },
            (errorMessage) =>
            {
                _errorText.text = errorMessage;
                _onError.Invoke();
            });
    }
}

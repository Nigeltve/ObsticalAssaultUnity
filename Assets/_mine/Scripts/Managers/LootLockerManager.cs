using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using LootLocker.Requests;
using MyBox;
using Unity.VisualScripting;

public class LootLockerManager : MyBox.Singleton<LootLockerManager> {
    private void Awake()
    {
        InitializeSingleton(false);
    }
    
    public void RegisterUser(string email, string password, string retype, Action onSuccess, Action<string> onError)
    {
        if (!CheckEmail(email))
        {
            onError("Email was not valid");
            return;
        }
        
        if (!CheckPasswordMatch(password, retype))
        {
            onError("Passwords did not match");
            return;
        }

        if (!CheckPasswordLength(password))
        {
            onError("Password did not contain at least 8 characters");
            return;
        }
        
        if (!CheckPasswordCapital(password))
        {
            onError("Password did not contain a capital Letter");
            return;
        }

        if (!CheckPasswordNumber(password))
        {
            onError("Password did not contain a number");
            return;
        }

        if (!CheckPasswordSpecialCharacter(password))
        {
            onError("Password did not contain a special character");
            return;
        }
        
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while creating user");
                onError("There was an error when creating the account");
                return;
            }

            onSuccess();
        });
    }

    public void LoginUser(string email, string password, Action onSuccess, Action<string> onError)
    {
        if (!CheckEmail(email))
        {
            onError("Email was not ivalid");
            return;
        }
        
        LootLockerSDKManager.WhiteLabelLogin(email, password, (response) =>
        {
            if (!response.success)
            {
                onError("Something went wrong");
                return;
            }

            onSuccess();
        });
        

    }
    
    public bool CheckEmail(string email)
    {
        if (CheckRegEx( new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"), email))
            return true;
        
        return false;
    }
    
    public bool CheckPassword(string password)
    {
        if (CheckRegEx(new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$"), password))
            return true;
        
        return false;
    }
    
    private bool CheckPasswordCapital(string password)
    {
        if (CheckRegEx(new Regex(@"[A-Z]"), password))
            return true;
        
        return false;
    }

    private bool CheckPasswordNumber(string password)
    {
        if (CheckRegEx(new Regex(@"\d"), password))
            return true;
        
        return false;
    }

    private bool CheckPasswordSpecialCharacter(string password)
    {
        if (CheckRegEx(new Regex(@"[\W_]"), password))
            return true;
        
        return false;
    }

    private bool CheckPasswordMatch(string password, string retype)
    {
        if (string.Equals(password, retype))
            return true;
        
        return false;
    }

    private bool CheckPasswordLength(string password)
    {
        if (password.Length >= 8)
            return true;

        return false;
    }
    private bool CheckRegEx(Regex re, string input)
    {
        return re.IsMatch(input);
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using MyBox;
using LootLocker.Requests;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LockerManager : Singleton<LockerManager> {
    private void Awake()
    {
        InitializeSingleton(false);
    }

    public void RegisterUser(string email, string password, string retype, Action onSuccess, 
        Action<string> onError)
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

        LootLockerSDKManager.WhiteLabelSignUp(email, password,
            response =>
            {
                if (!response.success)
                {
                    onError(GetErrorMessage(response.Error));
                    return;
                }

                onSuccess();
            });
    }

    public void LoginUser(string email, string password, bool remember, Action<bool> onSuccess, Action<string> onError)
    {
        if (!CheckEmail(email))
        {
            onError("Email was not valid");
            return;
        }

        LootLockerSDKManager.WhiteLabelLoginAndStartSession(email, password, remember,(response) =>
        {
            if (!response.success)
            {
                onError(GetErrorMessage(response.Error));
                return;
            }

            
            
            PlayerPrefs.SetString("PlayerID", response.SessionResponse.public_uid);
            

            LootLockerSDKManager.GetPlayerName((resName) =>
            {
                if (!resName.success)
                {
                    onError(GetErrorMessage(resName.Error));
                    return;
                }
                if(!resName.name.IsNullOrEmpty())
                    PlayerPrefs.SetString("PlayerName", resName.name);
                onSuccess(!resName.name.IsNullOrEmpty());
            });
        });
    }

    public void CheckValidSession(Action onSuccess, Action onError)
    {
        LootLockerSDKManager.CheckWhiteLabelSession((valid) =>
        {
            if (valid)
            {
                onSuccess();
            }
            else
            {
                onError();
            }
        });
    }
    
    public void LogoutUser(Action onSuccess, Action<string> onError)
    {
        LootLockerSDKManager.EndSession((res) =>
        {
            if(!res.success)
            {
                onError(GetErrorMessage(res.Error));
                return;
            }

            ClearWhiteLabelPlayerPrefs();
            onSuccess();
        });
    }

    public void ClearWhiteLabelPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("LootLockerWhiteLabelSessionToken"))
        {
            PlayerPrefs.DeleteKey("LootLockerWhiteLabelSessionToken");
        }

        if (PlayerPrefs.HasKey("PlayerID"))
        {
            PlayerPrefs.DeleteKey("PlayerID");
        }

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            PlayerPrefs.DeleteKey("PlayerName");
        }

        if (PlayerPrefs.HasKey("LootLockerWhiteLabelSessionEmail"))
        {
            PlayerPrefs.DeleteKey("LootLockerWhiteLabelSessionEmail");
        }
    }
    
    public void ChangeUsername(string username, Action onSuccess, Action<string> onError)
    {
        if (username.IsNullOrEmpty())
        {
            onError("Username empty");
            return;
        }
        
        if (username.Length < 4)
        {
            onError("Username has to be a minimum of 4 characters");
            return;
        }

        if (username.Length > 16)
        {
            onError("Username too long. only 16 characters allowed");
        }
        
        LootLockerSDKManager.SetPlayerName(username,
            nameResponse =>
            {
                if (!nameResponse.success)
                {
                    onError(GetErrorMessage(nameResponse.Error));
                }
                
                PlayerPrefs.SetString("PlayerName", nameResponse.name);
                onSuccess();
            });
    }

    public bool CheckEmail(string email)
    {
        if (CheckRegEx( new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"), email))
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


    public void GetItem()
    {
        LootLockerSDKManager.GetInventory((response) =>
        {
            var inventory = response.inventory;

            var asset = inventory[0].asset;
            LootLockerFile file = asset.files[0];
            
            if (file == null)
            {
                Debug.Log("File was not there");
            }
            else
            {
                Debug.Log("file Did exist");
                // StartCoroutine(DownloadJsonFile(file.url,
                //     (res) =>
                //     {
                //
                //         string text = RemoveInvisibleChars(res);
                //         
                //         Debug.Log(text);
                //         
                //         TestItem item = JsonConvert.DeserializeObject<TestItem>(text);
                //         Debug.Log($"{item.first_name} {item.last_name} is {item.age} years old and makes {item.salary}");
                //         
                //     },
                //     (error) =>
                //     {
                //         Debug.Log($"Error -> {error}");
                //     }));
            }
        });
    }
    
    public string RemoveInvisibleChars(string input)
    {
        // Replace all invisible characters with an empty string using a regular expression
        return Regex.Replace(input, @"\p{C}+", string.Empty);
    }

    private string GetErrorMessage(string str)
    {
        int startIndex = str.IndexOf("message\":\"") + 10;
        int endIndex = str.IndexOf("\",\"request_id\"");

        return str.Substring(startIndex, endIndex - startIndex);
        
    }

    private IEnumerator DownloadJsonFile(string url, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error has occured");
                onError(request.error);
            }
            else
            {
                Debug.Log($"Successfully Downloaded -> {request.downloadHandler.text}");
                onSuccess(request.downloadHandler.text);
                
            }
        }
    }

    public enum LeadBoardIds
    {
        LevelOne = 12226,
        LevelTwo = 12227,
        LevelThree = 12264,
        LevelFour = 12265,
        LevelFive = 12266,
    }
    
    public void AddtoLeaderboard(LeadBoardIds leaderboardID, int score, Action onSuccess, Action<string> onError)
    {
        
        Debug.Log($"leaderboard -> {(int)leaderboardID}");
        
        LootLockerSDKManager.SubmitScore(PlayerPrefs.GetString("PlayerName"), score, ((int)leaderboardID).ToString(),
            (response) =>
            {
                if (!response.success)
                {
                    onError(response.text);
                    return;
                }

                onSuccess();
            });
    }
}


using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.PlayerLoop;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _TimeText;

    private int _time = 0;
    private void Awake()
    {
        _coinText.text = CoinString(0);
        InvokeRepeating(nameof(UpdateTime),1, 1);
    }

    private void OnEnable()
    {
        Coin.OnCoinCollected += SetNewCoinText;
        EndZone.OnLevelCompleted += EndTimeInvoke;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= SetNewCoinText;
        EndZone.OnLevelCompleted -= EndTimeInvoke;
    }

    private void SetNewCoinText()
    {
        _coinText.text = CoinString(Coin.GetNumKeysCollected());
    }

    private void EndTimeInvoke()
    {
        CancelInvoke(nameof(UpdateTime));
    }

    private void UpdateTime()
    {
        _time++;
        _TimeText.text = TimeString(_time);
        
    }

    private string CoinString(int amount)
    {
        return $"Coins: {amount}";
    }

    private string TimeString(int secs)
    {
        TimeSpan t = TimeSpan.FromSeconds( secs );

        string answer = "";
        
        if (secs < 60)
        {
            answer = string.Format("{0:D2}s", t.Seconds);   
        }
        else if (secs < 3600)
        {
            answer = string.Format("{0:D2}m:{1:D2}s", t.Minutes, t.Seconds); 
        }
        else
        {
            answer = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds); 
        }
         
        return $"Time: {answer}";
    }
}
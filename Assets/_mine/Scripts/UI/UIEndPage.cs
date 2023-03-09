using System;
using LootLocker.Requests;
using Unity.VisualScripting;
using UnityEngine;

public class UIEndPage : MonoBehaviour
{
    [SerializeField] private GameObject _innerFailedJump;
    [SerializeField] private GameObject _innerSuccess;
    
    private void OnEnable()
    {
        KillZone.OnPlayerFell += FailedJumpPage;
        EndZone.OnLevelCompleted += LevelComplete;
    }

    private void OnDisable()
    {
        KillZone.OnPlayerFell -= FailedJumpPage;
        EndZone.OnLevelCompleted -= LevelComplete;
    }

    private void LevelComplete()
    {
        _innerSuccess.SetActive(true);
    }
    
    private void FailedJumpPage()
    {
        _innerFailedJump.SetActive(true);
    }

    public void UploadScore()
    {
        LockerManager.Instance.AddtoLeaderboard(LockerManager.LeadBoardIds.LevelOne, 1000, () =>
            {
                Debug.Log("[leaderboard] onSuccess");
            },
            (errorMessage) =>
            {
                Debug.Log($"[leaderboard] {errorMessage}");
            });
    }

    public void LoadNextLevel()
    {
        Loader.Load(Loader.GetNextLevel());
    }

    public void ReloaLevel()
    {
        Loader.Load(Loader.GetCurrentScene());
    }

    public void LoadMainMenu()
    {
        Loader.Load(Loader.Scenes.Menu);
    }
}

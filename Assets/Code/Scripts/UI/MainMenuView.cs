using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _garageButton;
    [SerializeField] Button _leaderboardButton;

    public void Initialize(Action onPlayButtonClick, Action onGarageButtonClick, Action onLeaderboardButtonClick)
    {
        _playButton.onClick.AddListener(onPlayButtonClick.Invoke);
        _garageButton.onClick.AddListener(onGarageButtonClick.Invoke);
        _leaderboardButton.onClick.AddListener(onLeaderboardButtonClick.Invoke);
    }

    public void Terminate()
    {
        _playButton.onClick.RemoveAllListeners();
        _garageButton.onClick.RemoveAllListeners();
        _leaderboardButton.onClick.RemoveAllListeners();
    }
}

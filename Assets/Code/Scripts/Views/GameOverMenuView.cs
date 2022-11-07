using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Code.Scripts.Views
{
    public class GameOverMenuView : MonoBehaviour
    {
        [SerializeField] Button _playButton;
        [SerializeField] Button _garageButton;
        [SerializeField] Button _leaderboardButton;
        [SerializeField] TMP_Text _scoreText;
        [SerializeField] TMP_InputField _inputField;

        public void Initialize(Action onPlayButtonClick, Action onGarageButtonClick, Action onLeaderboardButtonClick)
        {
            _playButton.onClick.AddListener(onPlayButtonClick.Invoke);
            _garageButton.onClick.AddListener(onGarageButtonClick.Invoke);
            _leaderboardButton.onClick.AddListener(onLeaderboardButtonClick.Invoke);
        }

        public void OnGameOver(int finalScore)
        {
            _scoreText.text = finalScore.ToString();
        }

        public void Terminate()
        {
            _playButton.onClick.RemoveAllListeners();
            _garageButton.onClick.RemoveAllListeners();
            _leaderboardButton.onClick.RemoveAllListeners();
        }
    }
}
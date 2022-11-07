using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Scripts.UI
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] JoystickHandler _joystickHandler;
        [SerializeField] Button _fireButton;
        [SerializeField] Button _shieldButton;
        [SerializeField] TMP_Text _scoreText;

        public int Score { get; private set; }

        public void Initialize(Action onFireButtonClickedEvent, Action onShieldButtonEvent, Action<float, float> onJoystickInput)
        {
            _fireButton.onClick.AddListener(onFireButtonClickedEvent.Invoke);
            _shieldButton.onClick.AddListener(onShieldButtonEvent.Invoke);
            _joystickHandler.Initialize(onJoystickInput);
            _scoreText.text = "Score: " + 0;
        }

        public void OnGameStart()
        {
            Score = 0;
        }

        public void UpdateScore(int points)
        {
            Score += points;
            _scoreText.text = "Score: " + Score;
        }

        public void Terminate()
        {
            _fireButton.onClick.RemoveAllListeners();
            _shieldButton.onClick.RemoveAllListeners();
        }
    }
}

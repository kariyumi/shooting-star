using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Scripts.Views
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] JoystickHandler _joystickHandler;
        [SerializeField] Button _fireButton;
        [SerializeField] Button _shieldButton;
        [SerializeField] TMP_Text _scoreText;
        [SerializeField] TMP_Text _starCounterText;

        public void Initialize(Action onFireButtonClickedEvent, Action onShieldButtonEvent, Action<float, float> onJoystickInput)
        {
            _fireButton.onClick.AddListener(onFireButtonClickedEvent.Invoke);
            _shieldButton.onClick.AddListener(onShieldButtonEvent.Invoke);
            _joystickHandler.Initialize(onJoystickInput);
        }

        public void OnGameStart()
        {
            UpdateScore(0);
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = "Score: " + score;
        }

        public void UpdateSoftCurrency(int value)
        {
            _starCounterText.text = value.ToString();
        }

        public void Terminate()
        {
            _fireButton.onClick.RemoveAllListeners();
            _shieldButton.onClick.RemoveAllListeners();
        }
    }
}

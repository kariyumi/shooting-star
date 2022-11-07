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

        private int _score = 0;

        public void Initialize(Action onFireButtonClickedEvent, Action onShieldButtonEvent, Action<float, float> onJoystickInput)
        {
            _fireButton.onClick.AddListener(onFireButtonClickedEvent.Invoke);
            _shieldButton.onClick.AddListener(onShieldButtonEvent.Invoke);
            _joystickHandler.Initialize(onJoystickInput);
            _scoreText.text = "Score: " + 0;
        }

        public void UpdateScore(int points)
        {
            _score += points;
            _scoreText.text = "Score: " + _score;
        }

        public void Terminate()
        {
            _fireButton.onClick.RemoveAllListeners();
            _shieldButton.onClick.RemoveAllListeners();
        }
    }
}

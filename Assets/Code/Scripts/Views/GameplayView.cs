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
        [SerializeField] TMP_Text _shieldCount;
        [SerializeField] TMP_Text _scoreText;
        [SerializeField] TMP_Text _starCounterText;

        public void Initialize(Action onFireButtonClicked, Action onShieldButtonClicked, Action<float, float> onJoystickInput)
        {
            _fireButton.onClick.AddListener(onFireButtonClicked.Invoke);
            _shieldButton.onClick.AddListener(onShieldButtonClicked.Invoke);
            _joystickHandler.Initialize(onJoystickInput);
        }

        public void OnGameStart(int shieldCount)
        {
            UpdateScore(0);
            UpdateShieldCount(shieldCount);
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = "Score: " + score;
        }

        public void UpdateSoftCurrency(int value)
        {
            _starCounterText.text = value.ToString();
        }

        public void UpdateShieldCount(int value)
        {
            if (value == 0)
            {
                _shieldButton.interactable = false;
            }
            else
            {
                _shieldButton.interactable = true;
            }

            _shieldCount.text = value.ToString();
        }

        public void Terminate()
        {
            _fireButton.onClick.RemoveAllListeners();
            _shieldButton.onClick.RemoveAllListeners();
        }
    }
}

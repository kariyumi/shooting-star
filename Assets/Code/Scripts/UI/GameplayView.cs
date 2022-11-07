using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Scripts.UI
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] JoystickHandler _joystickHandler;
        [SerializeField] Button _fireButton;
        [SerializeField] Button _shieldButton;

        public void Initialize(Action onFireButtonClickedEvent, Action<float, float> onJoystickInput)
        {
            _fireButton.onClick.AddListener(onFireButtonClickedEvent.Invoke);
            //_shieldButton.onClick.AddListener(onShieldButtonEvent.Invoke);
            _joystickHandler.Initialize(onJoystickInput);
        }

        public void Terminate()
        {
            _fireButton.onClick.RemoveAllListeners();
            _shieldButton.onClick.RemoveAllListeners();
        }
    }
}

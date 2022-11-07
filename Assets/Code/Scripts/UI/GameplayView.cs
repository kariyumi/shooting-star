using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayView : MonoBehaviour
{
    [SerializeField] JoystickHandler _joystickHandler;
    [SerializeField] Button _fireButton;
    [SerializeField] Button _multipleFireButton;

    public void Initialize(Action onFireButtonClickedEvent, Action<float, float> onJoystickInput)
    {
        _fireButton.onClick.AddListener(onFireButtonClickedEvent.Invoke);
        //_multipleFireButton.onClick.AddListener(onMultipleFireButtonEvent.Invoke);
        _joystickHandler.Initialize(onJoystickInput);
    }

    public void Terminate()
    {
        _fireButton.onClick.RemoveAllListeners();
        _multipleFireButton.onClick.RemoveAllListeners();
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image _joystick;
    [SerializeField] Image _joystickBackground;

    private Vector2 _inputPosition;
    private Action<float,float> OnJoystickInput;

    public void Initialize(Action<float, float> onJoystickInput)
    {
        OnJoystickInput = onJoystickInput;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        bool isTouchingJoystick = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _joystickBackground.rectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera,
                out _inputPosition);

        if (isTouchingJoystick)
        {
            _inputPosition.x /= _joystickBackground.rectTransform.sizeDelta.x;
            _inputPosition.y /= _joystickBackground.rectTransform.sizeDelta.y;

            if (_inputPosition.magnitude > 1.0f)
            {
                _inputPosition = _inputPosition.normalized;
            }

            _joystick.rectTransform.anchoredPosition = new Vector2(
                _inputPosition.x * _joystickBackground.rectTransform.sizeDelta.x / 4,
                _inputPosition.y * _joystickBackground.rectTransform.sizeDelta.y / 4);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        StartCoroutine(JoystickInput());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputPosition = Vector2.zero;
        _joystick.rectTransform.anchoredPosition = Vector2.zero;
        StopAllCoroutines();
    }

    IEnumerator JoystickInput()
    {
        while (true)
        {
            OnJoystickInput.Invoke(GetHorizontalInput(), GetVerticalInput());
            yield return new WaitForFixedUpdate();
        }
    }

    public float GetHorizontalInput()
    {
        if (_inputPosition.x != 0)
        {
            return _inputPosition.x;
        }

        return Input.GetAxis("Horizontal");
    }

    public float GetVerticalInput()
    {
        if (_inputPosition.y != 0)
        {
            return _inputPosition.y;
        }

        return Input.GetAxis("Vertical");
    }
}

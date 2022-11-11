using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Scripts.PlayFab;
using UnityEngine.Events;
using TMPro;
using System;

namespace Assets.Code.Scripts.Controllers
{
    public class AuthenticationController : MonoBehaviour
    {
        public bool useDevLogin = false;
        public Transform developerLogin;

        public Button signInWithDeviceID;
        public TMP_Text DeviceIdDisplay;

        private Action _onLoginSuccess;

        public void Initialize(Action onLoginSuccess)
        {
            PlayFabAuthentication.OnLoginFail += HandleOnLoginFail;
            PlayFabAuthentication.OnLoginSuccess += HandleOnLoginSuccess;

            _onLoginSuccess = onLoginSuccess;
        }

        void OnDisable()
        {
            PlayFabAuthentication.OnLoginFail -= HandleOnLoginFail;
            PlayFabAuthentication.OnLoginSuccess -= HandleOnLoginSuccess;
        }


        public void Authenticate()
        {
            if (useDevLogin)
            {
                EnableDeveloperMode();
            }
            else if (PlayFabAuthentication.isLoggedOut)
            {
                EnableUserSelectMode();
            }
            else
            {
                EnableAutoMode();

                if (PlayFabAuthentication.CheckForSupportedMobilePlatform())
                {
                    if (PlayFabAuthentication.GetDeviceId())
                    {
                        DeviceIdDisplay.text = PlayFabAuthentication.android_id ?? PlayFabAuthentication.ios_id;
                    }

                    SigninWithDeviceID();
                }
                else
                {
                    //Status.text = "\n Unsupported Platform (Using Facebook in Test Mode)";
                    EnableUserSelectMode();
                }
            }
        }

        public void EnableDeveloperMode()
        {
            DisableUserSelectMode();
            developerLogin.gameObject.SetActive(true);
        }

        public void DisableDeveloperMode()
        {
            developerLogin.gameObject.SetActive(false);
        }

        public void EnableUserSelectMode()
        {
            DisableDeveloperMode();
            signInWithDeviceID.gameObject.SetActive(true);

            signInWithDeviceID.onClick.RemoveAllListeners();
            signInWithDeviceID.onClick.AddListener(() => SigninWithDeviceID(true));
        }

        public void DisableUserSelectMode()
        {
            signInWithDeviceID.gameObject.SetActive(false);
        }

        public void EnableAutoMode()
        {
            DisableDeveloperMode();
            DisableUserSelectMode();
        }

        void SigninWithDeviceID(bool createAccount = false)
        {
            UnityAction accountNotFoundCallback = EnableUserSelectMode;

            PlayFabAuthentication.LoginWithDeviceId(createAccount, accountNotFoundCallback);
        }

        void HandleOnLoginSuccess(string message, MessageDisplayStyle style)
        {
            if (message.Contains("SUCCESS"))
            {
                _onLoginSuccess.Invoke();
            }
        }

        void HandleOnLoginFail(string message, MessageDisplayStyle style)
        {
            Debug.Log(message);
            EnableUserSelectMode();
        }
    }
}

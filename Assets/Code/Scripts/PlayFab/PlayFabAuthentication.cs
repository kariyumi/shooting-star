
using System;
using System.Text.RegularExpressions;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Scripts.PlayFab
{ 
    public static class PlayFabAuthentication
    {
        public static string android_id = string.Empty;
        public static string ios_id = string.Empty;
        public static string custom_id = string.Empty;

        public static bool isLoggedOut = false;

        public delegate void SuccessfulLoginHandler(string details, MessageDisplayStyle style);
        public static event SuccessfulLoginHandler OnLoginSuccess;

        public delegate void FailedLoginHandler(string details, MessageDisplayStyle style);
        public static event FailedLoginHandler OnLoginFail;

        private const string emailPattern = @"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";

        #region PlayFab API calls

        public static void LoginWithDeviceId(bool createAcount, UnityAction errCallback)
        {
            Action<bool> processResponse = (bool response) =>
            {
                if (response && GetDeviceId())
                {
                    if (!string.IsNullOrEmpty(android_id))
                    {
                        Debug.Log("Using Android Device ID: " + android_id);
                        var request = new LoginWithAndroidDeviceIDRequest
                        {
                            AndroidDeviceId = android_id,
                            TitleId = PlayFabSettings.TitleId,
                            CreateAccount = createAcount
                        };

                        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginResult, (PlayFabError error) =>
                        {
                            if (errCallback != null && error.Error == PlayFabErrorCode.AccountNotFound)
                            {
                                errCallback();
                                PlayFabBridge.RaiseCallbackError("Account not found, please select a login method to continue.", PlayFabAPIMethods.GenericLogin, MessageDisplayStyle.none);
                            }
                            else
                            {
                                OnLoginError(error);
                            }

                        });
                    }
                    else if (!string.IsNullOrEmpty(ios_id))
                    {
                        Debug.Log("Using IOS Device ID: " + ios_id);
                        var request = new LoginWithIOSDeviceIDRequest
                        {
                            DeviceId = ios_id,
                            TitleId = PlayFabSettings.TitleId,
                            CreateAccount = createAcount
                        };

                        PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginResult, (PlayFabError error) =>
                        {
                            if (errCallback != null && error.Error == PlayFabErrorCode.AccountNotFound)
                            {
                                errCallback();
                                PlayFabBridge.RaiseCallbackError("Account not found, please select a login method to continue.", PlayFabAPIMethods.GenericLogin, MessageDisplayStyle.none);
                            }
                            else
                            {
                                OnLoginError(error);
                            }
                        });
                    }
                }
                else
                {
                    Debug.Log("Using custom device ID: " + custom_id);
                    var request = new LoginWithCustomIDRequest
                    {
                        CustomId = custom_id,
                        TitleId = PlayFabSettings.TitleId,
                        CreateAccount = createAcount
                    };

                    PlayFabClientAPI.LoginWithCustomID(request, OnLoginResult, error =>
                    {
                        if (errCallback != null && error.Error == PlayFabErrorCode.AccountNotFound)
                        {
                            errCallback();
                            PlayFabBridge.RaiseCallbackError("Account not found, please select a login method to continue.", PlayFabAPIMethods.GenericLogin, MessageDisplayStyle.none);
                        }
                        else
                        {
                            OnLoginError(error);
                        }
                    });
                }
            };

            processResponse(true);
        }

        public static bool GetDeviceId(bool silent = false) // silent suppresses the error
        {
            if (CheckForSupportedMobilePlatform())
            {
#if UNITY_ANDROID
                //http://answers.unity3d.com/questions/430630/how-can-i-get-android-id-.html
                AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
                AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
                android_id = clsSecure.CallStatic<string>("getString", objResolver, "android_id");
#endif

#if UNITY_IPHONE
			ios_id = UnityEngine.iOS.Device.vendorIdentifier;
#endif
                return true;
            }
            else
            {
                custom_id = SystemInfo.deviceUniqueIdentifier;
                return false;
            }
        }

        private static void OnLoginResult(LoginResult result)
        {
            PlayFabPlayerData.PlayerId = result.PlayFabId;

            PlayFabBridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.GenericLogin, MessageDisplayStyle.none);
            if (OnLoginSuccess != null)
                OnLoginSuccess(string.Format("SUCCESS: {0}", result.SessionTicket), MessageDisplayStyle.error);
        }

        private static void OnLoginError(PlayFabError error)
        {
            string errorMessage;
            if (error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Password"))
                errorMessage = "Invalid Password";
            else if (error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Username") || (error.Error == PlayFabErrorCode.InvalidUsername))
                errorMessage = "Invalid Username";
            else if (error.Error == PlayFabErrorCode.AccountNotFound)
                errorMessage = "Account Not Found, you must have a linked PlayFab account. Start by registering a new account or using your device id";
            else if (error.Error == PlayFabErrorCode.AccountBanned)
                errorMessage = "Account Banned";
            else if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
                errorMessage = "Invalid Username or Password";
            else
                errorMessage = string.Format("Error {0}: {1}", error.HttpCode, error.ErrorMessage);

            if (OnLoginFail != null)
                OnLoginFail(errorMessage, MessageDisplayStyle.error);
        }

        public static void RegisterNewPlayfabAccount(string user, string pass1, string pass2, string email)
        {
            if (user.Length == 0 || pass1.Length == 0 || pass2.Length == 0 || email.Length == 0)
            {
                if (OnLoginFail != null)
                    OnLoginFail("All fields are required.", MessageDisplayStyle.error);
                return;
            }

            var passwordCheck = ValidatePassword(pass1, pass2);
            var emailCheck = ValidateEmail(email);

            if (!passwordCheck)
            {
                if (OnLoginFail != null)
                    OnLoginFail("Passwords must match and be longer than 5 characters.", MessageDisplayStyle.error);
                return;
            }
            else if (!emailCheck)
            {
                if (OnLoginFail != null)
                    OnLoginFail("Invalid Email format.", MessageDisplayStyle.error);
                return;
            }
            else
            {
                var request = new RegisterPlayFabUserRequest
                {
                    TitleId = PlayFabSettings.TitleId,
                    Username = user,
                    Email = email,
                    Password = pass1
                };

                PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterResult, OnLoginError);
            }
        }

        public static void LoginWithUsername(string user, string password)
        {
            if (user.Length > 0 && password.Length > 0)
            {
                var request = new LoginWithPlayFabRequest
                {
                    Username = user,
                    Password = password,
                    TitleId = PlayFabSettings.TitleId
                };

                PlayFabClientAPI.LoginWithPlayFab(request, OnLoginResult, OnLoginError);
            }
            else
            {
                if (OnLoginFail != null)
                    OnLoginFail("User Name and Password cannot be blank.", MessageDisplayStyle.error);
            }
        }

        public static void LoginWithEmail(string user, string password)
        {
            if (user.Length > 0 && password.Length > 0 && ValidateEmail(user))
            {
                //LoginMethodUsed = LoginPathways.pf_email;
                var request = new LoginWithEmailAddressRequest
                {
                    Email = user,
                    Password = password,
                    TitleId = PlayFabSettings.TitleId
                };

                PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginResult, OnLoginError);

            }
            else
            {
                if (OnLoginFail != null)
                    OnLoginFail("Username or Password is invalid. Check credentails and try again", MessageDisplayStyle.error);
            }
        }

        public static bool CheckForSupportedMobilePlatform()
        {
            return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        }

        #endregion

        #region pf_callbacks

        private static void OnRegisterResult(RegisterPlayFabUserResult result)
        {
            if (OnLoginSuccess != null)
                OnLoginSuccess("New Account Registered", MessageDisplayStyle.none);
        }

        #endregion

        #region helperfunctions

        public static bool ValidateEmail(string em)
        {
            return Regex.IsMatch(em, emailPattern);
        }

        public static bool ValidatePassword(string p1, string p2)
        {
            return ((p1 == p2) && p1.Length > 5);
        }

        #endregion
    }
}
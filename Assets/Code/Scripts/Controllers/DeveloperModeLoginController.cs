using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Scripts.PlayFab;
using TMPro;

namespace Assets.Code.Scripts.Controllers
{
    public class DeveloperModeLoginController : MonoBehaviour
    {
        [SerializeField] Transform LoginGroup;
        [SerializeField] TMP_InputField User;
        [SerializeField] TMP_InputField Password;
        [SerializeField] Button Login;
        [SerializeField] Button CreateAccountButton;

        [SerializeField] Transform RegisterGroup;
        [SerializeField] TMP_InputField UserId;
        [SerializeField] TMP_InputField Email;
        [SerializeField] TMP_InputField PasswordInput;
        [SerializeField] TMP_InputField PasswordConfirmation;
        [SerializeField] Button Register;
        [SerializeField] Button MakeLoginButton;

        void Start()
        {
            Login.onClick.AddListener(() => LogIn());
            Register.onClick.AddListener(() => RegisterNewAccount());
            CreateAccountButton.onClick.AddListener(() => CreateAccountToggle());
            MakeLoginButton.onClick.AddListener(() => MakeLoginToggle());
        }

        public void LogIn()
        {
            if (User.text.Contains("@"))
            {
                if (PlayFabAuthentication.ValidateEmail(User.text))
                {
                    PlayFabAuthentication.LoginWithEmail(User.text, Password.text);
                }
            }
            else
            {
                PlayFabAuthentication.LoginWithUsername(User.text, Password.text);
            }
        }

        public void CreateAccountToggle()
        {
            LoginGroup.gameObject.SetActive(false);
            RegisterGroup.gameObject.SetActive(true);
        }

        public void MakeLoginToggle()
        {
            LoginGroup.gameObject.SetActive(true);
            RegisterGroup.gameObject.SetActive(false);
        }

        public void RegisterNewAccount()
        {
            PlayFabAuthentication.RegisterNewPlayfabAccount(UserId.text, Password.text, PasswordConfirmation.text, Email.text);
        }
    }
}

using Assets.Code.Scripts.Views;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] AuthenticationController _authenticationController;

        [SerializeField] PlayerController _playerController;
        [SerializeField] EnemySpawnController _enemySpawnController;
        [SerializeField] StarSpawnController _starSpawnController;

        [SerializeField] GameplayView _gameplayView;
        [SerializeField] MainMenuView _mainMenuView;
        [SerializeField] GameOverMenuView _gameOverMenuView;
        [SerializeField] StarGarageView _starGarageView;

        [SerializeField] CurrencyModel _currencyModel;
        [SerializeField] ScoreModel _scoreModel;
        [SerializeField] ShieldModel _shieldModel;

        private void Awake()
        {
            _authenticationController.Initialize(MainMenu);

            _gameplayView.Initialize(_playerController.FireLaser, UseShield, _playerController.MovePlayer);
            _mainMenuView.Initialize(StartGame, GarageMenu, LeaderboardMenu);
            _gameOverMenuView.Initialize(StartGame, GarageMenu, LeaderboardMenu);
            _starGarageView.Initialize(BuySoftCurrency, BuyHardCurrency, BuyShield, AccelerateShield, MainMenu);

            _enemySpawnController.Initialize(UpdateScore);
            _starSpawnController.Initialize(UpdateSoftCurrency);
            _playerController.Initialize(EndGame);

            _scoreModel.Initialize();
            _currencyModel.Initialize();
            _shieldModel.Initialize();

            _authenticationController.Authenticate();
        }

        private void StartGame()
        {
            _mainMenuView.gameObject.SetActive(false);
            _gameplayView.gameObject.SetActive(true);
            _gameOverMenuView.gameObject.SetActive(false);
            _starGarageView.gameObject.SetActive(false);

            _enemySpawnController.OnStartGame();
            _starSpawnController.OnStartGame();
            _playerController.OnGameStart();
            _gameplayView.OnGameStart(_shieldModel.ShieldCounter);
        }

        private void EndGame()
        {
            _enemySpawnController.OnGameOver();
            _starSpawnController.OnGameOver();
            _gameOverMenuView.OnGameOver(_scoreModel.Score);
            _scoreModel.ClearScore();

            _gameOverMenuView.gameObject.SetActive(true);
            _mainMenuView.gameObject.SetActive(false);
            _gameplayView.gameObject.SetActive(false);
            _starGarageView.gameObject.SetActive(false);
        }

        private void MainMenu()
        {
            _authenticationController.gameObject.SetActive(false);
            _mainMenuView.gameObject.SetActive(true);
            _gameplayView.gameObject.SetActive(false);
            _gameOverMenuView.gameObject.SetActive(false);
            _starGarageView.gameObject.SetActive(false);
        }

        private void GarageMenu()
        {
            _mainMenuView.gameObject.SetActive(false);
            _gameplayView.gameObject.SetActive(false);
            _gameOverMenuView.gameObject.SetActive(false);
            _starGarageView.gameObject.SetActive(true);

            _starGarageView.OnActive(
                _currencyModel.SoftCurrencyCounter,
                _currencyModel.HardCurrencyCounter,
                _shieldModel.ShieldCounter,
                _shieldModel.Timer
                );
        }

        private void LeaderboardMenu()
        {

        }

        private void UpdateScore(int value)
        {
            _scoreModel.UpdateScore(value);
            _gameplayView.UpdateScore(_scoreModel.Score);
        }

        private void UpdateSoftCurrency(int value)
        {
            _currencyModel.UpdateSoftCurrency(value);
            _gameplayView.UpdateSoftCurrency(_currencyModel.SoftCurrencyCounter);
        }

        private void BuySoftCurrency()
        {
            _currencyModel.UpdateSoftCurrency(10);
            _starGarageView.UpdateSoftCurrecy(_currencyModel.SoftCurrencyCounter);
        }

        private void BuyHardCurrency()
        {
            _currencyModel.UpdateHardCurrency(10);
            _starGarageView.UpdateHardCurrecy(_currencyModel.HardCurrencyCounter);
        }

        public void BuyShield()
        {
            if (_currencyModel.SoftCurrencyCounter / _shieldModel.ShieldPrice < 1)
            {
                return;
            }

            _currencyModel.UpdateSoftCurrency(-_shieldModel.ShieldPrice);
            _shieldModel.OnBuyShield();

            _starGarageView.OnBuyShield(_currencyModel.SoftCurrencyCounter, _shieldModel.Timer);
        }

        public void UseShield()
        {
            _playerController.ActivateShield();
            _shieldModel.OnShieldUsed();
            _gameplayView.UpdateShieldCount(_shieldModel.ShieldCounter);
        }

        public void AccelerateShield()
        {
            if (_currencyModel.HardCurrencyCounter / _shieldModel.AccelerationPrice < 1 ||
                _shieldModel.Timer == System.TimeSpan.Zero)
            {
                return;
            }

            _currencyModel.UpdateHardCurrency(-_shieldModel.AccelerationPrice);
            _shieldModel.OnBuyAcceleration();

            _starGarageView.OnAccelerateShield(_currencyModel.HardCurrencyCounter, _shieldModel.Timer);
        }


    }
}
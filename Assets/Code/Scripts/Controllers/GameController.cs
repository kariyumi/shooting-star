using System;
using Assets.Code.Scripts.PlayFab;
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
        [SerializeField] LeaderboardView _leaderboardView;

        [SerializeField] CurrencyModel _currencyModel;
        [SerializeField] ScoreModel _scoreModel;
        [SerializeField] ShieldModel _shieldModel;

        private void Awake()
        {
            _authenticationController.Initialize(GetPlayFabPlayerData);

            _mainMenuView.Initialize(StartGame, GarageMenu, LeaderboardMenu);
            _gameplayView.Initialize(_playerController.FireLaser, UseShield, _playerController.MovePlayer);
            _starGarageView.Initialize(AddSoftCurrency, AddHardCurrency, BuyShield, AccelerateShield, MainMenu);
            _gameOverMenuView.Initialize(StartGame, GarageMenu, LeaderboardMenu);
            _leaderboardView.Initialize(MainMenu);

            _enemySpawnController.Initialize(UpdateScore);
            _starSpawnController.Initialize(CountDespawnedStar);
            _playerController.Initialize(EndGame);

            Authentication();
        }

        private void GetPlayFabPlayerData()
        {
            PlayFabPlayerData.GetUserInventory(() =>
            {
                _currencyModel.SoftCurrencyCounter = PlayFabPlayerData.GetCurrencyAmount(PlayFabPlayerData.SOFT_CURRENCY_KEY);
                _currencyModel.HardCurrencyCounter = PlayFabPlayerData.GetCurrencyAmount(PlayFabPlayerData.HARD_CURRENCY_KEY);
                _shieldModel.ShieldCounter = PlayFabPlayerData.GetItemAmount(PlayFabPlayerData.SHIELD_ID);
            });


            PlayFabPlayerData.GetUserData(() =>
            {
                string lastShieldRetrieve = PlayFabPlayerData.GetUserData(PlayFabPlayerData.LAST_SHIELD_RETRIEVED_KEY);
                _shieldModel.ShieldRetrieved = lastShieldRetrieve == "" ? true : bool.Parse(lastShieldRetrieve);
                string lastShieldBuy = PlayFabPlayerData.GetUserData(PlayFabPlayerData.LAST_SHIELD_BUY_KEY);
                _shieldModel.ReadyTime = lastShieldBuy == "" ? DateTime.Now : Convert.ToDateTime(lastShieldBuy);
            });

            MainMenu();
        }

        private void Authentication()
        {
            SetViewActive(Views.Authentication);
            _authenticationController.Authenticate();
        }

        private void MainMenu()
        {
            SetViewActive(Views.MainMenu);
        }

        private void GamePlay()
        {
            SetViewActive(Views.GamePlay);
        }

        private void GarageMenu()
        {
            SetViewActive(Views.StarGarage);

            _starGarageView.OnActive(
                _currencyModel.SoftCurrencyCounter,
                _currencyModel.HardCurrencyCounter,
                _shieldModel.ShieldCounter,
                _shieldModel.Timer
                );
        }

        private void LeaderboardMenu()
        {
            PlayFabPlayerData.RequestLeaderBoard(() => _leaderboardView.GenerateLeaderboardTable(PlayFabPlayerData.GetLeaderboard()));
            SetViewActive(Views.Leaderboard);
        }

        private void GameOver()
        {
            SetViewActive(Views.GameOver);
        }

        private void StartGame()
        {
            GamePlay();

            _enemySpawnController.OnStartGame();
            _starSpawnController.OnStartGame();
            _playerController.OnGameStart();
            _gameplayView.OnGameStart(_shieldModel.ShieldCounter);
        }

        private void EndGame()
        {
            _currencyModel.AddStar(_starSpawnController.StarDespawnedCounter);
            PlayFabPlayerData.SubmitScore(_scoreModel.Score);

            _enemySpawnController.OnGameOver();
            _starSpawnController.OnGameOver();
            _gameOverMenuView.OnGameOver(_scoreModel.Score);
            _scoreModel.ClearScore();

            GameOver();
        }

        private void UpdateScore(int value)
        {
            _scoreModel.UpdateScore(value);
            _gameplayView.UpdateScore(_scoreModel.Score);
        }

        private void AddSoftCurrency()
        {
            _currencyModel.AddStar(10, () => _starGarageView.UpdateSoftCurrecy(_currencyModel.SoftCurrencyCounter));
        }

        private void AddHardCurrency()
        {
            _currencyModel.AddRedStar(10, () => _starGarageView.UpdateHardCurrecy(_currencyModel.HardCurrencyCounter));
        }

        public void BuyShield()
        {
            if (_currencyModel.SoftCurrencyCounter / _shieldModel.ShieldPrice < 1 ||
                _shieldModel.Timer != TimeSpan.Zero)
            {
                return;
            }

            _currencyModel.SubtractStar(_shieldModel.ShieldPrice, () =>
                _starGarageView.OnBuyShield(_currencyModel.SoftCurrencyCounter, _shieldModel.Timer));
            _shieldModel.OnBuyShield();
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
                _shieldModel.Timer == TimeSpan.Zero)
            {
                return;
            }

            _currencyModel.SubtractRedStar(_shieldModel.AccelerationPrice, () =>
                _starGarageView.OnAccelerateShield(_currencyModel.HardCurrencyCounter, _shieldModel.Timer));
            _shieldModel.OnBuyAcceleration();
        }

        public void CountDespawnedStar(int value)
        {
            _starSpawnController.CountDespawnedStar(value);
            _gameplayView.UpdateSoftCurrency(_starSpawnController.StarDespawnedCounter);
        }

        public void SetViewActive(Views view)
        {
            SetAllViewsInactive();

            switch (view)
            {
                case Views.Authentication:
                    _authenticationController.gameObject.SetActive(true);
                    break;
                case Views.MainMenu:
                    _mainMenuView.gameObject.SetActive(true);
                    break;
                case Views.GamePlay:
                    _gameplayView.gameObject.SetActive(true);
                    break;
                case Views.GameOver:
                    _gameOverMenuView.gameObject.SetActive(true);
                    break;
                case Views.StarGarage:
                    _starGarageView.gameObject.SetActive(true);
                    break;
                case Views.Leaderboard:
                    _leaderboardView.gameObject.SetActive(true);
                    break;
            }
        }

        public void SetAllViewsInactive()
        {
            _authenticationController.gameObject.SetActive(false);
            _mainMenuView.gameObject.SetActive(false);
            _gameplayView.gameObject.SetActive(false);
            _gameOverMenuView.gameObject.SetActive(false);
            _starGarageView.gameObject.SetActive(false);
            _leaderboardView.gameObject.SetActive(false);
        }
    }

    public enum Views
    {
        Authentication,
        MainMenu,
        GamePlay,
        GameOver,
        StarGarage,
        Leaderboard
    }
}
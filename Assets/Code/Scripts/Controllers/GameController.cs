using Assets.Code.Scripts.Views;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] PlayerController _playerController;
        [SerializeField] EnemySpawnController _enemySpawnController;
        [SerializeField] StarSpawnController _starSpawnController;

        [SerializeField] GameplayView _gameplayView;
        [SerializeField] MainMenuView _mainMenuView;
        [SerializeField] GameOverMenuView _gameOverMenuView;

        [SerializeField] CurrencyModel _currencyModel;
        [SerializeField] ScoreModel _scoreModel;

        private void Awake()
        {
            _gameplayView.Initialize(_playerController.FireLaser, _playerController.ActivateShield, _playerController.MovePlayer);
            _mainMenuView.Initialize(StartGame, Garage, Leaderboard);
            _gameOverMenuView.Initialize(StartGame, Garage, Leaderboard);

            _enemySpawnController.Initialize(UpdateScore);
            _starSpawnController.Initialize(UpdateSoftCurrency);
            _playerController.Initialize(EndGame);

            _scoreModel.Initialize();
            _currencyModel.Initialize();

            MainMenu();
        }

        private void StartGame()
        {
            _mainMenuView.gameObject.SetActive(false);
            _gameplayView.gameObject.SetActive(true);
            _gameOverMenuView.gameObject.SetActive(false);

            _enemySpawnController.OnStartGame();
            _starSpawnController.OnStartGame();
            _playerController.OnGameStart();
            _gameplayView.OnGameStart();
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
        }

        private void MainMenu()
        {
            _mainMenuView.gameObject.SetActive(true);
            _gameplayView.gameObject.SetActive(false);
            _gameOverMenuView.gameObject.SetActive(false);
        }

        private void Garage()
        {

        }

        private void Leaderboard()
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
    }
}
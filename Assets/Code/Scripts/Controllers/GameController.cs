using Assets.Code.Scripts.UI;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] PlayerController _playerController;
        [SerializeField] SpawnController _spawnController;

        [SerializeField] GameplayView _gameplayView;
        [SerializeField] MainMenuView _mainMenuView;
        [SerializeField] GameOverMenuView _gameOverMenuView;

        private void Awake()
        {
            _gameplayView.Initialize(_playerController.FireLaser, _playerController.ActivateShield, _playerController.MovePlayer);
            _mainMenuView.Initialize(StartGame, Garage, Leaderboard);
            _gameOverMenuView.Initialize(StartGame, Garage, Leaderboard);

            _spawnController.Initialize(_gameplayView.UpdateScore);
            _playerController.Initialize(EndGame);

            MainMenu();
        }

        private void StartGame()
        {
            _mainMenuView.gameObject.SetActive(false);
            _gameplayView.gameObject.SetActive(true);
            _gameOverMenuView.gameObject.SetActive(false);

            _spawnController.OnStartGame();
            _playerController.OnGameStart();
            _gameplayView.OnGameStart();
        }

        private void EndGame()
        {
            _spawnController.OnGameOver();
            _gameOverMenuView.OnGameOver(_gameplayView.Score);

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

        
    }
}
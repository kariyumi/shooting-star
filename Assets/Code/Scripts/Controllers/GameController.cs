using Assets.Code.Scripts.UI;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] PlayerController _playerController;
        [SerializeField] SpawnController _spawnController;

        [SerializeField] GameplayView _gameplayView;

        private void Start()
        {
            _gameplayView.Initialize(_playerController.FireLaser, _playerController.ActivateShield, _playerController.MovePlayer);
            _spawnController.Initialize(_gameplayView.UpdateScore);
            _playerController.Initialize(EndGame);
        }

        private void EndGame()
        {
            _spawnController.OnEndGame();
        }
    }
}
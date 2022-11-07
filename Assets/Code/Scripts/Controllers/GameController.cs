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
            _gameplayView.Initialize(_playerController.OnFire, _playerController.MovePlayer);
            _playerController.Initialize(EndGame);
        }

        private void EndGame()
        {
            _spawnController.OnEndGame();
        }
    }
}
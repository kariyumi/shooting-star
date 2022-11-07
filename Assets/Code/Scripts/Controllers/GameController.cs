using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] PlayerController _playerController;

        [SerializeField] GameplayView _gameplayView;

        private void Start()
        {
            _gameplayView.Initialize(_playerController.OnFire, _playerController.MovePlayer);
        }
    }
}
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] JoystickHandler _joystickHandler;
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] float _speed = 10f;
        [SerializeField] GameObject _laser;
        [SerializeField] float _laserVerticalOffset = 1.0f;

        public void MovePlayer(float horizontalInput, float verticalInput)
        {
            Vector2 directionInput = new Vector2(horizontalInput, verticalInput);

            transform.Translate(_speed * Time.deltaTime * directionInput);
            BoundariesClamp();
        }

        private void BoundariesClamp()
        {
            float playerHalfWidth = _spriteRenderer.bounds.size.x / 2;
            float playerHalfHeight = _spriteRenderer.bounds.size.y / 2;

            float horizontalLimit = Camera.main.orthographicSize * Screen.width / Screen.height - playerHalfWidth;
            float verticalLimit = Camera.main.orthographicSize - playerHalfHeight;

            float horizontalClamp = Mathf.Clamp(transform.position.x, -horizontalLimit, horizontalLimit);
            float verticalClamp = Mathf.Clamp(transform.position.y, -verticalLimit, verticalLimit);

            transform.position = new Vector2(horizontalClamp, verticalClamp);
        }

        public void OnFire()
        {
            Vector3 laserOffset = new Vector2(0, _laserVerticalOffset);
            Instantiate(_laser, transform.position + laserOffset, Quaternion.identity);
        }
    }
}


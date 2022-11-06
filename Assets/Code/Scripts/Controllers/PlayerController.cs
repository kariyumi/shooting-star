using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] JoystickHandler _joystickHandler;
        [SerializeField] SpriteRenderer _spriteRenderer;

        public float Speed = 10f;

        void Update()
        {
            MovePlayer();
            BoundariesClamp();
        }

        private void MovePlayer()
        {
            float horizontalInput = _joystickHandler.GetHorizontalInput();
            float verticalInput = _joystickHandler.GetVerticalInput();

            Vector2 directionInput = new Vector2(horizontalInput, verticalInput);

            transform.Translate(directionInput * Speed * Time.deltaTime);
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
    }
}


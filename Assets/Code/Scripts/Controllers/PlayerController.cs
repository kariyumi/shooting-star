using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {

        public float Speed = 10f;

        void Update()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector2 directionInput = new Vector2(horizontalInput, verticalInput);

            transform.Translate(directionInput * Speed * Time.deltaTime);
        }
    }
}


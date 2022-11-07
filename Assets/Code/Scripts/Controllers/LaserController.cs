using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class LaserController : MonoBehaviour
    {
        public float Speed = 6f;

        private const string ENEMY_TAG = "Enemy";

        private void Update()
        {
            transform.Translate(Speed * Time.deltaTime * Vector3.up);

            if (transform.position.y >= Camera.main.orthographicSize)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(ENEMY_TAG))
            {
                Destroy(gameObject);
            }
        }
    }
}

using System;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] float _speed = 4.6f;
        [SerializeField] int _points = 100;

        private float enemyHalfWidth;
        private float enemyHalfHeight;
        private float horizontalLimit;
        private float verticalLimit;

        const string PLAYER_TAG = "Player";
        const string LASER_TAG = "Laser";

        public Action<int> OnEnemyDespawn;

        private void Start()
        {
            enemyHalfWidth = _spriteRenderer.bounds.size.x / 2;
            enemyHalfHeight = _spriteRenderer.bounds.size.y / 2;
            horizontalLimit = Camera.main.orthographicSize * Screen.width / Screen.height - enemyHalfWidth;
            verticalLimit = Camera.main.orthographicSize;
        }

        private void Update()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.down);

            if (transform.position.y <= -Camera.main.orthographicSize)
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            float randomHorizontalSpawnPosition = UnityEngine.Random.Range(-horizontalLimit, horizontalLimit);
            float verticalSpawnPosition = verticalLimit + enemyHalfHeight;

            transform.position = new Vector2(randomHorizontalSpawnPosition, verticalSpawnPosition);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(PLAYER_TAG) || collision.CompareTag(LASER_TAG))
            {
                OnEnemyDespawn.Invoke(_points);
                Destroy(gameObject);
            }
        }
    }
}
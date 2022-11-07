using UnityEngine;
using Assets.Code.Scripts.Controllers.Interface;
using System;

namespace Assets.Code.Scripts.Controllers.Abstract
{
    public abstract class AObstacleController : MonoBehaviour, IOnTriggerEnter2D
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] float _speed = 4.6f;

        protected float enemyHalfWidth;
        protected float enemyHalfHeight;
        protected float horizontalLimit;
        protected float verticalLimit;

        public Action<int> OnDespawn;

        protected virtual void Start()
        {
            enemyHalfWidth = _spriteRenderer.bounds.size.x / 2;
            enemyHalfHeight = _spriteRenderer.bounds.size.y / 2;
            horizontalLimit = Camera.main.orthographicSize * Screen.width / Screen.height - enemyHalfWidth;
            verticalLimit = Camera.main.orthographicSize;
        }

        protected virtual void Update()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.down);

            if (transform.position.y <= -Camera.main.orthographicSize)
            {
                Respawn();
            }
        }

        protected virtual void Respawn()
        {
            float randomHorizontalSpawnPosition = UnityEngine.Random.Range(-horizontalLimit, horizontalLimit);
            float verticalSpawnPosition = verticalLimit + enemyHalfHeight;

            transform.position = new Vector2(randomHorizontalSpawnPosition, verticalSpawnPosition);
        }

        public abstract void OnTriggerEnter2D(Collider2D collision);
    }
}
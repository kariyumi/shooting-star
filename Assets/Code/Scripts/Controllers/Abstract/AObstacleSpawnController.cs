using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers.Abstract
{
    public abstract class AObstacleSpawnController : MonoBehaviour
    {
        [SerializeField] GameObject _obstaclePrefab;
        [SerializeField] GameObject _obstacleContainer;

        protected float _xSpawnRange = 9f;
        protected float _ySpawn = -8f;
        protected float _spawnRate = 10f;

        protected Action<int> _onDespawn;

        public virtual void Initialize(Action<int> onDespawn)
        {
            _onDespawn = onDespawn;
        }

        public virtual void OnStartGame()
        {
            StartCoroutine(SpawnStarCorroutine());
        }

        public virtual void OnGameOver()
        {
            StopAllCoroutines();
            CleanContainer();
        }

        private void CleanContainer()
        {
            int childCount = _obstacleContainer.transform.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(_obstacleContainer.transform.GetChild(i).gameObject);
            }
        }

        protected virtual IEnumerator SpawnStarCorroutine()
        {
            while (true)
            {
                float randomXSpawn = UnityEngine.Random.Range(-_xSpawnRange, _xSpawnRange);
                GameObject obstacle = Instantiate(_obstaclePrefab, new Vector2(randomXSpawn, _ySpawn), Quaternion.identity);
                obstacle.transform.parent = _obstacleContainer.transform;
                obstacle.GetComponent<AObstacleController>().OnDespawn = _onDespawn;

                yield return new WaitForSeconds(_spawnRate);
            }
        }
    }
}

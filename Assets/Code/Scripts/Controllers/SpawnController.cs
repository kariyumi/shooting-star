using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] GameObject _enemyPrefab;
        [SerializeField] GameObject _enemyContainer;

        private float _xSpawnRange = 9f;
        private float _ySpawn = -8f;
        private float _spawnRate = 5f;

        private Action<int> _onEnemyDespawn;

        public void Initialize(Action<int> onEnemyDespawn)
        {
            _onEnemyDespawn = onEnemyDespawn;
        }

        public void OnStartGame()
        {
            StartCoroutine(SpawnCorroutine());
        }

        public void OnGameOver()
        {
            StopAllCoroutines();
        }

        IEnumerator SpawnCorroutine()
        {
            while (true)
            {
                float randomXSpawn = UnityEngine.Random.Range(-_xSpawnRange, _xSpawnRange);
                GameObject enemy = Instantiate(_enemyPrefab, new Vector2(randomXSpawn, _ySpawn), Quaternion.identity);
                enemy.transform.parent = _enemyContainer.transform;
                enemy.GetComponent<EnemyController>().OnEnemyDespawn = _onEnemyDespawn;

                yield return new WaitForSeconds(_spawnRate);
            }
        }
    }
}

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

        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(SpawnCorroutine());
        }

        // Update is called once per frame
        public void OnEndGame()
        {
            StopAllCoroutines();
        }

        IEnumerator SpawnCorroutine()
        {
            while (true)
            {
                float randomXSpawn = Random.Range(-_xSpawnRange, _xSpawnRange);
                GameObject enemy = Instantiate(_enemyPrefab, new Vector2(randomXSpawn, _ySpawn), Quaternion.identity);
                enemy.transform.parent = _enemyContainer.transform;

                yield return new WaitForSeconds(_spawnRate);
            }
        }
    }
}

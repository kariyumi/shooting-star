using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;

        [SerializeField] float _speed = 10f;
        [SerializeField] int _lives = 1;
        [SerializeField] float _shieldDuration = 5f;

        [SerializeField] GameObject _laser;
        [SerializeField] float _laserVerticalOffset = 1.0f;
        [SerializeField] float _fireRate = 0.3f;

        private float _whenCanFire = -1f;
        private Action _onPlayerDeath;
        private bool _isShieldActive = false;

        private const string ENEMY_TAG = "Enemy";

        public void Initialize(Action onPlayerDeath)
        {
            _onPlayerDeath = onPlayerDeath;
        }

        public void OnGameStart()
        {
            gameObject.SetActive(true);
        }

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

        public void FireLaser()
        {
            if (Time.time < _whenCanFire)
            {
                return;
            }
            
            _whenCanFire = Time.time + _fireRate;
            
            Vector3 laserOffset = new Vector2(0, _laserVerticalOffset);
            Instantiate(_laser, transform.position + laserOffset, Quaternion.identity);
        }

        public void ActivateShield()
        {
            _spriteRenderer.color = Color.red;
            _isShieldActive = true;
            StartCoroutine(DeactivateShieldAfter(_shieldDuration));
        }

        IEnumerator DeactivateShieldAfter(float seconds)
        {
            yield return new WaitForSecondsRealtime(seconds);

            _spriteRenderer.color = Color.white;
            _isShieldActive = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(ENEMY_TAG))
            {
                TakeDamage();
            }
        }

        public void TakeDamage()
        {
            if (_isShieldActive) return;

            _lives--;

            if (_lives <= 0)
            {
                gameObject.SetActive(false);
                _onPlayerDeath.Invoke();
            }
        }
    }
}


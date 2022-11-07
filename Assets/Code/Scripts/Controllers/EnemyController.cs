using System;
using Assets.Code.Scripts.Controllers.Abstract;
using UnityEngine;

namespace Assets.Code.Scripts.Controllers
{
    public class EnemyController : AObstacleController
    {
        [SerializeField] int _points = 100;

        const string PLAYER_TAG = "Player";
        const string LASER_TAG = "Laser";

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(PLAYER_TAG) || collision.CompareTag(LASER_TAG))
            {
                OnDespawn.Invoke(_points);
                Destroy(gameObject);
            }
        }
    }
}
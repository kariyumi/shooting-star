using UnityEngine;
using Assets.Code.Scripts.Controllers.Abstract;

namespace Assets.Code.Scripts.Controllers
{
    public class StarController : AObstacleController
    {
        const string PLAYER_TAG = "Player";

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(PLAYER_TAG))
            {
                OnDespawn.Invoke(1);
                Destroy(gameObject);
            }
        }
    }
}
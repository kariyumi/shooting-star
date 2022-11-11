using System;
using Assets.Code.Scripts.Controllers.Abstract;

namespace Assets.Code.Scripts.Controllers
{
    public class StarSpawnController : AObstacleSpawnController
    {
        public int StarDespawnedCounter;

        public override void OnGameOver()
        {
            base.OnGameOver();

            ClearStarDespawnedCounter();
        }

        public void CountDespawnedStar(int value)
        {
            StarDespawnedCounter += value;
        }

        public void ClearStarDespawnedCounter()
        {
            StarDespawnedCounter = 0;
        }
    }
}

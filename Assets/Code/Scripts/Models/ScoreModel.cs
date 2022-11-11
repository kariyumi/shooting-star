using UnityEngine;

namespace Assets.Code.Scripts.Models
{
    public class ScoreModel : MonoBehaviour
    {
        public int Score { get; private set; }

        public void UpdateScore(int value)
        {
            Score += value;
        }

        public void ClearScore()
        {
            Score = 0;
        }
    }
}
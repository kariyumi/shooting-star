using UnityEngine;

public class ScoreModel : MonoBehaviour
{
    public int Score { get; private set; }

    public void Initialize()
    {
        Score = 0;
    }

    public void UpdateScore(int value)
    {
        Score += value;
    }

    public void ClearScore()
    {
        Score = 0;
    }
}
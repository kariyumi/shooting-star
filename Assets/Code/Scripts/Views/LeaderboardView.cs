using System;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Scripts.Views
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] Transform _leaderboardContainer;
        [SerializeField] Transform _leaderboardEntryTemplate;
        [SerializeField] Button _returnButton;

        private const float TEMPLATE_HEIGHT = 100f;

        public void Initialize(Action onReturnButtonClicked)
        {
            _returnButton.onClick.AddListener(onReturnButtonClicked.Invoke);
        }

        public void GenerateLeaderboardTable(List<PlayerLeaderboardEntry> leaderboard)
        {
            if (leaderboard.Count == 0)
            {
                return;
            }

            for (int i = 0; i < leaderboard.Count; i++)
            {
                Transform entry = Instantiate(_leaderboardEntryTemplate, _leaderboardContainer);
                RectTransform entryRectTransform = entry.GetComponent<RectTransform>();
                entryRectTransform.anchoredPosition = new Vector2(0, - TEMPLATE_HEIGHT * i);
                entry.gameObject.SetActive(true);

                var leaderboardEntry = leaderboard[i];

                entry.Find("PositionText").GetComponent<TMP_Text>().text = (leaderboardEntry.Position + 1).ToString();
                entry.Find("NameText").GetComponent<TMP_Text>().text = leaderboardEntry.PlayFabId.ToString();
                entry.Find("PointsText").GetComponent<TMP_Text>().text = leaderboardEntry.StatValue.ToString();
            }
        }

        public void OnDestroy()
        {
            _returnButton.onClick.RemoveAllListeners();
        }
    }
}

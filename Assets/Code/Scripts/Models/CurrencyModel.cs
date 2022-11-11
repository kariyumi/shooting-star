using UnityEngine;
using Assets.Code.Scripts.PlayFab;
using System;

namespace Assets.Code.Scripts.Models
{
    public class CurrencyModel : MonoBehaviour
    {
        public int SoftCurrencyCounter;
        public int HardCurrencyCounter;

        public void AddStar(int value, Action callback = null)
        {
            PlayFabPlayerData.AddStarForPlayer(value, callback);
            SoftCurrencyCounter += value;
        }

        public void AddRedStar(int value, Action callback = null)
        {
            PlayFabPlayerData.AddRedStarForPlayer(value, callback);
            HardCurrencyCounter += value;
        }

        public void SubtractStar(int value, Action callback = null)
        {
            PlayFabPlayerData.SubtractStarFromPlayer(value, callback);
            SoftCurrencyCounter -= value;
        }

        public void SubtractRedStar(int value, Action callback = null)
        {
            PlayFabPlayerData.SubtractRedStarFromPlayer(value, callback);
            HardCurrencyCounter -= value;
        }
    }
}
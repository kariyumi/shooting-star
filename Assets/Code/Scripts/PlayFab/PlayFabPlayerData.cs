using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;

namespace Assets.Code.Scripts.PlayFab
{
    public static class PlayFabPlayerData
    {
        public static string PlayerId = string.Empty;

        private static List<string> _playerDataKeys = new List<string>() { LAST_SHIELD_BUY_KEY, LAST_SHIELD_RETRIEVED_KEY };
        private static Dictionary<string, string> _playerData = new Dictionary<string, string>();
        private static Dictionary<string, int> _virtualCurrency = new Dictionary<string, int>();
        private static List<ItemInstance> _playerInventory = new List<ItemInstance>();
        private static List<PlayerLeaderboardEntry> _leaderboard = new List<PlayerLeaderboardEntry>();

        public const string LAST_SHIELD_BUY_KEY = "LastShieldBuy";
        public const string LAST_SHIELD_RETRIEVED_KEY = "LastShieldRetrieved";
        public const string SOFT_CURRENCY_KEY = "SC";
        public const string HARD_CURRENCY_KEY = "HC";
        public const string SHIELD_ID = "Shield";
        public const string SHIELD_INSTANCE_ID = "826776869DE7E2F1";
        public const string STORE_ID = "Powers";
        public const string LEADERBOARD_ID = "HighScore";

        #region UserData
        public static void GetUserData(Action callback = null)
        {
            var request = new GetUserDataRequest
            {
                Keys = _playerDataKeys,
                PlayFabId = PlayerId,
            };

            PlayFabClientAPI.GetUserData(request, result =>
           {
               foreach (var data in result.Data)
               {
                   _playerData.Add(data.Key, data.Value.Value);
               }

               callback?.Invoke();

               PlayFabBridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.GetUserData, MessageDisplayStyle.none);
           }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static void UpdateUserData(Dictionary<string, string> updates, string permission = "Public", UnityAction<UpdateUserDataResult> callback = null)
        {
            var request = new UpdateUserDataRequest
            {
                Data = updates,
                Permission = (UserDataPermission)Enum.Parse(typeof(UserDataPermission), permission),
            };

            PlayFabClientAPI.UpdateUserData(request, result =>
            {
                if (callback != null)
                    callback(result);
                PlayFabBridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.UpdateUserData, MessageDisplayStyle.none);
            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static string GetUserData(string key)
        {
            var output = "";
            foreach (var data in _playerData)
            {
                if (data.Key != key)
                    continue;

                output = data.Value;
            }
            return output;
        }

        #endregion

        #region UserInventory
        public static void GetUserInventory(Action callback = null)
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
            {
                _virtualCurrency.Clear();
                _playerInventory.Clear();

                foreach (var pair in result.VirtualCurrency)
                    _virtualCurrency.Add(pair.Key, pair.Value);

                foreach (var eachItem in result.Inventory)
                    _playerInventory.Add(eachItem);

                callback?.Invoke();

                PlayFabBridge.RaiseCallbackSuccess("", PlayFabAPIMethods.GetUserInventory, MessageDisplayStyle.none);
            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static void AddStarForPlayer(int amount, Action callback = null)
        {
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "AddStar",
                FunctionParameter = new { amount = amount }
            };
            PlayFabClientAPI.ExecuteCloudScript(request, result =>
            {
                if (!PlayFabBridge.VerifyErrorFreeCloudScriptResult(result))
                    return;

                PlayFabBridge.RaiseCallbackSuccess("", PlayFabAPIMethods.ExecuteCloudScript, MessageDisplayStyle.none);
                if (callback != null)
                    callback();

            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static void SubtractStarFromPlayer(int amount, Action callback = null)
        {
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "SubtractStar",
                FunctionParameter = new { amount = amount }
            };
            PlayFabClientAPI.ExecuteCloudScript(request, result =>
            {
                if (!PlayFabBridge.VerifyErrorFreeCloudScriptResult(result))
                    return;
                PlayFabBridge.RaiseCallbackSuccess("", PlayFabAPIMethods.ExecuteCloudScript, MessageDisplayStyle.none);
                if (callback != null)
                    callback();

            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static void AddRedStarForPlayer(int amount, Action callback = null)
        {
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "AddRedStar",
                FunctionParameter = new { amount = amount }
            };
            PlayFabClientAPI.ExecuteCloudScript(request, result =>
            {
                if (!PlayFabBridge.VerifyErrorFreeCloudScriptResult(result))
                    return;
                PlayFabBridge.RaiseCallbackSuccess("", PlayFabAPIMethods.ExecuteCloudScript, MessageDisplayStyle.none);
                if (callback != null)
                    callback();

            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static void SubtractRedStarFromPlayer(int amount, Action callback = null)
        {
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "SubtractRedStar",
                FunctionParameter = new { amount = amount }
            };
            PlayFabClientAPI.ExecuteCloudScript(request, result =>
            {
                if (!PlayFabBridge.VerifyErrorFreeCloudScriptResult(result))
                    return;
                PlayFabBridge.RaiseCallbackSuccess("", PlayFabAPIMethods.ExecuteCloudScript, MessageDisplayStyle.none);
                if (callback != null)
                    callback();

            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static void PurchaseItem(string id, string currencyKey, int price)
        {
            var request = new PurchaseItemRequest
            {
                ItemId = id,
                VirtualCurrency = currencyKey,
                Price = price,
                StoreId = STORE_ID,
            };

            PlayFabClientAPI.PurchaseItem(request, result =>
            {
                PlayFabBridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.MakePurchase, MessageDisplayStyle.none);
            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static void ConsumeItem(string id)
        {
            var request = new ConsumeItemRequest
            {
                ConsumeCount = 1,
                ItemInstanceId = id
            };

            PlayFabClientAPI.ConsumeItem(request, result =>
            {
                PlayFabBridge.RaiseCallbackSuccess("", PlayFabAPIMethods.ConsumeItemUse, MessageDisplayStyle.none);
            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static int GetItemAmount(string itemId)
        {
            var output = 0;
            foreach (var eachItem in _playerInventory)
            {
                if (eachItem.ItemId != itemId)
                    continue;
                if (eachItem.RemainingUses == null)
                    return -1;
                if (eachItem.RemainingUses.Value > 0)
                    output += eachItem.RemainingUses.Value;
            }
            return output;
        }

        public static int GetCurrencyAmount(string id)
        {
            int amount = 0;

            foreach (var currency in _virtualCurrency)
            {
                if (currency.Key == id)
                {
                    amount = currency.Value;
                }
            }

            return amount;
        }
        #endregion

        #region Leaderboard

        public static void SubmitScore(int playerScore)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = LEADERBOARD_ID,
                        Value = playerScore
                    }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, result =>
            {
                PlayFabBridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.UpdateUserStatistics, MessageDisplayStyle.none);
            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static void RequestLeaderBoard(Action callback = null)
        {
            var leaderboard = new List<PlayerLeaderboardEntry>();

            var request = new GetLeaderboardRequest
            {
                StatisticName = LEADERBOARD_ID,
                StartPosition = 0,
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboard(request, result =>
            {
                _leaderboard = result.Leaderboard;
                callback?.Invoke();

                PlayFabBridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.UpdateUserStatistics, MessageDisplayStyle.none);
            }, PlayFabBridge.PlayFabErrorCallback);
        }

        public static List<PlayerLeaderboardEntry> GetLeaderboard()
        {
            return _leaderboard;
        }

        #endregion
    }
}

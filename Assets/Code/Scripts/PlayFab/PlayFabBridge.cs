
using PlayFab;
using PlayFab.ClientModels;

namespace Assets.Code.Scripts.PlayFab
{
    public static class PlayFabBridge
    {
        public delegate void PlayFabErrorHandler(string details, PlayFabAPIMethods method, MessageDisplayStyle displayStyle);
        public static event PlayFabErrorHandler OnPlayFabCallbackError;

        public delegate void CallbackSuccess(string details, PlayFabAPIMethods method, MessageDisplayStyle displayStyle);
        public static event CallbackSuccess OnPlayfabCallbackSuccess;

        public static void RaiseCallbackSuccess(string details, PlayFabAPIMethods method, MessageDisplayStyle style)
        {
            if (OnPlayfabCallbackSuccess != null)
                OnPlayfabCallbackSuccess(details, method, style);
        }

        public static void RaiseCallbackError(string details, PlayFabAPIMethods method, MessageDisplayStyle style)
        {
            if (OnPlayFabCallbackError != null)
                OnPlayFabCallbackError(details, method, style);
        }

        public static void PlayFabErrorCallback(PlayFabError error)
        {
            if (OnPlayFabCallbackError != null)
                OnPlayFabCallbackError(error.ErrorMessage, PlayFabAPIMethods.Generic, MessageDisplayStyle.error);
        }

        public static bool VerifyErrorFreeCloudScriptResult(ExecuteCloudScriptResult result)
        {
            if (result.Error != null)
                OnPlayFabCallbackError(string.Format("{0}: ERROR: [{1}] -- {2}", result.FunctionName, result.Error.Error, result.Error.Message), PlayFabAPIMethods.ExecuteCloudScript, MessageDisplayStyle.error);
            return result.Error == null;
        }
    }

    public enum MessageDisplayStyle
    {
        none,
        success,
        context,
        error
    }

    public enum PlayFabAPIMethods
    {
        Null,
        Generic,
        GenericLogin,
        GenericCloudScript,
        ExecuteCloudScript,
        RegisterPlayFabUser,
        LoginWithPlayFab,
        LoginWithDeviceId,
        LoginWithFacebook,
        GetAccountInfo,
        GetCDNConent,
        GetTitleData_General,
        GetTitleData_Specific,
        GetEvents,
        GetActiveEvents,
        GetTitleNews,
        GetCloudScriptUrl,
        GetAllUsersCharacters,
        GetCharacterData,
        GetCharacterReadOnlyData,
        GetUserStatistics,
        GetCharacterStatistics,
        GetPlayerLeaderboard,
        GetFriendsLeaderboard,
        GetMyPlayerRank,
        GetUserData,
        GetUserInventory,
        GetOffersCatalog,
        UpdateUserStatistics,
        UpdateCharacterStatistics,
        GetStoreItems,
        GrantCharacterToUser,
        DeleteCharacter,
        UpdateDisplayName,
        SendAccountRecoveryEmail,
        SavePlayerInfo,
        RetriveQuestItems,
        RegisterForPush,
        AddUsernamePassword,
        LinkDeviceID,
        LinkFacebookId,
        LinkGameCenterId,
        UnlinkAndroidDeviceID,
        UnlinkIOSDeviceID,
        UnlinkFacebookId,
        UnlinkGameCenterId,
        UnlockContainerItem,
        UpdateUserData,
        AddFriend,
        RemoveFriend,
        RedeemCoupon,
        SetFriendTags,
        GetFriendList,
        GetCharacterLeaderboard,
        GetLeaderboardAroundCharacter,
        ConsumeOffer,
        ValidateIAP,
        ConsumeItemUse,
        UnlockContainer,
        MakePurchase,
    }
}

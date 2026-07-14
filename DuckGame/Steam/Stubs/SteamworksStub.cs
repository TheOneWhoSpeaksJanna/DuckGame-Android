// Compile-time stub for the Steamworks.NET public API used by the game's Steam
// wrapper (DGSteam). Steam is non-functional on Android (no Steam client / native
// steam_api), so these inert shims let DGSteam.dll load and the game compile.
// All members return dynamic / accept params object[] so any call site binds;
// the game guards real Steam usage behind Steam.IsInitialized (false on Android).
namespace Steamworks
{
    public enum EResult { }
    public enum EFriendFlags { }
    public enum ELobbyType { }
    public enum ELobbyDistanceFilter { }
    public enum ELobbyComparison { }
    public enum EWorkshopFileType { }
    public enum EItemStatistic { }
    public enum ERemoteStoragePublishedFileVisibility { }
    public enum EGamepadTextInputMode { }
    public enum EItemState { }
    public enum EChatEntryType { }
    public enum EUGCMatchingUGCType { }
    public enum EUserUGCList { }
    public enum EUserUGCListSortOrder { }
    public enum EUGCQuery { }
    public enum EAccountType { }
    public enum EP2PSessionError { }
    public enum EChatRoomEnterResponse { }
    public enum EChatMemberStateChange { }
    public enum ELeaderboardUploadScoreMethod { }
    public enum EGlobalStatType { }
    public enum ESteamItemFlags { }

    public struct CSteamID
    {
        public dynamic m_SteamID;
        public dynamic m_PublishedFileId;
        public CSteamID(object h) { }
        public static implicit operator CSteamID(ulong v) => default;
    }

    public struct CGameID
    {
        public dynamic m_gameID;
        public dynamic m_nAppID;
        public dynamic m_nType;
        public CGameID(object h) { }
        public static implicit operator CGameID(ulong v) => default;
    }

    public struct PublishedFileId_t
    {
        public dynamic m_PublishedFileId;
        public dynamic m_SteamID;
        public PublishedFileId_t(object h) { }
        public static implicit operator PublishedFileId_t(ulong v) => default;
    }

    public struct AccountID_t
    {
        public dynamic m_AccountID;
        public AccountID_t(object h) { }
        public static implicit operator AccountID_t(ulong v) => default;
    }

    public struct AppId_t
    {
        public dynamic m_AppId;
        public AppId_t(object h) { }
        public static implicit operator AppId_t(ulong v) => default;
    }

    public struct UGCUpdateHandle_t
    {
        public dynamic m_UGCUpdateHandle;
        public UGCUpdateHandle_t(object h) { }
        public static implicit operator UGCUpdateHandle_t(ulong v) => default;
    }

    public struct UGCQueryHandle_t
    {
        public dynamic m_UGCQueryHandle;
        public dynamic m_UGCUpdateHandle;
        public UGCQueryHandle_t(object h) { }
        public static implicit operator UGCQueryHandle_t(ulong v) => default;
    }

    public struct SteamAPICall_t
    {
        public dynamic m_SteamAPICall;
        public SteamAPICall_t(object h) { }
        public static implicit operator SteamAPICall_t(ulong v) => default;
    }

    public struct FriendGameInfo_t
    {
        public dynamic m_steamIDLobby;
        public dynamic m_rgchGameName;
        public dynamic m_gameID;
        public FriendGameInfo_t(object h) { }
        public static implicit operator FriendGameInfo_t(ulong v) => default;
    }

    public struct CreateItemResult_t
    {
        public dynamic m_bUserNeedsToAcceptWorkshopLegalAgreement;
        public dynamic m_eResult;
        public dynamic m_nPublishedFileId;
        public CreateItemResult_t(object h) { }
        public static implicit operator CreateItemResult_t(ulong v) => default;
    }

    public struct DownloadItemResult_t
    {
        public dynamic m_eResult;
        public dynamic m_nPublishedFileId;
        public dynamic m_unAppId;
        public DownloadItemResult_t(object h) { }
        public static implicit operator DownloadItemResult_t(ulong v) => default;
    }

    public struct LobbyChatMsg_t
    {
        public dynamic m_iChatID;
        public dynamic m_ulSteamIDLobby;
        public dynamic m_ulSteamIDUser;
        public LobbyChatMsg_t(object h) { }
        public static implicit operator LobbyChatMsg_t(ulong v) => default;
    }

    public struct LobbyChatUpdate_t
    {
        public dynamic m_rgfChatMemberStateChange;
        public dynamic m_ulSteamIDLobby;
        public dynamic m_ulSteamIDMakingChange;
        public dynamic m_ulSteamIDUserChanged;
        public LobbyChatUpdate_t(object h) { }
        public static implicit operator LobbyChatUpdate_t(ulong v) => default;
    }

    public struct LobbyCreated_t
    {
        public dynamic m_eResult;
        public dynamic m_ulSteamIDLobby;
        public LobbyCreated_t(object h) { }
        public static implicit operator LobbyCreated_t(ulong v) => default;
    }

    public struct LobbyEnter_t
    {
        public dynamic m_EChatRoomEnterResponse;
        public dynamic m_bLocked;
        public dynamic m_rgfChatPermissions;
        public dynamic m_ulSteamIDLobby;
        public LobbyEnter_t(object h) { }
        public static implicit operator LobbyEnter_t(ulong v) => default;
    }

    public struct LobbyMatchList_t
    {
        public dynamic m_nLobbiesMatching;
        public LobbyMatchList_t(object h) { }
        public static implicit operator LobbyMatchList_t(ulong v) => default;
    }

    public struct P2PSessionConnectFail_t
    {
        public dynamic m_eP2PSessionError;
        public dynamic m_steamIDRemote;
        public P2PSessionConnectFail_t(object h) { }
        public static implicit operator P2PSessionConnectFail_t(ulong v) => default;
    }

    public struct P2PSessionRequest_t
    {
        public dynamic m_steamIDRemote;
        public P2PSessionRequest_t(object h) { }
        public static implicit operator P2PSessionRequest_t(ulong v) => default;
    }

    public struct SubmitItemUpdateResult_t
    {
        public dynamic m_bUserNeedsToAcceptWorkshopLegalAgreement;
        public dynamic m_eResult;
        public SubmitItemUpdateResult_t(object h) { }
        public static implicit operator SubmitItemUpdateResult_t(ulong v) => default;
    }

    public struct UserStatsReceived_t
    {
        public dynamic m_eResult;
        public dynamic m_nGameID;
        public dynamic m_steamIDUser;
        public UserStatsReceived_t(object h) { }
        public static implicit operator UserStatsReceived_t(ulong v) => default;
    }

    public struct GameLobbyJoinRequested_t
    {
        public dynamic m_steamIDFriend;
        public dynamic m_steamIDLobby;
        public GameLobbyJoinRequested_t(object h) { }
        public static implicit operator GameLobbyJoinRequested_t(ulong v) => default;
    }

    public struct GamepadTextInputDismissed_t
    {
        public dynamic m_bSubmitted;
        public dynamic m_unSubmittedText;
        public GamepadTextInputDismissed_t(object h) { }
        public static implicit operator GamepadTextInputDismissed_t(ulong v) => default;
    }

    public struct SteamRemotePlaySessionConnected_t
    {
        public dynamic m_unSessionID;
        public SteamRemotePlaySessionConnected_t(object h) { }
        public static implicit operator SteamRemotePlaySessionConnected_t(ulong v) => default;
    }

    public struct GlobalStatsReceived_t
    {
        public dynamic m_eResult;
        public dynamic m_nGameID;
        public GlobalStatsReceived_t(object h) { }
        public static implicit operator GlobalStatsReceived_t(ulong v) => default;
    }

    public struct SteamUGCDetails_t
    {
        public dynamic m_bBanned;
        public dynamic m_eVisibility;
        public dynamic m_fScore;
        public dynamic m_nFileSize;
        public dynamic m_nPublishedFileId;
        public dynamic m_nVotesDown;
        public dynamic m_nVotesUp;
        public dynamic m_rgchDescription;
        public dynamic m_rgchDetails;
        public dynamic m_rgchOwner;
        public dynamic m_rgchTags;
        public dynamic m_rgchTitle;
        public dynamic m_rtimeUpdated;
        public dynamic m_ulLastUpdated;
        public dynamic m_ulSteamIDOwner;
        public dynamic m_unNumChildren;
        public dynamic m_unNumPreview;
        public SteamUGCDetails_t(object h) { }
        public static implicit operator SteamUGCDetails_t(ulong v) => default;
    }

    public struct SteamUGCQueryCompleted_t
    {
        public dynamic m_eResult;
        public dynamic m_handle;
        public dynamic m_unNumResultsReturned;
        public SteamUGCQueryCompleted_t(object h) { }
        public static implicit operator SteamUGCQueryCompleted_t(ulong v) => default;
    }

    public struct SteamUGCRequestUGCDetailsResult_t
    {
        public dynamic m_details;
        public dynamic m_eResult;
        public SteamUGCRequestUGCDetailsResult_t(object h) { }
        public static implicit operator SteamUGCRequestUGCDetailsResult_t(ulong v) => default;
    }

    public class CallResult<T>
    {
        public delegate void APIDispatchDelegate(T result, bool ioFailure);
        public static CallResult<T> Create(APIDispatchDelegate func) { return null; }
        public CallResult(APIDispatchDelegate d = null) { }
        public void Set(SteamAPICall_t hAPICall, APIDispatchDelegate d = null) { }
    }

    public class Callback<T>
    {
        public delegate void CallbackDelegate(T param);
        public static object Create(CallbackDelegate func) { return null; }
    }

    public class CallbackDispatcher { }
    public class NativeMethods { }
    public class ISteamMatchmaking { }
    public class ISteamFriends { }
    public class ISteamUser { }
    public class ISteamUserStats { }
    public class ISteamUtils { }
    public class ISteamApps { }
    public class ISteamNetworking { }
    public class ISteamNetworkingUtils { }
    public class ISteamRemoteStorage { }
    public class ISteamUGC { }
    public class ISteamGameServer { }
    public class SteamClient { }
    public class SteamGameServer { }

    public static class SteamAPI
    {
        public static dynamic Init(params object[] args) { return null; }
        public static dynamic RestartAppIfNecessary(params object[] args) { return null; }
        public static dynamic RunCallbacks(params object[] args) { return null; }
        public static dynamic Shutdown(params object[] args) { return null; }
    }
    public static class SteamApps
    {
        public static dynamic BIsSubscribedApp(params object[] args) { return null; }
        public static dynamic GetAppBuildId(params object[] args) { return null; }
        public static dynamic MarkContentCorrupt(params object[] args) { return null; }
    }
    public static class SteamFriends
    {
        public static dynamic ActivateGameOverlayInviteDialog(params object[] args) { return null; }
        public static dynamic ActivateGameOverlayToWebPage(params object[] args) { return null; }
        public static dynamic GetFriendByIndex(params object[] args) { return null; }
        public static dynamic GetFriendCount(params object[] args) { return null; }
        public static dynamic GetFriendGamePlayed(params object[] args) { return null; }
        public static dynamic GetFriendPersonaName(params object[] args) { return null; }
        public static dynamic GetFriendPersonaState(params object[] args) { return null; }
        public static dynamic GetFriendRelationship(params object[] args) { return null; }
        public static dynamic GetMediumFriendAvatar(params object[] args) { return null; }
        public static dynamic GetPersonaName(params object[] args) { return null; }
        public static dynamic GetSmallFriendAvatar(params object[] args) { return null; }
    }
    public static class SteamMatchmaking
    {
        public static dynamic AddRequestLobbyListCompatibleMembersFilter(params object[] args) { return null; }
        public static dynamic AddRequestLobbyListDistanceFilter(params object[] args) { return null; }
        public static dynamic AddRequestLobbyListFilterSlotsAvailable(params object[] args) { return null; }
        public static dynamic AddRequestLobbyListNearValueFilter(params object[] args) { return null; }
        public static dynamic AddRequestLobbyListNumericalFilter(params object[] args) { return null; }
        public static dynamic AddRequestLobbyListResultCountFilter(params object[] args) { return null; }
        public static dynamic AddRequestLobbyListStringFilter(params object[] args) { return null; }
        public static dynamic CreateLobby(params object[] args) { return null; }
        public static dynamic GetLobbyByIndex(params object[] args) { return null; }
        public static dynamic GetLobbyChatEntry(params object[] args) { return null; }
        public static dynamic GetLobbyData(params object[] args) { return null; }
        public static dynamic GetLobbyMemberByIndex(params object[] args) { return null; }
        public static dynamic GetLobbyMemberLimit(params object[] args) { return null; }
        public static dynamic GetLobbyOwner(params object[] args) { return null; }
        public static dynamic GetNumLobbyMembers(params object[] args) { return null; }
        public static dynamic InviteUserToLobby(params object[] args) { return null; }
        public static dynamic JoinLobby(params object[] args) { return null; }
        public static dynamic LeaveLobby(params object[] args) { return null; }
        public static dynamic RequestLobbyList(params object[] args) { return null; }
        public static dynamic SendLobbyChatMsg(params object[] args) { return null; }
        public static dynamic SetLobbyData(params object[] args) { return null; }
        public static dynamic SetLobbyJoinable(params object[] args) { return null; }
        public static dynamic SetLobbyMemberLimit(params object[] args) { return null; }
        public static dynamic SetLobbyOwner(params object[] args) { return null; }
        public static dynamic SetLobbyType(params object[] args) { return null; }
    }
    public static class SteamNetworking
    {
        public static dynamic AcceptP2PSessionWithUser(params object[] args) { return null; }
        public static dynamic CloseP2PSessionWithUser(params object[] args) { return null; }
        public static dynamic GetP2PSessionState(params object[] args) { return null; }
        public static dynamic IsP2PPacketAvailable(params object[] args) { return null; }
        public static dynamic ReadP2PPacket(params object[] args) { return null; }
        public static dynamic SendP2PPacket(params object[] args) { return null; }
    }
    public static class SteamNetworkingUtils
    {
        public static dynamic ConvertPingLocationToString(params object[] args) { return null; }
        public static dynamic EstimatePingTimeFromLocalHost(params object[] args) { return null; }
        public static dynamic GetLocalPingLocation(params object[] args) { return null; }
        public static dynamic InitRelayNetworkAccess(params object[] args) { return null; }
        public static dynamic ParsePingLocationString(params object[] args) { return null; }
    }
    public static class SteamRemoteStorage
    {
        public static dynamic FileDelete(params object[] args) { return null; }
        public static dynamic FileExists(params object[] args) { return null; }
        public static dynamic FileRead(params object[] args) { return null; }
        public static dynamic FileWrite(params object[] args) { return null; }
        public static dynamic GetFileCount(params object[] args) { return null; }
        public static dynamic GetFileNameAndSize(params object[] args) { return null; }
        public static dynamic GetFileSize(params object[] args) { return null; }
        public static dynamic GetFileTimestamp(params object[] args) { return null; }
        public static dynamic IsCloudEnabledForAccount(params object[] args) { return null; }
        public static dynamic IsCloudEnabledForApp(params object[] args) { return null; }
    }
    public static class SteamUGC
    {
        public static dynamic AddDependency(params object[] args) { return null; }
        public static dynamic AddExcludedTag(params object[] args) { return null; }
        public static dynamic AddRequiredTag(params object[] args) { return null; }
        public static dynamic CreateItem(params object[] args) { return null; }
        public static dynamic CreateQueryAllUGCRequest(params object[] args) { return null; }
        public static dynamic CreateQueryUGCDetailsRequest(params object[] args) { return null; }
        public static dynamic CreateQueryUserUGCRequest(params object[] args) { return null; }
        public static dynamic DownloadItem(params object[] args) { return null; }
        public static dynamic GetItemDownloadInfo(params object[] args) { return null; }
        public static dynamic GetItemInstallInfo(params object[] args) { return null; }
        public static dynamic GetItemState(params object[] args) { return null; }
        public static dynamic GetItemUpdateProgress(params object[] args) { return null; }
        public static dynamic GetNumSubscribedItems(params object[] args) { return null; }
        public static dynamic GetQueryUGCAdditionalPreview(params object[] args) { return null; }
        public static dynamic GetQueryUGCChildren(params object[] args) { return null; }
        public static dynamic GetQueryUGCMetadata(params object[] args) { return null; }
        public static dynamic GetQueryUGCNumAdditionalPreviews(params object[] args) { return null; }
        public static dynamic GetQueryUGCPreviewURL(params object[] args) { return null; }
        public static dynamic GetQueryUGCResult(params object[] args) { return null; }
        public static dynamic GetQueryUGCStatistic(params object[] args) { return null; }
        public static dynamic GetSubscribedItems(params object[] args) { return null; }
        public static dynamic ReleaseQueryUGCRequest(params object[] args) { return null; }
        public static dynamic RemoveDependency(params object[] args) { return null; }
        public static dynamic SendQueryUGCRequest(params object[] args) { return null; }
        public static dynamic SetAllowCachedResponse(params object[] args) { return null; }
        public static dynamic SetCloudFileNameFilter(params object[] args) { return null; }
        public static dynamic SetItemContent(params object[] args) { return null; }
        public static dynamic SetItemDescription(params object[] args) { return null; }
        public static dynamic SetItemPreview(params object[] args) { return null; }
        public static dynamic SetItemTags(params object[] args) { return null; }
        public static dynamic SetItemTitle(params object[] args) { return null; }
        public static dynamic SetItemVisibility(params object[] args) { return null; }
        public static dynamic SetMatchAnyTag(params object[] args) { return null; }
        public static dynamic SetRankedByTrendDays(params object[] args) { return null; }
        public static dynamic SetReturnAdditionalPreviews(params object[] args) { return null; }
        public static dynamic SetReturnChildren(params object[] args) { return null; }
        public static dynamic SetReturnLongDescription(params object[] args) { return null; }
        public static dynamic SetReturnMetadata(params object[] args) { return null; }
        public static dynamic SetReturnOnlyIDs(params object[] args) { return null; }
        public static dynamic SetReturnTotalOnly(params object[] args) { return null; }
        public static dynamic SetSearchText(params object[] args) { return null; }
        public static dynamic StartItemUpdate(params object[] args) { return null; }
        public static dynamic SubmitItemUpdate(params object[] args) { return null; }
        public static dynamic SubscribeItem(params object[] args) { return null; }
        public static dynamic UnsubscribeItem(params object[] args) { return null; }
    }
    public static class SteamUser
    {
        public static dynamic GetSteamID(params object[] args) { return null; }
    }
    public static class SteamUserStats
    {
        public static dynamic GetAchievement(params object[] args) { return null; }
        public static dynamic GetGlobalStat(params object[] args) { return null; }
        public static dynamic GetGlobalStatHistory(params object[] args) { return null; }
        public static dynamic GetLeaderboardName(params object[] args) { return null; }
        public static dynamic GetStat(params object[] args) { return null; }
        public static dynamic IndicateAchievementProgress(params object[] args) { return null; }
        public static dynamic RequestGlobalStats(params object[] args) { return null; }
        public static dynamic SetAchievement(params object[] args) { return null; }
        public static dynamic SetStat(params object[] args) { return null; }
        public static dynamic StoreStats(params object[] args) { return null; }
        public static dynamic UploadLeaderboardScore(params object[] args) { return null; }
    }
    public static class SteamUtils
    {
        public static dynamic GetAppID(params object[] args) { return null; }
        public static dynamic GetEnteredGamepadTextInput(params object[] args) { return null; }
        public static dynamic GetEnteredGamepadTextLength(params object[] args) { return null; }
        public static dynamic GetImageRGBA(params object[] args) { return null; }
        public static dynamic GetImageSize(params object[] args) { return null; }
        public static dynamic ShowGamepadTextInput(params object[] args) { return null; }
    }
}

// Compile-time stub for the Steamworks.NET API used by the game's Steam wrapper.
// Steam is non-functional on Android (no Steam client / native steam_api). These
// inert shims let DGSteam.dll load and the game compile; all calls return null.
// Methods accept (params object[]) so any call site (any args/overloads) compiles.
namespace Steamworks
{
    public class CallResult<T>
    {
        public delegate void APIDispatchDelegate(T param, bool bIOFailure);
        public CallResult(APIDispatchDelegate func = null) { }
        public void Set(SteamAPICall_t hAPICall, APIDispatchDelegate func = null) { }
    }
    public struct CreateItemResult_t { public ulong m_handle; public CreateItemResult_t(ulong h){ m_handle = h; } }
    public struct DownloadItemResult_t { public ulong m_handle; public DownloadItemResult_t(ulong h){ m_handle = h; } }
    public enum EChatEntryType { }
    public enum EFriendFlags { }
    public enum EGamepadTextInputMode { }
    public enum EItemStatistic { }
    public enum ELobbyComparison { }
    public enum ELobbyDistanceFilter { }
    public enum ELobbyType { }
    public enum ERemoteStoragePublishedFileVisibility { }
    public enum EResult { }
    public enum EUGCMatchingUGCType { }
    public enum EUGCQuery { }
    public enum EUserUGCList { }
    public enum EUserUGCListSortOrder { }
    public enum EWorkshopFileType { }
    public struct GameLobbyJoinRequested_t { public ulong m_handle; public GameLobbyJoinRequested_t(ulong h){ m_handle = h; } }
    public struct GamepadTextInputDismissed_t { public ulong m_handle; public GamepadTextInputDismissed_t(ulong h){ m_handle = h; } }
    public struct GlobalStatsReceived_t { public ulong m_handle; public GlobalStatsReceived_t(ulong h){ m_handle = h; } }
    public struct LobbyChatMsg_t { public ulong m_handle; public LobbyChatMsg_t(ulong h){ m_handle = h; } }
    public struct LobbyChatUpdate_t { public ulong m_handle; public LobbyChatUpdate_t(ulong h){ m_handle = h; } }
    public struct LobbyCreated_t { public ulong m_handle; public LobbyCreated_t(ulong h){ m_handle = h; } }
    public struct LobbyEnter_t { public ulong m_handle; public LobbyEnter_t(ulong h){ m_handle = h; } }
    public struct LobbyMatchList_t { public ulong m_handle; public LobbyMatchList_t(ulong h){ m_handle = h; } }
    public struct NET { public ulong m_handle; public NET(ulong h){ m_handle = h; } }
    public struct P2PSessionConnectFail_t { public ulong m_handle; public P2PSessionConnectFail_t(ulong h){ m_handle = h; } }
    public struct P2PSessionRequest_t { public ulong m_handle; public P2PSessionRequest_t(ulong h){ m_handle = h; } }
    public struct P2PSessionState_t { public ulong m_handle; public P2PSessionState_t(ulong h){ m_handle = h; } }
    public struct SteamAPICall_t { public ulong m_handle; public SteamAPICall_t(ulong h){ m_handle = h; } }
    public struct SteamLeaderboard_t { public ulong m_handle; public SteamLeaderboard_t(ulong h){ m_handle = h; } }
    public struct SteamNetworkPingLocation_t { public ulong m_handle; public SteamNetworkPingLocation_t(ulong h){ m_handle = h; } }
    public struct SteamRemotePlaySessionConnected_t { public ulong m_handle; public SteamRemotePlaySessionConnected_t(ulong h){ m_handle = h; } }
    public struct SteamUGCDetails_t { public ulong m_handle; public SteamUGCDetails_t(ulong h){ m_handle = h; } }
    public struct SteamUGCQueryCompleted_t { public ulong m_handle; public SteamUGCQueryCompleted_t(ulong h){ m_handle = h; } }
    public struct SubmitItemUpdateResult_t { public ulong m_handle; public SubmitItemUpdateResult_t(ulong h){ m_handle = h; } }
    public struct UGCQueryHandle_t { public ulong m_handle; public UGCQueryHandle_t(ulong h){ m_handle = h; } }
    public struct UserStatsReceived_t { public ulong m_handle; public UserStatsReceived_t(ulong h){ m_handle = h; } }
    public struct CSteamID { public ulong m_SteamID; public CSteamID(ulong id){m_SteamID=id;} public static implicit operator CSteamID(ulong v){return new CSteamID(v);} public ulong m_ulSteamIDLobby; }
    public struct PublishedFileId_t { public ulong m_PublishedFileId; public PublishedFileId_t(ulong id){m_PublishedFileId=id;} public ulong id {get{return m_PublishedFileId;}} }
    public struct AccountID_t { public uint m_AccountID; public AccountID_t(uint id){m_AccountID=id;} }
    public struct AppId_t { public uint m_AppId; public AppId_t(uint id){m_AppId=id;} }
    public struct UGCUpdateHandle_t { public ulong m_UGCUpdateHandle; public UGCUpdateHandle_t(ulong h){m_UGCUpdateHandle=h;} public ulong updateHandle {get{return m_UGCUpdateHandle;}} }
    public struct FriendGameInfo_t { }
    public class CallbackDispatcher { }
    public class NativeMethods { }
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
    public static class SteamUtils
    {
        public static dynamic GetAppID(params object[] args) { return null; }
        public static dynamic GetEnteredGamepadTextInput(params object[] args) { return null; }
        public static dynamic GetEnteredGamepadTextLength(params object[] args) { return null; }
        public static dynamic GetImageRGBA(params object[] args) { return null; }
        public static dynamic GetImageSize(params object[] args) { return null; }
        public static dynamic ShowGamepadTextInput(params object[] args) { return null; }
    }
    public static class SteamUser
    {
        public static dynamic GetSteamID(params object[] args) { return null; }
    }
    public static class SteamApps
    {
        public static dynamic BIsSubscribedApp(params object[] args) { return null; }
        public static dynamic GetAppBuildId(params object[] args) { return null; }
        public static dynamic MarkContentCorrupt(params object[] args) { return null; }
    }
    public static class SteamAPI
    {
        public static dynamic Init(params object[] args) { return null; }
        public static dynamic RestartAppIfNecessary(params object[] args) { return null; }
        public static dynamic RunCallbacks(params object[] args) { return null; }
        public static dynamic Shutdown(params object[] args) { return null; }
    }
}

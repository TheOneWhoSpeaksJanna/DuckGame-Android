// Compile-time stub for the Steamworks.NET API used by the game's Steam wrapper.
// Steam is non-functional on Android (no Steam client / native steam_api). These
// inert shims let DGSteam.dll load and the game compile; all calls return null.
// Real game behavior is unchanged.
namespace Steamworks
{
    public struct CSteamID { public ulong m_SteamID; public CSteamID(ulong id){m_SteamID=id;} public static implicit operator CSteamID(ulong v){return new CSteamID(v);} public ulong m_ulSteamIDLobby; }
    public struct PublishedFileId_t { public ulong m_PublishedFileId; public PublishedFileId_t(ulong id){m_PublishedFileId=id;} public ulong id {get{return m_PublishedFileId;}} }
    public struct AccountID_t { public uint m_AccountID; public AccountID_t(uint id){m_AccountID=id;} }
    public struct AppId_t { public uint m_AppId; public AppId_t(uint id){m_AppId=id;} }
    public struct UGCUpdateHandle_t { public ulong m_UGCUpdateHandle; public UGCUpdateHandle_t(ulong h){m_UGCUpdateHandle=h;} public ulong updateHandle {get{return m_UGCUpdateHandle;}} }
    public struct FriendGameInfo_t { }
    public class CallResult<T> { }
    public enum EFriendFlags { k_EFriendFlagAll }
    public enum ELobbyType { }
    public enum ELobbyDistanceFilter { k_ELobbyDistanceFilterFar, k_ELobbyDistanceFilterWorldwide }
    public enum ELobbyComparison { }
    public enum EWorkshopFileType { k_EWorkshopFileTypeFirst }
    public enum EItemStatistic { }
    public enum ERemoteStoragePublishedFileVisibility { }
    public enum EGamepadTextInputMode { k_EGamepadTextInputModeNormal }
    public enum EItemState { }
    public enum EResult { }
    public enum EChatEntryType { }
    public class CallbackDispatcher { }
    public class NativeMethods { }
    public static class SteamUGC
    {
        public static dynamic AddDependency(object p0, object p1) { return null; }
        public static dynamic AddExcludedTag(object p0, object p1) { return null; }
        public static dynamic AddRequiredTag(object p0, object p1) { return null; }
        public static dynamic CreateItem(object p0, object p1) { return null; }
        public static dynamic CreateQueryAllUGCRequest(object p0, object p1, object p2, object p3, object p4) { return null; }
        public static dynamic CreateQueryUGCDetailsRequest(object p0, object p1) { return null; }
        public static dynamic CreateQueryUserUGCRequest(object p0, object p1, object p2, object p3, object p4, object p5, object p6) { return null; }
        public static dynamic DownloadItem(object p0, object p1) { return null; }
        public static dynamic GetItemDownloadInfo(object p0, out bytesDownloaded, out bytesTotal) { return null; }
        public static dynamic GetItemInstallInfo(object p0, out SizeOnDisk, out Folder, object p3, out punTimeStamp) { return null; }
        public static dynamic GetItemState(object p0) { return null; }
        public static dynamic GetItemUpdateProgress(object p0, out bytesDownloaded, out bytesTotal) { return null; }
        public static dynamic GetNumSubscribedItems() { return null; }
        public static dynamic GetQueryUGCAdditionalPreview(object p0, object p1, object p2, out url, object p4, out name, object p6, out type) { return null; }
        public static dynamic GetQueryUGCChildren(object p0, object p1, object p2, object p3) { return null; }
        public static dynamic GetQueryUGCMetadata(object p0, object p1, object p2, object p3) { return null; }
        public static dynamic GetQueryUGCNumAdditionalPreviews(object p0, object p1) { return null; }
        public static dynamic GetQueryUGCPreviewURL(object p0, object p1, object p2, object p3) { return null; }
        public static dynamic GetQueryUGCResult(object p0, object p1, out ugcDetails) { return null; }
        public static dynamic GetQueryUGCResult(object p0, object p1, out details) { return null; }
        public static dynamic GetQueryUGCStatistic(object p0, object p1, object p2, out val) { return null; }
        public static dynamic GetSubscribedItems(object p0, object p1) { return null; }
        public static dynamic ReleaseQueryUGCRequest(object p0) { return null; }
        public static dynamic RemoveDependency(object p0, object p1) { return null; }
        public static dynamic SendQueryUGCRequest(object p0) { return null; }
        public static dynamic SetAllowCachedResponse(object p0, object p1) { return null; }
        public static dynamic SetCloudFileNameFilter(object p0, object p1) { return null; }
        public static dynamic SetItemContent(object p0, object p1) { return null; }
        public static dynamic SetItemDescription(object p0, object p1) { return null; }
        public static dynamic SetItemPreview(object p0, object p1) { return null; }
        public static dynamic SetItemTags(object p0, object p1) { return null; }
        public static dynamic SetItemTitle(object p0, object p1) { return null; }
        public static dynamic SetItemVisibility(object p0, object p1) { return null; }
        public static dynamic SetMatchAnyTag(object p0, object p1) { return null; }
        public static dynamic SetRankedByTrendDays(object p0, object p1) { return null; }
        public static dynamic SetReturnAdditionalPreviews(object p0, object p1) { return null; }
        public static dynamic SetReturnChildren(object p0, object p1) { return null; }
        public static dynamic SetReturnLongDescription(object p0, object p1) { return null; }
        public static dynamic SetReturnMetadata(object p0, object p1) { return null; }
        public static dynamic SetReturnOnlyIDs(object p0, object p1) { return null; }
        public static dynamic SetReturnTotalOnly(object p0, object p1) { return null; }
        public static dynamic SetSearchText(object p0, object p1) { return null; }
        public static dynamic StartItemUpdate(object p0, object p1) { return null; }
        public static dynamic SubmitItemUpdate(object p0, object p1) { return null; }
        public static dynamic SubscribeItem(object p0) { return null; }
        public static dynamic UnsubscribeItem(object p0) { return null; }
    }
    public static class SteamMatchmaking
    {
        public static dynamic AddRequestLobbyListCompatibleMembersFilter(object p0) { return null; }
        public static dynamic AddRequestLobbyListDistanceFilter(object p0) { return null; }
        public static dynamic AddRequestLobbyListFilterSlotsAvailable(object p0) { return null; }
        public static dynamic AddRequestLobbyListNearValueFilter(object p0, object p1) { return null; }
        public static dynamic AddRequestLobbyListNumericalFilter(object p0, object p1, (ELobbyComparison) compareType) { return null; }
        public static dynamic AddRequestLobbyListResultCountFilter(object p0) { return null; }
        public static dynamic AddRequestLobbyListStringFilter(object p0, object p1, (ELobbyComparison) compareType) { return null; }
        public static dynamic CreateLobby((ELobbyType) lobbyType, object p1) { return null; }
        public static dynamic GetLobbyByIndex(object p0) { return null; }
        public static dynamic GetLobbyChatEntry(object p0, object p1, out CSteamID PlayerID, object p3, object p4, out EChatEntryType peChatEntryType) { return null; }
        public static dynamic GetLobbyData(object p0, object p1) { return null; }
        public static dynamic GetLobbyMemberByIndex(object p0, object p1) { return null; }
        public static dynamic GetLobbyMemberLimit(object p0) { return null; }
        public static dynamic GetLobbyOwner(object p0) { return null; }
        public static dynamic GetNumLobbyMembers(object p0) { return null; }
        public static dynamic InviteUserToLobby(object p0, object p1) { return null; }
        public static dynamic JoinLobby(object p0) { return null; }
        public static dynamic LeaveLobby(object p0) { return null; }
        public static dynamic RequestLobbyList() { return null; }
        public static dynamic SendLobbyChatMsg(object p0, object p1, object p2) { return null; }
        public static dynamic SetLobbyData(object p0, object p1, object p2) { return null; }
        public static dynamic SetLobbyJoinable(object p0, object p1) { return null; }
        public static dynamic SetLobbyMemberLimit(object p0, _maxMembers = value) { return null; }
        public static dynamic SetLobbyOwner(object p0, object p1) { return null; }
        public static dynamic SetLobbyType(object p0, (ELobbyType) value) { return null; }
    }
    public static class SteamFriends
    {
        public static dynamic ActivateGameOverlayInviteDialog(object p0) { return null; }
        public static dynamic ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + id) { return null; }
        public static dynamic ActivateGameOverlayToWebPage(object p0) { return null; }
        public static dynamic GetFriendByIndex(object p0, object p1) { return null; }
        public static dynamic GetFriendCount(object p0) { return null; }
        public static dynamic GetFriendGamePlayed(object p0, out game) { return null; }
        public static dynamic GetFriendPersonaName(object p0) { return null; }
        public static dynamic GetFriendPersonaState(object p0) { return null; }
        public static dynamic GetFriendRelationship(object p0) { return null; }
        public static dynamic GetMediumFriendAvatar(object p0) { return null; }
        public static dynamic GetPersonaName() { return null; }
        public static dynamic GetSmallFriendAvatar(object p0) { return null; }
    }
    public static class SteamRemoteStorage
    {
        public static dynamic FileDelete(object p0) { return null; }
        public static dynamic FileExists(object p0) { return null; }
        public static dynamic FileRead(object p0, object p1, object p2) { return null; }
        public static dynamic FileWrite(object p0, object p1, object p2) { return null; }
        public static dynamic GetFileCount() { return null; }
        public static dynamic GetFileNameAndSize(object p0, out size) { return null; }
        public static dynamic GetFileSize(object p0) { return null; }
        public static dynamic GetFileTimestamp(object p0) { return null; }
        public static dynamic IsCloudEnabledForAccount() { return null; }
        public static dynamic IsCloudEnabledForApp() { return null; }
    }
    public static class SteamUtils
    {
        public static dynamic GetAppID() { return null; }
        public static dynamic GetEnteredGamepadTextInput(out szTextInput, object p1) { return null; }
        public static dynamic GetEnteredGamepadTextLength() { return null; }
        public static dynamic GetImageRGBA(object p0, object p1, object p2) { return null; }
        public static dynamic GetImageSize(object p0, out w, out h) { return null; }
        public static dynamic ShowGamepadTextInput(object p0, object p1, object p2, object p3, object p4) { return null; }
    }
    public static class SteamUser
    {
        public static dynamic GetSteamID() { return null; }
    }
    public static class SteamApps
    {
        public static dynamic BIsSubscribedApp(object p0) { return null; }
        public static dynamic GetAppBuildId() { return null; }
        public static dynamic MarkContentCorrupt(object p0) { return null; }
    }
    public static class SteamAPI
    {
        public static dynamic Init() { return null; }
        public static dynamic RestartAppIfNecessary(object p0) { return null; }
        public static dynamic RunCallbacks() { return null; }
        public static dynamic Shutdown() { return null; }
    }
}

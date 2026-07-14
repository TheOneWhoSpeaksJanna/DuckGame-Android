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
        public static dynamic AddDependency(new PublishedFileId_titemid, new PublishedFileId_tdependsOnid) { return null; }
        public static dynamic AddExcludedTag(object _handle, object tag) { return null; }
        public static dynamic AddRequiredTag(object _handle, object tag) { return null; }
        public static dynamic CreateItem(object SteamUtilsGetAppID, object EWorkshopFileTypek_EWorkshopFileTypeFirst) { return null; }
        public static dynamic CreateQueryAllUGCRequest(object _queryType, object _fileType, object AppId_t312530, object AppId_t312530, object _page) { return null; }
        public static dynamic CreateQueryUGCDetailsRequest(SteamHelper.GetArray(files, id => new PublishedFileId_tid, (uint) filesCount) { return null; }
        public static dynamic CreateQueryUGCDetailsRequest(SteamHelper.GetArray(items, item => new PublishedFileId_titemid, (uint) itemsCount) { return null; }
        public static dynamic CreateQueryUserUGCRequest(object _accountID, object _listType, object _type, object _sortOrder, object AppId_t312530, object AppId_t312530, object _page) { return null; }
        public static dynamic DownloadItem(new PublishedFileId_titemid, object true) { return null; }
        public static dynamic GetItemDownloadInfo(object _id, out object bytesDownloaded, out object bytesTotal) { return null; }
        public static dynamic GetItemInstallInfo(object _id, out object SizeOnDisk, out object Folder, object p1024, out object punTimeStamp) { return null; }
        public static dynamic GetItemInstallInfo(object _id, out object SizeOnDisk, out object Folder, object p256, out object punTimeStamp) { return null; }
        public static dynamic GetItemState(object _id) { return null; }
        public static dynamic GetItemUpdateProgress(object _currentUpdateHandle, out object bytesDownloaded, out object bytesTotal) { return null; }
        public static dynamic GetNumSubscribedItems() { return null; }
        public static dynamic GetQueryUGCAdditionalPreview(object queryCompletedm_handle, object resulti, object previewi, out object url, object p260, out object name, object p260, out object type) { return null; }
        public static dynamic GetQueryUGCChildren(object queryCompletedm_handle, object resulti, object children, object uintchildrenLength) { return null; }
        public static dynamic GetQueryUGCChildren(object resultm_handle, object i, object dependencies, object uintdependencyCount) { return null; }
        public static dynamic GetQueryUGCMetadata(object queryCompletedm_handle, object resulti, out object resultmetadata, object p260) { return null; }
        public static dynamic GetQueryUGCNumAdditionalPreviews(object queryCompletedm_handle, object resulti) { return null; }
        public static dynamic GetQueryUGCPreviewURL(object queryCompletedm_handle, object resulti, out object resultpreviewURL, object p260) { return null; }
        public static dynamic GetQueryUGCPreviewURL(object resultm_handle, object i, out object workshopDatapreviewPath, object p256) { return null; }
        public static dynamic GetQueryUGCResult(object queryCompletedm_handle, object resulti, out object ugcDetails) { return null; }
        public static dynamic GetQueryUGCResult(object resultm_handle, object i, out object details) { return null; }
        public static dynamic GetQueryUGCStatistic(object queryCompletedm_handle, object resulti, object EItemStatisticstat, out object val) { return null; }
        public static dynamic GetSubscribedItems(object tmp, (uint) tmpLength) { return null; }
        public static dynamic ReleaseQueryUGCRequest(object _handle) { return null; }
        public static dynamic ReleaseQueryUGCRequest(object resultm_handle) { return null; }
        public static dynamic RemoveDependency(new PublishedFileId_titemid, new PublishedFileId_tdependsOnid) { return null; }
        public static dynamic SendQueryUGCRequest(object _handle) { return null; }
        public static dynamic SendQueryUGCRequest(object query) { return null; }
        public static dynamic SetAllowCachedResponse(object _handle, object _maxCacheTime) { return null; }
        public static dynamic SetCloudFileNameFilter(object _handle, object cloudNameFileFilter) { return null; }
        public static dynamic SetItemContent(object handle, object datacontentFolder) { return null; }
        public static dynamic SetItemDescription(object handle, object datadescription) { return null; }
        public static dynamic SetItemPreview(object handle, object datapreviewPath) { return null; }
        public static dynamic SetItemTags(object handle, object datatags) { return null; }
        public static dynamic SetItemTitle(object handle, object dataname) { return null; }
        public static dynamic SetItemVisibility(object handle, (ERemoteStoragePublishedFileVisibility) datavisibility) { return null; }
        public static dynamic SetMatchAnyTag(object _handle, object matchAnyTag) { return null; }
        public static dynamic SetRankedByTrendDays(object _handle, object trendRankDays) { return null; }
        public static dynamic SetReturnAdditionalPreviews(object _handle, (dataToFetch & WorkshopQueryData.AdditionalPreviews) != p0) { return null; }
        public static dynamic SetReturnAdditionalPreviews(object _handle, object false) { return null; }
        public static dynamic SetReturnAdditionalPreviews(object _handle, object true) { return null; }
        public static dynamic SetReturnChildren(object _handle, (dataToFetch & WorkshopQueryData.Children) != p0) { return null; }
        public static dynamic SetReturnChildren(object _handle, object false) { return null; }
        public static dynamic SetReturnChildren(object _handle, object true) { return null; }
        public static dynamic SetReturnLongDescription(object _handle, (dataToFetch & WorkshopQueryData.LongDescription) != p0) { return null; }
        public static dynamic SetReturnLongDescription(object _handle, object false) { return null; }
        public static dynamic SetReturnLongDescription(object _handle, object true) { return null; }
        public static dynamic SetReturnMetadata(object _handle, (dataToFetch & WorkshopQueryData.Metadata) != p0) { return null; }
        public static dynamic SetReturnMetadata(object _handle, object false) { return null; }
        public static dynamic SetReturnMetadata(object _handle, object true) { return null; }
        public static dynamic SetReturnOnlyIDs(object _handle, object true) { return null; }
        public static dynamic SetReturnTotalOnly(object _handle, object true) { return null; }
        public static dynamic SetSearchText(object _handle, object searchText) { return null; }
        public static dynamic StartItemUpdate(object SteamUtilsGetAppID, object _id) { return null; }
        public static dynamic SubmitItemUpdate(new UGCUpdateHandle_titemupdateHandle, object itemdatachangeNotes) { return null; }
        public static dynamic SubscribeItem(object _id) { return null; }
        public static dynamic SubscribeItem(new PublishedFileId_tid) { return null; }
        public static dynamic UnsubscribeItem(new PublishedFileId_tid) { return null; }
    }
    public static class SteamMatchmaking
    {
        public static dynamic AddRequestLobbyListCompatibleMembersFilter(new CSteamIDwhoid) { return null; }
        public static dynamic AddRequestLobbyListDistanceFilter(object ELobbyDistanceFilterk_ELobbyDistanceFilterFar) { return null; }
        public static dynamic AddRequestLobbyListDistanceFilter(object ELobbyDistanceFilterk_ELobbyDistanceFilterWorldwide) { return null; }
        public static dynamic AddRequestLobbyListFilterSlotsAvailable(object slots) { return null; }
        public static dynamic AddRequestLobbyListNearValueFilter(object key, object filt) { return null; }
        public static dynamic AddRequestLobbyListNumericalFilter(object key, object value, (ELobbyComparison) compareType) { return null; }
        public static dynamic AddRequestLobbyListResultCountFilter(object max) { return null; }
        public static dynamic AddRequestLobbyListStringFilter(object key, object value, (ELobbyComparison) compareType) { return null; }
        public static dynamic CreateLobby((ELobbyType) lobbyType, object maxMembers) { return null; }
        public static dynamic GetLobbyByIndex(object p0) { return null; }
        public static dynamic GetLobbyByIndex(object index) { return null; }
        public static dynamic GetLobbyChatEntry(new CSteamIDpResultm_ulSteamIDLobby, object CHatID, out CSteamID PlayerID, object PvChat, object CubeData, out EChatEntryType peChatEntryType) { return null; }
        public static dynamic GetLobbyData(object _id, object name) { return null; }
        public static dynamic GetLobbyMemberByIndex(object _id, object i) { return null; }
        public static dynamic GetLobbyMemberByIndex(new CSteamIDwhichid, object member) { return null; }
        public static dynamic GetLobbyMemberLimit(object _id) { return null; }
        public static dynamic GetLobbyOwner(object _id) { return null; }
        public static dynamic GetNumLobbyMembers(object _id) { return null; }
        public static dynamic GetNumLobbyMembers(new CSteamIDwhichid) { return null; }
        public static dynamic InviteUserToLobby(new CSteamIDlobbyValid, new CSteamIDuserValid) { return null; }
        public static dynamic JoinLobby(new CSteamIDlobbyID) { return null; }
        public static dynamic LeaveLobby(new CSteamIDwhichid) { return null; }
        public static dynamic RequestLobbyList() { return null; }
        public static dynamic SendLobbyChatMsg(new CSteamIDpLobbyid, object pData, object intpSize) { return null; }
        public static dynamic SetLobbyData(object _id, object mods, object value) { return null; }
        public static dynamic SetLobbyData(object _id, object name, object value) { return null; }
        public static dynamic SetLobbyJoinable(object _id, object value) { return null; }
        public static dynamic SetLobbyMemberLimit(object _id, _maxMembers = value) { return null; }
        public static dynamic SetLobbyOwner(object CSteamIDthisid, object CSteamIDvalueid) { return null; }
        public static dynamic SetLobbyType(object _id, (ELobbyType) value) { return null; }
    }
    public static class SteamFriends
    {
        public static dynamic ActivateGameOverlayInviteDialog(/*requires current lobby) { return null; }
        public static dynamic ActivateGameOverlayInviteDialog(new CSteamIDlobbyid) { return null; }
        public static dynamic ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + id) { return null; }
        public static dynamic ActivateGameOverlayToWebPage(object url) { return null; }
        public static dynamic GetFriendByIndex(object i, object EFriendFlagsk_EFriendFlagAll) { return null; }
        public static dynamic GetFriendCount(object EFriendFlagsk_EFriendFlagAll) { return null; }
        public static dynamic GetFriendGamePlayed(object _id, out object game) { return null; }
        public static dynamic GetFriendPersonaName(object _id) { return null; }
        public static dynamic GetFriendPersonaState(object _id) { return null; }
        public static dynamic GetFriendRelationship(object _id) { return null; }
        public static dynamic GetMediumFriendAvatar(object _id) { return null; }
        public static dynamic GetPersonaName() { return null; }
        public static dynamic GetSmallFriendAvatar(object _id) { return null; }
    }
    public static class SteamRemoteStorage
    {
        public static dynamic FileDelete(object name) { return null; }
        public static dynamic FileExists(object name) { return null; }
        public static dynamic FileRead(object managedname, object data, object size) { return null; }
        public static dynamic FileRead(object name, object data, object size) { return null; }
        public static dynamic FileWrite(object managedname, object data, object dataLength) { return null; }
        public static dynamic FileWrite(object name, object data, object length) { return null; }
        public static dynamic GetFileCount() { return null; }
        public static dynamic GetFileNameAndSize(object file, out object size) { return null; }
        public static dynamic GetFileSize(object managedname) { return null; }
        public static dynamic GetFileSize(object name) { return null; }
        public static dynamic GetFileTimestamp(object name) { return null; }
        public static dynamic IsCloudEnabledForAccount() { return null; }
        public static dynamic IsCloudEnabledForApp() { return null; }
    }
    public static class SteamUtils
    {
        public static dynamic GetAppID() { return null; }
        public static dynamic GetEnteredGamepadTextInput(out object szTextInput, object length) { return null; }
        public static dynamic GetEnteredGamepadTextLength() { return null; }
        public static dynamic GetImageRGBA(object id, object data, object dataLength) { return null; }
        public static dynamic GetImageSize(object id, out object w, out object h) { return null; }
        public static dynamic ShowGamepadTextInput(object EGamepadTextInputModek_EGamepadTextInputModeNormal, object egamepadTextInputLineMode, object description, object uintmaxChars, object existingText) { return null; }
    }
    public static class SteamUser
    {
        public static dynamic GetSteamID() { return null; }
    }
    public static class SteamApps
    {
        public static dynamic BIsSubscribedApp(object SteamUtilsGetAppID) { return null; }
        public static dynamic GetAppBuildId() { return null; }
        public static dynamic MarkContentCorrupt(object false) { return null; }
    }
    public static class SteamAPI
    {
        public static dynamic Init() { return null; }
        public static dynamic RestartAppIfNecessary(object SteamUtilsGetAppID) { return null; }
        public static dynamic RunCallbacks() { return null; }
        public static dynamic Shutdown() { return null; }
    }
}

// Global-namespace Steam types used by the unmodified game source.
// We deliberately DO NOT compile the game's Steam wrapper project (DGSteam) nor
// reference Steamworks.NET, because Steam is non-functional on Android (no Steam
// client / native steam_api). These inert, self-contained stubs let the game
// compile and load unchanged. All members are dynamic/params-object so any call
// site (including lambda callbacks) binds; runtime Steam calls are never reached
// on Android because the game guards them behind Steam.IsInitialized (false here).

public enum SteamResult { OK, Fail, FileNotFound, AccessDenied }
public enum WorkshopItemState { None, Subscribed, Installed, NeedsUpdate, Downloading }
public enum SteamUserState { Offline, Online, Busy, Away, Snooze, LookingToTrade, LookingToPlay }
public enum FriendRelationship { None, Blocked, RequestRecipient, Friend, RequestInitiator, Ignored, IgnoredFriend }
public enum ItemUpdateStatus { Invalid, PreparingConfig, PreparingContent, UploadingContent, UploadingPreviewFile, CommittingChanges }

public static class Lobby
{
    public delegate void ChatMessageDelegate(params object[] args);
    public delegate void UserStatusChangeDelegate(params object[] args);
}

public static class User
{
    public static dynamic GetUser(params object[] args) { return null; }
}

public static class WorkshopItem
{
    public static dynamic GetItem(params object[] args) { return null; }
}

public static class Steam
{
    public delegate void ConnectionFailedDelegate(params object[] args);
    public delegate void ConnectionRequestedDelegate(params object[] args);
    public delegate void InviteReceivedDelegate(params object[] args);
    public delegate void LobbySearchCompleteDelegate(params object[] args);
    public delegate void RemotePlayDelegate(params object[] args);
    public delegate void RequestCurrentStatsDelegate(params object[] args);
    public delegate void TextEntryCompleteDelegate(params object[] args);
    public static dynamic ConnectionFailed { get; set; }
    public static dynamic ConnectionRequested { get; set; }
    public static dynamic Debug { get; set; }
    public static dynamic InviteReceived { get; set; }
    public static dynamic LobbySearchComplete { get; set; }
    public static dynamic RemotePlay { get; set; }
    public static dynamic RequestCurrentStatsComplete { get; set; }
    public static dynamic TextEntryComplete { get; set; }
    public static dynamic dll { get; set; }
    public static dynamic friends { get; set; }
    public static dynamic lobbiesFound { get; set; }
    public static dynamic lobby { get; set; }
    public static dynamic lobbySearchComplete { get; set; }
    public static dynamic lobbySearchResult { get; set; }
    public static dynamic user { get; set; }
    public static dynamic waitingForGlobalStats { get; set; }
    public static dynamic AcceptConnection(params object[] args) { return null; }
    public static dynamic AddLobbyNearFilter(params object[] args) { return null; }
    public static dynamic AddLobbyNumericalFilter(params object[] args) { return null; }
    public static dynamic AddLobbyStringFilter(params object[] args) { return null; }
    public static dynamic Authorize(params object[] args) { return null; }
    public static dynamic CloseConnection(params object[] args) { return null; }
    public static dynamic CloudEnabled(params object[] args) { return null; }
    public static dynamic CreateItem(params object[] args) { return null; }
    public static dynamic CreateLobby(params object[] args) { return null; }
    public static dynamic CreateQueryAll(params object[] args) { return null; }
    public static dynamic CreateQueryUser(params object[] args) { return null; }
    public static dynamic DownloadWorkshopItem(params object[] args) { return null; }
    public static dynamic EstimatePing(params object[] args) { return null; }
    public static dynamic FileDelete(params object[] args) { return null; }
    public static dynamic FileExists(params object[] args) { return null; }
    public static dynamic FileGetCount(params object[] args) { return null; }
    public static dynamic FileGetName(params object[] args) { return null; }
    public static dynamic FileRead(params object[] args) { return null; }
    public static dynamic FileTimestamp(params object[] args) { return null; }
    public static dynamic FileWrite(params object[] args) { return null; }
    public static dynamic FilterText(params object[] args) { return null; }
    public static dynamic GetAchievement(params object[] args) { return null; }
    public static dynamic GetAllWorkshopItems(params object[] args) { return null; }
    public static dynamic GetDailyGlobalStat(params object[] args) { return null; }
    public static dynamic GetGameBuildID(params object[] args) { return null; }
    public static dynamic GetLocalPingString(params object[] args) { return null; }
    public static dynamic GetNumWorkshopItems(params object[] args) { return null; }
    public static dynamic GetSearchLobbyAtIndex(params object[] args) { return null; }
    public static dynamic GetSessionState(params object[] args) { return null; }
    public static dynamic GetStat(params object[] args) { return null; }
    public static dynamic Initialize(params object[] args) { return null; }
    public static dynamic InitializeCore(params object[] args) { return null; }
    public static dynamic InviteUser(params object[] args) { return null; }
    public static dynamic IsInitialized(params object[] args) { return null; }
    public static dynamic IsLoggedIn(params object[] args) { return null; }
    public static dynamic IsRunningInitializeProcedures(params object[] args) { return null; }
    public static dynamic JoinLobby(params object[] args) { return null; }
    public static dynamic LeaveLobby(params object[] args) { return null; }
    public static dynamic MarkForUpdateCheck(params object[] args) { return null; }
    public static dynamic OverlayOpenURL(params object[] args) { return null; }
    public static dynamic ReadPacket(params object[] args) { return null; }
    public static dynamic RequestGlobalStats(params object[] args) { return null; }
    public static dynamic RequestWorkshopInfo(params object[] args) { return null; }
    public static dynamic SearchForLobby(params object[] args) { return null; }
    public static dynamic SearchForLobbyWorldwide(params object[] args) { return null; }
    public static dynamic SendLobbyMessage(params object[] args) { return null; }
    public static dynamic SendPacket(params object[] args) { return null; }
    public static dynamic SetAchievement(params object[] args) { return null; }
    public static dynamic SetStat(params object[] args) { return null; }
    public static dynamic ShowOnscreenKeyboard(params object[] args) { return null; }
    public static dynamic ShowWorkshopLegalAgreement(params object[] args) { return null; }
    public static dynamic StoreStats(params object[] args) { return null; }
    public static dynamic Terminate(params object[] args) { return null; }
    public static dynamic ToString(params object[] args) { return null; }
    public static dynamic Update(params object[] args) { return null; }
    public static dynamic WorkshopAddDependency(params object[] args) { return null; }
    public static dynamic WorkshopRemoveDependency(params object[] args) { return null; }
    public static dynamic WorkshopSubscribe(params object[] args) { return null; }
    public static dynamic WorkshopUnsubscribe(params object[] args) { return null; }
}

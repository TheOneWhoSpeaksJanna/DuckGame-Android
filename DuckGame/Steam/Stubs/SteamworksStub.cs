// Compile-time stub for the Steamworks.NET public API used by the game's Steam
// wrapper (DGSteam). Steam is non-functional on Android (no Steam client / native
// steam_api), so these inert shims let DGSteam.dll compile and load without the
// native assembly. All members return object / use exact signatures so any call
// site binds; the game guards real Steam usage behind Steam.IsInitialized.
using System;
namespace Steamworks
{
    public enum EFriendFlags { k_EFriendFlagBlocked, k_EFriendFlagFriendshipRequested, k_EFriendFlagImmediate, k_EFriendFlagClanMember, k_EFriendFlagOnGameServer, k_EFriendFlagHasPlayedWith, k_EFriendFlagFriend, k_EFriendFlagRequestingFriendship, k_EFriendFlagRequestingFriendshipFromYou, k_EFriendFlagIgnored, k_EFriendFlagIgnoredFriend, k_EFriendFlagSuggested, k_EFriendFlagAll }
    public enum EResult { k_EResultOK, k_EResultFail, k_EResultNoConnection, k_EResultInvalidPassword, k_EResultLoggedInElsewhere, k_EResultInvalidProtocolVer, k_EResultInvalidParam, k_EResultFileNotFound, k_EResultBusy, k_EResultInvalidState, k_EResultAccessDenied }
    public enum ELobbyType { k_ELobbyTypePrivate, k_ELobbyTypeFriendsOnly, k_ELobbyTypePublic, k_ELobbyTypeInvisible, k_ELobbyTypePrivateUnique }
    public enum ELobbyDistanceFilter { k_ELobbyDistanceFilterClose, k_ELobbyDistanceFilterDefault, k_ELobbyDistanceFilterFar, k_ELobbyDistanceFilterWorldwide }
    public enum ELobbyComparison { k_ELobbyComparisonEqualToOrLessThan, k_ELobbyComparisonLessThan, k_ELobbyComparisonEqual, k_ELobbyComparisonGreaterThan, k_ELobbyComparisonEqualToOrGreaterThan, k_ELobbyComparisonNotEqual }
    public enum EWorkshopFileType { k_EWorkshopFileTypeFirst, k_EWorkshopFileTypeCommunity, k_EWorkshopFileTypeMicrotransaction, k_EWorkshopFileTypeCollection, k_EWorkshopFileTypeArt, k_EWorkshopFileTypeVideo, k_EWorkshopFileTypeScreenshot, k_EWorkshopFileTypeGame, k_EWorkshopFileTypeSoftware, k_EWorkshopFileTypeConcept, k_EWorkshopFileTypeWebGuide, k_EWorkshopFileTypeIntegratedGuide, k_EWorkshopFileTypeMerch, k_EWorkshopFileTypeControllerSkin, k_EWorkshopFileTypeSteamworksAccessInvite, k_EWorkshopFileTypeManagedSeries, k_EWorkshopFileTypeSeries }
    public enum EItemStatistic { k_EItemStatistic_NumSubscriptions, k_EItemStatistic_NumFavorites, k_EItemStatistic_NumFollowers, k_EItemStatistic_NumUniqueSubscriptions, k_EItemStatistic_NumUniqueFavorites, k_EItemStatistic_NumUniqueFollowers, k_EItemStatistic_NumUniqueWebsiteViews, k_EItemStatistic_ReportScore, k_EItemStatistic_NumSecondsPlayed, k_EItemStatistic_NumPlaytimeSessions, k_EItemStatistic_NumComments, k_EItemStatistic_NumSecondsPlayedDuringTimePeriod, k_EItemStatistic_NumPlaytimeSessionsDuringTimePeriod }
    public enum ERemoteStoragePublishedFileVisibility { k_ERemoteStoragePublishedFileVisibilityPublic, k_ERemoteStoragePublishedFileVisibilityFriendsOnly, k_ERemoteStoragePublishedFileVisibilityPrivate, k_ERemoteStoragePublishedFileVisibilityUnlisted }
    public enum EGamepadTextInputMode { k_EGamepadTextInputModeNormal, k_EGamepadTextInputModePassword }
    public enum EItemState { k_EItemStateNone, k_EItemStateSubscribed, k_EItemStateLegacyItem, k_EItemStateInstalled, k_EItemStateNeedsUpdate, k_EItemStateDownloading, k_EItemStateDownloadPending }
    public enum EChatEntryType { k_EChatEntryTypeInvalid, k_EChatEntryTypeChatMsg, k_EChatEntryTypeTyping, k_EChatEntryTypeInviteGame, k_EChatEntryTypeEmote, k_EChatEntryTypeLeftConversation, k_EChatEntryTypeEntered, k_EChatEntryTypeWasKicked, k_EChatEntryTypeWasBanned, k_EChatEntryTypeDisconnected, k_EChatEntryTypeHistoricalChat, k_EChatEntryTypeReserved1, k_EChatEntryTypeReserved2, k_EChatEntryTypeLinkBlocked }
    public enum EUGCMatchingUGCType { k_EUGCMatchingUGCType_Items, k_EUGCMatchingUGCType_Items_Mtx, k_EUGCMatchingUGCType_Items_ReadyToUse, k_EUGCMatchingUGCType_Collections, k_EUGCMatchingUGCType_Artwork, k_EUGCMatchingUGCType_Videos, k_EUGCMatchingUGCType_Screenshots, k_EUGCMatchingUGCType_AllGuides, k_EUGCMatchingUGCType_WebGuides, k_EUGCMatchingUGCType_IntegratedGuides, k_EUGCMatchingUGCType_UsableInGame, k_EUGCMatchingUGCType_ControllerBindings, k_EUGCMatchingUGCType_GameManagedItems, k_EUGCMatchingUGCType_All }
    public enum EUserUGCList { k_EUserUGCList_Published, k_EUserUGCList_VotedOn, k_EUserUGCList_VotedUp, k_EUserUGCList_VotedDown, k_EUserUGCList_WillVoteLater, k_EUserUGCList_Favorited, k_EUserUGCList_Subscribed, k_EUserUGCList_UsedOrPlayed, k_EUserUGCList_Followed, k_EUserUGCList_Moderated, k_EUserUGCList_MySupported, k_EUserUGCList_MyFavorites }
    public enum EUGCQuery { k_EUGCQuery_RankedByVote, k_EUGCQuery_RankedByPublicationDate, k_EUGCQuery_AcceptedForGameRankedByAcceptanceDate, k_EUGCQuery_RankedByTrend, k_EUGCQuery_FavoritedByFriendsRankedByPublicationDate, k_EUGCQuery_CreatedByFriendsRankedByPublicationDate, k_EUGCQuery_RankedByNumTimesReported, k_EUGCQuery_CreatedByFollowedUsersRankedByPublicationDate, k_EUGCQuery_NotYetRated, k_EUGCQuery_RankedByTotalVotesAsc, k_EUGCQuery_RankedByVotesUp, k_EUGCQuery_RankedByTextSearch, k_EUGCQuery_RankedByTotalUniqueSubscriptions, k_EUGCQuery_RankedByPlaytimeTrait, k_EUGCQuery_RankedByTotalPlaytime, k_EUGCQuery_RankedByAveragePlaytimeTrait, k_EUGCQuery_RankedByLifetimeAveragePlaytime, k_EUGCQuery_RankedByLifetimePlaytime, k_EUGCQuery_RankedByInappropriateContentBiased, k_EUGCQuery_RankedByIgnoreBelowContentBiased, k_EUGCQuery_RankedBySubscribers, k_EUGCQuery_RankedByHarmScore, k_EUGCQuery_RankedByBestContent, k_EUGCQuery_RankedByPlaytimeTrend, k_EUGCQuery_RankedByPurchaseTrend }
    public enum EAccountType { k_EAccountTypeInvalid, k_EAccountTypeIndividual, k_EAccountTypeMultiseat, k_EAccountTypeGameServer, k_EAccountTypeAnonGameServer, k_EAccountTypePending, k_EAccountTypeContentServer, k_EAccountTypeClan, k_EAccountTypeChat, k_EAccountTypeConsoleUser, k_EAccountTypeAnonUser }
    public enum EP2PSessionError { k_EP2PSessionErrorNone, k_EP2PSessionErrorNotConnected, k_EP2PSessionErrorOverflow, k_EP2PSessionErrorTimeout, k_EP2PSessionErrorRelay, k_EP2PSessionErrorLocalPolicy, k_EP2PSessionErrorMax }
    public enum EChatRoomEnterResponse { k_EChatRoomEnterResponseSuccess, k_EChatRoomEnterResponseDoesntExist, k_EChatRoomEnterResponseNotAllowed, k_EChatRoomEnterResponseFull, k_EChatRoomEnterResponseError, k_EChatRoomEnterResponseBanned, k_EChatRoomEnterResponseLimited, k_EChatRoomEnterResponseClanDisabled, k_EChatRoomEnterResponseCommunityBan, k_EChatRoomEnterResponseMemberBlockedYou, k_EChatRoomEnterResponseYouBlockedMember, k_EChatRoomEnterResponseRatelimitExceeded }
    public enum EChatMemberStateChange { k_EChatMemberStateChangeEntered, k_EChatMemberStateChangeLeft, k_EChatMemberStateChangeDisconnected, k_EChatMemberStateChangeKicked, k_EChatMemberStateChangeBanned }
    public enum ELeaderboardUploadScoreMethod { k_ELeaderboardUploadScoreMethodNone, k_ELeaderboardUploadScoreMethodKeepBest, k_ELeaderboardUploadScoreMethodForceUpdate }
    public enum EGlobalStatType { k_EGlobalStatTypeInvalid, k_EGlobalStatTypeInt, k_EGlobalStatTypeFloat, k_EGlobalStatTypeAVGRate }
    public enum ESteamItemFlags { k_ESteamItemNoTrade, k_ESteamItemRemoved, k_ESteamItemConsumed, k_ESteamItemStored, k_ESteamItemPlayerBag, k_ESteamItemPackage }
    public enum EUserUGCListSortOrder { k_EUserUGCListSortOrder_CreationOrderDesc, k_EUserUGCListSortOrder_CreationOrderAsc, k_EUserUGCListSortOrder_TitleAsc, k_EUserUGCListSortOrder_LastUpdatedDesc, k_EUserUGCListSortOrder_SubscriptionDateDesc, k_EUserUGCListSortOrder_VoteScoreDesc, k_EUserUGCListSortOrder_ForModeration }
    public enum EItemPreviewType { k_EItemPreviewType_Image, k_EItemPreviewType_YouTubeVideo, k_EItemPreviewType_Sketchfab, k_EItemPreviewType_EnvironmentMap, k_EItemPreviewType_Max }
    public enum EItemUpdateStatus { CommittingConfig, ValidatingConfig, CreatingContent, PreparingContent, PreparingConfig, UploadingContent, UploadingPreviewFile, CommittingChanges }
    public enum EGamepadTextInputLineMode { k_EGamepadTextInputLineModeSingleLine, k_EGamepadTextInputLineModeMultipleLines }
    public enum EP2PSend { k_EP2PSendUnreliable, k_EP2PSendReliable, k_EP2PSendUnreliableNoDelay, k_EP2PSendReliableWithBuffering }

    public struct CSteamID
    {
        public ulong m_SteamID;
        public CSteamID(object h) { m_SteamID = 0; }
        public static implicit operator CSteamID(ulong v) => new CSteamID(v);
        public static explicit operator ulong(CSteamID v) => v.m_SteamID;
    }

    public struct CGameID
    {
        public ulong m_GameID;
        public CGameID(object h) { m_GameID = 0; }
        public static implicit operator CGameID(ulong v) => new CGameID(v);
        public static explicit operator ulong(CGameID v) => v.m_GameID;
    }

    public struct PublishedFileId_t
    {
        public ulong m_PublishedFileId;
        public PublishedFileId_t(object h) { m_PublishedFileId = 0; }
        public static implicit operator PublishedFileId_t(ulong v) => new PublishedFileId_t(v);
    }

    public struct AccountID_t
    {
        public uint m_AccountID;
        public AccountID_t(object h) { m_AccountID = 0; }
        public static implicit operator AccountID_t(ulong v) => new AccountID_t(v);
        public static explicit operator uint(AccountID_t v) => v.m_AccountID;
    }

    public struct AppId_t
    {
        public uint m_AppId;
        public AppId_t(object h) { m_AppId = 0; }
        public static implicit operator AppId_t(ulong v) => new AppId_t(v);
        public static explicit operator uint(AppId_t v) => v.m_AppId;
    }

    public struct UGCUpdateHandle_t
    {
        public ulong m_UGCUpdateHandle;
        public UGCUpdateHandle_t(object h) { m_UGCUpdateHandle = 0; }
        public static implicit operator UGCUpdateHandle_t(ulong v) => new UGCUpdateHandle_t(v);
    }

    public struct UGCQueryHandle_t
    {
        public ulong m_UGCQueryHandle;
        public UGCQueryHandle_t(object h) { m_UGCQueryHandle = 0; }
        public static implicit operator UGCQueryHandle_t(ulong v) => new UGCQueryHandle_t(v);
    }

    public struct SteamAPICall_t
    {
        public ulong m_SteamAPICall;
        public SteamAPICall_t(object h) { m_SteamAPICall = 0; }
        public static implicit operator SteamAPICall_t(ulong v) => new SteamAPICall_t(v);
    }

    public struct SteamNetworkPingLocation_t
    {
        public byte[] m_data;
        public SteamNetworkPingLocation_t(object h) { m_data = null; }
        public static implicit operator SteamNetworkPingLocation_t(ulong v) => new SteamNetworkPingLocation_t(v);
    }

    public struct P2PSessionState_t
    {
        public byte m_bConnecting;
        public byte m_bConnectionActive;
        public byte m_bConnectionExists;
        public byte m_nConnectionState;
        public int m_nBytesSent;
        public int m_nPacketsSent;
        public int m_nBytesQueued;
        public int m_nPacketsQueued;
        public int m_nBytesRecv;
        public int m_nPacketsRecv;
        public byte m_bUsingRelay;
        public byte m_eP2PSessionError;
        public int m_nBytesQueuedForSend;
        public P2PSessionState_t(object h) { m_bConnecting = 0; m_bConnectionActive = 0; m_bConnectionExists = 0; m_nConnectionState = 0; m_nBytesSent = 0; m_nPacketsSent = 0; m_nBytesQueued = 0; m_nPacketsQueued = 0; m_nBytesRecv = 0; m_nPacketsRecv = 0; m_bUsingRelay = 0; m_eP2PSessionError = 0; m_nBytesQueuedForSend = 0; m_nRemoteIP = 0; m_nRemotePort = 0; }
        public static implicit operator P2PSessionState_t(ulong v) => default;
    }

    public struct FriendGameInfo_t
    {
        public object m_steamIDLobby;
        public object m_rgchGameName;
        public object m_gameID;
        public FriendGameInfo_t(object h) { }
        public static implicit operator FriendGameInfo_t(ulong v) => default;
    }

    public struct CreateItemResult_t
    {
        public object m_bUserNeedsToAcceptWorkshopLegalAgreement;
        public object m_eResult;
        public object m_nPublishedFileId;
        public CreateItemResult_t(object h) { }
        public static implicit operator CreateItemResult_t(ulong v) => default;
    }

    public struct DownloadItemResult_t
    {
        public object m_eResult;
        public object m_nPublishedFileId;
        public object m_unAppId;
        public DownloadItemResult_t(object h) { }
        public static implicit operator DownloadItemResult_t(ulong v) => default;
    }

    public struct LobbyChatMsg_t
    {
        public object m_iChatID;
        public object m_ulSteamIDLobby;
        public object m_ulSteamIDUser;
        public LobbyChatMsg_t(object h) { }
        public static implicit operator LobbyChatMsg_t(ulong v) => default;
    }

    public struct LobbyChatUpdate_t
    {
        public object m_rgfChatMemberStateChange;
        public object m_ulSteamIDLobby;
        public object m_ulSteamIDMakingChange;
        public object m_ulSteamIDUserChanged;
        public LobbyChatUpdate_t(object h) { }
        public static implicit operator LobbyChatUpdate_t(ulong v) => default;
    }

    public struct LobbyCreated_t
    {
        public object m_eResult;
        public object m_ulSteamIDLobby;
        public LobbyCreated_t(object h) { }
        public static implicit operator LobbyCreated_t(ulong v) => default;
    }

    public struct LobbyEnter_t
    {
        public object m_EChatRoomEnterResponse;
        public object m_bLocked;
        public object m_rgfChatPermissions;
        public object m_ulSteamIDLobby;
        public LobbyEnter_t(object h) { }
        public static implicit operator LobbyEnter_t(ulong v) => default;
    }

    public struct LobbyMatchList_t
    {
        public object m_nLobbiesMatching;
        public LobbyMatchList_t(object h) { }
        public static implicit operator LobbyMatchList_t(ulong v) => default;
    }

    public struct P2PSessionConnectFail_t
    {
        public object m_eP2PSessionError;
        public object m_steamIDRemote;
        public P2PSessionConnectFail_t(object h) { }
        public static implicit operator P2PSessionConnectFail_t(ulong v) => default;
    }

    public struct P2PSessionRequest_t
    {
        public object m_steamIDRemote;
        public P2PSessionRequest_t(object h) { }
        public static implicit operator P2PSessionRequest_t(ulong v) => default;
    }

    public struct SubmitItemUpdateResult_t
    {
        public object m_bUserNeedsToAcceptWorkshopLegalAgreement;
        public object m_eResult;
        public SubmitItemUpdateResult_t(object h) { }
        public static implicit operator SubmitItemUpdateResult_t(ulong v) => default;
    }

    public struct UserStatsReceived_t
    {
        public object m_eResult;
        public object m_nGameID;
        public object m_steamIDUser;
        public UserStatsReceived_t(object h) { }
        public static implicit operator UserStatsReceived_t(ulong v) => default;
    }

    public struct GameLobbyJoinRequested_t
    {
        public object m_steamIDFriend;
        public object m_steamIDLobby;
        public GameLobbyJoinRequested_t(object h) { }
        public static implicit operator GameLobbyJoinRequested_t(ulong v) => default;
    }

    public struct GamepadTextInputDismissed_t
    {
        public object m_bSubmitted;
        public object m_unSubmittedText;
        public GamepadTextInputDismissed_t(object h) { }
        public static implicit operator GamepadTextInputDismissed_t(ulong v) => default;
    }

    public struct SteamRemotePlaySessionConnected_t
    {
        public object m_unSessionID;
        public SteamRemotePlaySessionConnected_t(object h) { }
        public static implicit operator SteamRemotePlaySessionConnected_t(ulong v) => default;
    }

    public struct GlobalStatsReceived_t
    {
        public object m_eResult;
        public object m_nGameID;
        public GlobalStatsReceived_t(object h) { }
        public static implicit operator GlobalStatsReceived_t(ulong v) => default;
    }

    public struct SteamUGCDetails_t
    {
        public object m_bBanned;
        public object m_eVisibility;
        public object m_fScore;
        public object m_nFileSize;
        public object m_nPublishedFileId;
        public object m_nVotesDown;
        public object m_nVotesUp;
        public object m_rgchDescription;
        public object m_rgchDetails;
        public object m_rgchOwner;
        public object m_rgchTags;
        public object m_rgchTitle;
        public object m_rtimeUpdated;
        public object m_ulLastUpdated;
        public object m_ulSteamIDOwner;
        public object m_unNumChildren;
        public object m_unNumPreview;
        public object m_unVotesUp;
        public object m_unTotalMatchingResults;
        public object m_rtimeCreated;
        public object m_rtimeAddedToUserList;
        public object m_rgchURL;
        public object m_pchFileName;
        public object m_nPreviewFileSize;
        public object m_hPreviewFile;
        public object m_hFile;
        public object m_eResult;
        public object m_eFileType;
        public object m_unVotesDown;
        public object m_flScore;
        public object m_bTagsTruncated;
        public object m_bAcceptedForUse;
        public SteamUGCDetails_t(object h) { }
        public static implicit operator SteamUGCDetails_t(ulong v) => default;
    }

    public struct SteamUGCQueryCompleted_t
    {
        public object m_eResult;
        public object m_handle;
        public object m_unNumResultsReturned;
        public object m_unTotalMatchingResults;
        public SteamUGCQueryCompleted_t(object h) { }
        public static implicit operator SteamUGCQueryCompleted_t(ulong v) => default;
    }

    public struct SteamUGCRequestUGCDetailsResult_t
    {
        public object m_details;
        public object m_eResult;
        public SteamUGCRequestUGCDetailsResult_t(object h) { }
        public static implicit operator SteamUGCRequestUGCDetailsResult_t(ulong v) => default;
    }

    public static class CallbackDispatcher
    {
        public static bool IsInitialized => false;
    }

    public class CallResult<T>
    {
        public delegate void APIDispatchDelegate(T result, bool ioFailure);
        public static CallResult<T> Create(APIDispatchDelegate func) { return null; }
        public CallResult(APIDispatchDelegate d = null) { }
        public void Set(SteamAPICall_t hAPICall, APIDispatchDelegate d = null) { }
        public void Cancel() { }
    }
    public class Callback<T>
    {
        public delegate void CallbackDelegate(T param);
        public static IDisposable Create(CallbackDelegate func) { return null; }
    }

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
    public class ISteamInventory { }
    public class ISteamVideo { }
    public class ISteamParentalSettings { }
    public class ISteamAppList { }
    public class ISteamMatchmakingServers { }
    public class ISteamGameSearch { }
    public class ISteamInput { }
    public class ISteamParties { }
    public class ISteamRemotePlay { }
    public class ISteamScreenshots { }

    public static class SteamAPI
    {
        public static object Init()
        {
        return default;
        }
        public static object RestartAppIfNecessary(object p0)
        {
        return default;
        }
        public static object RunCallbacks()
        {
        return default;
        }
        public static object Shutdown()
        {
        return default;
        }
    }
    public static class SteamApps
    {
        public static object BIsSubscribedApp(object p0)
        {
        return default;
        }
        public static object GetAppBuildId()
        {
        return default;
        }
        public static object MarkContentCorrupt(object p0)
        {
        return default;
        }
    }
    public static class SteamFriends
    {
        public static object GetFriendGamePlayed(object p0, out FriendGameInfo_t game)
        {
        game = default;
        return default;
        }
        public static int GetFriendCount(object p0)
        {
        return default;
        }
        public static CSteamID GetFriendByIndex(int p0, EFriendFlags p1)
        {
        return default;
        }
        public static object ActivateGameOverlayInviteDialog(object p0)
        {
        return default;
        }
        public static object ActivateGameOverlayToWebPage(object p0)
        {
        return default;
        }
        public static object GetFriendPersonaName(object p0)
        {
        return default;
        }
        public static object GetFriendPersonaState(object p0)
        {
        return default;
        }
        public static object GetFriendRelationship(object p0)
        {
        return default;
        }
        public static object GetMediumFriendAvatar(object p0)
        {
        return default;
        }
        public static object GetPersonaName()
        {
        return default;
        }
        public static object GetSmallFriendAvatar(object p0)
        {
        return default;
        }
    }
    public static class SteamMatchmaking
    {
        public static int GetNumLobbyMembers(CSteamID p0)
        {
        return default;
        }
        public static CSteamID GetLobbyMemberByIndex(CSteamID p0, int p1)
        {
        return default;
        }
        public static object AddRequestLobbyListCompatibleMembersFilter(object p0)
        {
        return default;
        }
        public static object AddRequestLobbyListDistanceFilter(object p0)
        {
        return default;
        }
        public static object AddRequestLobbyListFilterSlotsAvailable(object p0)
        {
        return default;
        }
        public static object AddRequestLobbyListNearValueFilter(object p0, object p1)
        {
        return default;
        }
        public static object AddRequestLobbyListNumericalFilter(object p0, object p1, object p2)
        {
        return default;
        }
        public static object AddRequestLobbyListResultCountFilter(object p0)
        {
        return default;
        }
        public static object AddRequestLobbyListStringFilter(object p0, object p1, object p2)
        {
        return default;
        }
        public static object CreateLobby(object p0, object p1)
        {
        return default;
        }
        public static object GetLobbyByIndex(object p0)
        {
        return default;
        }
        public static object GetLobbyChatEntry(object p0, object p1, out CSteamID PlayerID, object p3, object p4, out EChatEntryType peChatEntryType)
        {
        PlayerID = default;
        peChatEntryType = default;
        return default;
        }
        public static object GetLobbyData(object p0, object p1)
        {
        return default;
        }
        public static object GetLobbyMemberLimit(object p0)
        {
        return default;
        }
        public static object GetLobbyOwner(object p0)
        {
        return default;
        }
        public static object InviteUserToLobby(object p0, object p1)
        {
        return default;
        }
        public static object JoinLobby(object p0)
        {
        return default;
        }
        public static object LeaveLobby(object p0)
        {
        return default;
        }
        public static object RequestLobbyList()
        {
        return default;
        }
        public static object SendLobbyChatMsg(object p0, object p1, object p2)
        {
        return default;
        }
        public static object SetLobbyData(object p0, object p1, object p2)
        {
        return default;
        }
        public static object SetLobbyJoinable(object p0, object p1)
        {
        return default;
        }
        public static object SetLobbyMemberLimit(object p0, object p1)
        {
        return default;
        }
        public static object SetLobbyOwner(object p0, object p1)
        {
        return default;
        }
        public static object SetLobbyType(object p0, object p1)
        {
        return default;
        }
    }
    public static class SteamNetworking
    {
        public static object ReadP2PPacket(byte[] p0, uint p1, out uint p2, out CSteamID p3)
        {
        p2 = default;
        p3 = default;
        return default;
        }
        public static object GetP2PSessionState(CSteamID p0, out P2PSessionState_t ConnectionState)
        {
        ConnectionState = default;
        return default;
        }
        public static object IsP2PPacketAvailable(out uint p0)
        {
        p0 = default;
        return default;
        }
        public static object SendP2PPacket(CSteamID p0, byte[] p1, uint p2, EP2PSend p3)
        {
        return default;
        }
        public static object AcceptP2PSessionWithUser(object p0)
        {
        return default;
        }
        public static object CloseP2PSessionWithUser(object p0)
        {
        return default;
        }
    }
    public static class SteamNetworkingUtils
    {
        public static object ParsePingLocationString(string p0, out SteamNetworkPingLocation_t pingL)
        {
        pingL = default;
        return default;
        }
        public static object EstimatePingTimeFromLocalHost(ref SteamNetworkPingLocation_t pingL)
        {
        pingL = default;
        return default;
        }
        public static object GetLocalPingLocation(out SteamNetworkPingLocation_t pingL)
        {
        pingL = default;
        return default;
        }
        public static object ConvertPingLocationToString(ref SteamNetworkPingLocation_t pingL, out string pszbuff, uint p2)
        {
        pingL = default;
        pszbuff = default;
        return default;
        }
        public static object InitRelayNetworkAccess()
        {
        return default;
        }
    }
    public static class SteamRemoteStorage
    {
        public static string GetFileNameAndSize(int p0, out int p1)
        {
        p1 = default;
        return default;
        }
        public static object FileDelete(object p0)
        {
        return default;
        }
        public static object FileExists(object p0)
        {
        return default;
        }
        public static object FileRead(object p0, object p1, object p2)
        {
        return default;
        }
        public static object FileWrite(object p0, object p1, object p2)
        {
        return default;
        }
        public static object GetFileCount()
        {
        return default;
        }
        public static object GetFileSize(object p0)
        {
        return default;
        }
        public static object GetFileTimestamp(object p0)
        {
        return default;
        }
        public static object IsCloudEnabledForAccount()
        {
        return default;
        }
        public static object IsCloudEnabledForApp()
        {
        return default;
        }
    }
    public static class SteamUGC
    {
        public static object GetItemInstallInfo(object p0, out ulong SizeOnDisk, out string Folder, object p3, out uint punTimeStamp)
        {
        SizeOnDisk = default;
        Folder = default;
        punTimeStamp = default;
        return default;
        }
        public static object GetQueryUGCResult(object p0, object p1, out SteamUGCDetails_t details)
        {
        details = default;
        return default;
        }
        public static object GetQueryUGCPreviewURL(object p0, object p1, out string previewURL, object p3)
        {
        previewURL = default;
        return default;
        }
        public static object GetQueryUGCMetadata(object p0, object p1, out string metadata, object p3)
        {
        metadata = default;
        return default;
        }
        public static object GetQueryUGCAdditionalPreview(object p0, object p1, object p2, out string url, object p4, out string name, object p6, out EItemPreviewType type)
        {
        url = default;
        name = default;
        type = default;
        return default;
        }
        public static object GetQueryUGCStatistic(object p0, object p1, object p2, out ulong val)
        {
        val = default;
        return default;
        }
        public static object GetItemDownloadInfo(object p0, out ulong bytesDownloaded, out ulong bytesTotal)
        {
        bytesDownloaded = default;
        bytesTotal = default;
        return default;
        }
        public static object GetItemUpdateProgress(object p0, out ulong bytesDownloaded, out ulong bytesTotal)
        {
        bytesDownloaded = default;
        bytesTotal = default;
        return default;
        }
        public static object AddDependency(object p0, object p1)
        {
        return default;
        }
        public static object AddExcludedTag(object p0, object p1)
        {
        return default;
        }
        public static object AddRequiredTag(object p0, object p1)
        {
        return default;
        }
        public static object CreateItem(object p0, object p1)
        {
        return default;
        }
        public static object CreateQueryAllUGCRequest(object p0, object p1, object p2, object p3, object p4)
        {
        return default;
        }
        public static object CreateQueryUGCDetailsRequest(object p0, object p1)
        {
        return default;
        }
        public static object CreateQueryUserUGCRequest(object p0, object p1, object p2, object p3, object p4, object p5, object p6)
        {
        return default;
        }
        public static object DownloadItem(object p0, object p1)
        {
        return default;
        }
        public static object GetItemState(object p0)
        {
        return default;
        }
        public static object GetNumSubscribedItems()
        {
        return default;
        }
        public static object GetQueryUGCChildren(object p0, object p1, object p2, object p3)
        {
        return default;
        }
        public static object GetQueryUGCNumAdditionalPreviews(object p0, object p1)
        {
        return default;
        }
        public static object GetSubscribedItems(object p0, object p1)
        {
        return default;
        }
        public static object ReleaseQueryUGCRequest(object p0)
        {
        return default;
        }
        public static object RemoveDependency(object p0, object p1)
        {
        return default;
        }
        public static object SendQueryUGCRequest(object p0)
        {
        return default;
        }
        public static object SetAllowCachedResponse(object p0, object p1)
        {
        return default;
        }
        public static object SetCloudFileNameFilter(object p0, object p1)
        {
        return default;
        }
        public static object SetItemContent(object p0, object p1)
        {
        return default;
        }
        public static object SetItemDescription(object p0, object p1)
        {
        return default;
        }
        public static object SetItemPreview(object p0, object p1)
        {
        return default;
        }
        public static object SetItemTags(object p0, object p1)
        {
        return default;
        }
        public static object SetItemTitle(object p0, object p1)
        {
        return default;
        }
        public static object SetItemVisibility(object p0, object p1)
        {
        return default;
        }
        public static object SetMatchAnyTag(object p0, object p1)
        {
        return default;
        }
        public static object SetRankedByTrendDays(object p0, object p1)
        {
        return default;
        }
        public static object SetReturnAdditionalPreviews(object p0, object p1)
        {
        return default;
        }
        public static object SetReturnChildren(object p0, object p1)
        {
        return default;
        }
        public static object SetReturnLongDescription(object p0, object p1)
        {
        return default;
        }
        public static object SetReturnMetadata(object p0, object p1)
        {
        return default;
        }
        public static object SetReturnOnlyIDs(object p0, object p1)
        {
        return default;
        }
        public static object SetReturnTotalOnly(object p0, object p1)
        {
        return default;
        }
        public static object SetSearchText(object p0, object p1)
        {
        return default;
        }
        public static object StartItemUpdate(object p0, object p1)
        {
        return default;
        }
        public static object SubmitItemUpdate(object p0, object p1)
        {
        return default;
        }
        public static object SubscribeItem(object p0)
        {
        return default;
        }
        public static object UnsubscribeItem(object p0)
        {
        return default;
        }
    }
    public static class SteamUser
    {
        public static CSteamID GetSteamID()
        {
        return default;
        }
    }
    public static class SteamUserStats
    {
        public static object GetStat(string p0, out float val)
        {
        val = default;
        return default;
        }
        public static object GetGlobalStat(string p0, out double val)
        {
        val = default;
        return default;
        }
        public static object GetAchievement(string p0, out bool hasAchievement)
        {
        hasAchievement = default;
        return default;
        }
        public static object GetGlobalStatHistory(object p0, object p1, object p2)
        {
        return default;
        }
        public static object GetLeaderboardName(object p0)
        {
        return default;
        }
        public static object IndicateAchievementProgress(object p0, object p1, object p2)
        {
        return default;
        }
        public static object RequestGlobalStats(object p0)
        {
        return default;
        }
        public static object SetAchievement(object p0)
        {
        return default;
        }
        public static object SetStat(object p0, object p1)
        {
        return default;
        }
        public static object StoreStats()
        {
        return default;
        }
        public static object UploadLeaderboardScore(object p0, object p1, object p2, object p3, object p4)
        {
        return default;
        }
    }
    public static class SteamUtils
    {
        public static object GetImageSize(object p0, out uint w, out uint h)
        {
        w = default;
        h = default;
        return default;
        }
        public static object GetImageRGBA(object p0, byte[] p1, int p2)
        {
        return default;
        }
        public static object GetEnteredGamepadTextInput(out string szTextInput, uint p1)
        {
        szTextInput = default;
        return default;
        }
        public static object ShowGamepadTextInput(EGamepadTextInputMode p0, EGamepadTextInputLineMode p1, string p2, uint p3, string p4)
        {
        return default;
        }
        public static object GetAppID()
        {
        return default;
        }
        public static object GetEnteredGamepadTextLength()
        {
        return default;
        }
    }
}

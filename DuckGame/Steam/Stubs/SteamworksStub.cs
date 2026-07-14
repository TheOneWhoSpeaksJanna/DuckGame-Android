// Compile-time stub for the Steamworks.NET public API used by the game's Steam
// wrapper (DGSteam). Steam is non-functional on Android (no Steam client / native
// steam_api), so these inert shims let DGSteam.dll compile and load without the
// native assembly. All members return dynamic / use exact signatures so any call
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
        public dynamic m_SteamID;
        public dynamic m_PublishedFileId;
        public dynamic m_ulSteamIDLobby;
        public CSteamID(object h) { }
        public static implicit operator CSteamID(ulong v) => default;
    }

    public struct CGameID
    {
        public dynamic m_gameID;
        public dynamic m_nAppID;
        public dynamic m_nType;
        public dynamic m_ulSteamIDLobby;
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

    public struct SteamNetworkPingLocation_t
    {
        public dynamic m_data;
        public SteamNetworkPingLocation_t(object h) { }
        public static implicit operator SteamNetworkPingLocation_t(ulong v) => default;
    }

    public struct P2PSessionState_t
    {
        public dynamic m_bConnecting;
        public dynamic m_bConnectionActive;
        public dynamic m_bConnectionExists;
        public dynamic m_nConnectionState;
        public dynamic m_nBytesSent;
        public dynamic m_nPacketsSent;
        public dynamic m_nBytesQueued;
        public dynamic m_nPacketsQueued;
        public dynamic m_nBytesRecv;
        public dynamic m_nPacketsRecv;
        public dynamic m_bUsingRelay;
        public dynamic m_eP2PSessionError;
        public dynamic m_nBytesQueuedForSend;
        public dynamic m_nPacketsQueuedForSend;
        public dynamic m_nRemoteIP;
        public dynamic m_nRemotePort;
        public P2PSessionState_t(object h) { }
        public static implicit operator P2PSessionState_t(ulong v) => default;
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
        public dynamic m_unVotesUp;
        public dynamic m_unTotalMatchingResults;
        public dynamic m_rtimeCreated;
        public dynamic m_rtimeAddedToUserList;
        public dynamic m_rgchURL;
        public dynamic m_pchFileName;
        public dynamic m_nPreviewFileSize;
        public dynamic m_hPreviewFile;
        public dynamic m_hFile;
        public dynamic m_eResult;
        public dynamic m_eFileType;
        public dynamic m_unVotesDown;
        public dynamic m_flScore;
        public dynamic m_bTagsTruncated;
        public dynamic m_bAcceptedForUse;
        public SteamUGCDetails_t(object h) { }
        public static implicit operator SteamUGCDetails_t(ulong v) => default;
    }

    public struct SteamUGCQueryCompleted_t
    {
        public dynamic m_eResult;
        public dynamic m_handle;
        public dynamic m_unNumResultsReturned;
        public dynamic m_unTotalMatchingResults;
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
        public static dynamic Init()
        {
        return default;
        }
        public static dynamic RestartAppIfNecessary(object p0)
        {
        return default;
        }
        public static dynamic RunCallbacks()
        {
        return default;
        }
        public static dynamic Shutdown()
        {
        return default;
        }
    }
    public static class SteamApps
    {
        public static dynamic BIsSubscribedApp(object p0)
        {
        return default;
        }
        public static dynamic GetAppBuildId()
        {
        return default;
        }
        public static dynamic MarkContentCorrupt(object p0)
        {
        return default;
        }
    }
    public static class SteamFriends
    {
        public static dynamic GetFriendGamePlayed(object p0, out FriendGameInfo_t game)
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
        public static dynamic ActivateGameOverlayInviteDialog(object p0)
        {
        return default;
        }
        public static dynamic ActivateGameOverlayToWebPage(object p0)
        {
        return default;
        }
        public static dynamic GetFriendPersonaName(object p0)
        {
        return default;
        }
        public static dynamic GetFriendPersonaState(object p0)
        {
        return default;
        }
        public static dynamic GetFriendRelationship(object p0)
        {
        return default;
        }
        public static dynamic GetMediumFriendAvatar(object p0)
        {
        return default;
        }
        public static dynamic GetPersonaName()
        {
        return default;
        }
        public static dynamic GetSmallFriendAvatar(object p0)
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
        public static dynamic AddRequestLobbyListCompatibleMembersFilter(object p0)
        {
        return default;
        }
        public static dynamic AddRequestLobbyListDistanceFilter(object p0)
        {
        return default;
        }
        public static dynamic AddRequestLobbyListFilterSlotsAvailable(object p0)
        {
        return default;
        }
        public static dynamic AddRequestLobbyListNearValueFilter(object p0, object p1)
        {
        return default;
        }
        public static dynamic AddRequestLobbyListNumericalFilter(object p0, object p1, object p2)
        {
        return default;
        }
        public static dynamic AddRequestLobbyListResultCountFilter(object p0)
        {
        return default;
        }
        public static dynamic AddRequestLobbyListStringFilter(object p0, object p1, object p2)
        {
        return default;
        }
        public static dynamic CreateLobby(object p0, object p1)
        {
        return default;
        }
        public static dynamic GetLobbyByIndex(object p0)
        {
        return default;
        }
        public static dynamic GetLobbyChatEntry(object p0, object p1, out CSteamID PlayerID, object p3, object p4, out EChatEntryType peChatEntryType)
        {
        PlayerID = default;
        peChatEntryType = default;
        return default;
        }
        public static dynamic GetLobbyData(object p0, object p1)
        {
        return default;
        }
        public static dynamic GetLobbyMemberLimit(object p0)
        {
        return default;
        }
        public static dynamic GetLobbyOwner(object p0)
        {
        return default;
        }
        public static dynamic InviteUserToLobby(object p0, object p1)
        {
        return default;
        }
        public static dynamic JoinLobby(object p0)
        {
        return default;
        }
        public static dynamic LeaveLobby(object p0)
        {
        return default;
        }
        public static dynamic RequestLobbyList()
        {
        return default;
        }
        public static dynamic SendLobbyChatMsg(object p0, object p1, object p2)
        {
        return default;
        }
        public static dynamic SetLobbyData(object p0, object p1, object p2)
        {
        return default;
        }
        public static dynamic SetLobbyJoinable(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetLobbyMemberLimit(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetLobbyOwner(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetLobbyType(object p0, object p1)
        {
        return default;
        }
    }
    public static class SteamNetworking
    {
        public static dynamic ReadP2PPacket(byte[] p0, uint p1, out uint p2, out CSteamID p3)
        {
        p2 = default;
        p3 = default;
        return default;
        }
        public static dynamic GetP2PSessionState(CSteamID p0, out P2PSessionState_t ConnectionState)
        {
        ConnectionState = default;
        return default;
        }
        public static dynamic IsP2PPacketAvailable(out uint p0)
        {
        p0 = default;
        return default;
        }
        public static dynamic SendP2PPacket(CSteamID p0, byte[] p1, uint p2, EP2PSend p3)
        {
        return default;
        }
        public static dynamic AcceptP2PSessionWithUser(object p0)
        {
        return default;
        }
        public static dynamic CloseP2PSessionWithUser(object p0)
        {
        return default;
        }
    }
    public static class SteamNetworkingUtils
    {
        public static dynamic ParsePingLocationString(string p0, out SteamNetworkPingLocation_t pingL)
        {
        pingL = default;
        return default;
        }
        public static dynamic EstimatePingTimeFromLocalHost(ref SteamNetworkPingLocation_t pingL)
        {
        pingL = default;
        return default;
        }
        public static dynamic GetLocalPingLocation(out SteamNetworkPingLocation_t pingL)
        {
        pingL = default;
        return default;
        }
        public static dynamic ConvertPingLocationToString(ref SteamNetworkPingLocation_t pingL, out string pszbuff, uint p2)
        {
        pingL = default;
        pszbuff = default;
        return default;
        }
        public static dynamic InitRelayNetworkAccess()
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
        public static dynamic FileDelete(object p0)
        {
        return default;
        }
        public static dynamic FileExists(object p0)
        {
        return default;
        }
        public static dynamic FileRead(object p0, object p1, object p2)
        {
        return default;
        }
        public static dynamic FileWrite(object p0, object p1, object p2)
        {
        return default;
        }
        public static dynamic GetFileCount()
        {
        return default;
        }
        public static dynamic GetFileSize(object p0)
        {
        return default;
        }
        public static dynamic GetFileTimestamp(object p0)
        {
        return default;
        }
        public static dynamic IsCloudEnabledForAccount()
        {
        return default;
        }
        public static dynamic IsCloudEnabledForApp()
        {
        return default;
        }
    }
    public static class SteamUGC
    {
        public static dynamic GetItemInstallInfo(object p0, out ulong SizeOnDisk, out string Folder, object p3, out uint punTimeStamp)
        {
        SizeOnDisk = default;
        Folder = default;
        punTimeStamp = default;
        return default;
        }
        public static dynamic GetQueryUGCResult(object p0, object p1, out SteamUGCDetails_t details)
        {
        details = default;
        return default;
        }
        public static dynamic GetQueryUGCPreviewURL(object p0, object p1, out string previewURL, object p3)
        {
        previewURL = default;
        return default;
        }
        public static dynamic GetQueryUGCMetadata(object p0, object p1, out string metadata, object p3)
        {
        metadata = default;
        return default;
        }
        public static dynamic GetQueryUGCAdditionalPreview(object p0, object p1, object p2, out string url, object p4, out string name, object p6, out EItemPreviewType type)
        {
        url = default;
        name = default;
        type = default;
        return default;
        }
        public static dynamic GetQueryUGCStatistic(object p0, object p1, object p2, out ulong val)
        {
        val = default;
        return default;
        }
        public static dynamic GetItemDownloadInfo(object p0, out ulong bytesDownloaded, out ulong bytesTotal)
        {
        bytesDownloaded = default;
        bytesTotal = default;
        return default;
        }
        public static dynamic GetItemUpdateProgress(object p0, out ulong bytesDownloaded, out ulong bytesTotal)
        {
        bytesDownloaded = default;
        bytesTotal = default;
        return default;
        }
        public static dynamic AddDependency(object p0, object p1)
        {
        return default;
        }
        public static dynamic AddExcludedTag(object p0, object p1)
        {
        return default;
        }
        public static dynamic AddRequiredTag(object p0, object p1)
        {
        return default;
        }
        public static dynamic CreateItem(object p0, object p1)
        {
        return default;
        }
        public static dynamic CreateQueryAllUGCRequest(object p0, object p1, object p2, object p3, object p4)
        {
        return default;
        }
        public static dynamic CreateQueryUGCDetailsRequest(object p0, object p1)
        {
        return default;
        }
        public static dynamic CreateQueryUserUGCRequest(object p0, object p1, object p2, object p3, object p4, object p5, object p6)
        {
        return default;
        }
        public static dynamic DownloadItem(object p0, object p1)
        {
        return default;
        }
        public static dynamic GetItemState(object p0)
        {
        return default;
        }
        public static dynamic GetNumSubscribedItems()
        {
        return default;
        }
        public static dynamic GetQueryUGCChildren(object p0, object p1, object p2, object p3)
        {
        return default;
        }
        public static dynamic GetQueryUGCNumAdditionalPreviews(object p0, object p1)
        {
        return default;
        }
        public static dynamic GetSubscribedItems(object p0, object p1)
        {
        return default;
        }
        public static dynamic ReleaseQueryUGCRequest(object p0)
        {
        return default;
        }
        public static dynamic RemoveDependency(object p0, object p1)
        {
        return default;
        }
        public static dynamic SendQueryUGCRequest(object p0)
        {
        return default;
        }
        public static dynamic SetAllowCachedResponse(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetCloudFileNameFilter(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetItemContent(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetItemDescription(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetItemPreview(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetItemTags(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetItemTitle(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetItemVisibility(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetMatchAnyTag(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetRankedByTrendDays(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetReturnAdditionalPreviews(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetReturnChildren(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetReturnLongDescription(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetReturnMetadata(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetReturnOnlyIDs(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetReturnTotalOnly(object p0, object p1)
        {
        return default;
        }
        public static dynamic SetSearchText(object p0, object p1)
        {
        return default;
        }
        public static dynamic StartItemUpdate(object p0, object p1)
        {
        return default;
        }
        public static dynamic SubmitItemUpdate(object p0, object p1)
        {
        return default;
        }
        public static dynamic SubscribeItem(object p0)
        {
        return default;
        }
        public static dynamic UnsubscribeItem(object p0)
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
        public static dynamic GetStat(string p0, out float val)
        {
        val = default;
        return default;
        }
        public static dynamic GetGlobalStat(string p0, out double val)
        {
        val = default;
        return default;
        }
        public static dynamic GetAchievement(string p0, out bool hasAchievement)
        {
        hasAchievement = default;
        return default;
        }
        public static dynamic GetGlobalStatHistory(object p0, object p1, object p2)
        {
        return default;
        }
        public static dynamic GetLeaderboardName(object p0)
        {
        return default;
        }
        public static dynamic IndicateAchievementProgress(object p0, object p1, object p2)
        {
        return default;
        }
        public static dynamic RequestGlobalStats(object p0)
        {
        return default;
        }
        public static dynamic SetAchievement(object p0)
        {
        return default;
        }
        public static dynamic SetStat(object p0, object p1)
        {
        return default;
        }
        public static dynamic StoreStats()
        {
        return default;
        }
        public static dynamic UploadLeaderboardScore(object p0, object p1, object p2, object p3, object p4)
        {
        return default;
        }
    }
    public static class SteamUtils
    {
        public static dynamic GetImageSize(object p0, out uint w, out uint h)
        {
        w = default;
        h = default;
        return default;
        }
        public static dynamic GetImageRGBA(object p0, byte[] p1, int p2)
        {
        return default;
        }
        public static dynamic GetEnteredGamepadTextInput(out string szTextInput, uint p1)
        {
        szTextInput = default;
        return default;
        }
        public static dynamic ShowGamepadTextInput(EGamepadTextInputMode p0, EGamepadTextInputLineMode p1, string p2, uint p3, string p4)
        {
        return default;
        }
        public static dynamic GetAppID()
        {
        return default;
        }
        public static dynamic GetEnteredGamepadTextLength()
        {
        return default;
        }
    }
}

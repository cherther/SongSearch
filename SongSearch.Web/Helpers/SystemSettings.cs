using System;

namespace SongSearch.Web {
	
	public static class SystemConfig {

		public static string BaseUrl { get; set; }
		public static string AdminEmailAddress { get; set; }
		public static string DefaultSiteProfileName { get; set; }
		public static string DefaultSiteProfileId { get; set; }
		public static string MediaPathFull { get; set; }
		public static string MediaPathPreview { get; set; }
		public static string MediaDefaultExtension { get; set; }
		public static string UploadPath { get; set; }
		public static string ZipPath { get; set; }
		public static string ZipFormat { get; set; }
		public static string ZipUserFormat { get; set; }
		public static bool LogSearchTerms { get; set; }
		public static bool LogSearchResults { get; set; }
		public static bool LogUserActions { get; set; }
		public static bool LogUserContentActions { get; set; }
		public static bool UseRemoteMedia { get; set; }
		public static string AWSAccessKey { get; set; }
		public static string AWSSecretKey { get; set; }
		public static string AWSMediaBucket { get; set; }
		public static string MediaUrlFormat { get; set; }
		public static string MediaFolderUrlFormat { get; set; }

		static SystemConfig() {

			BaseUrl = Configuration.Get("Sys_BaseUrl");
			DefaultSiteProfileName = Configuration.Get("Sys_DefaultSiteProfileName");
			DefaultSiteProfileId = Configuration.Get("Sys_DefaultSiteProfileId");
			MediaPathFull = Configuration.Get("Sys_MediaPathFull");
			MediaPathPreview = Configuration.Get("Sys_MediaPathPreview");
			MediaDefaultExtension = Configuration.Get("Sys_MediaDefaultExtension");
			UploadPath = Configuration.Get("Sys_UploadPath");
			ZipPath = Configuration.Get("Sys_ZipPath");
			ZipFormat = Configuration.Get("Sys_ZipFormat");
			AdminEmailAddress = Configuration.Get("Sys_AdminEmailAddress");
			ZipUserFormat = Configuration.Get("Sys_ZipUserFormat");
			LogSearchTerms = bool.Parse(Configuration.Get("Sys_LogSearchTerms"));
			LogSearchResults = bool.Parse(Configuration.Get("Sys_LogSearchResults"));
			LogUserActions = bool.Parse(Configuration.Get("Sys_LogUserActions"));
			LogUserContentActions = bool.Parse(Configuration.Get("Sys_LogUserContentActions"));
			UseRemoteMedia = bool.Parse(Configuration.Get("Sys_UseRemoteMedia"));
			AWSAccessKey = Configuration.Get("Sys_AWSAccessKey");
			AWSSecretKey = Configuration.Get("Sys_AWSSecretKey");
			AWSMediaBucket = Configuration.Get("Sys_AWSMediaBucket");
			MediaUrlFormat = Configuration.Get("Sys_MediaUrlFormat");
			MediaFolderUrlFormat = Configuration.Get("Sys_MediaFolderUrlFormat");
		}
	}
	public static class SystemMessages {

		public static string PasswordChangeSuccess { get; set; }
		public static string PasswordChangeSuccessSubjectLine { get; set; }
		public static string PasswordResetRequest { get; set; }
		public static string PasswordResetRequestSubjectLine { get; set; }
		public static string PasswordResetRequestLink { get; set; }
		public static string PasswordResetSuccess { get; set; }
		public static string PasswordResetSuccessSubjectLine { get; set; }
		public static string InvitationSubjectLine { get; set; }

		static SystemMessages() {

			PasswordChangeSuccess = Configuration.Get("Msg_PasswordChangeSuccess");
			PasswordChangeSuccessSubjectLine = Configuration.Get("Msg_PasswordChangeSuccessSubjectLine");
			PasswordResetRequest = Configuration.Get("Msg_PasswordResetRequest");
			PasswordResetRequestSubjectLine = Configuration.Get("Msg_PasswordResetRequestSubjectLine");
			PasswordResetRequestLink = Configuration.Get("Msg_PasswordResetRequestLink");
			PasswordResetSuccess = Configuration.Get("Msg_PasswordResetSuccess");
			PasswordResetSuccessSubjectLine = Configuration.Get("Msg_PasswordResetRequestSubjectLine");
			InvitationSubjectLine = Configuration.Get("Msg_InvitationSubjectLine");
			
		}
	}
	public static class SystemErrors {

		public static string LoginFailed { get; set; }
		public static string PasswordChangeFailed { get; set; }
		public static string PasswordResetFailed { get; set; }
		public static string UserAlreadyRegistered { get; set; }
		public static string UserCreationFailed { get; set; }
		public static string InviteCodeAlreadyUsed { get; set; }
		public static string InviteCodeExpired { get; set; }
		public static string InviteCodeNoMatch { get; set; }
		public static string UserDoesNotExist { get; set; }
		public static string ItemDoesNotExist { get; set; }
		public static string TagAlreadyExists { get; set; }

		static SystemErrors() {

			LoginFailed = Configuration.Get("Err_LoginFailed");
			PasswordChangeFailed = Configuration.Get("Err_PasswordChangeFailed");
			PasswordResetFailed = Configuration.Get("Err_PasswordResetFailed");
			UserAlreadyRegistered = Configuration.Get("Err_UserAlreadyRegistered");
			UserCreationFailed = Configuration.Get("Err_UserCreationFailed");
			InviteCodeAlreadyUsed = Configuration.Get("Err_InviteCodeAlreadyUsed");
			InviteCodeExpired = Configuration.Get("Err_InviteCodeExpired");
			InviteCodeNoMatch = Configuration.Get("Err_InviteCodeNoMatch");
			UserDoesNotExist = Configuration.Get("Err_UserDoesNotExist");
			ItemDoesNotExist = Configuration.Get("Err_ItemDoesNotExist");
			TagAlreadyExists = Configuration.Get("Err_TagAlreadyExists");

		}
	}
	
	public static class AssetPathSetting {
		public static string GetBasePath(string assetTypeName) {
			return Configuration.Get(String.Concat("Sys_AssetPath", assetTypeName));
		}
	}
}
using System;

namespace SongSearch.Web {
	// **************************************
	// Errors
	// **************************************
	public enum Errors {
		LoginFailed = 1,
		PasswordChangeFailed,
		PasswordResetFailed,
		UserAlreadyRegistered,
		UserCreationFailed,
		InviteCodeAlreadyUsed,
		InviteCodeExpired,
		InviteCodeNoMatch,
		UserDoesNotExist,
		ItemDoesNotExist,
		TagAlreadyExists
	}

	// **************************************
	// Messages
	// **************************************
	public enum Messages {
		PasswordChangeSuccess = 1,
		PasswordChangeSuccessSubjectLine,
		PasswordResetRequest,
		PasswordResetRequestSubjectLine,
		PasswordResetRequestLink,
		PasswordResetSuccess,
		PasswordResetSuccessSubjectLine,
		InvitationSubjectLine
	}

	// **************************************
	// Settings
	// **************************************
	public enum Settings {
		BaseUrl = 1,
		AssetPathFullSong,
		AssetPathPreview,
		ZipPath,
		ZipFormat,
		ZipUserFormat,
		UploadPath,
		AdminEmailAddress,
		ContactEmailAddress,
		LogSearchTerms,
		LogSearchResults,
		LogUserActions,
		LogUserContentActions
	}

	// **************************************
	// SystemSetting
	// **************************************
	public static class SystemSetting {
		// **************************************
		// Settings
		// **************************************

		// **************************************
		// Log Settings
		// **************************************
		public static bool LogSearchTerms {
			get { return bool.Parse(Settings.LogSearchTerms.Text()); }
		}

		public static bool LogSearchResults {
			get { return bool.Parse(Settings.LogSearchResults.Text()); }
		}

		public static bool LogUserActions {
			get { return bool.Parse(Settings.LogUserActions.Text()); }
		}

		public static bool LogUserContentActions {
			get { return bool.Parse(Settings.LogUserContentActions.Text()); }
		}

		public static string Text(this Errors error) {
			return GetErrorMessage(error);
		}

		public static string Text(this Messages message) {
			return GetMessage(message);
		}

		public static string Text(this Settings setting) {
			return GetSetting(setting);
		}

		// **************************************
		// GetErrorMessage
		// **************************************
		private static string GetErrorMessage(Errors errorCode) {
			return Configuration.Get(String.Format("Err_{0}", errorCode));
		}

		// **************************************
		// GetMessage
		// **************************************
		private static string GetMessage(Messages message) {
			return Configuration.Get(String.Format("Msg_{0}", message));
		}

		// **************************************
		// GetSetting
		// **************************************
		private static string GetSetting(Settings setting) {
			return Configuration.Get(String.Format("Sys_{0}", setting));
		}
	}

	public static class AssetPathSetting {
		public static string GetBasePath(string assetTypeName) {
			return Configuration.Get(String.Concat("Sys_AssetPath", assetTypeName));
		}
	}
}
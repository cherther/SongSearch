using System.Linq;

namespace SongSearch.Web {

	// **************************************
	// AccessLevels
	// **************************************    
	public enum Roles {
		None = 0,
		SuperAdmin = 1,
		Admin = 2,
		Plugger = 3,
		Client = 4
	}

	// **************************************
	// MediaVersion
	// **************************************    
	public enum MediaVersion {
		Preview = 1,
		FullSong
	}

	// **************************************
	// MediaVersion
	// **************************************    
	public enum FileTypes {
		MP3 = 1
	}

	// **************************************
	// CartStatusCodes
	// **************************************    
	public enum CartStatusCodes {
		Active = 0,
		Compressed = 1,
		Downloaded = 2
	}

	// **************************************
	// CartStatusCodes
	// **************************************    
	public enum Territories {
		Default = 1
	}

	// **************************************
	// CartStatusCodes
	// **************************************    
	public enum TagType {
		Moods,
		Styles,
		Brands,
		Gender,
		Tempo,
		Language,
		SoundsLike,
		Instruments
	}

	// **************************************
	// InvitationStatusCodes
	// **************************************    
	public enum InvitationStatusCodes {
		Open = 0,
		Registered = 1,
		Expired = 2
	}

	public enum SearchTypes {
		Contains = 1,
		HasValue = 2,
		Range = 3,
		Tag = 4
	}

	// **************************************
	// UserActionCodes
	// **************************************    
	public enum UserActionCodes {
		Registered = 0,
		LoggedIn,
		LoggedOut,
		ChangedPassword,
		UpdatedProfile,
		SentInvite
	}

	// **************************************
	// UserContentActionCodes
	// **************************************    
	public enum UserContentActionCodes {
		CreatedNewContent = 0,
		UpdatedContent,
		DeletedContent,
		PlayedPreview,
		PlayedFull,
		AddedToCart,
		RemovedFromCart,
		ZippedCart,
		DownloadedCart,
		DeletedCart
	}

	public static class ModelEnums {
		public static int[] GetRoles() {
//			return (int[])Enum.GetValues(typeof(SystemRoles));
			return new[]
			       	{
			       		(int) Roles.None,
			       		(int) Roles.SuperAdmin,
			       		(int) Roles.Admin,
			       		(int) Roles.Plugger,
			       		(int) Roles.Client
			       	};
		}

		public static int[] GetPublicRoles() {
			//			return (int[])Enum.GetValues(typeof(SystemRoles));
			return new[]
			       	{
			       		(int) Roles.None,
//				(int) AccessLevels.SuperAdmin, 
			       		(int) Roles.Admin,
			       		(int) Roles.Plugger,
			       		(int) Roles.Client
			       	};
		}
		public static int GetBestMatchForRole(this int[] roles, int roleId) {

			return !roles.Contains(roleId) ?
								(int)Roles.Client :
								roleId;
		}
	}
}

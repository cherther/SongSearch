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
	// FileTypes
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
	// Territories
	// **************************************    
	public enum Territories {
		Default = 1
	}

	// **************************************
	// TagType
	// **************************************    
	public enum TagType {
		Moods = 10,
		Styles = 11,
		Gender = 9,
		Tempo = 8,
		SoundsLike = 13,
		Instruments = 14,
		Language = 15,
		Brands = 20
		
	}
	
	public enum SortType {
		None = 0,
		Ascending = 1,
		Descending = 2
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
		HasValue,
		Range,
		Tag,
		Join,
		IsTrue
	}

	public enum RightsTypes {
		Master = 1,
		Comp
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

		public static TagType[] GetTagTypes() {
			return new[] 
				{ 
					TagType.Moods, 
					TagType.Styles,
					TagType.Gender,
					TagType.Tempo,
					TagType.SoundsLike,
					TagType.Instruments,
					TagType.Language,
					TagType.Brands
			};
		}
	}
}

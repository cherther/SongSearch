using System.Linq;

namespace SongSearch.Web {

	// **************************************
	// Roles
	// **************************************    
	public enum Roles {
		None       = 0,
		SuperAdmin = 1,
		Admin      = 2,
		Plugger    = 3,
		Client     = 4
	}

	// **************************************
	// Roles
	// **************************************    
	public enum PricingPlans {
		Member = 0,
		Basic = 1,
		Plus = 2,
		Business = 3,
		Pro = 4,
		Introductory = 5,
		SuperAdmin = 6
	}
	
	// **************************************
	// MediaVersion
	// **************************************    
	public enum MediaVersion {
		Preview = 1,
		Full
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
		Active     = 0,
		Compressed = 1,
		Downloaded = 2,
		Processing = 3
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
		Mood       = 10,
		Style      = 11,
		Gender      = 9,
		Tempo       = 8,
		SoundsLike  = 13,
		Instrument = 14,
		Language    = 15,
		Brand      = 20
		
	}
	
	public enum SortType {
		None       = 0,
		Ascending  = 1,
		Descending = 2
	}


	// **************************************
	// InvitationStatusCodes
	// **************************************    
	public enum InvitationStatusCodes {
		Open       = 0,
		Registered = 1,
		Expired    = 2
	}

	public enum SearchTypes {
		Contains = 1,
		HasValue,
		Range,
		Tag,
		TagText,
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

		public static MediaVersion[] MediaVersions() {
			return new[] 
				{ 
					MediaVersion.Preview,
					MediaVersion.Full
			};
		}
		public static int[] GetRoles() {
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
			return new[]
			       	{
			       		(int) Roles.None,
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
					TagType.Mood, 
					TagType.Style,
					TagType.Gender,
					TagType.Tempo,
					//TagType.SoundsLike,
					//TagType.Instrument,
					TagType.Language,
					TagType.Brand
			};
		}

		public static RightsTypes[] GetRightsTypes() {
			return new[] 
				{ 
					RightsTypes.Comp, 
					RightsTypes.Master
				};
		}

		public static PricingPlans[] GetPricingPlans(){
			return new[]
			{ 
				PricingPlans.Basic,
				PricingPlans.Plus,
				PricingPlans.Business,
				PricingPlans.Pro
			};
		}
	
	}
}

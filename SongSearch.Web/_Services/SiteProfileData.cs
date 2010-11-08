using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using SongSearch.Web.Data;
using Ninject;
using SongSearch.Web.Services;
using System.IO;

namespace SongSearch.Web {
	public static class SiteProfileData {

		// **************************************
		// SiteProfile
		// **************************************
		public static SiteProfile SiteProfile(bool cached = true) {
			var user = Account.User();
			var profile = SiteProfile(user.GetSiteProfileId(), cached);

			if (profile == null) {
				profile = SiteProfile(user.GetSiteProfileId(), cached: false);
			}

			return profile;
			//var pricingPlanId = user != null ? user.PricingPlanId : (int)PricingPlans.Level1;

			//switch (pricingPlanId) {

			//    case (int)PricingPlans.Level1:
			//        return SiteProfile(int.Parse(Settings.DefaultSiteProfileId.Value()), cached);
			//    case (int)PricingPlans.Level2:
			//        return SiteProfile(int.Parse(Settings.DefaultSiteProfileId.Value()), cached);
			//    case (int)PricingPlans.Level3:
			//        return SiteProfile(user.GetSiteProfileId(), cached);
			//    case (int)PricingPlans.Level4:
			//        return SiteProfile(user.GetSiteProfileId(), cached);
			//    default:
			//        return SiteProfile(Settings.DefaultSiteProfileName.Value(), cached);

			//}

		}
		
		public static SiteProfile SiteProfile(int profileId, bool cached = true) {
			if (cached) {
				return CacheService
					.SiteProfiles()
					.SingleOrDefault(s => s.ProfileId == profileId);
			} else {
				using (var ctx = new SongSearchContext()) {
					var profile = ctx.SiteProfiles
						.Include("Contacts")
						.SingleOrDefault(s => s.ProfileId == profileId);
					return profile;
				
				}
			}
		}

		public static SiteProfile SiteProfile(string profileName, bool cached = true) {
			if (cached) {
				return CacheService
					.SiteProfiles()
					.SingleOrDefault(s => s.ProfileName.Equals(profileName, StringComparison.InvariantCultureIgnoreCase));
			} else {
				using (var ctx = new SongSearchContext()) {
					var profile = ctx.SiteProfiles
						.Include("Contacts")
						.SingleOrDefault(s => s.ProfileName.Equals(profileName, StringComparison.InvariantCultureIgnoreCase));
					return profile;

				}
			}
		}
		
		public static IList<SiteProfile> SiteProfiles() {
			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				var profiles = ctx.SiteProfiles
					.Include("Contacts")
					.ToList();

				return profiles;

			}
		}

		public static string SiteProfileLogoUrl(this SiteProfile profile) {

			return profile.HasProfileLogo ?
				String.Concat(@"/public/images/profiles/", profile.ProfileName.ToLower(), ".png") :
				String.Empty;
				
		}
	}
}
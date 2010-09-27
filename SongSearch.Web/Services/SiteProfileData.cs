﻿using System;
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
			return SiteProfile(user.GetSiteProfileId(), cached);
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
				using (var session = App.DataSessionReadOnly) {
					var profile = session.GetObjectQuery<SiteProfile>()
						.Include("Contacts")
						.Where(s => s.ProfileId == profileId).SingleOrDefault();
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
				using (var session = App.DataSessionReadOnly) {
					var profile = session.GetObjectQuery<SiteProfile>()
						.Include("Contacts")
						.Where(s => s.ProfileName.Equals(profileName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
					return profile;

				}
			}
		}
		
		public static IList<SiteProfile> SiteProfiles() {
			using (var session = App.DataSessionReadOnly) {
				var profiles = session.GetObjectQuery<SiteProfile>()
					.Include("Contacts")
					.ToList();

				return profiles;

			}
		}

        public static string SiteProfileLogoUrl(this SiteProfile profile) {

            return profile.HasProfileLogo ?
                String.Concat(@"/public/images/profiles/", profile.ProfileName.ToLower(), ".jpg") :
                String.Empty;
                
        }
	}
}
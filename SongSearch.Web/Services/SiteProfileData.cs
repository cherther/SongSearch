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
		// User
		// **************************************
		public static SiteProfile SiteProfile(bool cached = true) {
			return SiteProfile(Settings.SiteProfile.Value(), cached);
		}


		public static SiteProfile SiteProfile(string profileName, bool cached = true) {
			if (cached) {
				return CacheService
					.SiteProfiles()
					.SingleOrDefault(s => s.ProfileName.Equals(profileName, StringComparison.InvariantCultureIgnoreCase));
			} else {
				using (var session = App.DataSessionReadOnly) {
					var profile = session.GetObjectQuery<SiteProfile>()
						.Where(s => s.ProfileName.Equals(profileName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

					return profile;
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web.Data {
	public class UserQuotas {
		public Quota NumberOfSongs { get; set; }
		public Quota NumberOfInvitedUsers { get; set; }
		public Quota NumberOfCatalogAdmins { get; set; }

		public UserQuotas() {
			NumberOfSongs = new Quota() { Default = 50, QuotaName = "Songs" };
			NumberOfInvitedUsers = new Quota() { Default = -1, QuotaName = "Users" };
			NumberOfCatalogAdmins = new Quota() { Default = -1, QuotaName = "Admins" };
		}
	}

	public class Quota {

		public int Default { get; set; }
		public int? Allowed { get; set; }
		public int Used { get; set; }

		public int? Remaining {
			get {
				return Allowed.HasValue ? (Allowed.Value > Used ? (int?)Allowed.Value - Used : 0) : null;
			}
		}

		public int Max {

			get {
				if (App.IsLicensedVersion) {
					return !Remaining.HasValue || Remaining.Value > Default ?
								Default :
								Remaining.Value > 0 ? Remaining.Value : 0;
				} else {
					return Default;
				}
			}
		}
		public decimal Usage {

			get {
				return Allowed > 0 ?
					((Used > Allowed) ? (decimal)1 : ((decimal)Used / (decimal)Allowed))
					: (decimal)0;
			}
		}

		public string QuotaName { get; set; }
		
		public string UsageDescription {
			get {

				return String.Format("{0} of {1} {2} ({3})",
								  Used.ToString("N0"),
								  Allowed.ToQuotaDescription(),
								  QuotaName,
								  Usage.ToPercentDescription());
			}
		}

		public bool IsAtTheLimit {
			get { return Remaining <= 0; }
		}

		public int IsGoodFor(int number) {
			return !Remaining.HasValue || number <= Remaining.Value ? number : Remaining.Value;
		}

	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web {

	public class UserBalances {
		public Balance NumberOfSongs { get; set; }
		public Balance NumberOfInvitedUsers { get; set; }
		public Balance NumberOfCatalogAdmins { get; set; }

		public UserBalances() {
			NumberOfSongs = new Balance() { Default = 50, BalanceName = "Songs" };
			NumberOfInvitedUsers = new Balance() { Default = -1, BalanceName = "Users" };
			NumberOfCatalogAdmins = new Balance() { Default = -1, BalanceName = "Admins" };
		}
	}

	public class Balance {

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

		public string BalanceName { get; set; }
		
		public string UsageDescription {
			get {

				return String.Format("{0} of {1} {2} ({3})",
								  Used.ToString("N0"),
								  Allowed.ToBalanceDescription(),
								  BalanceName,
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
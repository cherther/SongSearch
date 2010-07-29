using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web.Data {
	public class UserQuotas {
		public Quota NumberOfSongs { get; set; }
		public Quota NumberOfInvitedUsers { get; set; }
		public Quota NumberOfCatalogAdmins { get; set; }
	}

	public class Quota {
		public int Allowed { get; set; }
		public int Used { get; set; }

		public int Remaining {
			get {
				return Allowed > 0 ? Allowed - Used : -1;
			}
		}
		public decimal Usage {

			get {
				return Allowed > 0 ? ((decimal)Used / (decimal)Allowed) : (decimal)0;
			}
		}

		public string UsageDescription(string itemName) {

			return String.Format("{0} of {1} {2} ({3})",
							  Used.ToString("N0"),
							  Allowed.ToQuotaDescription(),
							  itemName,
							  Usage.ToPercentDescription());
		}

	}
}
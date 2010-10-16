using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web.Data {
	public partial class User {
		//public User ParentUser { get; set; }
		public IList<User> ChildUsers { get; set; }

		public bool IsPlanOwner {
			get {

				return (UserId == PlanUserId);
			}
		}
	}
}
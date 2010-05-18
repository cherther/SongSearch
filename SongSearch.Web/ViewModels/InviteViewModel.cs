using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {

	// **************************************
	// InviteViewModel
	// **************************************
	public class InviteViewModel : ViewModel {
		public string InviteId { get; set; }
		public string InviteUrl { get; set; }
		public string BaseUrl { get; set; }

		public string Sender { get; set; }
		public string Recipient { get; set; }

		private string[] _recipients;

		public string[] Recipients {
			get {
				if (_recipients != null && _recipients.Length > 0) {
					return _recipients;
				}

				return Recipient.Split(',').Where(r => r.Length > 0).ToArray();

			}
			set {
				_recipients = value;
			}
		}
		//
		//
		public InviteViewModel() {

			PageTitle = "";
			NavigationLocation = "";

		}
	}

	
}
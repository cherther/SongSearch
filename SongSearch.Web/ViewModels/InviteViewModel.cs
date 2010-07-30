using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SongSearch.Web {

	// **************************************
	// InviteViewModel
	// **************************************
	public class InviteViewModel : ViewModel {

		[Required]
		public string InviteId { get; set; }

		[Required]
		public bool IsPlanInvitation { get; set; }

		public string InviteUrl { get; set; }
		
		public string BaseUrl { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
		[DisplayName("Email address")]
		public string Sender { get; set; }

		public string Recipient { get; set; }

		private string[] _recipients;

		public string[] Recipients {
			get {
				if (_recipients != null && _recipients.Length > 0) {
					return _recipients;
				}

				return Recipient != null ? Recipient.Split(',').Where(r => r.Length > 0).Distinct().ToArray() : new string[] {};

			}
			set {
				_recipients = value;
			}
		}
		//
		//
		public InviteViewModel() {

			PageTitle = "";
			NavigationLocation = new string[] { "" };

		}
	}

	
}
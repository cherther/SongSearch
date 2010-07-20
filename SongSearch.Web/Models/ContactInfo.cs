using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace SongSearch.Web.Models {
	[MetadataType(typeof(ContactMetaData))]
	public partial class Contact {
	}

	public class ContactMetaData {

		[DisplayName("Contact Name")]
		public string ContactName { get; set; }

		[DisplayName("Company Name")]
		public string CompanyName { get; set; }

		[DisplayName("Address")]
		public string Address1 { get; set; }

		[DisplayName("Address (cont'd)")]
		public string Address2 { get; set; }

		[DisplayName("City")]
		public string City { get; set; }

		[DisplayName("State/Region")]
		public string StateRegion { get; set; }

		[DisplayName("Zip/Postal Code")]
		public string PostalCode { get; set; }

		[DisplayName("Country")]
		public string Country { get; set; }

		[DisplayName("Phone 1")]
		public string Phone1 { get; set; }

		[DisplayName("Phone 2")]
		public string Phone2 { get; set; }

		[DisplayName("Fax")]
		public string Fax { get; set; }

		[DataType(DataType.EmailAddress)]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "This is a not valid e-mail address.")]
		[DisplayName("Administrative email address")]
		public string AdminEmail { get; set; }
	}
}
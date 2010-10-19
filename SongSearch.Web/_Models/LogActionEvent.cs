using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web.Data {
	
	public class LogActionEvent {

		public long EventId { get; set; }
		public DateTime EventDate { get; set; }
		public string Sessionid { get; set; }
		public int UserId { get; set; }
	}
}
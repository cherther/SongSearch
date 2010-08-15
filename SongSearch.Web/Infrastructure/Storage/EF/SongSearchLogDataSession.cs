using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Infrastructure.Storage.EF;

namespace SongSearch.Web {
	public class SongSearchLogDataSession : LogDataSessionEF {
		public SongSearchLogDataSession() : base(
			new SongSearchLogContext(Connections.ConnectionString(ConnectionStrings.SongSearchLogContext))) { }
	}
}
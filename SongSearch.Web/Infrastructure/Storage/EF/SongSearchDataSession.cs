using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {
	public class SongSearchDataSession : DataSessionEF {
        public SongSearchDataSession() : base(new SongSearchContext(Connections.ConnectionString(ConnectionStrings.SongSearchContext))) {}
    }
	public class SongSearchDataSessionReadOnly : DataSessionReadOnlyEF {
		public SongSearchDataSessionReadOnly() : base(new SongSearchContext(Connections.ConnectionString(ConnectionStrings.SongSearchContext))) { }
	}
}
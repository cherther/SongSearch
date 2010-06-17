using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	public class BaseService {
		protected IDataSession DataSession {get;set;}
		protected IDataSessionReadOnly ReadSession { get; set; }

		private string _activeUserIdentity;

		public string ActiveUserName {

			get {
				return _activeUserIdentity;
			}
			set {
				_activeUserIdentity = value;
				ActiveUser = ActiveUser ?? SessionService.Session().User(_activeUserIdentity);
			}

		}

		public User ActiveUser { get; set; }

		public BaseService(IDataSession session) {
			DataSession = session;
		}
		
		public BaseService(IDataSession dataSession, IDataSessionReadOnly readSession) {
			DataSession = dataSession;
			ReadSession = readSession;
		}

		//for testing
		public BaseService(string activeUserIdentity) {
			if (DataSession == null) {
				DataSession = new SongSearchDataSession();
			}

			if (ReadSession == null) {
				ReadSession = new SongSearchDataSessionReadOnly();
			}

			_activeUserIdentity = activeUserIdentity;
			ActiveUser = SessionService.Session().User(activeUserIdentity);
		}
	}
}
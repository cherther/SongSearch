using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	public class BaseService {
		protected IDataSession Session {get;set;}
		protected IDataSessionReadOnly SessionReadOnly { get; set; }

		private string _activeUserIdentity;

		public string ActiveUserName {

			get {
				return _activeUserIdentity;
			}
			set {
				_activeUserIdentity = value;
				ActiveUser = ActiveUser ?? CacheService.User(_activeUserIdentity);
			}

		}

		public User ActiveUser { get; set; }

		public BaseService(IDataSession session) {
			Session = session;
		}
		
		public BaseService(IDataSession session, IDataSessionReadOnly sessionReadOnly) {
			Session = session;
			SessionReadOnly = sessionReadOnly;
		}

		//for testing
		public BaseService(string activeUserIdentity) {
			if (Session == null) {
				Session = new SongSearchDataSession();
			}

			if (SessionReadOnly == null) {
				SessionReadOnly = new SongSearchDataSessionReadOnly();
			}

			_activeUserIdentity = activeUserIdentity;
			ActiveUser = CacheService.User(activeUserIdentity);
		}
	}
}
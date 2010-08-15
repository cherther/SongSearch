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
		protected IUserEventLogService LogService { get; set; }

		private string _activeUserIdentity;

		public string ActiveUserName {

			get {
				return _activeUserIdentity;
			}
			set {

				_activeUserIdentity = value;
				
			}

		}

		public BaseService(IDataSession session) {
			DataSession = session;
		}

		public BaseService(IDataSession dataSession, IUserEventLogService logService) {
			DataSession = dataSession;
			LogService = logService;
		}
		
		public BaseService(IDataSession dataSession, IDataSessionReadOnly readSession) {
			DataSession = dataSession;
			ReadSession = readSession;
		}
		public BaseService(IDataSession dataSession, IDataSessionReadOnly readSession, IUserEventLogService logService) {
			DataSession = dataSession;
			ReadSession = readSession;
			LogService = logService;
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
			
		}
	}
}
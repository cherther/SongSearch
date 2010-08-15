using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using System.Web.Caching;
using System.Web.SessionState;
using Ninject;
using Ninject.Web.Mvc;

namespace SongSearch.Web.Services {
	public class SessionService {

		private HttpSessionState _session;
		
		private bool _sessionIsInitialized;
		
		private bool _hasSession;

		private delegate void UpdateDelegate(CacheKeys key, params object[] list);
		private IDictionary<CacheKeys, UpdateDelegate> _sessionMatrix = new Dictionary<CacheKeys, UpdateDelegate>();
		private IDictionary<CacheKeys, UpdateDelegate> _cacheMatrix = new Dictionary<CacheKeys, UpdateDelegate>();

		public SessionService() {
			
			if (HttpContext.Current != null) {
				_session = HttpContext.Current.Session;
				_hasSession = _session != null;
			}
	
			// set up the caching matrices with keys and caching actions
			_sessionMatrix.Add(CacheKeys.User, SessionUpdateUser);
			_sessionMatrix.Add(CacheKeys.ActiveCartContents, SessionUpdateUserActiveCart);

		}

		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		public enum CacheKeys {
			User,
			ActiveCartContents
		}
	
		// **************************************
		// Initialize
		// **************************************
		public void Initialize() {

			if (HttpContext.Current.User != null && !String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name)) {
				string userName = HttpContext.Current.User.Identity.Name;
				InitializeSession(userName, false);
			}
		}

		
		public void InitializeSession() {
			InitializeSession(false);
		}
		public void InitializeSession(bool force) {

			if (HttpContext.Current.User != null && !String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name)) {
				string userName = HttpContext.Current.User.Identity.Name;
				InitializeSession(userName, force);
			} else {
				InitializeSession(null, force);
			}
		}

		public void InitializeSession(string userName) {

			InitializeSession(userName, false);
		}

		public void InitializeSession(string userName, bool force) {

			if (HttpContext.Current != null) {
				_session = HttpContext.Current.Session;
				_hasSession = _session != null;

				if ((!_sessionIsInitialized || force) &&
					(userName != null && _hasSession)) {
					// either app is not yet initialized 
					// or we're forcing an update
					foreach (var key in _sessionMatrix.Keys) {
						_sessionMatrix[key](key, userName);
					}

					

					_sessionIsInitialized = true;
				}
			}
		}

		public static SessionService Session() {
			return new SessionService();
		}
		// **************************************
		// Quick Get method for cache or session 
		//	values
		// **************************************
		public string Get(string key) {

			return (Session(key)) as string;
		
		}

		// ----------------------------------------------------------------------------
		// Session
		// ----------------------------------------------------------------------------
		public User User(string userName) {
			if (_hasSession) {

				if (Session(CacheKeys.User) == null) { SessionUpdateUser(CacheKeys.User, userName); }
				return Session(CacheKeys.User) as User;
				
			} else {
				return GetDataUser(userName);
			}
		}

		public Cart MyActiveCart(string userName) {
			if (_hasSession) {
				if (Session(CacheKeys.ActiveCartContents) == null) { SessionUpdateUserActiveCart(CacheKeys.ActiveCartContents, userName); }
				return Session(CacheKeys.ActiveCartContents) as Cart;
				
			} else {
				return GetDataUserActiveCart(userName);
			}
		}

		public bool IsInMyActiveCart(int contentId, string userName) {
			Cart activeCart;

			if (_hasSession) {
				if (Session(CacheKeys.ActiveCartContents) == null) { SessionUpdateUserActiveCart(CacheKeys.ActiveCartContents, userName); }
				activeCart = Session(CacheKeys.ActiveCartContents) as Cart;

			} else {
				activeCart = GetDataUserActiveCart(userName);
			}

			return activeCart != null && activeCart.Contents != null && activeCart.Contents.Any(c => c.ContentId == contentId);
			
		}
		public int MyActiveCartCount(string userName) {
			
			Cart activeCart;

			if (_hasSession) {
				if (Session(CacheKeys.ActiveCartContents) == null) { SessionUpdateUserActiveCart(CacheKeys.ActiveCartContents, userName); }
				activeCart = Session(CacheKeys.ActiveCartContents) as Cart;

			} else {
				activeCart = GetDataUserActiveCart(userName);
			}

			return activeCart != null && activeCart.Contents != null ? activeCart.Contents.Count() : 0;

		}

		public void RefreshMyActiveCart(string userName) {
			if (_hasSession) {
				SessionUpdateUserActiveCart(CacheKeys.ActiveCartContents, userName); 
			}
		}
		

		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------
		
		// **************************************
		// DataSession
		// **************************************
		private object Session(CacheKeys key) {
			return Session(key.ToString());
		}
		public object Session(string key) {
			return _session[key];
		}

		
		// **************************************
		// SessionUpdate
		// **************************************
		public void SessionUpdate(CacheKeys cacheKey) {

			_sessionMatrix[cacheKey](cacheKey);
		}
		
		public void SessionUpdate(object cacheObject, CacheKeys cacheKey) {
			SessionUpdate(cacheObject, cacheKey.ToString());
		}
		public void SessionUpdate(object cacheObject, string cacheKey) {
			if (_hasSession) {
				_session.Remove(cacheKey.ToString());

				if (cacheObject != null) {
					_session.Add(
						cacheKey.ToString(),
						cacheObject
						);
				}
			}
		}

		private void SessionUpdateUser(CacheKeys key, params object[] list) {
			var obj = GetDataUser(list[0] as string);
			SessionUpdate(obj, key);				
			
		}
		private void SessionUpdateUserActiveCart(CacheKeys key, params object[] list) {
			var obj = GetDataUserActiveCart(list[0] as string);
			SessionUpdate(obj, key);

		}
		
	
		// **************************************
		// GetData
		// **************************************
		private User GetDataUser(string userName) {
			return Account.User(userName, false);
		}

		private Cart GetDataUserActiveCart(string userName) {
			using (var cartService = App.Container.Get<CartService>()) {
				cartService.ActiveUserName = userName;
				return cartService.MyActiveCartContents();
			}
		}

	}
}
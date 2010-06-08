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
	public static class CacheService {

		private static Cache _cache;
		private static HttpSessionState _session;
		
		private static bool _appIsInitialized;
		private static bool _sessionIsInitialized;
		
		private static bool _hasCache;
		private static bool _hasSession;

		private delegate void UpdateDelegate(CacheKeys key, params object[] list);
		private static IDictionary<CacheKeys, UpdateDelegate> _sessionMatrix = new Dictionary<CacheKeys, UpdateDelegate>();
		private static IDictionary<CacheKeys, UpdateDelegate> _cacheMatrix = new Dictionary<CacheKeys, UpdateDelegate>();

		static CacheService() {

			// set up the caching matrices with keys and caching actions
			_sessionMatrix.Add(CacheKeys.User, SessionUpdateUser);
			_sessionMatrix.Add(CacheKeys.ActiveCartContents, SessionUpdateUserActiveCart);

			_cacheMatrix.Add(CacheKeys.Catalogs, CacheUpdateCatalogs);
			_cacheMatrix.Add(CacheKeys.Users, CacheUpdateUsers);
			_cacheMatrix.Add(CacheKeys.SearchProperties, CacheUpdateSearchProperties);
			_cacheMatrix.Add(CacheKeys.Tags, CacheUpdateTags);
			_cacheMatrix.Add(CacheKeys.TopTags, CacheUpdateTopTags);
			_cacheMatrix.Add(CacheKeys.Content, CacheUpdateContentFields);
			_cacheMatrix.Add(CacheKeys.Rights, CacheUpdateContentRightsFields);
			_cacheMatrix.Add(CacheKeys.Territories, CacheUpdateTerritories);
			//Initialize();
		}

		public static string[] CachedContentFields = new string[] { "Artist", "RecordLabel", "Writers" };
		public static string[] CachedContentRightsFields = new string[] { "RightsHolderName" };

		public static DateTime LastUpdated { get; private set; }

		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		public enum CacheKeys {
			User,
			ActiveCartContents,
			Catalogs,
			Users,
			SearchProperties,
			Tags,
			TopTags,
			Content,
			Rights,
			Territories
		}
		// **************************************
		// Initialize
		// **************************************
		public static void Initialize() {

			InitializeApp(false);

			if (HttpContext.Current.User != null && !String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name)) {
				string userName = HttpContext.Current.User.Identity.Name;
				InitializeSession(userName, false);
			}
		}

		public static void InitializeApp(bool force) {

			if (HttpContext.Current != null) {
				_cache = HttpContext.Current.Cache;
				_hasCache = _cache != null;

				if ((!_appIsInitialized || force) && _hasCache) {
					// either app is not yet initialized 
					// or we're forcing an update

					foreach (var key in _cacheMatrix.Keys) {
						//call the delegated function for each key
						_cacheMatrix[key](key);
					}

					_appIsInitialized = true;
					LastUpdated = DateTime.Now;
				}
			}
		}

		public static void InitializeSession() {
			InitializeSession(false);
		}
		public static void InitializeSession(bool force) {

			if (HttpContext.Current.User != null && !String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name)) {
				string userName = HttpContext.Current.User.Identity.Name;
				InitializeSession(userName, force);
			} else {
				InitializeSession(null, force);
			}
		}

		public static void InitializeSession(string userName) {

			InitializeSession(userName, false);
		}

		public static void InitializeSession(
			string userName, bool force) {

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


		// **************************************
		// Quick Get method for cache or session 
		//	values
		// **************************************
		public static string Get(string key) {

			return (Session(key) ?? Cache(key)) as string;
		
		}

		// ----------------------------------------------------------------------------
		// Session
		// ----------------------------------------------------------------------------
		public static User User(string userName) {
			if (_hasSession) {

				if (Session(CacheKeys.User) == null) { SessionUpdateUser(CacheKeys.User, userName); }
				return Session(CacheKeys.User) as User;
				
			} else {
				return GetDataUser(userName);
			}
		}

		public static Cart MyActiveCart(string userName) {
			if (_hasSession) {
				if (Session(CacheKeys.ActiveCartContents) == null) { SessionUpdateUserActiveCart(CacheKeys.ActiveCartContents, userName); }
				return Session(CacheKeys.ActiveCartContents) as Cart;
				
			} else {
				return GetDataUserActiveCart(userName);
			}
		}

		public static bool IsInMyActiveCart(int contentId, string userName) {
			Cart activeCart;

			if (_hasSession) {
				if (Session(CacheKeys.ActiveCartContents) == null) { SessionUpdateUserActiveCart(CacheKeys.ActiveCartContents, userName); }
				activeCart = Session(CacheKeys.ActiveCartContents) as Cart;

			} else {
				activeCart = GetDataUserActiveCart(userName);
			}

			return activeCart != null && activeCart.Contents != null && activeCart.Contents.Any(c => c.ContentId == contentId);
			
		}
		public static int MyActiveCartCount(string userName) {
			
			Cart activeCart;

			if (_hasSession) {
				if (Session(CacheKeys.ActiveCartContents) == null) { SessionUpdateUserActiveCart(CacheKeys.ActiveCartContents, userName); }
				activeCart = Session(CacheKeys.ActiveCartContents) as Cart;

			} else {
				activeCart = GetDataUserActiveCart(userName);
			}

			return activeCart != null && activeCart.Contents != null ? activeCart.Contents.Count() : 0;

		}

		public static void RefreshMyActiveCart(string userName) {
			if (_hasSession) {
				SessionUpdateUserActiveCart(CacheKeys.ActiveCartContents, userName); 
			}
		}
		// ----------------------------------------------------------------------------
		//  App
		// ----------------------------------------------------------------------------

		// **************************************
		// Catalogs
		// **************************************
		public static IList<Catalog> Catalogs() {
			if (_hasCache) {
				if (Cache(CacheKeys.Catalogs) == null) { CacheUpdateCatalogs(CacheKeys.Catalogs); }
				return Cache(CacheKeys.Catalogs) as IList<Catalog>;
				
			} else {
				return GetDataCatalogs();
			}
		}
		
		// **************************************
		// Catalogs
		// **************************************
		public static IList<User> Users() {
			if (_hasCache) {
				if (Cache(CacheKeys.Users) == null) { CacheUpdateUsers(CacheKeys.Users); }
				return Cache(CacheKeys.Users) as IList<User>;

			} else {
				return GetDataUsers();
			}
		}

		// **************************************
		// SearchProperties
		// **************************************
		public static IList<SearchProperty> SearchProperties(Roles role) {
			
			if (_hasCache) {
				if (Cache(CacheKeys.SearchProperties) == null) { CacheUpdateSearchProperties(CacheKeys.SearchProperties); }
				var value = Cache(CacheKeys.SearchProperties) as IList<SearchProperty>;
				return value.Where(x => x.AccessLevel >= (int)role).ToList();
			} else {
				return GetDataSearchProperties().Where(x => x.AccessLevel >= (int)role).ToList();
			}

		}

		// **************************************
		// Tags
		// **************************************
		public static IList<Tag> Tags() {
			if (_hasCache) {
				if (Cache(CacheKeys.Tags) == null) { CacheUpdateTags(CacheKeys.Tags); }
				return Cache(CacheKeys.Tags) as IList<Tag>;
				
			} else {
				return GetDataTags();
			}
		}

		// **************************************
		// TopTags
		// **************************************
		public static IDictionary<TagType, IList<Tag>> TopTags() {
			if (_hasCache) {
				if (Cache(CacheKeys.TopTags) == null) { CacheUpdateTopTags(CacheKeys.TopTags); }
				return Cache(CacheKeys.TopTags) as IDictionary<TagType, IList<Tag>>;
				
			} else {
				return GetDataTopTags();
			}
		}
		
		// **************************************
		// ContentField
		// **************************************
		public static IList<string> ContentField(string fieldName) {
			fieldName = fieldName.ToUpper();
			if (_hasCache){
				if (Cache(CacheKeys.Content) == null) { CacheUpdateContentFields(CacheKeys.Content); }
				var lookup = Cache(CacheKeys.Content) as IDictionary<string, IList<string>>;
				return lookup.Keys.Contains(fieldName) ? lookup[fieldName] : new List<string> { "" };
			} else {
				return GetDataContentField(fieldName);
			}
		}

		// **************************************
		// ContentRightsField
		// **************************************
		public static IList<string> ContentRightsField(string fieldName) {
			if (_hasCache) {
				if (Cache(CacheKeys.Rights) == null) { CacheUpdateContentRightsFields(CacheKeys.Content); }
				var lookup = Cache(CacheKeys.Rights) as IDictionary<string, IList<string>>;
				return lookup[fieldName.ToUpper()]; ;
			} else {
				return GetDataContentRightsField(fieldName.ToUpper());
			}
		}

		// **************************************
		// Tags
		// **************************************
		public static IList<Territory> Territories() {
			if (_hasCache) {
				if (Cache(CacheKeys.Territories) == null) { CacheUpdateTerritories(CacheKeys.Territories); }
				return Cache(CacheKeys.Territories) as IList<Territory>;
			} else {
				return GetDataTerritories();
			}
		}

		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------
		
		// **************************************
		// DataSession
		// **************************************
		private static object Session(CacheKeys key) {
			return Session(key.ToString());
		}
		public static object Session(string key) {
			return _session[key];
		}

		// **************************************
		// Cache
		// **************************************
		private static object Cache(CacheKeys key) {
			return Cache(key.ToString());
		}

		public static object Cache(string key) {
			return _cache[key];
		}
	
		// **************************************
		// SessionUpdate
		// **************************************
		public static void SessionUpdate(CacheKeys cacheKey) {

			_sessionMatrix[cacheKey](cacheKey);
		}
		
		public static void SessionUpdate(object cacheObject, CacheKeys cacheKey) {
			SessionUpdate(cacheObject, cacheKey.ToString());
		}
		public static void SessionUpdate(object cacheObject, string cacheKey) {
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

		private static void SessionUpdateUser(CacheKeys key, params object[] list) {
			var obj = GetDataUser(list[0] as string);
			SessionUpdate(obj, key);				
			
		}
		private static void SessionUpdateUserActiveCart(CacheKeys key, params object[] list) {
			var obj = GetDataUserActiveCart(list[0] as string);
			SessionUpdate(obj, key);

		}
		// **************************************
		// CacheUpdate
		// **************************************
		public static void CacheUpdate(CacheKeys cacheKey) {

			_cacheMatrix[cacheKey](cacheKey);
		}

		public static void CacheUpdate(object cacheObject, CacheKeys cacheKey) {
			CacheUpdate(cacheObject, cacheKey.ToString());
		}

		public static void CacheUpdate(object cacheObject, string cacheKey) {

			if (_hasCache) {
				_cache.Remove(cacheKey);

				if (cacheObject != null) {
					_cache.Insert(
						cacheKey,
						cacheObject,
						null,
						DateTime.Now.AddSeconds(3600),
						System.Web.Caching.Cache.NoSlidingExpiration
						);
				}
				
			}
		}

		private static void CacheUpdateCatalogs(CacheKeys key, params object[] list) {
			CacheUpdate(GetDataCatalogs(), key);
		}
		private static void CacheUpdateUsers(CacheKeys key, params object[] list) {
			CacheUpdate(GetDataUsers(), key);
		}

		private static void CacheUpdateSearchProperties(CacheKeys key, params object[] list) {
			CacheUpdate(GetDataSearchProperties(), key);	
		}

		private static void CacheUpdateTags(CacheKeys key, params object[] list) {
			CacheUpdate(GetDataTags(), key);		
		}
		private static void CacheUpdateTopTags(CacheKeys key, params object[] list) {
			CacheUpdate(GetDataTopTags(), key);
		}

		private static void CacheUpdateContentFields(CacheKeys key, params object[] list) {
			CacheUpdate(GetDataContentFields(), key);			
		}

		private static void CacheUpdateContentRightsFields(CacheKeys key, params object[] list) {
			CacheUpdate(GetDataContentRightsFields(), key);
		}

		private static void CacheUpdateTerritories(CacheKeys key, params object[] list) {
			CacheUpdate(GetDataTerritories(), key);
		}

		// **************************************
		// GetData
		// **************************************
		private static User GetDataUser(string userName) {
			return AccountData.UserComplete(userName);
		}

		private static Cart GetDataUserActiveCart(string userName) {
			using (var cartService = App.Container.Get<CartService>()) {
				cartService.ActiveUserName = userName;
				return cartService.MyActiveCartContents();
			}
		}

		private static IList<Catalog> GetDataCatalogs() {
			return SearchService.GetLookupList<Catalog>();
		}

		private static IList<User> GetDataUsers() {
			return SearchService.GetLookupList<User>();
		}

		private static IList<SearchProperty> GetDataSearchProperties() {
			return SearchService.GetSearchMenuProperties();
		}
		
		private static IList<Tag> GetDataTags() {
			return SearchService.GetLookupList<Tag>();
		}

		private static IDictionary<TagType, IList<Tag>> GetDataTopTags() {
			var obj = new Dictionary<TagType, IList<Tag>>();
			var tagTypes = ModelEnums.GetTagTypes();

			foreach (var tagType in tagTypes) {

				obj.Add(tagType, SearchService.GetTopTags(tagType));

			}
			return obj;
		}

		private static IDictionary<string, IList<string>> GetDataContentFields() {
			
			var lookup = new Dictionary<string, IList<string>>();
			foreach (var field in CachedContentFields) {

				lookup.Add(field.ToUpper(), GetDataContentField(field));
			}

			return lookup;
		}

		private static IList<string> GetDataContentField(string fieldName){

			return SearchService.GetLookupListContent(fieldName);
		}

		private static IDictionary<string, IList<string>> GetDataContentRightsFields() {
			
			var lookup = new Dictionary<string, IList<string>>();
			foreach (var field in CachedContentRightsFields) {

				lookup.Add(field.ToUpper(), GetDataContentRightsField(field));
			}

			return lookup; 
		}
		private static IList<string> GetDataContentRightsField(string fieldName) {

			return SearchService.GetLookupListContentRights(fieldName);
		}

		private static IList<Territory> GetDataTerritories() {
			return SearchService.GetLookupList<Territory>();
		}
	}
}
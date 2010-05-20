using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using System.Web.Caching;
using System.Web.SessionState;

namespace SongSearch.Web.Services {
	public static class CacheService {

		private static Cache _cache;
		private static HttpSessionState _session;
		private static bool _appIsInitialized;
		private static bool _sessionIsInitialized;
		//private static bool _hasHttpContext;
		private static bool _hasCache;
		private static bool _hasSession;

		private delegate void UpdateDelegate(CacheKeys key, params object[] list);
		private static IDictionary<CacheKeys, UpdateDelegate> _sessionMatrix = new Dictionary<CacheKeys, UpdateDelegate>();
		private static IDictionary<CacheKeys, UpdateDelegate> _cacheMatrix = new Dictionary<CacheKeys, UpdateDelegate>();

		static CacheService() {

			// set up the caching matrices with keys and caching actions
			_sessionMatrix.Add(CacheKeys.User, SessionUpdateUser);

			_cacheMatrix.Add(CacheKeys.Catalogs, CacheUpdateCatalogs);
			_cacheMatrix.Add(CacheKeys.SearchProperties, CacheUpdateSearchProperties);
			_cacheMatrix.Add(CacheKeys.Tags, CacheUpdateTags);
			_cacheMatrix.Add(CacheKeys.TopTags, CacheUpdateTopTags);
			_cacheMatrix.Add(CacheKeys.Content, CacheUpdateContentFields);
			_cacheMatrix.Add(CacheKeys.Rights, CacheUpdateContentFields);
			//Initialize();
		}

		public static DateTime LastUpdated { get; private set; }

		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		public enum CacheKeys {
			User,
			Catalogs,
			SearchProperties,
			Tags,
			TopTags,
			Content,
			Rights
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
				if ((!_appIsInitialized) || (force)) {
					if (_hasCache) {

						foreach (var key in _cacheMatrix.Keys) {
							_cacheMatrix[key](key);
						}
					}
					_appIsInitialized = true;
					LastUpdated = DateTime.Now;
				}
			}
		}

		public static void InitializeSession() {

			if (HttpContext.Current.User != null && !String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name)) {
				string userName = HttpContext.Current.User.Identity.Name;
				InitializeSession(userName, false);
			} else {
				InitializeSession(null, false);
			}
		}

		public static void InitializeSession(string userName) {

			InitializeSession(userName, false);
		}

		public static void InitializeSession(string userName, bool force) {

			if (HttpContext.Current != null) {
				_session = HttpContext.Current.Session;
				_hasSession = _session != null;

				if ((!_sessionIsInitialized) || (force)) {

					if (userName != null && _hasSession) {

						_sessionMatrix[CacheKeys.User](CacheKeys.User, userName); 
					
					}
					
					_sessionIsInitialized = true;
				}
			}
			
		}

		// ----------------------------------------------------------------------------
		// Session
		// ----------------------------------------------------------------------------
		public static User User(string userName) {
			if (_hasSession) {

				if (Session(CacheKeys.User) == null) { SessionUpdateUser(CacheKeys.User, userName); }
				var value = Session(CacheKeys.User) as User;
				return value;

			} else {
				return GetDataUser(userName);
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
				var value = Cache(CacheKeys.Catalogs) as IList<Catalog>;
				return value;
			} else {
				return GetDataCatalogs();
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
				var value = Cache(CacheKeys.Tags) as IList<Tag>;
				return value;
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
				var value = Cache(CacheKeys.TopTags) as IDictionary<TagType, IList<Tag>>;
				return value;
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
				var value = lookup.Keys.Contains(fieldName) ? lookup[fieldName] : new List<string> { "" } ;
				return value;
			} else {
				return GetDataContentField(fieldName);
			}
		}

		// **************************************
		// Catalogs
		// **************************************
		public static IList<string> RightsField(string fieldName) {
			if (_hasCache) {
				if (Cache(CacheKeys.Rights) == null) { CacheUpdateContentRightsFields(CacheKeys.Content); }
				var lookup = Cache(CacheKeys.Rights) as IDictionary<string, IList<string>>;
				var value = lookup[fieldName];
				return value;
			} else {
				return GetDataContentRightsField(fieldName);
			}
		}

		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------
		
		// **************************************
		// Session
		// **************************************
		private static object Session(CacheKeys key) {
			return _session[key.ToString()];
		}

		// **************************************
		// Cache
		// **************************************
		private static object Cache(CacheKeys key) {
			return Cache(key.ToString());
		}

		private static object Cache(string key) {
			return _cache[key];
		}
	
		// **************************************
		// SessionUpdate
		// **************************************
		private static void SessionUpdate(object cacheObject, CacheKeys cacheKey) {
			if (_hasSession && cacheObject != null) {
				_session.Remove(cacheKey.ToString());

				_session.Add(
					cacheKey.ToString(),
					cacheObject
					);
			}
		}

		private static void SessionUpdateUser(CacheKeys key, params object[] list) {
			var obj = GetDataUser(list[0] as string);
			SessionUpdate(obj, key);				
			
		}

		// **************************************
		// CacheUpdate
		// **************************************
		private static void CacheUpdate(object cacheObject, CacheKeys cacheKey) {
			CacheUpdate(cacheObject, cacheKey.ToString());
		}

		private static void CacheUpdate(object cacheObject, string cacheKey) {

			if (_hasCache && cacheObject != null) {
				_cache.Remove(cacheKey);

				_cache.Insert(
					cacheKey,
					cacheObject,
					null,
					DateTime.Now.AddSeconds(3600),
					System.Web.Caching.Cache.NoSlidingExpiration
					);
			}
		}

		private static void CacheUpdateCatalogs(CacheKeys key, params object[] list) {
			var obj = GetDataCatalogs();
			CacheUpdate(obj, key);
		}

		private static void CacheUpdateSearchProperties(CacheKeys key, params object[] list) {
			var obj = GetDataSearchProperties();
			CacheUpdate(obj, key);
			
		}

		private static void CacheUpdateTags(CacheKeys key, params object[] list) {
			var obj = GetDataTags();
			CacheUpdate(obj, key);
			
		}
		private static void CacheUpdateTopTags(CacheKeys key, params object[] list) {
			var obj = GetDataTopTags();
			CacheUpdate(obj, key);
			
		}

		private static void CacheUpdateContentFields(CacheKeys key, params object[] list) {
			var lookup = GetDataContentFields();
			CacheUpdate(lookup, key);			
		}

		private static void CacheUpdateContentRightsFields(CacheKeys key, params object[] list) {
			var lookup = GetDataContentRightsFields();
			CacheUpdate(lookup, key);
		}
		
		// **************************************
		// GetData
		// **************************************
		private static User GetDataUser(string userName) {
			var obj = AccountData.UserComplete(userName);
			return obj;
		}

		private static IList<Catalog> GetDataCatalogs() {
			var obj = SearchService.GetLookupList<Catalog>();
			return obj;
		}

		private static IList<SearchProperty> GetDataSearchProperties() {
			var obj = SearchService.GetSearchMenuProperties();
			return obj;
		}
		
		private static IList<Tag> GetDataTags() {
			var obj = SearchService.GetLookupList<Tag>();
			return obj;
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
			
			var fields = new string[] { "Title", "Artist", "RecordLabel", "Writers" };
			var lookup = new Dictionary<string, IList<string>>();
			foreach (var field in fields) {

				lookup.Add(field.ToUpper(), GetDataContentField(field));
			}

			return lookup;
		}

		private static IList<string> GetDataContentField(string fieldName){

			return SearchService.GetLookupListContent(fieldName);
		}

		private static IDictionary<string, IList<string>> GetDataContentRightsFields() {
			var fields = new string[] { "Title", "Artist", "RecordLabel" };
			var lookup = new Dictionary<string, IList<string>>();
			foreach (var field in fields) {

				lookup.Add(field.ToUpper(), GetDataContentRightsField(field));
			}

			return lookup; 
		}
		private static IList<string> GetDataContentRightsField(string fieldName) {

			return SearchService.GetLookupListContentRights(fieldName);
		}

		
	}
}
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SongSearch.Web
{

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
	public class RequiresVersion : FilterAttribute, IAuthorizationFilter
	{

		private AppVersion _minAppVersion;
		private bool _versionIsSet;
		public AppVersion MinAppVersion
		{
			get
			{
				return _versionIsSet ? _minAppVersion : App.BaseVersion;
			}
			set
			{
				_versionIsSet = true;
				_minAppVersion = value;
			}
		}
		
		// This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.
		protected virtual bool VersionCore(HttpContextBase httpContext)
		{
			if (httpContext == null)
			{
				throw new ArgumentNullException("httpContext");
			}

			if (App.Version < MinAppVersion)
			{
				return false;
			}
 
			return true;
		}

		private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus) {
			validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
		}
		public void OnAuthorization(AuthorizationContext filterContext) {
			if (filterContext == null) {
				throw new ArgumentNullException("filterContext");
			}

			if (VersionCore(filterContext.HttpContext)) {
				// ** IMPORTANT **
				// Since we're performing authorization at the action level, the authorization code runs
				// after the output caching module. In the worst case this could allow an authorized myUser
				// to cause the page to be cached, then an unauthorized myUser would later be served the
				// cached page. We work around this by telling proxies not to cache the sensitive page,
				// then we hook our custom authorization code into the caching mechanism so that we have
				// the final say on whether a page should be served from the cache.

				var cachePolicy = filterContext.HttpContext.Response.Cache;
				cachePolicy.SetProxyMaxAge(new TimeSpan(0));
				cachePolicy.AddValidationCallback(CacheValidateHandler, null /* dataSession */);
			} else {
				HandleUnauthorizedRequest(filterContext);
			}
		}

		protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
			// Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
			filterContext.Result = new HttpUnauthorizedResult();
		}

		// This method must be thread-safe since it is called by the caching module.
		protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext) {
			if (httpContext == null) {
				throw new ArgumentNullException("httpContext");
			}

			bool isAuthorized = VersionCore(httpContext);
			return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
		}

	}
}


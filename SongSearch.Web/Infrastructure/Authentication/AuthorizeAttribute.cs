using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SongSearch.Web.Services
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RequireAuthorization : FilterAttribute, IAuthorizationFilter
    {

        private readonly object _typeId = new object();

        //private string _roles;
        //private string[] _rolesSplit = new string[0];
		private Roles _minAccessLevel;// = AccessLevels.Client;
        private bool _accessIsSet = false;


        private string _users;
        private string[] _usersSplit = new string[0];

        //public string SystemRoles
        //{
        //    get
        //    {
        //        return _roles ?? String.Empty;
        //    }
        //    set
        //    {
        //        _roles = value;
        //        _rolesSplit = SplitString(value);
        //    }
        //}

		public Roles MinAccessLevel
        {
            get
            {
				return _accessIsSet ? _minAccessLevel : Roles.Client;
            }
            set
            {
                _accessIsSet = true;
                _minAccessLevel = value;
            }
        }
		public Roles[] MyAccessLevels { get; set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public string Users
        {
            get
            {
                return _users ?? String.Empty;
            }
            set
            {
                _users = value;
                _usersSplit = SplitString(value);
            }
        }

        // This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

			var user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            
            if (_usersSplit.Length > 0 && !_usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            //if (_rolesSplit.Length > 0 && UserIsInAnyRole(MyAccessLevels))//!_rolesSplit.Any(myUser.IsInRole))
            if (MyAccessLevels != null && MyAccessLevels.Count() > 0 && !user.UserIsInAnyRole(MyAccessLevels))
            {
                return false;
            }
            
            
            if (_accessIsSet && !user.UserIsAtLeastInRole(MinAccessLevel))//UserIsInAnyRole(MyAccessLevels))//!_rolesSplit.Any(myUser.IsInRole))
            {
                return false;
            } 

            return true;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (AuthorizeCore(filterContext.HttpContext))
            {
                // ** IMPORTANT **
                // Since we're performing authorization at the action level, the authorization code runs
                // after the output caching module. In the worst case this could allow an authorized myUser
                // to cause the page to be cached, then an unauthorized myUser would later be served the
                // cached page. We work around this by telling proxies not to cache the sensitive page,
                // then we hook our custom authorization code into the caching mechanism so that we have
                // the final say on whether a page should be served from the cache.

				var cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            filterContext.Result = new HttpUnauthorizedResult();
        }

        // This method must be thread-safe since it is called by the caching module.
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            bool isAuthorized = AuthorizeCore(httpContext);
            return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }

    }
}


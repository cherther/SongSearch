using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using SongSearch.Web.Data;
using System.Web.Routing;
using System.IO;

namespace SongSearch.Web.Controllers
{
	[RequireAuthorization]
	[HandleError]
	public class MediaController : Controller
    {
		private User _currentUser;

		protected override void Initialize(RequestContext requestContext) {
			_currentUser = CacheService.User(requestContext.HttpContext.User.Identity.Name);
			base.Initialize(requestContext);
		}

		// **************************************
		// Download
		// **************************************
		public ActionResult Download(int id) {

			return Get(id, MediaVersion.FullSong);

		}

		// **************************************
		// Download
		// **************************************
		//[OutputCache(Duration = 60, VaryByParam = "id;version")]
		public ActionResult Get(int id, MediaVersion version = MediaVersion.Preview) {

			var media = MediaService.GetContentMedia(id, (MediaVersion)version);
			var content = SearchService.GetContent(id, _currentUser);
			if (content != null) {
				var downloadName = String.Concat(content.UserDownloadableName, MediaService.ContentMediaExtension);
				var contentType = "application/unknown";
				Response.ContentType = contentType;
				return base.File(media, contentType, downloadName);
			} else {
				var msg = "You do not have access to this file";
				return RedirectToAction("Index", "Error", new { exc = new AccessViolationException(msg), message = msg, controllerName = "Media", actionName = "Download" });
			}

		}

		// **************************************
		// Stream
		// **************************************
		//[OutputCache(Duration = 60, VaryByParam = "id;version")]
		public ActionResult Stream(int id, MediaVersion version = MediaVersion.Preview) {

			var mediaPath = MediaService.GetContentMediaFilePath(id, (MediaVersion) version);
			var content = SearchService.GetContent(id, _currentUser);
			if (content != null) {
				var downloadName = String.Concat(content.UserDownloadableName, MediaService.ContentMediaExtension);
				var contentType = "application/mp3";
				Response.ContentType = contentType;
				// if this is not embedded, adding the header will force it to open the application responsible for the extension
				Response.AddHeader("content-disposition", String.Format("filename={0}", downloadName));
				return new FileStreamResult(new FileStream(mediaPath, System.IO.FileMode.Open), contentType);
			} else {
				var msg = "You do not have access to this file";
				return RedirectToAction("Index", "Error", new { exc = new AccessViolationException(msg), message = msg, controllerName = "Media", actionName = "Download" });
			}
		}



    }
}

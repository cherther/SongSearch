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
	public partial class MediaController : Controller
	{

		protected override void Initialize(RequestContext requestContext) {
			base.Initialize(requestContext);
		}

		// **************************************
		// Download
		// **************************************
		public virtual ActionResult Download(int id) {
			try {
				return Get(id, MediaVersion.FullSong);
			}
			catch {
				this.FeedbackInfo("There was an error downloading this item. Please try again in a bit.");
				return RedirectToAction(MVC.Search.Index());
			}
		}

		// **************************************
		// Download
		// **************************************
		//[OutputCache(Duration = 60, VaryByParam = "id;version")]
		public virtual ActionResult Get(int id, MediaVersion version = MediaVersion.Preview) {
			var user = Account.User();
			var media = MediaService.GetContentMedia(id, (MediaVersion)version, user);
			var content = SearchService.GetContent(id, user);
			if (content != null) {
				var downloadName = String.Concat(content.UserDownloadableName, MediaService.ContentMediaExtension);
				var contentType = "application/unknown";
				Response.ContentType = contentType;
				return base.File(media, contentType, downloadName);
			} else {
				var msg = "You do not have access to this file";
				this.FeedbackError(msg);
				return RedirectToAction(MVC.Error.Index(new AccessViolationException(msg), msg, "Media", "Download" ));
			}

		}

		// **************************************
		// Stream
		// **************************************
		//[OutputCache(Duration = 60, VaryByParam = "id;version")]
		public virtual ActionResult Stream(int id, MediaVersion version = MediaVersion.Preview) {
			try {
				var mediaPath = MediaService.GetContentMediaFilePath(id, (MediaVersion)version);
				var content = SearchService.GetContent(id, Account.User());
				if (content != null) {
					//var downloadName = String.Concat(contentModel.UserDownloadableName, MediaService.ContentMediaExtension);
					var contentType = "application/mp3";
					Response.ContentType = contentType;
					// if this is not embedded, adding the header will force it to open the application responsible for the extension
					//Response.AddHeader("contentModel-disposition", String.Format("filename={0}", downloadName));
					return new FileStreamResult(new FileStream(mediaPath, System.IO.FileMode.Open), contentType);
				} else {
					var msg = "You do not have access to this file";
					this.FeedbackError(msg);
					return RedirectToAction(MVC.Error.Index(new AccessViolationException(msg), msg, "Media", "Stream"));
				}
			}
			catch {
				this.FeedbackError("There was an error loading this page. Please try again in a bit.");
				return RedirectToAction(MVC.Search.Index());
			}
		}



	}
}

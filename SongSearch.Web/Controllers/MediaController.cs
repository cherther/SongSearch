﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using SongSearch.Web.Data;
using System.Web.Routing;
using System.IO;
using System.Configuration;
using Amazon.S3.Model;

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

				var user = Account.User();

				var content = user != null ? SearchService.GetContent(id, user) : null;

				if (content != null) {

					var downloadName = String.Concat(content.UserDownloadableName, SystemConfig.MediaDefaultExtension);
					var contentType = "application/unknown";
					Response.ContentType = contentType;

					//if (SystemConfig.UseRemoteMedia) {
					//    var mediaUrl = _mediaService.GetContentMediaPath(id, MediaVersion.Full);
					//    return base.File(mediaUrl, contentType, downloadName);
					//} else {
					byte[] media = MediaService.GetContentMedia(content.ContentMedia.FullVersion(), user);

					media = MediaService.WriteMediaSignature(media, content, user);

					return base.File(media, contentType, downloadName);
					//}
				} else {
					var msg = "You do not have access to this file";
					this.FeedbackError(msg);
					var ex = new AccessViolationException(msg);
					Log.Error(ex);
					return RedirectToAction(MVC.Error.Problem());
				}
			}
			catch (Exception ex) {
				Log.Error(ex);
				this.FeedbackInfo("There was an error downloading this item. Please try again in a bit.");
				return RedirectToAction(MVC.Search.Index());
			}
		}
		//// **************************************
		//// Download
		//// **************************************
		//public virtual ActionResult DownloadUrl(int id) {
		//    try {
		//        var contentType = "application/unknown";
		//        Response.ContentType = contentType;
		//        var mediaUrl = GetUrl(id, MediaVersion.Full);

		//        if (mediaUrl != null) {
		//            return Redirect(mediaUrl);
		//        } else {
		//            var msg = "You do not have access to this file";
		//            this.FeedbackError(msg);
		//            return RedirectToAction(MVC.Error.Index(new AccessViolationException(msg), msg, "Media", "Stream"));
		//        }
		//    }
		//    catch {
		//        this.FeedbackError("There was an error loading this page. Please try again in a bit.");
		//        return RedirectToAction(MVC.Search.Index());
		//    }
		//}
		//// **************************************
		//// Download
		//// **************************************
		////[OutputCache(Duration = 60, VaryByParam = "id;version")]
		//public virtual ActionResult Get(int id, MediaVersion version = MediaVersion.Preview) {
			
		//    var user = Account.User();
		//    var content = SearchService.GetContent(id, user);
			
		//    if (content != null) {
		//        var downloadName = String.Concat(content.UserDownloadableName, SystemConfig.MediaDefaultExtension);
		//        var contentType = "application/unknown";
		//        Response.ContentType = contentType;

		//        if (SystemConfig.UseRemoteMedia) {
		//            var url = GetUrl(id, version);
		//            return base.File(url, contentType, downloadName);
		//        } else {
		//            var media = _mediaService.GetContentMedia(id, (MediaVersion)version, user);

		//            return base.File(media, contentType, downloadName);
		//        }
		//    } else {
		//        var msg = "You do not have access to this file";
		//        this.FeedbackError(msg);
		//        return RedirectToAction(MVC.Error.Index(new AccessViolationException(msg), msg, "Media", "Download" ));
		//    }

		//}

		// **************************************
		// Stream
		// **************************************
		//[OutputCache(Duration = 60, VaryByParam = "id;version")]
		public virtual ActionResult Stream(int id, MediaVersion version = MediaVersion.Preview) {
			try {

				var content = SearchService.GetContent(id, Account.User());
				var contentMedia = content.ContentMedia.SingleOrDefault(x => x.MediaVersion == (int)version);

				if (content != null) {

					if (SystemConfig.UseRemoteMedia && content.IsMediaOnRemoteServer) {

						return RedirectToAction(Actions.StreamUrl(id, version));

					} else {

						var mediaPath = MediaService.GetContentMediaPath(contentMedia);
						var contentType = "application/mp3";
						Response.ContentType = contentType;
		
						return new FileStreamResult(new FileStream(mediaPath, System.IO.FileMode.Open), contentType);
					}
				} else {
					var msg = "You do not have access to this file";
					this.FeedbackError(msg);
					var ex = new AccessViolationException(msg);
					Log.Error(ex);
					return RedirectToAction(MVC.Error.Problem());
				}
				

			}
			catch (Exception ex) {
				Log.Error(ex);

				this.FeedbackError("There was an error loading this page. Please try again in a bit.");
				return RedirectToAction(MVC.Search.Index());
			}
		}

		public virtual ActionResult StreamUrl(int id, MediaVersion version = MediaVersion.Preview) {
			try {
				var contentType = "application/mp3";
				Response.ContentType = contentType;
				var mediaUrl = GetUrl(id, version);

				if (mediaUrl != null){
					return Redirect(mediaUrl);
				} else {
				var msg = "You do not have access to this file";
					this.FeedbackError(msg);
					var ex = new AccessViolationException(msg);
					Log.Error(ex);
					return RedirectToAction(MVC.Error.Problem());
				}
			}
			catch (Exception ex) {
				Log.Error(ex);

				this.FeedbackError("There was an error loading this page. Please try again in a bit.");
				return RedirectToAction(MVC.Search.Index());
			}
		}
		// **************************************
		// Stream
		// **************************************
		//[OutputCache(Duration = 60, VaryByParam = "id;version")]
		private string GetUrl(int id, MediaVersion version = MediaVersion.Preview) {

			var content = SearchService.GetContent(id, Account.User());
			var contentMedia = content.ContentMedia.SingleOrDefault(x => x.MediaVersion == (int)version);
			if (content != null) {
				return MediaService.GetContentMediaPath(contentMedia);
			} else {
				return null;
			}				
		}

	}
}

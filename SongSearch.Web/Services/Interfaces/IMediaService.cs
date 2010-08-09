using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	public interface IMediaService : IDisposable {

		byte[] GetContentMedia(ContentMedia contentMedia);
		byte[] GetContentMedia(ContentMedia contentMedia, User user);
		//string GetContentMediaFileName(int contentId);
		string GetContentMediaPath(ContentMedia contentMedia);
		void SaveContentMedia(string fromFilePath, ContentMedia contentMedia);
	}
}
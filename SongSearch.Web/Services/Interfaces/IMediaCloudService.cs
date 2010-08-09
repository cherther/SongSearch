using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	public interface IMediaCloudService : IDisposable {

		string GetContentKey(ContentMedia contentMedia);
		string GetContentPrefix(MediaVersion version);
		IList<RemoteContent> GetContentList(MediaVersion version, string filter = null);
		byte[] GetContentMedia(ContentMedia contentMedia);
		void GetContentMedia(string target, ContentMedia contentMedia);
		string GetContentMediaUrl(ContentMedia contentMedia);
		void PutContentMedia(string source, ContentMedia contentMedia);
		
	}
}
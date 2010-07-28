using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	public interface IMediaCloudService : IDisposable {

		string GetContentKey(Content content, MediaVersion version);
		string GetContentPrefix(MediaVersion version);
		IList<RemoteContent> GetContentList(MediaVersion version, string filter = null);
		byte[] GetContentMedia(Content content, MediaVersion version);
		string GetContentMediaUrl(Content content, MediaVersion version);
		void SaveContentMedia(string fromFilePath, Content content, MediaVersion version);
	}
}
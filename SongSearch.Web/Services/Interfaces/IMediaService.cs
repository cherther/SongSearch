using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	public interface IMediaService : IDisposable {

		byte[] GetContentMedia(Content content, MediaVersion version);
		byte[] GetContentMedia(Content content, MediaVersion version, User user);
		string GetContentMediaFileName(int contentId);
		string GetContentMediaPath(Content content, MediaVersion version);
		void SaveContentMedia(string fromFilePath, Content content, MediaVersion version);
	}
}
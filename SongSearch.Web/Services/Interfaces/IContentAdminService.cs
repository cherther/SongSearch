﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	// **************************************
	// IContentAdminService
	// **************************************
	public interface IContentAdminService : IDisposable {
		string ActiveUserName { get; set; }
		//User ActiveUser { get; set; }
		void Update(Content content, 
			IList<int> tags,
			IDictionary<TagType, string> newTagsModel, 
			IList<ContentRightViewModel> rights);

		void UpdateContentMedia(int contentId, IList<UploadFile> uploadFiles);
		void SaveMetaDataToFile(int contentId);
		void Delete(int contentId);
		void Delete(int[] contentIds);
		void DeleteTag(int tagId);
	}
}
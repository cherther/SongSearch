using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {

	public interface ICatalogUploadService : IDisposable {
		string ActiveUserName { get; set; }
		User ActiveUser { get; set; }

		WorkflowEngine<CatalogUploadState> CatalogUploadWorkflow { get; set; }
		
		WorkflowStep<CatalogUploadState> NextStep(CatalogUploadState state);

		CatalogUploadState RunNextStep(CatalogUploadState state); //?

		bool AllCatalogStepsAreComplete();
		bool Validate();
		
	}

}
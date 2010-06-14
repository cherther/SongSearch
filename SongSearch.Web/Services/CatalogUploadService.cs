using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {

	// **************************************
	// CatalogUploadState
	//	Model to hold state information 
	//	while user is going through upload 
	//	process
	// **************************************
	public class CatalogUploadState {

		public IDictionary<int, WorkflowStepStatus> WorkflowStepsStatus { get; set; }
		public int CurrentStepIndex { get; set; }
		public int CatalogId { get; set; }
		public string CatalogName { get; set; }
		public IList<Content> Content { get; set; }
		public IDictionary<MediaVersion, string> MediaFiles { get; set; }

		public CatalogUploadState() {
			WorkflowStepsStatus = new Dictionary<int, WorkflowStepStatus>();
		}
		public CatalogUploadState(int numberOfSteps) {
			WorkflowStepsStatus = new Dictionary<int, WorkflowStepStatus>();
			for (var i = 0; i < numberOfSteps; i++) {
				WorkflowStepsStatus.Add(i, WorkflowStepStatus.Incomplete);
			}
		}

	}

	// **************************************
	// CatalogUploadService
	// **************************************
	public class CatalogUploadService : BaseService,  ICatalogUploadService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private bool _disposed;
		
				
		public CatalogUploadService(IDataSession session) : base(session) {

			CatalogUploadWorkflow = CreateCatalogUploadWorkflow();
		
		}
		public CatalogUploadService(string activeUserIdentity) : base(activeUserIdentity) { }


		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		public WorkflowEngine<CatalogUploadState> CatalogUploadWorkflow { get; set; }

		public WorkflowStep<CatalogUploadState> NextStep(CatalogUploadState state) {

			var incompleteSteps = state.WorkflowStepsStatus.Where(s => s.Value == WorkflowStepStatus.Incomplete);

			return incompleteSteps.Count() > 0 ?
				CatalogUploadWorkflow.WorkflowSteps.SingleOrDefault(s => s.StepIndex == incompleteSteps.First().Key)
				: null;

		}

		public CatalogUploadState RunNextStep(CatalogUploadState state) {

			var step = NextStep(state);
			step.Process(state);
			if (state.WorkflowStepsStatus.ContainsKey(step.StepIndex)) {
				state.WorkflowStepsStatus[step.StepIndex] = WorkflowStepStatus.Complete;
			} else {
				state.WorkflowStepsStatus.Add(step.StepIndex, WorkflowStepStatus.Complete);
			}

			return state;
		}

		public bool AllCatalogStepsAreComplete() {
			throw new NotImplementedException();
		}

		public bool Validate() {
			throw new NotImplementedException();
		}


		// **************************************
		// CatalogUploadWorkflow
		// **************************************
		private WorkflowEngine<CatalogUploadState> CreateCatalogUploadWorkflow() {
			//Type t = Type.GetType("CatalogUploadWizardModel");

			var wf = new WorkflowEngine<CatalogUploadState>(new CatalogUploadState());
			//wf.Model = model;

			wf.WorkflowSteps = new List<WorkflowStep<CatalogUploadState>>();

			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(SelectCatalog, 0, "Select Catalog", "wfSelectCatalog"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(AddSongFiles, 1, "Add Song Files", "wfAddSongFiles"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(AddSongPreviews, 2, "Add Preview Files", "wfAddPreviewFiles"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(EditMetadata, 3, "Edit Metadata", "wfEditMetadata"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(SaveCatalog, 4, "Save Catalog", "wfSaveCatalog"));

			return wf;
		}


		// **************************************	
		// Steps
		// **************************************

		// **************************************	
		// SelectCatalog
		// **************************************
		private CatalogUploadState SelectCatalog(CatalogUploadState state) {

			System.Diagnostics.Debug.Write("Step1");

			// lookup catalog id
			//if (state.CatalogId > 0) {
			//    if (ActiveUser.IsAtLeastInCatalogRole(Roles.Admin, state.CatalogId)) {
			//        throw new AccessViolationException("You do not have admin rights to this catalog");
			//    }
			//} else {
			//    if (ActiveUser.IsAtLeastInRole(Roles.Admin)) {

			//        var catalog = DataSession.Single<Catalog>(c => c.CatalogName.ToUpper() == state.CatalogName) ??
			//            new Catalog() { CatalogName = state.CatalogName };

			//        if (catalog.CatalogId > 0) {
			//            DataSession.Add<Catalog>(catalog);
			//            DataSession.CommitChanges();
			//        }

			//        state.CatalogId = catalog.CatalogId;// CatalogManagementService.CreateCatalog(new Catalog() { CatalogName = model.CatalogName });
			//    }
			//}
			return state;
		}

		// **************************************	
		// AddSongFiles
		// **************************************
		private CatalogUploadState AddSongFiles(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step2");
			return state;
		}

		// **************************************	
		// AddSongPreviews
		// **************************************
		private CatalogUploadState AddSongPreviews(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step3");
			return state;
		}

		// **************************************	
		// EditMetadata
		// **************************************
		private CatalogUploadState EditMetadata(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step4");
			return state;
		}

		// **************************************	
		// SaveCatalog
		// **************************************
		private CatalogUploadState SaveCatalog(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step5");
			return state;
		}

		// ----------------------------------------------------------------------------
		// (Dispose)
		// ----------------------------------------------------------------------------

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		private void Dispose(bool disposing) {
			if (!_disposed) {
				{
					if (DataSession != null) {
						DataSession.Dispose();
						DataSession = null;
					}
					if (ReadSession != null) {
						ReadSession.Dispose();
						ReadSession = null;
					}
				}

				_disposed = true;
			}
		}
	}
}
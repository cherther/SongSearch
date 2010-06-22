using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using System.IO;

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
		public IList<string> TempFiles { get; set; }
		public IList<UploadFile> UploadFiles { get; set; }
		public MediaVersion MediaVersion { get; set; }

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

		// **************************************
		// NextStep
		// **************************************
		public WorkflowStep<CatalogUploadState> NextStep(CatalogUploadState state) {

			var incompleteSteps = state.WorkflowStepsStatus.Where(s => s.Value == WorkflowStepStatus.Incomplete);

			return incompleteSteps.Count() > 0 ?
				CatalogUploadWorkflow.WorkflowSteps.SingleOrDefault(s => s.StepIndex == incompleteSteps.First().Key)
				: null;

		}

		// **************************************
		// RunNextStep
		// **************************************
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

		// **************************************
		// AllCatalogStepsAreComplete
		// **************************************
		public bool AllCatalogStepsAreComplete() {
			throw new NotImplementedException();
		}

		// **************************************
		// Validate
		// **************************************
		public bool Validate() {
			throw new NotImplementedException();
		}

		// **************************************
		// GetUploadPath
		// **************************************
		public string GetUploadPath(string fileName, string mediaVersion = "") {
			string uploadPath = Settings.UploadPath.Text(); // @"D:\Inetpub\wwwroot\Assets\Uploads";// Path.Combine(asset.AssetTypeLocation, "Uploads");

			//string userFileName = String.Concat(_currentUser.UserId, "_", Path.GetFileName(fileName));
			string userFolder = Path.Combine(uploadPath, ActiveUser.UserId.ToString(), mediaVersion);
			if (!Directory.Exists(userFolder)) { Directory.CreateDirectory(userFolder); }

			//string userFileName = String.Concat(User.UserId(), "_", Path.GetFileName(fileName));
			return Path.Combine(userFolder, fileName);
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

			
			// lookup catalog id
			if (state.CatalogId > 0) {
			    if (!ActiveUser.IsAtLeastInCatalogRole(Roles.Admin, state.CatalogId)) {
			        throw new AccessViolationException("You do not have admin rights to this catalog");
			    }

				var catalog = DataSession.Single<Catalog>(c => c.CatalogId == state.CatalogId);
				if (catalog == null) {
					throw new ArgumentOutOfRangeException("Catalog does not exist");
				}
				state.CatalogName = catalog.CatalogName;

			} else {
				state.CatalogName = state.CatalogName.ToUpper();
			}
			return state;
		}

		// **************************************	
		// AddSongFiles
		// **************************************
		private CatalogUploadState AddSongFiles(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step2");
			
			var uploadFiles = MoveToMediaVersionFolder(state.TempFiles.Distinct().ToList(), state.MediaVersion);
			uploadFiles = state.UploadFiles != null ?
				(uploadFiles != null ?
					uploadFiles.Union(state.UploadFiles).ToList()
					: state.UploadFiles)
				: uploadFiles;
			state.UploadFiles = uploadFiles;
			return state;
		}

		

		// **************************************	
		// AddSongPreviews
		// **************************************
		private CatalogUploadState AddSongPreviews(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step3");
//			state.UploadFiles = MoveToMediaVersionFolder(state.TempFiles.Distinct().ToList(), state.MediaVersion);
			if (state.TempFiles != null && state.TempFiles.Count() > 0) {
				
				var uploadFiles = MoveToMediaVersionFolder(state.TempFiles.Distinct().ToList(), state.MediaVersion);
				
				//Attach previews
				uploadFiles = state.UploadFiles != null ?
					(uploadFiles != null ?
						uploadFiles.Union(state.UploadFiles).ToList()
						: state.UploadFiles)
					: uploadFiles;

				state.UploadFiles = uploadFiles;
			}

			IList<Content> content = new List<Content>();

			var contentFiles = state.UploadFiles.Select(f => f.FileName).Distinct().ToList();
			foreach (var file in contentFiles) {

				var fileName = Path.GetFileNameWithoutExtension(file).Split('-');
				var title = fileName.First();
				var artist = fileName.Length > 1 ? fileName[1] : "(N/A)";
				int releaseYear;
				int.TryParse(fileName.Length > 2 ? fileName[2] : "", out releaseYear);

				var files = state.UploadFiles.Where(f => f.FileName == file).ToList();

				var full = files.SingleOrDefault(f => f.FileMediaVersion == MediaVersion.FullSong);
				var preview = files.SingleOrDefault(f => f.FileMediaVersion == MediaVersion.Preview);

				content.Add(new Content() {
					Title = title,
					Artist = artist,
					ReleaseYear = releaseYear.AsNullIfZero(),
					HasMediaFullVersion = full != null,
					HasMediaPreviewVersion = preview != null,
					UploadFiles = files
				});
			}

			state.Content = content;
			
			return state;
		}

		// **************************************	
		// EditMetadata
		// **************************************
		private CatalogUploadState EditMetadata(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step4");
			// Check Metadata, e.g. make stuff uppercase etc
			var content = state.Content;
			foreach (var itm in content) {
				itm.Title = itm.Title.ToUpper();
				itm.Artist = itm.Artist.ToUpper();
				itm.RecordLabel = itm.RecordLabel.ToUpper();
				itm.ReleaseYear = itm.ReleaseYear.GetValueOrDefault().AsNullIfZero();
			}
			state.Content = content;

			return state;
		}

		// **************************************	
		// SaveCatalog
		// **************************************
		private CatalogUploadState SaveCatalog(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step5");

			// Save/create Catalog
			if (ActiveUser.IsAtLeastInRole(Roles.Admin)) {

				var catalog = DataSession.Single<Catalog>(c => c.CatalogName.ToUpper() == state.CatalogName) ??
					new Catalog() { CatalogName = state.CatalogName };

				if (catalog.CatalogId == 0) {

					DataSession.Add<Catalog>(catalog);
					
					var userCatalog = new UserCatalogRole() {
						UserId = ActiveUser.UserId,
						CatalogId = catalog.CatalogId,
						RoleId = (int)Roles.Admin
					};
					
					//Make current user an admin
					DataSession.Add<UserCatalogRole>(userCatalog);
					// defer?
					DataSession.CommitChanges();
				}

				state.CatalogId = catalog.CatalogId;
				state.CatalogName = catalog.CatalogName.ToUpper();


				// Save Content
				foreach (var itm in state.Content) {

					itm.CatalogId = state.CatalogId;
					itm.CreatedByUserId = ActiveUser.UserId;
					itm.CreatedOn = DateTime.Now.Date;
					itm.LastUpdatedByUserId = ActiveUser.UserId;
					itm.LastUpdatedOn = DateTime.Now.Date;
					
					DataSession.Add<Content>(itm);
					DataSession.CommitChanges();

					foreach (var file in itm.UploadFiles) {

						var filePath = itm.MediaFilePath(file.FileMediaVersion);

						Files.SafeMove(file.FilePath, filePath, true);

					}
				}
				
			
			}

			CacheService.InitializeApp(true);
			
			return state;
		}

		// **************************************	
		// MoveToMediaVersionFolder
		// **************************************
		private IList<UploadFile> MoveToMediaVersionFolder(List<string> files, MediaVersion mediaVersion) {

			var newFiles = new List<string>();
			foreach (var file in files) {
				var filePath = GetUploadPath(file);
				var newFilePath = GetUploadPath(file, mediaVersion.ToString());
				if (File.Exists(newFilePath)) { File.Delete(newFilePath);}
				File.Move(filePath, newFilePath);
				newFiles.Add(newFilePath);
			}
			return newFiles.GetUploadFiles(mediaVersion);
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
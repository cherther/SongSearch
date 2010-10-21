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
	public class CatalogUploadService : ICatalogUploadService {


		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private bool _disposed;
				
		public CatalogUploadService(){

			CatalogUploadWorkflow = CreateCatalogUploadWorkflow();
		
		}


		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		public WorkflowEngine<CatalogUploadState> CatalogUploadWorkflow { get; set; }

		// **************************************
		// NextStep
		// **************************************
		public WorkflowStep<CatalogUploadState> NextStep(CatalogUploadState state) {
			if (state.WorkflowStepsStatus == null) {
				return null;
			}
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
			if (step != null) {
				step.Process(state);
				if (state.WorkflowStepsStatus.ContainsKey(step.StepIndex)) {
					state.WorkflowStepsStatus[step.StepIndex] = WorkflowStepStatus.Complete;
				} else {
					state.WorkflowStepsStatus.Add(step.StepIndex, WorkflowStepStatus.Complete);
				}
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
		// CatalogUploadWorkflow
		// **************************************
		private WorkflowEngine<CatalogUploadState> CreateCatalogUploadWorkflow() {
			//Type t = Type.GetType("CatalogUploadWizardModel");

			var wf = new WorkflowEngine<CatalogUploadState>(new CatalogUploadState());
			//wf.Model = model;

			wf.WorkflowSteps = new List<WorkflowStep<CatalogUploadState>>();

			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(SelectCatalog, 0, "Select Catalog", "wfSelectCatalog", "Next Step"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(AddSongFiles, 1, "Upload Song Files", "wfAddSongFiles", "Next Step"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(AddSongPreviews, 2, "Upload Preview Files", "wfAddPreviewFiles", "Next Step"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(EditMetadata, 3, "Review Song Data", "wfEditMetadata", "Next Step"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(SaveCatalog, 4, "Save Catalog", "wfSaveCatalog", "Save"));
			wf.WorkflowSteps.Add(new WorkflowStep<CatalogUploadState>(ReviewComplete, 5, "Complete", "wfComplete", "Done"));

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
			    if (!Account.User().IsAtLeastInCatalogRole(Roles.Admin, state.CatalogId)) {
					throw new AccessViolationException("You do not have admin rights to this catalog.", innerException: null);
			    }

				using(var ctx = new SongSearchContext()){
					var catalog = ctx.Catalogs.SingleOrDefault(c => c.CatalogId == state.CatalogId);
					if (catalog == null) {
						throw new ArgumentOutOfRangeException("Catalog does not exist.", innerException: null);
					}
					state.CatalogName = catalog.CatalogName;
					return state;
				}

			} 
			
			if (!String.IsNullOrWhiteSpace(state.CatalogName)) {
				state.CatalogName = state.CatalogName.ToUpper();
				return state;
			} else {
				throw new ArgumentNullException("Catalog name cannot be blank. Please enter a catalog name or select an existing catalog.", innerException: null);
			}
			
			
		}

		// **************************************	
		// AddSongFiles
		// **************************************
		private CatalogUploadState AddSongFiles(CatalogUploadState state) {

			if (state.TempFiles == null){
				throw new ArgumentNullException("There was an error uploading your song files", innerException: null);
			}

			var filesList = state.TempFiles.Distinct().ToList();
			var uploadFiles = MoveToMediaVersionFolder(filesList, state.MediaVersion);

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

			if (state.TempFiles != null && state.TempFiles.Count() > 0) {
				
				var uploadFiles = MoveToMediaVersionFolder(state.TempFiles.Distinct().ToList(), state.MediaVersion);
				
				//Attach previews
				uploadFiles = state.UploadFiles != null ?
					(uploadFiles != null ?
						uploadFiles.Union(state.UploadFiles).ToList()
						: state.UploadFiles)
					: uploadFiles;

				state.UploadFiles = uploadFiles.Distinct().ToList();
			}

			IList<Content> contentList = new List<Content>();
			IDictionary<UploadFile, ID3Data> taggedContent = new Dictionary<UploadFile, ID3Data>();

			//var contentFiles = state.UploadFiles.Select(f => f.FileName).Distinct().ToList();

			foreach (var file in state.UploadFiles) {

				taggedContent.Add(file, ID3Reader.GetID3Metadata(file.FilePath));
			}

			// try filename
			// try title + artist
			var taggedFullSongs = taggedContent.Where(c => c.Key.FileMediaVersion == MediaVersion.Full);
			var taggedPreviews = taggedContent.Where(c => c.Key.FileMediaVersion == MediaVersion.Preview);
			foreach (var itm in taggedFullSongs) {

				var preview = new KeyValuePair<UploadFile, ID3Data>();

				if (itm.Value.Title != null && itm.Value.Artist != null) {
					//Get a matching preview file based on ID3 data or filename
					preview = taggedPreviews.Where(c => c.Value.Title != null && c.Value.Artist != null)
										.FirstOrDefault(c =>
										c.Value.Title.Equals(itm.Value.Title, StringComparison.InvariantCultureIgnoreCase) &&
										c.Value.Artist.Equals(itm.Value.Artist, StringComparison.InvariantCultureIgnoreCase)
										);
				}
				if (preview.Key == null && itm.Key.FileName != null) {
					preview = taggedPreviews.Where(c => c.Key.FileName != null)
							.FirstOrDefault(c =>
							c.Key.FileName.Equals(itm.Key.FileName, StringComparison.InvariantCultureIgnoreCase)
							);
				}
							
				
				var id3 = itm.Value;
				//var file = new FileInfo(itm.Key.FilePath);
				var fileNameData = Path.GetFileNameWithoutExtension(itm.Key.FileName).Split('-');

				var title = id3.Title.AsNullIfWhiteSpace() ?? fileNameData.First();
				var artist = id3.Artist.AsNullIfWhiteSpace() ?? (fileNameData.Length > 1 ? fileNameData[1] : "");
				var album = id3.Album.AsNullIfWhiteSpace() ?? "";
				string year = id3.Year.AsNullIfWhiteSpace() ?? (fileNameData.Length > 2 ? fileNameData[2] : "");
				int releaseYear;
				int.TryParse(year, out releaseYear);


				//var kb = (int)(file.Length * 8 / 1024);
				//var secs = (id3.LengthMilliseconds / 1000);
				
				var content = new Content() {
					Title = title,
					Artist = artist,
					ReleaseYear = releaseYear.AsNullIfZero(),
					Notes = !String.IsNullOrWhiteSpace(album) ? String.Concat("Album: ", album) : null,
					HasMediaFullVersion = true,
					HasMediaPreviewVersion = preview.Key != null,
					//FileType = "mp3",
					//FileSize = (int)file.Length,
					//BitRate = secs > 0 ? (kb / secs) : 0,
					UploadFiles = new List<UploadFile>()
				};

				content.UploadFiles.Add(itm.Key);
				if (preview.Key != null) { content.UploadFiles.Add(preview.Key); }

				contentList.Add(content);
			}

			state.Content = contentList;
			
			return state;
		}

		// **************************************	
		// EditMetadata
		// **************************************
		private CatalogUploadState EditMetadata(CatalogUploadState state) {
			
			// Check Metadata, e.g. make stuff uppercase etc
			foreach (var itm in state.Content) {
				itm.Title = itm.Title.AsEmptyIfNull();//.ToUpper();
				itm.Artist = itm.Artist.AsEmptyIfNull();//.ToUpper();
				itm.RecordLabel = itm.RecordLabel.AsEmptyIfNull();//.ToUpper();
				itm.ReleaseYear = itm.ReleaseYear.GetValueOrDefault().AsNullIfZero();
				itm.Notes = itm.Notes;
			}
			
			return state;
		}

		// **************************************	
		// SaveCatalog
		// **************************************
		private CatalogUploadState SaveCatalog(CatalogUploadState state) {
			System.Diagnostics.Debug.Write("Step5");

			var user = Account.User();
			// Save/create Catalog
			if (user.IsAtLeastInRole(Roles.Admin) && !user.MyBalances().NumberOfSongs.IsAtTheLimit) {

				using (var ctx = new SongSearchContext()) {

					var catalog = ctx.Catalogs.SingleOrDefault(c => c.CatalogName.ToUpper() == state.CatalogName) ??
						new Catalog() {
							CatalogName = state.CatalogName,
							CreatedByUserId = user.UserId,
							CreatedOn = DateTime.Now
						};

					if (catalog.CatalogId == 0) {

						ctx.Catalogs.AddObject(catalog);
						//DataSession.CommitChanges();

						//Make current user an admin
						var userCatalog = new UserCatalogRole() {
							UserId = user.UserId,
							CatalogId = catalog.CatalogId,
							RoleId = (int)Roles.Admin
						};
						ctx.UserCatalogRoles.AddObject(userCatalog);

						//Make parent user an admin
						if (user.ParentUserId.HasValue) {
							var parentUserCatalog =
								//DataSession.Single<UserCatalogRole>(
								//    x => x.UserId == user.ParentUserId.Value &&
								//        x.CatalogId == catalog.CatalogId &&
								//        x.RoleId == (int)Roles.Admin
								//        ) ?? 
								new UserCatalogRole() {
									UserId = user.ParentUserId.Value,
									CatalogId = catalog.CatalogId,
									RoleId = (int)Roles.Admin
								};
							ctx.UserCatalogRoles.AddObject(parentUserCatalog);
						}

						//Make plan user an admin
						if (!user.IsPlanOwner && user.PlanUserId != user.ParentUserId.GetValueOrDefault()) {
							var planUserCatalog =
								//DataSession.Single<UserCatalogRole>(
								//    x => x.UserId == user.PlanUserId && 
								//        x.CatalogId == catalog.CatalogId && 
								//        x.RoleId == (int)Roles.Admin
								//        ) ??
								new UserCatalogRole() {
									UserId = user.PlanUserId,
									CatalogId = catalog.CatalogId,
									RoleId = (int)Roles.Admin
								};
							ctx.UserCatalogRoles.AddObject(planUserCatalog);
						}

						// defer?
						ctx.SaveChanges();
					}

					state.CatalogId = catalog.CatalogId;
					state.CatalogName = catalog.CatalogName.ToUpper();

					// Save Content
					var content = App.IsLicensedVersion ?
						state.Content.Take(user.MyBalances().NumberOfSongs.IsGoodFor(state.Content.Count())).ToList() :
						state.Content;

					foreach (var itm in content) {

						itm.CatalogId = state.CatalogId;
						itm.CreatedByUserId = user.UserId;
						itm.CreatedOn = DateTime.Now;
						itm.LastUpdatedByUserId = user.UserId;
						itm.LastUpdatedOn = DateTime.Now;
						//itm.IsMediaOnRemoteServer = false;

						itm.Title = itm.Title.AsEmptyIfNull();//.CamelCase();//.ToUpper();
						itm.Artist = itm.Artist.AsEmptyIfNull();//.CamelCase();//.ToUpper();
						itm.RecordLabel = itm.RecordLabel.AsEmptyIfNull();//.CamelCase();//.ToUpper();
						itm.ReleaseYear = itm.ReleaseYear.GetValueOrDefault().AsNullIfZero();
						itm.Notes = itm.Notes;

						var full = itm.UploadFiles.SingleOrDefault(f => f.FileMediaVersion == MediaVersion.Full);
						foreach (var version in ModelEnums.MediaVersions()) {

							var upl = itm.UploadFiles.SingleOrDefault(f => f.FileMediaVersion == version);
							if (upl != null) {

								var file = new FileInfo(upl.FilePath);
								var id3 = ID3Writer.NormalizeTag(upl.FilePath, itm);

								var media = new ContentMedia() {

									MediaVersion = (int)version,
									MediaType = "mp3",
									MediaSize = file.Length,
									MediaLength = id3.MediaLength,
									MediaDate = file.GetMediaDate(),
									MediaBitRate = id3.GetBitRate(file.Length)
								};

								media.IsRemote = false;

								itm.ContentMedia.Add(media);
							}


						}
						ctx.Contents.AddObject(itm);
						ctx.AddToSongsBalance(user);
						ctx.SaveChanges();

						if (itm.ContentId > 0) {
							foreach (var file in itm.UploadFiles) {
								MediaService.SaveContentMedia(file.FilePath, itm.Media(file.FileMediaVersion));
							}
						}


					}

				}
			}

			SessionService.Session().InitializeSession(true);
			CacheService.InitializeApp(true);
			
			return state;
		}

		
		private CatalogUploadState ReviewComplete(CatalogUploadState state) {
			return state;
		}
		// **************************************	
		// MoveToMediaVersionFolder
		// **************************************
		private IList<UploadFile> MoveToMediaVersionFolder(List<string> files, MediaVersion mediaVersion) {

			var newFiles = new List<string>();
			var user = Account.User();

			foreach (var file in files) {
				var filePath = user.UploadFile(fileName: file);

				if (File.Exists(filePath)) {

					var newFilePath = user.UploadFile(file, mediaVersion.ToString());
					if (File.Exists(newFilePath)) { File.Delete(newFilePath); }

					File.Move(filePath, newFilePath);
					newFiles.Add(newFilePath);
				}
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
					
				}

				_disposed = true;
			}
		}
	}
}
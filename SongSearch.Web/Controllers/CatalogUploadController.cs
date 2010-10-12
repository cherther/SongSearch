using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using SongSearch.Web.Data;
using System.Web.Routing;
using System.IO;

namespace SongSearch.Web.Controllers
{
	[HandleError]
	[RequireAuthorization(MinAccessLevel = Roles.Admin)]
	public partial class CatalogUploadController : Controller
	{
		ICatalogUploadService _catUploadService;
		IUserEventLogService _logService;
		
		protected override void Initialize(RequestContext requestContext) {

			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_catUploadService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
				_logService.SessionId = requestContext.HttpContext.Session.SessionID;
			}
			base.Initialize(requestContext);

		}

		public CatalogUploadController(ICatalogUploadService catUploadService, IUserEventLogService logService) {
			_catUploadService = catUploadService;
			_logService = logService;
		}


		// GET: /CatalogUpload/
		public virtual ActionResult Index() {

			

			return View();
		}

		public virtual ActionResult Complete()
		{
			// Cleanup upload folder
			FileSystem.SafeDeleteFolder(User.User().UploadFolder(create: false));
			//SessionService.Session().InitializeSession(true);
			//CacheService.InitializeApp(true);

			return RedirectToAction(MVC.CatalogManagement.Index());
		}

		// **************************************
		// URL: /Catalog/Upload
		// **************************************
		public virtual ActionResult Upload(int? id) {

			// Cleanup upload folder
			FileSystem.SafeDeleteFolder(User.User().UploadFolder(create: false));

			var state = new CatalogUploadState(_catUploadService.CatalogUploadWorkflow.WorkflowSteps.Count);
			if (id.GetValueOrDefault() > 0) {
				state.CatalogId = id.GetValueOrDefault();
			}
			var nextStep = _catUploadService.NextStep(state);
			state.CurrentStepIndex = nextStep.StepIndex;
			Session["CatalogUploadState.WorkflowStepsStatus"] = state.WorkflowStepsStatus;
			Session["CatalogUploadState.UploadFiles"] = state.UploadFiles;

			var vm = GetCatalogViewModel(nextStep, state);

			return View(vm);
			
		}

		[HttpPost]
		public virtual ActionResult Upload(CatalogUploadState state) {

			var stepStatus = Session["CatalogUploadState.WorkflowStepsStatus"] as IDictionary<int, WorkflowStepStatus>;
			var uploadFiles = Session["CatalogUploadState.UploadFiles"] as IList<UploadFile>;
			
			if (state == null) {
				state = new CatalogUploadState();
			} else {
				state.WorkflowStepsStatus = stepStatus;
				state.UploadFiles = uploadFiles;
//				state.TempFiles = state.TempFiles ?? sessionFiles;
			}
			var wf = _catUploadService.CatalogUploadWorkflow;
			try {
				state = _catUploadService.RunNextStep(state);

				var nextStep = _catUploadService.NextStep(state);

				if (nextStep != null) {
					state.CurrentStepIndex = nextStep.StepIndex;
					Session["CatalogUploadState.WorkflowStepsStatus"] = state.WorkflowStepsStatus;
					// Add any new files from this step
					//uploadFiles = state.UploadFiles != null ?
					//    (uploadFiles != null ?
					//        uploadFiles.Union(state.UploadFiles).ToList()
					//        : state.UploadFiles)
					//    : uploadFiles;
					Session["CatalogUploadState.UploadFiles"] = state.UploadFiles != null ? state.UploadFiles.Distinct().ToList() : state.UploadFiles;
					var vm = GetCatalogViewModel(nextStep, state);
					vm.StepActionName = nextStep.StepButton;
					
					return View(vm);
				} else {
					_logService.LogUserEvent(UserActions.UploadCatalog);

					return RedirectToAction("Complete");
				}
			}
			catch (Exception ex){
			    Log.Error(ex);
			    this.FeedbackError(ex.Message ?? "There was an error uploading your song files. Please try again.");
			    return RedirectToAction(Actions.Upload());
			}

		}

		// **************************************
		// URL: /Catalog/UserMediaUpload
		// **************************************
		public virtual void UserMediaUpload() {

			//IDictionary<string, string> files = new Dictionary<string, string>();
			
			int chunk = Request.QueryString["chunk"] != null ? int.Parse(Request.QueryString["chunk"]) : 0;
			string fileName = Request.QueryString["name"] != null ? Request.QueryString["name"] : "";
			string mediaVersion = Request.QueryString["mediaVersion"] != null ? Request.QueryString["mediaVersion"] : "";

			var filePath = User.User().UploadFile(fileName: fileName, mediaVersion: mediaVersion);
			
			var buffer = new Byte[Request.InputStream.Length];
			Request.InputStream.Read(buffer, 0, buffer.Length);

			//open a file, if our chunk is 1 or more, we should be appending to an existing file, otherwise create a new file
			var fs = new FileStream(filePath, chunk == 0 ? FileMode.OpenOrCreate : FileMode.Append);

			//write the buffer to a file.
			fs.Write(buffer, 0, buffer.Length);
			fs.Close();

			//return Json("1");//new { Files = new string[] { filePath }});
		}

		private CatalogUploadViewModel GetCatalogViewModel(WorkflowStep<CatalogUploadState> nextStep, CatalogUploadState state){

			var vm = new CatalogUploadViewModel();
			vm.PageTitle = nextStep.StepName;
			vm.NavigationLocation = new string[] { "Admin" };
			vm.CatalogUploadState = state;
			vm.StepView = nextStep.StepView;
			vm.StepActionName = nextStep.StepButton;// "Next Step";
			vm.MyCatalogs = Account.User(false).MyAdminCatalogs().OrderBy(c => c.CatalogName).ToList();
			vm.MyPricingPlan = Account.User().PricingPlan;
			vm.MyUserQuotas = Account.User().MyQuotas();
//			vm.DefaultSongQuota = vm.PricingPlans.Where(p => p.IsEnabled == true).Min(p => p.NumberOfSongs);
			vm.MinUploadFiles = 1;
			
			return vm;
		}


	}
}

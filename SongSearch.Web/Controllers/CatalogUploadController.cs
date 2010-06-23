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
		private ICatalogUploadService _catUploadService;
		private User _currentUser;

		protected override void Initialize(RequestContext requestContext) {

			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_catUploadService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
				_currentUser = _catUploadService.ActiveUser;
			}
			base.Initialize(requestContext);

		}

		public CatalogUploadController(ICatalogUploadService catUploadService) {
			_catUploadService = catUploadService;
		}


		// GET: /CatalogUpload/
		public virtual ActionResult Index() {

			

			return View();
		}

		public virtual ActionResult Complete()
		{
			// Cleanup upload folder
			FileSystem.SafeDeleteFolder(_currentUser.UploadFolder(create: false));
			CacheService.InitializeApp(true);
			SessionService.Session().InitializeSession(true);

			return RedirectToAction(MVC.CatalogManagement.Index());
		}

		// **************************************
		// URL: /Catalog/Upload
		// **************************************
		public virtual ActionResult Upload() {

			// Cleanup upload folder
			FileSystem.SafeDeleteFolder(_currentUser.UploadFolder(create: false));

			var state = new CatalogUploadState(_catUploadService.CatalogUploadWorkflow.WorkflowSteps.Count);

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
				if (state.CurrentStepIndex == state.WorkflowStepsStatus.Count() - 1) {
					vm.StepActionName = "Save";
				}
	
				return View(vm);
			} else {
				return RedirectToAction("Complete");
			}

		}

		// **************************************
		// URL: /Catalog/UserMediaUpload
		// **************************************
		public virtual ActionResult UserMediaUpload() {

			IDictionary<string, string> files = new Dictionary<string, string>();
			
			int chunk = Request.QueryString["chunk"] != null ? int.Parse(Request.QueryString["chunk"]) : 0;
			string fileName = Request.QueryString["name"] != null ? Request.QueryString["name"] : "";

			var filePath = _currentUser.UploadFile(fileName: fileName);
			
			var buffer = new Byte[Request.InputStream.Length];
			Request.InputStream.Read(buffer, 0, buffer.Length);

			//open a file, if our chunk is 1 or more, we should be appending to an existing file, otherwise create a new file
			var fs = new FileStream(filePath, chunk == 0 ? FileMode.OpenOrCreate : FileMode.Append);

			//write the buffer to a file.
			fs.Write(buffer, 0, buffer.Length);
			fs.Close();

			return Json(new { Files = new string[] { filePath }});
		}

		private CatalogUploadViewModel GetCatalogViewModel(WorkflowStep<CatalogUploadState> nextStep, CatalogUploadState state){

			var vm = new CatalogUploadViewModel();
			vm.PageTitle = nextStep.StepName;
			vm.CatalogUploadState = state;
			vm.StepView = nextStep.StepView;
			vm.StepActionName = "Next";
			vm.MyCatalogs = _currentUser.MyAdminCatalogs().OrderBy(c => c.CatalogName).ToList();
			return vm;
		}


	}
}

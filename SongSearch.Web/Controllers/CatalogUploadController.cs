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
		public ActionResult Index() {

			return View();
		}

		public virtual ActionResult Complete()
		{
			CacheService.InitializeApp(true);
			SessionService.Session().InitializeSession(true);

			return RedirectToAction(MVC.CatalogManagement.Index());
		}

		// **************************************
		// URL: /Catalog/UploadWizard
		// **************************************
		public virtual ActionResult Upload() {

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

		public virtual ActionResult MediaUpload() {

			IDictionary<string, string> files = new Dictionary<string, string>();
			//foreach (string fileName in Request.Files) {
			//    var file = Request.Files[fileName] as HttpPostedFileBase;

			//    if (file.ContentLength > 0) {
			//        //TODO: CommitChanges Asset Data
			//        string uploadPath = Settings.UploadPath.Text(); // @"D:\Inetpub\wwwroot\Assets\Uploads";// Path.Combine(asset.AssetTypeLocation, "Uploads");

			//        string userFileName = String.Concat(_currentUser.UserId, "_", Path.GetFileName(file.FileName));
			//        string filePath = Path.Combine(uploadPath, userFileName);
			//        file.SaveAs(filePath);
			//        files.Add("FileName", userFileName);
			//    }
			//}

			int chunk = Request.QueryString["chunk"] != null ? int.Parse(Request.QueryString["chunk"]) : 0;
			string fileName = Request.QueryString["name"] != null ? Request.QueryString["name"] : "";

			var filePath = _catUploadService.GetUploadPath(fileName);

			var buffer = new Byte[Request.InputStream.Length];
			Request.InputStream.Read(buffer, 0, buffer.Length);

			//open a file, if our chunk is 1 or more, we should be appending to an existing file, otherwise create a new file
			var fs = new FileStream(filePath, chunk == 0 ? FileMode.OpenOrCreate : FileMode.Append);

			//write the buffer to a file.
			fs.Write(buffer, 0, buffer.Length);
			fs.Close();

			////open a file, if our chunk is 1 or more, we should be appending to an existing file, otherwise create a new file
			//string uploadPath = Settings.UploadPath.Text(); // @"D:\Inetpub\wwwroot\Assets\Uploads";// Path.Combine(asset.AssetTypeLocation, "Uploads");

			////string userFileName = String.Concat(User.UserID(), "_", Path.GetFileName(fileName));
			//string filePath = Path.Combine(uploadPath, fileName);

			//FileStream fs = new FileStream(filePath, chunk == 0 ? FileMode.OpenOrCreate : FileMode.Append);

			//write our input stream to a buffer

			return Json(new { Files = files.ToArray() });
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

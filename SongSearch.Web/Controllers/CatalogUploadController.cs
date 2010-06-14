using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using SongSearch.Web.Data;
using System.Web.Routing;

namespace SongSearch.Web.Controllers
{
	[HandleError]
	[RequireAuthorization(MinAccessLevel = Roles.Admin)]
	public class CatalogUploadController : Controller
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

		public ActionResult Index()
		{
			return View();
		}

		// **************************************
		// URL: /Catalog/UploadWizard
		// **************************************
		public ActionResult Upload() {

			var state = new CatalogUploadState(_catUploadService.CatalogUploadWorkflow.WorkflowSteps.Count);

			var nextStep = _catUploadService.NextStep(state);
			state.CurrentStepIndex = nextStep.StepIndex;

			var vm = new CatalogUploadViewModel();
			vm.PageTitle = nextStep.StepName;
			vm.CatalogUploadState = state;
			vm.StepView = nextStep.StepView;
//			vm.WizardModel = model;
			Session["CatalogUploadState"] = state;

			return View(vm);
			
		}

		[HttpPost]
		public ActionResult Upload(int stepIndex) {
			var state = Session["CatalogUploadState"] as CatalogUploadState;
			if (state == null) {
				state = new CatalogUploadState();
			}

			var wf = _catUploadService.CatalogUploadWorkflow;
			state = _catUploadService.RunNextStep(state);

			var nextStep = _catUploadService.NextStep(state);

			if (nextStep != null) {
				state.CurrentStepIndex = nextStep.StepIndex;
				var vm = new CatalogUploadViewModel();
				vm.PageTitle = nextStep.StepName;
				vm.CatalogUploadState = state;
				vm.StepView = nextStep.StepView;
				Session["CatalogUploadState"] = state;
				return View(vm);
			} else {
				return RedirectToAction("Index");
			}

		}


	}
}

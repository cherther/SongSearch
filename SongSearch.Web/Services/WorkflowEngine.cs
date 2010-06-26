using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	
	// **************************************
	// WorkflowEngine
	// **************************************
	public class WorkflowEngine<T> {

		public T WorkflowState { get; set; }
		public IList<WorkflowStep<T>> WorkflowSteps { get; set; }

		public WorkflowEngine(T state){
			WorkflowState = state;
		}

		public T Process(int stepIndex) {
			var step = WorkflowSteps.Where(s => s.StepIndex == stepIndex).SingleOrDefault();

			if (step != null) {

				return step.Process(WorkflowState);
			}

			
			return WorkflowState;

		}
	}

	// **************************************
	// WorkflowStep
	// **************************************
	public enum WorkflowStepStatus {
		Incomplete = 0,
		InProgress,
		Complete
	}

	public class WorkflowStep<T> {

		public delegate T StepActionDelegate(T state);

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		public int StepIndex { get; set; }
		//public bool IsStepComplete { get; set; }
		public string StepName { get; set; }
		public string StepButton { get; set; }
		public StepActionDelegate StepAction { get; set; }
		public string StepView { get; set; }
		public WorkflowStepStatus StepStatus {get;set;}

		public WorkflowStep(StepActionDelegate stepAction, int stepIndex, string stepName, string stepView, string stepButton) {
			StepAction = stepAction;
			StepIndex = stepIndex;
			StepName = stepName;
			StepView = stepView;
			StepButton = stepButton.AsNullIfWhiteSpace() ?? "Next";
		}

		public T Process(T state) {
			if (state != null) {
				
				return StepAction(state);
			}
			return state;
		}

	}

}
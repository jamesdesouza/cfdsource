using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class ParallelExecutionComponentBranchValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		ParallelExecutionComponentBranch parallelExecutionComponentBranch = obj as ParallelExecutionComponentBranch;
		if (parallelExecutionComponentBranch.Parent != null && parallelExecutionComponentBranch.Parent.EnabledActivities.Count > 0)
		{
			if (parallelExecutionComponentBranch.EnabledActivities.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ParallelExecutionBranch.EmptySequence"), 0, isWarning: false, ""));
			}
			if (parallelExecutionComponentBranch != parallelExecutionComponentBranch.Parent.EnabledActivities[0] && !ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(parallelExecutionComponentBranch.GetRootFlow().FileObject.GetProjectObject(), parallelExecutionComponentBranch.EnabledActivities))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ParallelExecutionBranch.CallRelatedComponentsNotAllowed"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}

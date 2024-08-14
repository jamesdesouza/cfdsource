using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

public class ParallelExecutionComponentValidator : CompositeActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		ParallelExecutionComponent parallelExecutionComponent = obj as ParallelExecutionComponent;
		if (parallelExecutionComponent.Parent != null)
		{
			if (parallelExecutionComponent.EnabledActivities.Count < 2)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ParallelExecution.InvalidBranchCount"), 0, isWarning: false, ""));
			}
			int num = 0;
			ProjectObject projectObject = parallelExecutionComponent.GetRootFlow().FileObject.GetProjectObject();
			foreach (ParallelExecutionComponentBranch enabledActivity in parallelExecutionComponent.EnabledActivities)
			{
				if (ComponentDesignerHelper.IsMakeCallComponentUsed(projectObject, enabledActivity.EnabledActivities))
				{
					num++;
				}
			}
			if (num > 1)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ParallelExecution.MakeCallInMoreThanOneBranch"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}

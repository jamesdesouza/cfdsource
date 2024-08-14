using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class ConditionalComponentValidator : CompositeActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		ConditionalComponent conditionalComponent = obj as ConditionalComponent;
		if (conditionalComponent.Parent != null && conditionalComponent.EnabledActivities.Count == 0)
		{
			validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Conditional.InvalidBranchCount"), 0, isWarning: false, ""));
		}
		return validationErrorCollection;
	}
}

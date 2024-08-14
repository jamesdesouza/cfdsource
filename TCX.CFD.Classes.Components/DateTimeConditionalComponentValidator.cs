using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class DateTimeConditionalComponentValidator : CompositeActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		DateTimeConditionalComponent dateTimeConditionalComponent = obj as DateTimeConditionalComponent;
		if (dateTimeConditionalComponent.Parent != null && dateTimeConditionalComponent.EnabledActivities.Count == 0)
		{
			validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DateTimeConditional.InvalidBranchCount"), 0, isWarning: false, ""));
		}
		return validationErrorCollection;
	}
}

using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class SetCallerNameComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		SetCallerNameComponent setCallerNameComponent = obj as SetCallerNameComponent;
		if (setCallerNameComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(setCallerNameComponent.DisplayName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SetCallerName.DisplayNameRequired"), 0, isWarning: false, "DisplayName"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(setCallerNameComponent), setCallerNameComponent.DisplayName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SetCallerName.InvalidDisplayName"), 0, isWarning: false, "DisplayName"));
			}
		}
		return validationErrorCollection;
	}
}

using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class LoggerComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		LoggerComponent loggerComponent = obj as LoggerComponent;
		if (loggerComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(loggerComponent.Text))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Logger.TextRequired"), 0, isWarning: false, "Text"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(loggerComponent), loggerComponent.Text).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Logger.InvalidText"), 0, isWarning: false, "Text"));
			}
		}
		return validationErrorCollection;
	}
}

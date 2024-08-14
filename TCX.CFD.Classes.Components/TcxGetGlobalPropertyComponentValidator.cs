using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxGetGlobalPropertyComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxGetGlobalPropertyComponent tcxGetGlobalPropertyComponent = obj as TcxGetGlobalPropertyComponent;
		if (tcxGetGlobalPropertyComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(tcxGetGlobalPropertyComponent.PropertyName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetGlobalProperty.PropertyNameRequired"), 0, isWarning: false, "PropertyName"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(tcxGetGlobalPropertyComponent), tcxGetGlobalPropertyComponent.PropertyName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetGlobalProperty.InvalidPropertyName"), 0, isWarning: false, "PropertyName"));
			}
		}
		return validationErrorCollection;
	}
}

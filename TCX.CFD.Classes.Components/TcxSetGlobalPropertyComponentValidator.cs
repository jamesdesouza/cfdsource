using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxSetGlobalPropertyComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxSetGlobalPropertyComponent tcxSetGlobalPropertyComponent = obj as TcxSetGlobalPropertyComponent;
		if (tcxSetGlobalPropertyComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(tcxSetGlobalPropertyComponent);
			if (string.IsNullOrEmpty(tcxSetGlobalPropertyComponent.PropertyName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetGlobalProperty.PropertyNameRequired"), 0, isWarning: false, "PropertyName"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, tcxSetGlobalPropertyComponent.PropertyName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetGlobalProperty.InvalidPropertyName"), 0, isWarning: false, "PropertyName"));
			}
			if (string.IsNullOrEmpty(tcxSetGlobalPropertyComponent.PropertyValue))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetGlobalProperty.PropertyValueRequired"), 0, isWarning: false, "PropertyValue"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, tcxSetGlobalPropertyComponent.PropertyValue).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetGlobalProperty.InvalidPropertyValue"), 0, isWarning: false, "PropertyValue"));
			}
		}
		return validationErrorCollection;
	}
}

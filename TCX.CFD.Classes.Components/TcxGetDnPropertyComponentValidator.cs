using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxGetDnPropertyComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxGetDnPropertyComponent tcxGetDnPropertyComponent = obj as TcxGetDnPropertyComponent;
		if (tcxGetDnPropertyComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(tcxGetDnPropertyComponent);
			if (string.IsNullOrEmpty(tcxGetDnPropertyComponent.Extension))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetDnProperty.ExtensionRequired"), 0, isWarning: false, "Extension"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, tcxGetDnPropertyComponent.Extension).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetDnProperty.InvalidExtension"), 0, isWarning: false, "Extension"));
			}
			if (string.IsNullOrEmpty(tcxGetDnPropertyComponent.PropertyName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetDnProperty.PropertyNameRequired"), 0, isWarning: false, "PropertyName"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, tcxGetDnPropertyComponent.PropertyName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetDnProperty.InvalidPropertyName"), 0, isWarning: false, "PropertyName"));
			}
		}
		return validationErrorCollection;
	}
}

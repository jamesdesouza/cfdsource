using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxSetDnPropertyComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxSetDnPropertyComponent tcxSetDnPropertyComponent = obj as TcxSetDnPropertyComponent;
		if (tcxSetDnPropertyComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(tcxSetDnPropertyComponent);
			if (string.IsNullOrEmpty(tcxSetDnPropertyComponent.Extension))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetDnProperty.ExtensionRequired"), 0, isWarning: false, "Extension"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, tcxSetDnPropertyComponent.Extension).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetDnProperty.InvalidExtension"), 0, isWarning: false, "Extension"));
			}
			if (string.IsNullOrEmpty(tcxSetDnPropertyComponent.PropertyName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetDnProperty.PropertyNameRequired"), 0, isWarning: false, "PropertyName"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, tcxSetDnPropertyComponent.PropertyName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetDnProperty.InvalidPropertyName"), 0, isWarning: false, "PropertyName"));
			}
			if (string.IsNullOrEmpty(tcxSetDnPropertyComponent.PropertyValue))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetDnProperty.PropertyValueRequired"), 0, isWarning: false, "PropertyValue"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, tcxSetDnPropertyComponent.PropertyValue).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetDnProperty.InvalidPropertyValue"), 0, isWarning: false, "PropertyValue"));
			}
		}
		return validationErrorCollection;
	}
}

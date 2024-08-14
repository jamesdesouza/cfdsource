using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxExtensionValidatorComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxExtensionValidatorComponent tcxExtensionValidatorComponent = obj as TcxExtensionValidatorComponent;
		if (tcxExtensionValidatorComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(tcxExtensionValidatorComponent.Extension))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxExtensionValidator.ExtensionRequired"), 0, isWarning: false, "Extension"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(tcxExtensionValidatorComponent), tcxExtensionValidatorComponent.Extension).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxExtensionValidator.InvalidExtension"), 0, isWarning: false, "Extension"));
			}
		}
		return validationErrorCollection;
	}
}

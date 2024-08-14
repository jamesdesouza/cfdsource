using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxGetExtensionStatusComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxGetExtensionStatusComponent tcxGetExtensionStatusComponent = obj as TcxGetExtensionStatusComponent;
		if (tcxGetExtensionStatusComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(tcxGetExtensionStatusComponent.Extension))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetExtensionStatus.ExtensionRequired"), 0, isWarning: false, "Extension"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(tcxGetExtensionStatusComponent), tcxGetExtensionStatusComponent.Extension).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetExtensionStatus.InvalidExtension"), 0, isWarning: false, "Extension"));
			}
		}
		return validationErrorCollection;
	}
}

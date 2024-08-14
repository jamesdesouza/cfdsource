using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxSetExtensionStatusComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxSetExtensionStatusComponent tcxSetExtensionStatusComponent = obj as TcxSetExtensionStatusComponent;
		if (tcxSetExtensionStatusComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(tcxSetExtensionStatusComponent.Extension))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetExtensionStatus.ExtensionRequired"), 0, isWarning: false, "Extension"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(tcxSetExtensionStatusComponent), tcxSetExtensionStatusComponent.Extension).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetExtensionStatus.InvalidExtension"), 0, isWarning: false, "Extension"));
			}
		}
		return validationErrorCollection;
	}
}

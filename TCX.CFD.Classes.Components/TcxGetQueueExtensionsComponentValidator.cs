using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxGetQueueExtensionsComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxGetQueueExtensionsComponent tcxGetQueueExtensionsComponent = obj as TcxGetQueueExtensionsComponent;
		if (tcxGetQueueExtensionsComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(tcxGetQueueExtensionsComponent.QueueExtension))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetQueueExtensions.ExtensionRequired"), 0, isWarning: false, "QueueExtension"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(tcxGetQueueExtensionsComponent), tcxGetQueueExtensionsComponent.QueueExtension).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetQueueExtensions.InvalidQueueExtension"), 0, isWarning: false, "QueueExtension"));
			}
		}
		return validationErrorCollection;
	}
}

using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxGetQueueStatusComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxGetQueueStatusComponent tcxGetQueueStatusComponent = obj as TcxGetQueueStatusComponent;
		if (tcxGetQueueStatusComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(tcxGetQueueStatusComponent.QueueExtension))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetQueueStatus.ExtensionRequired"), 0, isWarning: false, "QueueExtension"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(tcxGetQueueStatusComponent), tcxGetQueueStatusComponent.QueueExtension).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetQueueStatus.InvalidQueueExtension"), 0, isWarning: false, "QueueExtension"));
			}
		}
		return validationErrorCollection;
	}
}

using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxSetQueueExtensionStatusComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxSetQueueExtensionStatusComponent tcxSetQueueExtensionStatusComponent = obj as TcxSetQueueExtensionStatusComponent;
		if (tcxSetQueueExtensionStatusComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(tcxSetQueueExtensionStatusComponent);
			if (string.IsNullOrEmpty(tcxSetQueueExtensionStatusComponent.Extension))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetQueueExtensionStatus.ExtensionRequired"), 0, isWarning: false, "Extension"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, tcxSetQueueExtensionStatusComponent.Extension).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetQueueExtensionStatus.InvalidExtension"), 0, isWarning: false, "Extension"));
			}
			if (tcxSetQueueExtensionStatusComponent.QueueMode == QueueStatusOperationModes.SpecificQueue)
			{
				if (string.IsNullOrEmpty(tcxSetQueueExtensionStatusComponent.QueueExtension))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetQueueExtensionStatus.QueueExtensionRequired"), 0, isWarning: false, "QueueExtension"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, tcxSetQueueExtensionStatusComponent.QueueExtension).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxSetQueueExtensionStatus.InvalidQueueExtension"), 0, isWarning: false, "QueueExtension"));
				}
			}
		}
		return validationErrorCollection;
	}
}

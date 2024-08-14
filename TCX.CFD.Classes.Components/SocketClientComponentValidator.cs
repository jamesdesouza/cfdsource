using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class SocketClientComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		SocketClientComponent socketClientComponent = obj as SocketClientComponent;
		if (socketClientComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(socketClientComponent);
			if (string.IsNullOrEmpty(socketClientComponent.Host))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SocketClient.HostRequired"), 0, isWarning: false, "Host"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, socketClientComponent.Host).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SocketClient.InvalidHost"), 0, isWarning: false, "Host"));
			}
			if (string.IsNullOrEmpty(socketClientComponent.Port))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SocketClient.PortRequired"), 0, isWarning: false, "Port"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, socketClientComponent.Port).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SocketClient.InvalidPort"), 0, isWarning: false, "Port"));
			}
			if (string.IsNullOrEmpty(socketClientComponent.Data))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SocketClient.DataRequired"), 0, isWarning: false, "Data"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, socketClientComponent.Data).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SocketClient.InvalidData"), 0, isWarning: false, "Data"));
			}
		}
		return validationErrorCollection;
	}
}

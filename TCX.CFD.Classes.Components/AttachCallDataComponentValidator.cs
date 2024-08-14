using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class AttachCallDataComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		AttachCallDataComponent attachCallDataComponent = obj as AttachCallDataComponent;
		if (attachCallDataComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(attachCallDataComponent);
			if (string.IsNullOrEmpty(attachCallDataComponent.DataName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.AttachCallData.DataNameRequired"), 0, isWarning: false, "DataName"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, attachCallDataComponent.DataName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.AttachCallData.InvalidDataName"), 0, isWarning: false, "DataName"));
			}
			if (string.IsNullOrEmpty(attachCallDataComponent.DataValue))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.AttachCallData.DataValueRequired"), 0, isWarning: false, "DataValue"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, attachCallDataComponent.DataValue).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.AttachCallData.InvalidDataValue"), 0, isWarning: false, "DataValue"));
			}
		}
		return validationErrorCollection;
	}
}

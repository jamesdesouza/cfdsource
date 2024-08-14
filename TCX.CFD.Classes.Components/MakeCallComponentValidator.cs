using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class MakeCallComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		MakeCallComponent makeCallComponent = obj as MakeCallComponent;
		if (makeCallComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(makeCallComponent);
			if (string.IsNullOrEmpty(makeCallComponent.Origin))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.MakeCall.OriginRequired"), 0, isWarning: false, "Origin"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, makeCallComponent.Origin).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.MakeCall.InvalidOrigin"), 0, isWarning: false, "Origin"));
			}
			if (string.IsNullOrEmpty(makeCallComponent.Destination))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.MakeCall.DestinationRequired"), 0, isWarning: false, "Destination"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, makeCallComponent.Destination).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.MakeCall.InvalidDestination"), 0, isWarning: false, "Destination"));
			}
		}
		return validationErrorCollection;
	}
}

using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TransferComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TransferComponent transferComponent = obj as TransferComponent;
		if (transferComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(transferComponent.Destination))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Transfer.DestinationRequired"), 0, isWarning: false, "Destination"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(transferComponent), transferComponent.Destination).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Transfer.InvalidDestination"), 0, isWarning: false, "Destination"));
			}
		}
		return validationErrorCollection;
	}
}

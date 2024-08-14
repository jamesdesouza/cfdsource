using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class GetAttachedCallDataComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		GetAttachedCallDataComponent getAttachedCallDataComponent = obj as GetAttachedCallDataComponent;
		if (getAttachedCallDataComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(getAttachedCallDataComponent.DataName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.GetAttachedCallData.DataNameRequired"), 0, isWarning: false, "DataName"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(getAttachedCallDataComponent), getAttachedCallDataComponent.DataName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.GetAttachedCallData.InvalidDataName"), 0, isWarning: false, "DataName"));
			}
		}
		return validationErrorCollection;
	}
}

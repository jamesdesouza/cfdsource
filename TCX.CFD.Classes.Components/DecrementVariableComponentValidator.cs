using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class DecrementVariableComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		DecrementVariableComponent decrementVariableComponent = obj as DecrementVariableComponent;
		if (decrementVariableComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(decrementVariableComponent.VariableName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DecrementVariable.VariableNameRequired"), 0, isWarning: false, "VariableName"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(decrementVariableComponent), decrementVariableComponent.VariableName).IsVariableName())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DecrementVariable.InvalidVariableName"), 0, isWarning: false, "VariableName"));
			}
		}
		return validationErrorCollection;
	}
}

using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class IncrementVariableComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		IncrementVariableComponent ıncrementVariableComponent = obj as IncrementVariableComponent;
		if (ıncrementVariableComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(ıncrementVariableComponent.VariableName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.IncrementVariable.VariableNameRequired"), 0, isWarning: false, "VariableName"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(ıncrementVariableComponent), ıncrementVariableComponent.VariableName).IsVariableName())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.IncrementVariable.InvalidVariableName"), 0, isWarning: false, "VariableName"));
			}
		}
		return validationErrorCollection;
	}
}

using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class VariableAssignmentComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		VariableAssignmentComponent variableAssignmentComponent = obj as VariableAssignmentComponent;
		if (variableAssignmentComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(variableAssignmentComponent);
			if (string.IsNullOrEmpty(variableAssignmentComponent.VariableName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VariableAssignment.VariableNameRequired"), 0, isWarning: false, "VariableName"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, variableAssignmentComponent.VariableName).IsVariableName())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VariableAssignment.InvalidVariableName"), 0, isWarning: false, "VariableName"));
			}
			if (string.IsNullOrEmpty(variableAssignmentComponent.Expression))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VariableAssignment.ExpressionRequired"), 0, isWarning: false, "Expression"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, variableAssignmentComponent.Expression).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VariableAssignment.InvalidExpression"), 0, isWarning: false, "Expression"));
			}
		}
		return validationErrorCollection;
	}
}

using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class ConditionalComponentBranchValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		ConditionalComponentBranch conditionalComponentBranch = obj as ConditionalComponentBranch;
		if (conditionalComponentBranch.Parent != null)
		{
			if (string.IsNullOrEmpty(conditionalComponentBranch.Condition))
			{
				if (conditionalComponentBranch != conditionalComponentBranch.Parent.EnabledActivities[conditionalComponentBranch.Parent.EnabledActivities.Count - 1])
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ConditionalBranch.ConditionRequired"), 0, isWarning: false, "Condition"));
				}
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(conditionalComponentBranch), conditionalComponentBranch.Condition).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ConditionalBranch.InvalidCondition"), 0, isWarning: false, "Condition"));
			}
		}
		return validationErrorCollection;
	}
}

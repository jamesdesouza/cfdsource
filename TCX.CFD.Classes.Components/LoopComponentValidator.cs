using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class LoopComponentValidator : CompositeActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		LoopComponent loopComponent = obj as LoopComponent;
		if (loopComponent.Parent != null)
		{
			if (loopComponent.EnabledActivities.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Loop.NoChildren"), 0, isWarning: false, ""));
			}
			if (string.IsNullOrEmpty(loopComponent.Condition))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Loop.ConditionRequired"), 0, isWarning: false, "Condition"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(loopComponent), loopComponent.Condition).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Loop.InvalidCondition"), 0, isWarning: false, "Condition"));
			}
		}
		return validationErrorCollection;
	}
}

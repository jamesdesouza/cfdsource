using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TcxGetLastCallComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TcxGetLastCallComponent tcxGetLastCallComponent = obj as TcxGetLastCallComponent;
		if (tcxGetLastCallComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(tcxGetLastCallComponent.Number))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetLastCall.NumberRequired"), 0, isWarning: false, "Number"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(tcxGetLastCallComponent), tcxGetLastCallComponent.Number).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TcxGetLastCall.InvalidNumber"), 0, isWarning: false, "Number"));
			}
		}
		return validationErrorCollection;
	}
}

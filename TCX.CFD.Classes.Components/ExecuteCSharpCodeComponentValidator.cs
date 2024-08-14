using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class ExecuteCSharpCodeComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		ExecuteCSharpCodeComponent executeCSharpCodeComponent = obj as ExecuteCSharpCodeComponent;
		if (executeCSharpCodeComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(executeCSharpCodeComponent.MethodName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ExecuteCSharpCode.MethodNameRequired"), 0, isWarning: false, "MethodName"));
			}
			if (string.IsNullOrEmpty(executeCSharpCodeComponent.Code))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ExecuteCSharpCode.CodeRequired"), 0, isWarning: false, "Code"));
			}
			int num = 0;
			List<string> validVariables = ExpressionHelper.GetValidVariables(executeCSharpCodeComponent);
			foreach (ScriptParameter parameter in executeCSharpCodeComponent.Parameters)
			{
				if (!string.IsNullOrEmpty(parameter.Name) || !string.IsNullOrEmpty(parameter.Value))
				{
					if (string.IsNullOrEmpty(parameter.Name))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.ExecuteCSharpCode.ParameterNameRequired"), num), 0, isWarning: false, "Parameters"));
					}
					else if (string.IsNullOrEmpty(parameter.Value))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.ExecuteCSharpCode.ParameterValueRequired"), num), 0, isWarning: false, "Parameters"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, parameter.Value).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.ExecuteCSharpCode.InvalidParameterValue"), num), 0, isWarning: false, "Parameters"));
					}
					num++;
				}
			}
		}
		return validationErrorCollection;
	}
}

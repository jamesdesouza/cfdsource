using System.Collections.Generic;
using System.IO;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class ExternalCodeExecutionComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		ExternalCodeExecutionComponent externalCodeExecutionComponent = obj as ExternalCodeExecutionComponent;
		if (externalCodeExecutionComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(externalCodeExecutionComponent.LibraryFileName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ExternalCodeExecution.LibraryFileNameRequired"), 0, isWarning: false, "LibraryFileName"));
			}
			else if (!File.Exists(Path.Combine(externalCodeExecutionComponent.GetRootFlow().FileObject.GetProjectObject().GetFolderPath(), "Libraries", externalCodeExecutionComponent.LibraryFileName)))
			{
				validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.ExternalCodeExecution.InvalidLibraryFileName"), externalCodeExecutionComponent.LibraryFileName), 0, isWarning: false, "LibraryFileName"));
			}
			if (string.IsNullOrEmpty(externalCodeExecutionComponent.MethodName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ExternalCodeExecution.MethodNameRequired"), 0, isWarning: false, "MethodName"));
			}
			if (string.IsNullOrEmpty(externalCodeExecutionComponent.ClassName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.ExternalCodeExecution.ClassNameRequired"), 0, isWarning: false, "ClassName"));
			}
			int num = 0;
			List<string> validVariables = ExpressionHelper.GetValidVariables(externalCodeExecutionComponent);
			foreach (ScriptParameter parameter in externalCodeExecutionComponent.Parameters)
			{
				if (!string.IsNullOrEmpty(parameter.Name) || !string.IsNullOrEmpty(parameter.Value))
				{
					if (string.IsNullOrEmpty(parameter.Name))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.ExternalCodeExecution.ParameterNameRequired"), num), 0, isWarning: false, "Parameters"));
					}
					else if (string.IsNullOrEmpty(parameter.Value))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.ExternalCodeExecution.ParameterValueRequired"), num), 0, isWarning: false, "Parameters"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, parameter.Value).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.ExternalCodeExecution.InvalidParameterValue"), num), 0, isWarning: false, "Parameters"));
					}
					num++;
				}
			}
		}
		return validationErrorCollection;
	}
}

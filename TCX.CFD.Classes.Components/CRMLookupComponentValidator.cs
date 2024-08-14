using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class CRMLookupComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		CRMLookupComponent cRMLookupComponent = obj as CRMLookupComponent;
		if (cRMLookupComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(cRMLookupComponent);
			if (string.IsNullOrEmpty(cRMLookupComponent.LookupInputParameter))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.CRMLookup.LookupInputParameterRequired"), 0, isWarning: false, "LookupInputParameter"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, cRMLookupComponent.LookupInputParameter).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.CRMLookup.InvalidLookupInputParameter"), 0, isWarning: false, "LookupInputParameter"));
			}
			int num = 0;
			foreach (ResponseMapping responseMapping in cRMLookupComponent.ResponseMappings)
			{
				if (!string.IsNullOrEmpty(responseMapping.Path) || !string.IsNullOrEmpty(responseMapping.Variable))
				{
					if (string.IsNullOrEmpty(responseMapping.Path))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.CRMLookup.ResponseMappingPathRequired"), num), 0, isWarning: false, "ResponseMappings"));
					}
					else if (string.IsNullOrEmpty(responseMapping.Variable))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.CRMLookup.ResponseMappingVariableRequired"), num), 0, isWarning: false, "ResponseMappings"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, responseMapping.Variable).IsVariableName())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.CRMLookup.InvalidResponseMappingVariable"), num), 0, isWarning: false, "ResponseMappings"));
					}
					num++;
				}
			}
		}
		return validationErrorCollection;
	}
}

using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class JsonXmlParserComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		JsonXmlParserComponent jsonXmlParserComponent = obj as JsonXmlParserComponent;
		if (jsonXmlParserComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(jsonXmlParserComponent);
			if (string.IsNullOrEmpty(jsonXmlParserComponent.Input))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.JsonXmlParser.InputRequired"), 0, isWarning: false, "Input"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, jsonXmlParserComponent.Input).IsVariableName())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.JsonXmlParser.InvalidInput"), 0, isWarning: false, "Input"));
			}
			int num = 0;
			foreach (ResponseMapping responseMapping in jsonXmlParserComponent.ResponseMappings)
			{
				if (!string.IsNullOrEmpty(responseMapping.Path) || !string.IsNullOrEmpty(responseMapping.Variable))
				{
					if (string.IsNullOrEmpty(responseMapping.Path))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.JsonXmlParser.ResponseMappingPathRequired"), num), 0, isWarning: false, "ResponseMappings"));
					}
					else if (string.IsNullOrEmpty(responseMapping.Variable))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.JsonXmlParser.ResponseMappingVariableRequired"), num), 0, isWarning: false, "ResponseMappings"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, responseMapping.Variable).IsVariableName())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.JsonXmlParser.InvalidResponseMappingVariable"), num), 0, isWarning: false, "ResponseMappings"));
					}
					num++;
				}
			}
		}
		return validationErrorCollection;
	}
}

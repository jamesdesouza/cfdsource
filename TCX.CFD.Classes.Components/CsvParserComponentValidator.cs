using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class CsvParserComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		CsvParserComponent csvParserComponent = obj as CsvParserComponent;
		if (csvParserComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(csvParserComponent);
			if (string.IsNullOrEmpty(csvParserComponent.Text))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.CsvParser.TextRequired"), 0, isWarning: false, "Text"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, csvParserComponent.Text).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.CsvParser.InvalidText"), 0, isWarning: false, "Text"));
			}
		}
		return validationErrorCollection;
	}
}

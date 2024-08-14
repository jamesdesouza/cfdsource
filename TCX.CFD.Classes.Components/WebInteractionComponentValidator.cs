using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class WebInteractionComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		WebInteractionComponent webInteractionComponent = obj as WebInteractionComponent;
		if (webInteractionComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(webInteractionComponent);
			if (string.IsNullOrEmpty(webInteractionComponent.URI))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebInteraction.UriRequired"), 0, isWarning: false, "URI"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, webInteractionComponent.URI).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebInteraction.InvalidURI"), 0, isWarning: false, "URI"));
			}
			if ((string.IsNullOrEmpty(webInteractionComponent.Content) && !string.IsNullOrEmpty(webInteractionComponent.ContentType)) || (string.IsNullOrEmpty(webInteractionComponent.ContentType) && !string.IsNullOrEmpty(webInteractionComponent.Content)))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebInteraction.ContentAndContentTypeRequired"), 0, isWarning: false, "Content"));
			}
			if (!string.IsNullOrEmpty(webInteractionComponent.Content) && !AbsArgument.BuildArgument(validVariables, webInteractionComponent.Content).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebInteraction.InvalidContent"), 0, isWarning: false, "Content"));
			}
			int num = 0;
			foreach (Parameter header in webInteractionComponent.Headers)
			{
				if (!string.IsNullOrEmpty(header.Name) || !string.IsNullOrEmpty(header.Value))
				{
					if (string.IsNullOrEmpty(header.Name))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebInteraction.HeaderNameRequired"), num), 0, isWarning: false, "Headers"));
					}
					else if (string.IsNullOrEmpty(header.Value))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebInteraction.HeaderValueRequired"), num), 0, isWarning: false, "Headers"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, header.Value).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebInteraction.InvalidHeaderValue"), num), 0, isWarning: false, "Headers"));
					}
					num++;
				}
			}
		}
		return validationErrorCollection;
	}
}

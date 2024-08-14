using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class WebServicesInteractionComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		WebServicesInteractionComponent webServicesInteractionComponent = obj as WebServicesInteractionComponent;
		if (webServicesInteractionComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(webServicesInteractionComponent);
			if (string.IsNullOrEmpty(webServicesInteractionComponent.URI))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.UriRequired"), 0, isWarning: false, "URI"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, webServicesInteractionComponent.URI).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.InvalidURI"), 0, isWarning: false, "URI"));
			}
			if (string.IsNullOrEmpty(webServicesInteractionComponent.WebServiceName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.WebServiceNameRequired"), 0, isWarning: false, "WebServiceName"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, webServicesInteractionComponent.WebServiceName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.InvalidWebServiceName"), 0, isWarning: false, "WebServiceName"));
			}
			if (string.IsNullOrEmpty(webServicesInteractionComponent.Content))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.ContentRequired"), 0, isWarning: false, "Content"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, webServicesInteractionComponent.Content).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.InvalidContent"), 0, isWarning: false, "Content"));
			}
			if (string.IsNullOrEmpty(webServicesInteractionComponent.ContentType))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.ContentTypeRequired"), 0, isWarning: false, "ContentType"));
			}
			int num = 0;
			foreach (Parameter header in webServicesInteractionComponent.Headers)
			{
				if (!string.IsNullOrEmpty(header.Name) || !string.IsNullOrEmpty(header.Value))
				{
					if (string.IsNullOrEmpty(header.Name))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.HeaderNameRequired"), num), 0, isWarning: false, "Headers"));
					}
					else if (string.IsNullOrEmpty(header.Value))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.HeaderValueRequired"), num), 0, isWarning: false, "Headers"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, header.Value).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesInteraction.InvalidHeaderValue"), num), 0, isWarning: false, "Headers"));
					}
					num++;
				}
			}
		}
		return validationErrorCollection;
	}
}

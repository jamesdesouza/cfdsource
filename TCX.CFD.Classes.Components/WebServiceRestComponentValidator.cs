using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class WebServiceRestComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		WebServiceRestComponent webServiceRestComponent = obj as WebServiceRestComponent;
		if (webServiceRestComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(webServiceRestComponent);
			if (string.IsNullOrEmpty(webServiceRestComponent.URI))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.UriRequired"), 0, isWarning: false, "URI"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, webServiceRestComponent.URI).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.InvalidURI"), 0, isWarning: false, "URI"));
			}
			if ((string.IsNullOrEmpty(webServiceRestComponent.Content) && !string.IsNullOrEmpty(webServiceRestComponent.ContentType)) || (string.IsNullOrEmpty(webServiceRestComponent.ContentType) && !string.IsNullOrEmpty(webServiceRestComponent.Content)))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.ContentAndContentTypeRequired"), 0, isWarning: false, "Content"));
			}
			if (!string.IsNullOrEmpty(webServiceRestComponent.Content) && !AbsArgument.BuildArgument(validVariables, webServiceRestComponent.Content).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.InvalidContent"), 0, isWarning: false, "Content"));
			}
			if (webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.BasicUserPassword)
			{
				if (string.IsNullOrEmpty(webServiceRestComponent.AuthenticationUserName))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.AuthenticationUserNameRequired"), 0, isWarning: false, "AuthenticationUserName"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, webServiceRestComponent.AuthenticationUserName).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.InvalidAuthenticationUserName"), 0, isWarning: false, "AuthenticationUserName"));
				}
				if (string.IsNullOrEmpty(webServiceRestComponent.AuthenticationPassword))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.AuthenticationPasswordRequired"), 0, isWarning: false, "AuthenticationPassword"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, webServiceRestComponent.AuthenticationPassword).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.InvalidAuthenticationPassword"), 0, isWarning: false, "AuthenticationPassword"));
				}
			}
			if (webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.BasicApiKey)
			{
				if (string.IsNullOrEmpty(webServiceRestComponent.AuthenticationApiKey))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.AuthenticationApiKeyRequired"), 0, isWarning: false, "AuthenticationApiKey"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, webServiceRestComponent.AuthenticationApiKey).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.InvalidAuthenticationApiKey"), 0, isWarning: false, "AuthenticationApiKey"));
				}
			}
			if (webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.OAuth2)
			{
				if (string.IsNullOrEmpty(webServiceRestComponent.AuthenticationOAuth2AccessToken))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.AuthenticationOAuth2AccessTokenRequired"), 0, isWarning: false, "AuthenticationOAuth2AccessToken"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, webServiceRestComponent.AuthenticationOAuth2AccessToken).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.InvalidAuthenticationOAuth2AccessToken"), 0, isWarning: false, "AuthenticationOAuth2AccessToken"));
				}
			}
			int num = 0;
			foreach (Parameter header in webServiceRestComponent.Headers)
			{
				if (!string.IsNullOrEmpty(header.Name) || !string.IsNullOrEmpty(header.Value))
				{
					if (string.IsNullOrEmpty(header.Name))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.HeaderNameRequired"), num), 0, isWarning: false, "Headers"));
					}
					else if (string.IsNullOrEmpty(header.Value))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.HeaderValueRequired"), num), 0, isWarning: false, "Headers"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, header.Value).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.WebServicesRest.InvalidHeaderValue"), num), 0, isWarning: false, "Headers"));
					}
					num++;
				}
			}
		}
		return validationErrorCollection;
	}
}

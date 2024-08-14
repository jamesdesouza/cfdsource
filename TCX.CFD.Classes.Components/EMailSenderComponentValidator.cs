using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class EMailSenderComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		EMailSenderComponent eMailSenderComponent = obj as EMailSenderComponent;
		if (eMailSenderComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(eMailSenderComponent);
			if (!eMailSenderComponent.UseServerSettings)
			{
				if (string.IsNullOrEmpty(eMailSenderComponent.Server))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.ServerRequired"), 0, isWarning: false, "Server"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Server).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidServer"), 0, isWarning: false, "Server"));
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.Port) && !AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Port).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidPort"), 0, isWarning: false, "Port"));
				}
				if (string.IsNullOrEmpty(eMailSenderComponent.EnableSSL))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.EnableSslRequired"), 0, isWarning: false, "EnableSSL"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, eMailSenderComponent.EnableSSL).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidEnableSSL"), 0, isWarning: false, "EnableSSL"));
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.UserName) && !AbsArgument.BuildArgument(validVariables, eMailSenderComponent.UserName).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidUserName"), 0, isWarning: false, "UserName"));
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.Password) && !AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Password).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidPassword"), 0, isWarning: false, "Password"));
				}
				if (string.IsNullOrEmpty(eMailSenderComponent.From))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.FromRequired"), 0, isWarning: false, "From"));
				}
				else if (eMailSenderComponent.From.Length > 2 && ExpressionHelper.IsStringLiteral(eMailSenderComponent.From) && !EMailValidator.IsEmail(eMailSenderComponent.From.Substring(1, eMailSenderComponent.From.Length - 2)))
				{
					validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.FromIsInvalid"), eMailSenderComponent.From), 0, isWarning: false, "From"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, eMailSenderComponent.From).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidFrom"), 0, isWarning: false, "From"));
				}
			}
			if (string.IsNullOrEmpty(eMailSenderComponent.To) && string.IsNullOrEmpty(eMailSenderComponent.CC) && string.IsNullOrEmpty(eMailSenderComponent.BCC))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.DestinationRequired"), 0, isWarning: false, "To"));
			}
			else
			{
				if (!string.IsNullOrEmpty(eMailSenderComponent.To))
				{
					if (eMailSenderComponent.To.Length > 2 && ExpressionHelper.IsStringLiteral(eMailSenderComponent.To) && !EMailValidator.IsEmailList(eMailSenderComponent.To.Substring(1, eMailSenderComponent.To.Length - 2)))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.ToIsInvalid"), eMailSenderComponent.To), 0, isWarning: false, "To"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, eMailSenderComponent.To).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidTo"), 0, isWarning: false, "To"));
					}
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.CC))
				{
					if (eMailSenderComponent.CC.Length > 2 && ExpressionHelper.IsStringLiteral(eMailSenderComponent.CC) && !EMailValidator.IsEmailList(eMailSenderComponent.CC.Substring(1, eMailSenderComponent.CC.Length - 2)))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.CCIsInvalid"), eMailSenderComponent.CC), 0, isWarning: false, "CC"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, eMailSenderComponent.CC).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidCC"), 0, isWarning: false, "CC"));
					}
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.BCC))
				{
					if (eMailSenderComponent.BCC.Length > 2 && ExpressionHelper.IsStringLiteral(eMailSenderComponent.BCC) && !EMailValidator.IsEmailList(eMailSenderComponent.BCC.Substring(1, eMailSenderComponent.BCC.Length - 2)))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.BCCIsInvalid"), eMailSenderComponent.BCC), 0, isWarning: false, "BCC"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, eMailSenderComponent.BCC).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidBCC"), 0, isWarning: false, "BCC"));
					}
				}
			}
			if (string.IsNullOrEmpty(eMailSenderComponent.Subject) && string.IsNullOrEmpty(eMailSenderComponent.Body))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.MessageRequired"), 0, isWarning: false, "Body"));
			}
			else
			{
				if (!string.IsNullOrEmpty(eMailSenderComponent.Subject) && !AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Subject).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidSubject"), 0, isWarning: false, "Subject"));
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.Body) && !AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Body).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidBody"), 0, isWarning: false, "Body"));
				}
			}
			int num = 0;
			foreach (MailAttachment attachment in eMailSenderComponent.Attachments)
			{
				if (!string.IsNullOrEmpty(attachment.Name) || !string.IsNullOrEmpty(attachment.File))
				{
					if (string.IsNullOrEmpty(attachment.Name))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.AttachmentNameRequired"), num), 0, isWarning: false, "Attachments"));
					}
					else if (string.IsNullOrEmpty(attachment.File))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.AttachmentFileRequired"), num), 0, isWarning: false, "Attachments"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, attachment.File).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.EMailSender.InvalidAttachmentFile"), num), 0, isWarning: false, "Attachments"));
					}
					num++;
				}
			}
		}
		return validationErrorCollection;
	}
}

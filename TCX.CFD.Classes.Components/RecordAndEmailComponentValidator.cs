using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class RecordAndEmailComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		RecordAndEmailComponent recordAndEmailComponent = obj as RecordAndEmailComponent;
		if (recordAndEmailComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(recordAndEmailComponent);
			if (recordAndEmailComponent.Prompts.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.PromptsAreEmpty"), 0, isWarning: false, "Prompts"));
			}
			if (!recordAndEmailComponent.UseServerSettings)
			{
				if (string.IsNullOrEmpty(recordAndEmailComponent.Server))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.ServerRequired"), 0, isWarning: false, "Server"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Server).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidServer"), 0, isWarning: false, "Server"));
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.Port) && !AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Port).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidPort"), 0, isWarning: false, "Port"));
				}
				if (string.IsNullOrEmpty(recordAndEmailComponent.EnableSSL))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.EnableSslRequired"), 0, isWarning: false, "EnableSSL"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.EnableSSL).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidEnableSSL"), 0, isWarning: false, "EnableSSL"));
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.UserName) && !AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.UserName).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidUserName"), 0, isWarning: false, "UserName"));
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.Password) && !AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Password).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidPassword"), 0, isWarning: false, "Password"));
				}
				if (string.IsNullOrEmpty(recordAndEmailComponent.From))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.FromRequired"), 0, isWarning: false, "From"));
				}
				else if (recordAndEmailComponent.From.Length > 2 && ExpressionHelper.IsStringLiteral(recordAndEmailComponent.From) && !EMailValidator.IsEmail(recordAndEmailComponent.From.Substring(1, recordAndEmailComponent.From.Length - 2)))
				{
					validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.FromIsInvalid"), recordAndEmailComponent.From), 0, isWarning: false, "From"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.From).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidFrom"), 0, isWarning: false, "From"));
				}
			}
			if (string.IsNullOrEmpty(recordAndEmailComponent.To) && string.IsNullOrEmpty(recordAndEmailComponent.CC) && string.IsNullOrEmpty(recordAndEmailComponent.BCC))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.DestinationRequired"), 0, isWarning: false, "To"));
			}
			else
			{
				if (!string.IsNullOrEmpty(recordAndEmailComponent.To))
				{
					if (recordAndEmailComponent.To.Length > 2 && ExpressionHelper.IsStringLiteral(recordAndEmailComponent.To) && !EMailValidator.IsEmailList(recordAndEmailComponent.To.Substring(1, recordAndEmailComponent.To.Length - 2)))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.ToIsInvalid"), recordAndEmailComponent.To), 0, isWarning: false, "To"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.To).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidTo"), 0, isWarning: false, "To"));
					}
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.CC))
				{
					if (recordAndEmailComponent.CC.Length > 2 && ExpressionHelper.IsStringLiteral(recordAndEmailComponent.CC) && !EMailValidator.IsEmailList(recordAndEmailComponent.CC.Substring(1, recordAndEmailComponent.CC.Length - 2)))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.CCIsInvalid"), recordAndEmailComponent.CC), 0, isWarning: false, "CC"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.CC).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidCC"), 0, isWarning: false, "CC"));
					}
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.BCC))
				{
					if (recordAndEmailComponent.BCC.Length > 2 && ExpressionHelper.IsStringLiteral(recordAndEmailComponent.BCC) && !EMailValidator.IsEmailList(recordAndEmailComponent.BCC.Substring(1, recordAndEmailComponent.BCC.Length - 2)))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.BCCIsInvalid"), recordAndEmailComponent.BCC), 0, isWarning: false, "BCC"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.BCC).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidBCC"), 0, isWarning: false, "BCC"));
					}
				}
			}
			if (string.IsNullOrEmpty(recordAndEmailComponent.Subject) && string.IsNullOrEmpty(recordAndEmailComponent.Body))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.MessageRequired"), 0, isWarning: false, "Body"));
			}
			else
			{
				if (!string.IsNullOrEmpty(recordAndEmailComponent.Subject) && !AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Subject).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidSubject"), 0, isWarning: false, "Subject"));
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.Body) && !AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Body).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.RecordAndEMail.InvalidBody"), 0, isWarning: false, "Body"));
				}
			}
			bool flag = false;
			foreach (Prompt prompt in recordAndEmailComponent.Prompts)
			{
				if (prompt is TextToSpeechAudioPrompt)
				{
					flag = true;
					break;
				}
			}
			if (flag && !recordAndEmailComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}

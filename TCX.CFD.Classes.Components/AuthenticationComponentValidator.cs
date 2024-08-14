using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class AuthenticationComponentValidator : CompositeActivityValidator
{
	private bool IsStopDigitUsed(UserInputComponent userInputComponent)
	{
		return userInputComponent.StopDigit switch
		{
			DtmfDigits.Digit0 => userInputComponent.IsValidDigit_0, 
			DtmfDigits.Digit1 => userInputComponent.IsValidDigit_1, 
			DtmfDigits.Digit2 => userInputComponent.IsValidDigit_2, 
			DtmfDigits.Digit3 => userInputComponent.IsValidDigit_3, 
			DtmfDigits.Digit4 => userInputComponent.IsValidDigit_4, 
			DtmfDigits.Digit5 => userInputComponent.IsValidDigit_5, 
			DtmfDigits.Digit6 => userInputComponent.IsValidDigit_6, 
			DtmfDigits.Digit7 => userInputComponent.IsValidDigit_7, 
			DtmfDigits.Digit8 => userInputComponent.IsValidDigit_8, 
			DtmfDigits.Digit9 => userInputComponent.IsValidDigit_9, 
			DtmfDigits.DigitStar => userInputComponent.IsValidDigit_Star, 
			DtmfDigits.DigitPound => userInputComponent.IsValidDigit_Pound, 
			_ => false, 
		};
	}

	private void ValidateUserInput(ValidationErrorCollection validationErrorCollection, UserInputComponent userInputComponent, string errorPrefix, string propertyPrefix)
	{
		if (userInputComponent.InitialPrompts.Count == 0)
		{
			validationErrorCollection.Add(new ValidationError(errorPrefix + LocalizedResourceMgr.GetString("ComponentValidators.UserInput.InitialPromptsAreEmpty"), 0, isWarning: false, "InitialPrompts"));
		}
		if (userInputComponent.SubsequentPrompts.Count == 0 && userInputComponent.MaxRetryCount > 1)
		{
			validationErrorCollection.Add(new ValidationError(errorPrefix + LocalizedResourceMgr.GetString("ComponentValidators.UserInput.SubsequentPromptsAreEmpty"), 0, isWarning: false, "SubsequentPrompts"));
		}
		if (userInputComponent.MinDigits > userInputComponent.MaxDigits)
		{
			validationErrorCollection.Add(new ValidationError(errorPrefix + LocalizedResourceMgr.GetString("ComponentValidators.UserInput.MinDigitsGreaterThanMaxDigits"), 0, isWarning: false, propertyPrefix + "MinDigits"));
		}
		if (!userInputComponent.IsValidDigit_0 && !userInputComponent.IsValidDigit_1 && !userInputComponent.IsValidDigit_2 && !userInputComponent.IsValidDigit_3 && !userInputComponent.IsValidDigit_4 && !userInputComponent.IsValidDigit_5 && !userInputComponent.IsValidDigit_6 && !userInputComponent.IsValidDigit_7 && !userInputComponent.IsValidDigit_8 && !userInputComponent.IsValidDigit_9 && !userInputComponent.IsValidDigit_Star && !userInputComponent.IsValidDigit_Pound)
		{
			validationErrorCollection.Add(new ValidationError(errorPrefix + LocalizedResourceMgr.GetString("ComponentValidators.UserInput.NoValidDigits"), 0, isWarning: false, propertyPrefix + "IsValidDigit_0"));
		}
		if (IsStopDigitUsed(userInputComponent))
		{
			validationErrorCollection.Add(new ValidationError(errorPrefix + LocalizedResourceMgr.GetString("ComponentValidators.UserInput.StopDigitIsValidDigit"), 0, isWarning: false, propertyPrefix + "StopDigit"));
		}
	}

	private bool PromptsUseTTS(List<Prompt> prompts)
	{
		foreach (Prompt prompt in prompts)
		{
			if (prompt is TextToSpeechAudioPrompt)
			{
				return true;
			}
		}
		return false;
	}

	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		AuthenticationComponent authenticationComponent = obj as AuthenticationComponent;
		if (authenticationComponent.Parent != null)
		{
			if (authenticationComponent.EnabledActivities.Count != 2)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Authentication.InvalidBranchCount"), 0, isWarning: false, ""));
			}
			ValidateUserInput(validationErrorCollection, authenticationComponent.RequestID, "Request ID - ", "RequestId");
			if (authenticationComponent.IsPinRequired)
			{
				ValidateUserInput(validationErrorCollection, authenticationComponent.RequestPIN, "Request PIN - ", "RequestPin");
			}
			bool flag = PromptsUseTTS(authenticationComponent.RequestID.InitialPrompts);
			if (!flag)
			{
				flag = PromptsUseTTS(authenticationComponent.RequestID.SubsequentPrompts);
			}
			if (!flag)
			{
				flag = PromptsUseTTS(authenticationComponent.RequestID.TimeoutPrompts);
			}
			if (!flag)
			{
				flag = PromptsUseTTS(authenticationComponent.RequestID.InvalidDigitPrompts);
			}
			if (!flag && authenticationComponent.IsPinRequired)
			{
				flag = PromptsUseTTS(authenticationComponent.RequestPIN.InitialPrompts);
				if (!flag)
				{
					flag = PromptsUseTTS(authenticationComponent.RequestPIN.SubsequentPrompts);
				}
				if (!flag)
				{
					flag = PromptsUseTTS(authenticationComponent.RequestPIN.TimeoutPrompts);
				}
				if (!flag)
				{
					flag = PromptsUseTTS(authenticationComponent.RequestPIN.InvalidDigitPrompts);
				}
			}
			if (flag && !authenticationComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}

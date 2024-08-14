using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class CreditCardComponentValidator : CompositeActivityValidator
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
		CreditCardComponent creditCardComponent = obj as CreditCardComponent;
		if (creditCardComponent.Parent != null)
		{
			if (creditCardComponent.EnabledActivities.Count != 2)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.CreditCard.InvalidBranchCount"), 0, isWarning: false, ""));
			}
			ValidateUserInput(validationErrorCollection, creditCardComponent.RequestNumber, "Request Card Number - ", "RequestNumber");
			if (creditCardComponent.IsExpirationRequired)
			{
				ValidateUserInput(validationErrorCollection, creditCardComponent.RequestExpiration, "Request Expiration Date - ", "RequestExpiration");
				if (creditCardComponent.IsSecurityCodeRequired)
				{
					ValidateUserInput(validationErrorCollection, creditCardComponent.RequestSecurityCode, "Request Security Code - ", "RequestSecurityCode");
				}
			}
			bool flag = PromptsUseTTS(creditCardComponent.RequestNumber.InitialPrompts);
			if (!flag)
			{
				flag = PromptsUseTTS(creditCardComponent.RequestNumber.SubsequentPrompts);
			}
			if (!flag)
			{
				flag = PromptsUseTTS(creditCardComponent.RequestNumber.TimeoutPrompts);
			}
			if (!flag)
			{
				flag = PromptsUseTTS(creditCardComponent.RequestNumber.InvalidDigitPrompts);
			}
			if (!flag && creditCardComponent.IsExpirationRequired)
			{
				flag = PromptsUseTTS(creditCardComponent.RequestExpiration.InitialPrompts);
				if (!flag)
				{
					flag = PromptsUseTTS(creditCardComponent.RequestExpiration.SubsequentPrompts);
				}
				if (!flag)
				{
					flag = PromptsUseTTS(creditCardComponent.RequestExpiration.TimeoutPrompts);
				}
				if (!flag)
				{
					flag = PromptsUseTTS(creditCardComponent.RequestExpiration.InvalidDigitPrompts);
				}
				if (!flag && creditCardComponent.IsSecurityCodeRequired)
				{
					flag = PromptsUseTTS(creditCardComponent.RequestSecurityCode.InitialPrompts);
					if (!flag)
					{
						flag = PromptsUseTTS(creditCardComponent.RequestSecurityCode.SubsequentPrompts);
					}
					if (!flag)
					{
						flag = PromptsUseTTS(creditCardComponent.RequestSecurityCode.TimeoutPrompts);
					}
					if (!flag)
					{
						flag = PromptsUseTTS(creditCardComponent.RequestSecurityCode.InvalidDigitPrompts);
					}
				}
			}
			if (flag && !creditCardComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}

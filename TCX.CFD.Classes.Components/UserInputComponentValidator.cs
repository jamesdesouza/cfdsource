using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class UserInputComponentValidator : CompositeActivityValidator
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
		UserInputComponent userInputComponent = obj as UserInputComponent;
		if (userInputComponent.Parent != null)
		{
			if (userInputComponent.EnabledActivities.Count != 2)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.UserInput.InvalidBranchCount"), 0, isWarning: false, ""));
			}
			if (userInputComponent.InitialPrompts.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.UserInput.InitialPromptsAreEmpty"), 0, isWarning: false, "InitialPrompts"));
			}
			if (userInputComponent.SubsequentPrompts.Count == 0 && userInputComponent.MaxRetryCount > 1)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.UserInput.SubsequentPromptsAreEmpty"), 0, isWarning: false, "SubsequentPrompts"));
			}
			if (userInputComponent.MinDigits > userInputComponent.MaxDigits)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.UserInput.MinDigitsGreaterThanMaxDigits"), 0, isWarning: false, "MinDigits"));
			}
			if (!userInputComponent.IsValidDigit_0 && !userInputComponent.IsValidDigit_1 && !userInputComponent.IsValidDigit_2 && !userInputComponent.IsValidDigit_3 && !userInputComponent.IsValidDigit_4 && !userInputComponent.IsValidDigit_5 && !userInputComponent.IsValidDigit_6 && !userInputComponent.IsValidDigit_7 && !userInputComponent.IsValidDigit_8 && !userInputComponent.IsValidDigit_9 && !userInputComponent.IsValidDigit_Star && !userInputComponent.IsValidDigit_Pound)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.UserInput.NoValidDigits"), 0, isWarning: false, "IsValidDigit_0"));
			}
			if (IsStopDigitUsed(userInputComponent))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.UserInput.StopDigitIsValidDigit"), 0, isWarning: false, "StopDigit"));
			}
			bool flag = PromptsUseTTS(userInputComponent.InitialPrompts);
			if (!flag)
			{
				flag = PromptsUseTTS(userInputComponent.SubsequentPrompts);
			}
			if (!flag)
			{
				flag = PromptsUseTTS(userInputComponent.TimeoutPrompts);
			}
			if (!flag)
			{
				flag = PromptsUseTTS(userInputComponent.InvalidDigitPrompts);
			}
			if (flag && !userInputComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}

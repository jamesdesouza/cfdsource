using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class MenuComponentValidator : CompositeActivityValidator
{
	private bool IsRepeatOptionUsed(MenuComponent menuComponent)
	{
		return menuComponent.RepeatOption switch
		{
			DtmfDigits.Digit0 => menuComponent.IsValidOption_0, 
			DtmfDigits.Digit1 => menuComponent.IsValidOption_1, 
			DtmfDigits.Digit2 => menuComponent.IsValidOption_2, 
			DtmfDigits.Digit3 => menuComponent.IsValidOption_3, 
			DtmfDigits.Digit4 => menuComponent.IsValidOption_4, 
			DtmfDigits.Digit5 => menuComponent.IsValidOption_5, 
			DtmfDigits.Digit6 => menuComponent.IsValidOption_6, 
			DtmfDigits.Digit7 => menuComponent.IsValidOption_7, 
			DtmfDigits.Digit8 => menuComponent.IsValidOption_8, 
			DtmfDigits.Digit9 => menuComponent.IsValidOption_9, 
			DtmfDigits.DigitStar => menuComponent.IsValidOption_Star, 
			DtmfDigits.DigitPound => menuComponent.IsValidOption_Pound, 
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
		MenuComponent menuComponent = obj as MenuComponent;
		if (menuComponent.Parent != null)
		{
			if (menuComponent.InitialPrompts.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Menu.InitialPromptsAreEmpty"), 0, isWarning: false, "InitialPrompts"));
			}
			if (menuComponent.SubsequentPrompts.Count == 0 && menuComponent.MaxRetryCount > 1)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Menu.SubsequentPromptsAreEmpty"), 0, isWarning: false, "SubsequentPrompts"));
			}
			bool flag = false;
			foreach (MenuComponentBranch enabledActivity in menuComponent.EnabledActivities)
			{
				if (enabledActivity.Option == MenuOptions.TimeoutOrInvalidOption)
				{
					flag = true;
				}
			}
			if (menuComponent.EnabledActivities.Count < 2 || !flag)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Menu.OptionsRequired"), 0, isWarning: false, "IsValidOption_0"));
			}
			if (IsRepeatOptionUsed(menuComponent))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Menu.RepeatOptionIsValidOption"), 0, isWarning: false, "RepeatOption"));
			}
			bool flag2 = PromptsUseTTS(menuComponent.InitialPrompts);
			if (!flag2)
			{
				flag2 = PromptsUseTTS(menuComponent.SubsequentPrompts);
			}
			if (!flag2)
			{
				flag2 = PromptsUseTTS(menuComponent.TimeoutPrompts);
			}
			if (!flag2)
			{
				flag2 = PromptsUseTTS(menuComponent.InvalidDigitPrompts);
			}
			if (flag2 && !menuComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}

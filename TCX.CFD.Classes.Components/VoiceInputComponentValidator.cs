using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

public class VoiceInputComponentValidator : CompositeActivityValidator
{
	private bool IsStopDigitUsed(VoiceInputComponent voiceInputComponent)
	{
		return voiceInputComponent.StopDigit switch
		{
			DtmfDigits.Digit0 => voiceInputComponent.IsValidDigit_0, 
			DtmfDigits.Digit1 => voiceInputComponent.IsValidDigit_1, 
			DtmfDigits.Digit2 => voiceInputComponent.IsValidDigit_2, 
			DtmfDigits.Digit3 => voiceInputComponent.IsValidDigit_3, 
			DtmfDigits.Digit4 => voiceInputComponent.IsValidDigit_4, 
			DtmfDigits.Digit5 => voiceInputComponent.IsValidDigit_5, 
			DtmfDigits.Digit6 => voiceInputComponent.IsValidDigit_6, 
			DtmfDigits.Digit7 => voiceInputComponent.IsValidDigit_7, 
			DtmfDigits.Digit8 => voiceInputComponent.IsValidDigit_8, 
			DtmfDigits.Digit9 => voiceInputComponent.IsValidDigit_9, 
			DtmfDigits.DigitStar => voiceInputComponent.IsValidDigit_Star, 
			DtmfDigits.DigitPound => voiceInputComponent.IsValidDigit_Pound, 
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
		VoiceInputComponent voiceInputComponent = obj as VoiceInputComponent;
		if (voiceInputComponent.Parent != null)
		{
			if (voiceInputComponent.EnabledActivities.Count != 2)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.InvalidBranchCount"), 0, isWarning: false, ""));
			}
			if (voiceInputComponent.InitialPrompts.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.InitialPromptsAreEmpty"), 0, isWarning: false, "InitialPrompts"));
			}
			if (voiceInputComponent.SubsequentPrompts.Count == 0 && voiceInputComponent.MaxRetryCount > 1)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.SubsequentPromptsAreEmpty"), 0, isWarning: false, "SubsequentPrompts"));
			}
			if (string.IsNullOrEmpty(voiceInputComponent.LanguageCode))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.LanguageCodeRequired"), 0, isWarning: false, "LanguageCode"));
			}
			List<string> validVariables = ExpressionHelper.GetValidVariables(voiceInputComponent);
			if (string.IsNullOrEmpty(voiceInputComponent.SaveToFile))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.SaveToFileRequired"), 0, isWarning: false, "SaveToFile"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, voiceInputComponent.SaveToFile).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.InvalidSaveToFile"), 0, isWarning: false, "SaveToFile"));
			}
			if (voiceInputComponent.SaveToFile != "false")
			{
				if (string.IsNullOrEmpty(voiceInputComponent.FileName))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.FileNameRequired"), 0, isWarning: false, "FileName"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, voiceInputComponent.FileName).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.InvalidFileName"), 0, isWarning: false, "FileName"));
				}
			}
			bool flag = false;
			for (int i = 0; i < voiceInputComponent.Dictionary.Count; i++)
			{
				string text = voiceInputComponent.Dictionary[i];
				if (string.IsNullOrEmpty(text))
				{
					flag = true;
				}
				else if (!AbsArgument.BuildArgument(validVariables, text).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.InvalidDictionaryExpression"), i, text), 0, isWarning: false, "Dictionary"));
				}
			}
			if (flag)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.EmptyDictionaryEntry"), 0, isWarning: false, "Dictionary"));
			}
			bool flag2 = false;
			for (int j = 0; j < voiceInputComponent.Hints.Count; j++)
			{
				if (string.IsNullOrEmpty(voiceInputComponent.Hints[j]))
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.EmptyHints"), 0, isWarning: false, "Hints"));
			}
			if (voiceInputComponent.AcceptDtmfInput)
			{
				if (voiceInputComponent.MinDigits > voiceInputComponent.MaxDigits)
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.MinDigitsGreaterThanMaxDigits"), 0, isWarning: false, "MinDigits"));
				}
				if (!voiceInputComponent.IsValidDigit_0 && !voiceInputComponent.IsValidDigit_1 && !voiceInputComponent.IsValidDigit_2 && !voiceInputComponent.IsValidDigit_3 && !voiceInputComponent.IsValidDigit_4 && !voiceInputComponent.IsValidDigit_5 && !voiceInputComponent.IsValidDigit_6 && !voiceInputComponent.IsValidDigit_7 && !voiceInputComponent.IsValidDigit_8 && !voiceInputComponent.IsValidDigit_9 && !voiceInputComponent.IsValidDigit_Star && !voiceInputComponent.IsValidDigit_Pound)
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.NoValidDigits"), 0, isWarning: false, "IsValidDigit_0"));
				}
				if (IsStopDigitUsed(voiceInputComponent))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.StopDigitIsValidDigit"), 0, isWarning: false, "StopDigit"));
				}
			}
			ProjectObject projectObject = voiceInputComponent.GetRootFlow().FileObject.GetProjectObject();
			if (!projectObject.OnlineServices.IsReadyForSTT())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.STTRequired"), 0, isWarning: false, ""));
			}
			bool flag3 = PromptsUseTTS(voiceInputComponent.InitialPrompts);
			if (!flag3)
			{
				flag3 = PromptsUseTTS(voiceInputComponent.SubsequentPrompts);
			}
			if (!flag3)
			{
				flag3 = PromptsUseTTS(voiceInputComponent.TimeoutPrompts);
			}
			if (!flag3)
			{
				flag3 = PromptsUseTTS(voiceInputComponent.InvalidInputPrompts);
			}
			if (flag3 && !projectObject.OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}

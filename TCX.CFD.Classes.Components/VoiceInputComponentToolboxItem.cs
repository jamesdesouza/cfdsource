using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class VoiceInputComponentToolboxItem : ActivityToolboxItem
{
	private static DtmfDigits GetStopDigit(string s)
	{
		return s switch
		{
			"0" => DtmfDigits.Digit0, 
			"1" => DtmfDigits.Digit1, 
			"2" => DtmfDigits.Digit2, 
			"3" => DtmfDigits.Digit3, 
			"4" => DtmfDigits.Digit4, 
			"5" => DtmfDigits.Digit5, 
			"6" => DtmfDigits.Digit6, 
			"7" => DtmfDigits.Digit7, 
			"8" => DtmfDigits.Digit8, 
			"9" => DtmfDigits.Digit9, 
			"*" => DtmfDigits.DigitStar, 
			"#" => DtmfDigits.DigitPound, 
			_ => DtmfDigits.None, 
		};
	}

	public static IComponent CreateDefaultComponent(IDesignerHost host)
	{
		VoiceInputComponent voiceInputComponent = new VoiceInputComponent
		{
			InputTimeout = Settings.Default.VoiceInputTemplateInputTimeout,
			MaxRetryCount = Settings.Default.VoiceInputTemplateMaxRetryCount,
			LanguageCode = Settings.Default.VoiceInputTemplateLanguageCode,
			SaveToFile = (Settings.Default.VoiceInputTemplateSaveToFile ? "true" : "false"),
			AcceptDtmfInput = Settings.Default.VoiceInputTemplateAcceptDtmfInput,
			DtmfTimeout = Settings.Default.VoiceInputTemplateDtmfTimeout,
			MinDigits = Settings.Default.VoiceInputTemplateMinDigits,
			MaxDigits = Settings.Default.VoiceInputTemplateMaxDigits,
			StopDigit = GetStopDigit(Settings.Default.VoiceInputTemplateStopDigit),
			IsValidDigit_0 = Settings.Default.VoiceInputTemplateIsValidOption0,
			IsValidDigit_1 = Settings.Default.VoiceInputTemplateIsValidOption1,
			IsValidDigit_2 = Settings.Default.VoiceInputTemplateIsValidOption2,
			IsValidDigit_3 = Settings.Default.VoiceInputTemplateIsValidOption3,
			IsValidDigit_4 = Settings.Default.VoiceInputTemplateIsValidOption4,
			IsValidDigit_5 = Settings.Default.VoiceInputTemplateIsValidOption5,
			IsValidDigit_6 = Settings.Default.VoiceInputTemplateIsValidOption6,
			IsValidDigit_7 = Settings.Default.VoiceInputTemplateIsValidOption7,
			IsValidDigit_8 = Settings.Default.VoiceInputTemplateIsValidOption8,
			IsValidDigit_9 = Settings.Default.VoiceInputTemplateIsValidOption9,
			IsValidDigit_Star = Settings.Default.VoiceInputTemplateIsValidOptionStar,
			IsValidDigit_Pound = Settings.Default.VoiceInputTemplateIsValidOptionPound
		};
		ComponentBranch componentBranch = new ComponentBranch();
		ComponentBranch componentBranch2 = new ComponentBranch();
		componentBranch.DisplayedText = "Valid Input";
		componentBranch2.DisplayedText = "Invalid Input";
		voiceInputComponent.Activities.Add(componentBranch);
		voiceInputComponent.Activities.Add(componentBranch2);
		FlowDesignerNameCreator.CreateName("VoiceInput", host.Container, voiceInputComponent);
		return voiceInputComponent;
	}

	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		return new IComponent[1] { CreateDefaultComponent(host) };
	}
}

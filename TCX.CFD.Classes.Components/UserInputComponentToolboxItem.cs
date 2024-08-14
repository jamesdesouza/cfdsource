using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class UserInputComponentToolboxItem : ActivityToolboxItem
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
		UserInputComponent userInputComponent = new UserInputComponent
		{
			AcceptDtmfInput = Settings.Default.UserInputTemplateAcceptDtmfInput,
			FirstDigitTimeout = Settings.Default.UserInputTemplateFirstDigitTimeout,
			InterDigitTimeout = Settings.Default.UserInputTemplateInterDigitTimeout,
			FinalDigitTimeout = Settings.Default.UserInputTemplateFinalDigitTimeout,
			MaxRetryCount = Settings.Default.UserInputTemplateMaxRetryCount,
			MinDigits = Settings.Default.UserInputTemplateMinDigits,
			MaxDigits = Settings.Default.UserInputTemplateMaxDigits,
			StopDigit = GetStopDigit(Settings.Default.UserInputTemplateStopDigit),
			IsValidDigit_0 = Settings.Default.UserInputTemplateIsValidOption0,
			IsValidDigit_1 = Settings.Default.UserInputTemplateIsValidOption1,
			IsValidDigit_2 = Settings.Default.UserInputTemplateIsValidOption2,
			IsValidDigit_3 = Settings.Default.UserInputTemplateIsValidOption3,
			IsValidDigit_4 = Settings.Default.UserInputTemplateIsValidOption4,
			IsValidDigit_5 = Settings.Default.UserInputTemplateIsValidOption5,
			IsValidDigit_6 = Settings.Default.UserInputTemplateIsValidOption6,
			IsValidDigit_7 = Settings.Default.UserInputTemplateIsValidOption7,
			IsValidDigit_8 = Settings.Default.UserInputTemplateIsValidOption8,
			IsValidDigit_9 = Settings.Default.UserInputTemplateIsValidOption9,
			IsValidDigit_Star = Settings.Default.UserInputTemplateIsValidOptionStar,
			IsValidDigit_Pound = Settings.Default.UserInputTemplateIsValidOptionPound
		};
		ComponentBranch componentBranch = new ComponentBranch();
		ComponentBranch componentBranch2 = new ComponentBranch();
		componentBranch.DisplayedText = "Valid Input";
		componentBranch2.DisplayedText = "Invalid Input";
		userInputComponent.Activities.Add(componentBranch);
		userInputComponent.Activities.Add(componentBranch2);
		FlowDesignerNameCreator.CreateName("UserInput", host.Container, userInputComponent);
		return userInputComponent;
	}

	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		return new IComponent[1] { CreateDefaultComponent(host) };
	}
}

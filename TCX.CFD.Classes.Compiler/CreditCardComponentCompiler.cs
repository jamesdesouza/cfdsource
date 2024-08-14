using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class CreditCardComponentCompiler : AbsComponentCompiler
{
	private readonly CreditCardComponent creditCardComponent;

	private void AppendOption(StringBuilder sb, string option)
	{
		if (sb.Length > 0)
		{
			sb.Append(", ");
		}
		sb.Append(option);
	}

	private string GetValidDigitList(UserInputComponent userInputComponent)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (userInputComponent.IsValidDigit_0)
		{
			AppendOption(stringBuilder, "'0'");
		}
		if (userInputComponent.IsValidDigit_1)
		{
			AppendOption(stringBuilder, "'1'");
		}
		if (userInputComponent.IsValidDigit_2)
		{
			AppendOption(stringBuilder, "'2'");
		}
		if (userInputComponent.IsValidDigit_3)
		{
			AppendOption(stringBuilder, "'3'");
		}
		if (userInputComponent.IsValidDigit_4)
		{
			AppendOption(stringBuilder, "'4'");
		}
		if (userInputComponent.IsValidDigit_5)
		{
			AppendOption(stringBuilder, "'5'");
		}
		if (userInputComponent.IsValidDigit_6)
		{
			AppendOption(stringBuilder, "'6'");
		}
		if (userInputComponent.IsValidDigit_7)
		{
			AppendOption(stringBuilder, "'7'");
		}
		if (userInputComponent.IsValidDigit_8)
		{
			AppendOption(stringBuilder, "'8'");
		}
		if (userInputComponent.IsValidDigit_9)
		{
			AppendOption(stringBuilder, "'9'");
		}
		if (userInputComponent.IsValidDigit_Star)
		{
			AppendOption(stringBuilder, "'*'");
		}
		if (userInputComponent.IsValidDigit_Pound)
		{
			AppendOption(stringBuilder, "'#'");
		}
		return stringBuilder.ToString();
	}

	private string GetStopDigitList(UserInputComponent userInputComponent)
	{
		return userInputComponent.StopDigit switch
		{
			DtmfDigits.Digit0 => "'0'", 
			DtmfDigits.Digit1 => "'1'", 
			DtmfDigits.Digit2 => "'2'", 
			DtmfDigits.Digit3 => "'3'", 
			DtmfDigits.Digit4 => "'4'", 
			DtmfDigits.Digit5 => "'5'", 
			DtmfDigits.Digit6 => "'6'", 
			DtmfDigits.Digit7 => "'7'", 
			DtmfDigits.Digit8 => "'8'", 
			DtmfDigits.Digit9 => "'9'", 
			DtmfDigits.DigitStar => "'*'", 
			DtmfDigits.DigitPound => "'#'", 
			_ => string.Empty, 
		};
	}

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

	public CreditCardComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, CreditCardComponent creditCardComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(creditCardComponent), creditCardComponent.GetRootFlow().FlowType, creditCardComponent)
	{
		this.creditCardComponent = creditCardComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (creditCardComponent.EnabledActivities.Count != 2)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InvalidBranchCount"), creditCardComponent.Name), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.RequestNumber.MinDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MinDigitsZero"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.RequestNumber.MaxDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MaxDigitsZero"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.RequestNumber.MinDigits > creditCardComponent.RequestNumber.MaxDigits)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MinDigitsGreaterThanMaxDigits"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
		}
		else if (!creditCardComponent.RequestNumber.IsValidDigit_0 && !creditCardComponent.RequestNumber.IsValidDigit_1 && !creditCardComponent.RequestNumber.IsValidDigit_2 && !creditCardComponent.RequestNumber.IsValidDigit_3 && !creditCardComponent.RequestNumber.IsValidDigit_4 && !creditCardComponent.RequestNumber.IsValidDigit_5 && !creditCardComponent.RequestNumber.IsValidDigit_6 && !creditCardComponent.RequestNumber.IsValidDigit_7 && !creditCardComponent.RequestNumber.IsValidDigit_8 && !creditCardComponent.RequestNumber.IsValidDigit_9 && !creditCardComponent.RequestNumber.IsValidDigit_Star && !creditCardComponent.RequestNumber.IsValidDigit_Pound)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.NoValidDigits"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
		}
		else if (IsStopDigitUsed(creditCardComponent.RequestNumber))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.StopDigitIsValidDigit"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.MinDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MinDigitsZero"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.MaxDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MaxDigitsZero"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.MinDigits > creditCardComponent.RequestExpiration.MaxDigits)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MinDigitsGreaterThanMaxDigits"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && !creditCardComponent.RequestExpiration.IsValidDigit_0 && !creditCardComponent.RequestExpiration.IsValidDigit_1 && !creditCardComponent.RequestExpiration.IsValidDigit_2 && !creditCardComponent.RequestExpiration.IsValidDigit_3 && !creditCardComponent.RequestExpiration.IsValidDigit_4 && !creditCardComponent.RequestExpiration.IsValidDigit_5 && !creditCardComponent.RequestExpiration.IsValidDigit_6 && !creditCardComponent.RequestExpiration.IsValidDigit_7 && !creditCardComponent.RequestExpiration.IsValidDigit_8 && !creditCardComponent.RequestExpiration.IsValidDigit_9 && !creditCardComponent.RequestExpiration.IsValidDigit_Star && !creditCardComponent.RequestExpiration.IsValidDigit_Pound)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.NoValidDigits"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && IsStopDigitUsed(creditCardComponent.RequestExpiration))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.StopDigitIsValidDigit"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.MinDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MinDigitsZero"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.MaxDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MaxDigitsZero"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.MinDigits > creditCardComponent.RequestSecurityCode.MaxDigits)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MinDigitsGreaterThanMaxDigits"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && !creditCardComponent.RequestSecurityCode.IsValidDigit_0 && !creditCardComponent.RequestSecurityCode.IsValidDigit_1 && !creditCardComponent.RequestSecurityCode.IsValidDigit_2 && !creditCardComponent.RequestSecurityCode.IsValidDigit_3 && !creditCardComponent.RequestSecurityCode.IsValidDigit_4 && !creditCardComponent.RequestSecurityCode.IsValidDigit_5 && !creditCardComponent.RequestSecurityCode.IsValidDigit_6 && !creditCardComponent.RequestSecurityCode.IsValidDigit_7 && !creditCardComponent.RequestSecurityCode.IsValidDigit_8 && !creditCardComponent.RequestSecurityCode.IsValidDigit_9 && !creditCardComponent.RequestSecurityCode.IsValidDigit_Star && !creditCardComponent.RequestSecurityCode.IsValidDigit_Pound)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.NoValidDigits"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
		}
		else if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && IsStopDigitUsed(creditCardComponent.RequestSecurityCode))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.StopDigitIsValidDigit"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
		}
		else
		{
			if (creditCardComponent.RequestNumber.InitialPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InitialPromptIsEmpty"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.RequestNumber.SubsequentPrompts.Count == 0 && creditCardComponent.RequestNumber.MaxRetryCount > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.SubsequentPromptIsEmpty"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.RequestNumber.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.TimeoutPromptIsEmpty"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.RequestNumber.InvalidDigitPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InvalidDigitPromptIsEmpty"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.RequestNumber.FirstDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.FirstDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.RequestNumber.InterDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InterDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.RequestNumber.FinalDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.FinalDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.RequestNumber.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MaxRetryCountMustBeGreaterThanZero"), creditCardComponent.Name, "Request Credit Card Number"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.InitialPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InitialPromptIsEmpty"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.SubsequentPrompts.Count == 0 && creditCardComponent.RequestExpiration.MaxRetryCount > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.SubsequentPromptIsEmpty"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.TimeoutPromptIsEmpty"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.InvalidDigitPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InvalidDigitPromptIsEmpty"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.FirstDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.FirstDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.InterDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InterDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.FinalDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.FinalDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.RequestExpiration.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MaxRetryCountMustBeGreaterThanZero"), creditCardComponent.Name, "Request Expiration Date"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.InitialPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InitialPromptIsEmpty"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.SubsequentPrompts.Count == 0 && creditCardComponent.RequestSecurityCode.MaxRetryCount > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.SubsequentPromptIsEmpty"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.TimeoutPromptIsEmpty"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.InvalidDigitPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InvalidDigitPromptIsEmpty"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.FirstDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.FirstDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.InterDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.InterDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.FinalDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.FinalDigitTimeoutMustBeGreaterThanZero"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
			}
			if (creditCardComponent.IsExpirationRequired && creditCardComponent.IsSecurityCodeRequired && creditCardComponent.RequestSecurityCode.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CreditCardComponent.MaxRetryCountMustBeGreaterThanZero"), creditCardComponent.Name, "Request Security Code"), fileObject, flowType, creditCardComponent);
			}
			string expression = string.Format("AND(LESS_THAN({0}.LoopCounter,{1}),NOT(variableMap[\"{0}.Validated\"].Value))", creditCardComponent.Name, 1 + creditCardComponent.MaxRetryCount);
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, expression);
			componentsInitializationScriptSb.AppendFormat("CreditCardLoopComponent {0} = new CreditCardLoopComponent(\"{0}\", callflow, myCall, logHeader);", creditCardComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Condition = () => {{ return Convert.ToBoolean({1}); }};", creditCardComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Container = new SequenceContainerComponent(\"{0}_Container\", callflow, myCall, logHeader);", creditCardComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, creditCardComponent.Name).AppendLine().Append("            ");
			string text = $"{creditCardComponent.Name}RequestNumber";
			componentsInitializationScriptSb.AppendFormat("UserInputComponent {0} = new UserInputComponent(\"{0}\", callflow, myCall, logHeader);", text).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.HasToPauseRecording = true;", text).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", text, creditCardComponent.RequestNumber.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", text, creditCardComponent.RequestNumber.MaxRetryCount - 1).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FirstDigitTimeout = {1};", text, 1000 * creditCardComponent.RequestNumber.FirstDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.InterDigitTimeout = {1};", text, 1000 * creditCardComponent.RequestNumber.InterDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FinalDigitTimeout = {1};", text, 1000 * creditCardComponent.RequestNumber.FinalDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MinDigits = {1};", text, creditCardComponent.RequestNumber.MinDigits).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxDigits = {1};", text, creditCardComponent.RequestNumber.MaxDigits).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ValidDigitList.AddRange(new char[] {{ {1} }});", text, GetValidDigitList(creditCardComponent.RequestNumber)).AppendLine().Append("            ");
			if (creditCardComponent.RequestNumber.StopDigit != DtmfDigits.None)
			{
				componentsInitializationScriptSb.AppendFormat("{0}.StopDigitList.AddRange(new char[] {{ {1} }});", text, GetStopDigitList(creditCardComponent.RequestNumber)).AppendLine().Append("            ");
			}
			foreach (Prompt ınitialPrompt in creditCardComponent.RequestNumber.InitialPrompts)
			{
				ınitialPrompt.Accept(this, isDebugBuild, text, "InitialPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt subsequentPrompt in creditCardComponent.RequestNumber.SubsequentPrompts)
			{
				subsequentPrompt.Accept(this, isDebugBuild, text, "SubsequentPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt ınvalidDigitPrompt in creditCardComponent.RequestNumber.InvalidDigitPrompts)
			{
				ınvalidDigitPrompt.Accept(this, isDebugBuild, text, "InvalidDigitPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt timeoutPrompt in creditCardComponent.RequestNumber.TimeoutPrompts)
			{
				timeoutPrompt.Accept(this, isDebugBuild, text, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Container.ComponentList.Add({1});", creditCardComponent.Name, text).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.NumberHandler = () => {{ return {1}.Buffer; }};", creditCardComponent.Name, text).AppendLine().Append("            ");
			string text2 = text + "_Conditional";
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text2).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Container.ComponentList.Add({1});", creditCardComponent.Name, text2).AppendLine().Append("            ");
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, $"{text}.Result == UserInputComponent.UserInputResults.ValidDigits");
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text2, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_ValidInput\", callflow, myCall, logHeader));", text2).AppendLine().Append("            ");
			string text3 = $"{text2}.ContainerList[0].ComponentList";
			string text4 = string.Empty;
			if (creditCardComponent.IsExpirationRequired)
			{
				string text5 = $"{creditCardComponent.Name}RequestExpiration";
				componentsInitializationScriptSb.AppendFormat("UserInputComponent {0} = new UserInputComponent(\"{0}\", callflow, myCall, logHeader);", text5).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.HasToPauseRecording = true;", text5).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", text5, creditCardComponent.RequestExpiration.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", text5, creditCardComponent.RequestExpiration.MaxRetryCount - 1).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.FirstDigitTimeout = {1};", text5, 1000 * creditCardComponent.RequestExpiration.FirstDigitTimeout).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.InterDigitTimeout = {1};", text5, 1000 * creditCardComponent.RequestExpiration.InterDigitTimeout).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.FinalDigitTimeout = {1};", text5, 1000 * creditCardComponent.RequestExpiration.FinalDigitTimeout).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.MinDigits = {1};", text5, creditCardComponent.RequestExpiration.MinDigits).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.MaxDigits = {1};", text5, creditCardComponent.RequestExpiration.MaxDigits).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ValidDigitList.AddRange(new char[] {{ {1} }});", text5, GetValidDigitList(creditCardComponent.RequestExpiration)).AppendLine().Append("            ");
				if (creditCardComponent.RequestExpiration.StopDigit != DtmfDigits.None)
				{
					componentsInitializationScriptSb.AppendFormat("{0}.StopDigitList.AddRange(new char[] {{ {1} }});", text5, GetStopDigitList(creditCardComponent.RequestExpiration)).AppendLine().Append("            ");
				}
				foreach (Prompt ınitialPrompt2 in creditCardComponent.RequestExpiration.InitialPrompts)
				{
					ınitialPrompt2.Accept(this, isDebugBuild, text5, "InitialPrompts", componentsInitializationScriptSb, audioFileCollector);
				}
				foreach (Prompt subsequentPrompt2 in creditCardComponent.RequestExpiration.SubsequentPrompts)
				{
					subsequentPrompt2.Accept(this, isDebugBuild, text5, "SubsequentPrompts", componentsInitializationScriptSb, audioFileCollector);
				}
				foreach (Prompt ınvalidDigitPrompt2 in creditCardComponent.RequestExpiration.InvalidDigitPrompts)
				{
					ınvalidDigitPrompt2.Accept(this, isDebugBuild, text5, "InvalidDigitPrompts", componentsInitializationScriptSb, audioFileCollector);
				}
				foreach (Prompt timeoutPrompt2 in creditCardComponent.RequestExpiration.TimeoutPrompts)
				{
					timeoutPrompt2.Accept(this, isDebugBuild, text5, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", text3, text5).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ExpirationHandler = () => {{ return {1}.Buffer; }};", creditCardComponent.Name, text5).AppendLine().Append("            ");
				string text6 = text5 + "_Conditional";
				componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text6).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", text3, text6).AppendLine().Append("            ");
				AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, $"{text5}.Result == UserInputComponent.UserInputResults.ValidDigits");
				componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text6, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_ValidInput\", callflow, myCall, logHeader));", text6).AppendLine().Append("            ");
				text3 = $"{text6}.ContainerList[0].ComponentList";
				text4 += string.Format(" || {0}.Result == UserInputComponent.UserInputResults.InvalidDigits || {0}.Result == UserInputComponent.UserInputResults.Timeout", text5);
				if (creditCardComponent.IsSecurityCodeRequired)
				{
					string text7 = $"{creditCardComponent.Name}RequestSecurityCode";
					componentsInitializationScriptSb.AppendFormat("UserInputComponent {0} = new UserInputComponent(\"{0}\", callflow, myCall, logHeader);", text7).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.HasToPauseRecording = true;", text7).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", text7, creditCardComponent.RequestSecurityCode.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", text7, creditCardComponent.RequestSecurityCode.MaxRetryCount - 1).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.FirstDigitTimeout = {1};", text7, 1000 * creditCardComponent.RequestSecurityCode.FirstDigitTimeout).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.InterDigitTimeout = {1};", text7, 1000 * creditCardComponent.RequestSecurityCode.InterDigitTimeout).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.FinalDigitTimeout = {1};", text7, 1000 * creditCardComponent.RequestSecurityCode.FinalDigitTimeout).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.MinDigits = {1};", text7, creditCardComponent.RequestSecurityCode.MinDigits).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.MaxDigits = {1};", text7, creditCardComponent.RequestSecurityCode.MaxDigits).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.ValidDigitList.AddRange(new char[] {{ {1} }});", text7, GetValidDigitList(creditCardComponent.RequestSecurityCode)).AppendLine().Append("            ");
					if (creditCardComponent.RequestSecurityCode.StopDigit != DtmfDigits.None)
					{
						componentsInitializationScriptSb.AppendFormat("{0}.StopDigitList.AddRange(new char[] {{ {1} }});", text7, GetStopDigitList(creditCardComponent.RequestSecurityCode)).AppendLine().Append("            ");
					}
					foreach (Prompt ınitialPrompt3 in creditCardComponent.RequestSecurityCode.InitialPrompts)
					{
						ınitialPrompt3.Accept(this, isDebugBuild, text7, "InitialPrompts", componentsInitializationScriptSb, audioFileCollector);
					}
					foreach (Prompt subsequentPrompt3 in creditCardComponent.RequestSecurityCode.SubsequentPrompts)
					{
						subsequentPrompt3.Accept(this, isDebugBuild, text7, "SubsequentPrompts", componentsInitializationScriptSb, audioFileCollector);
					}
					foreach (Prompt ınvalidDigitPrompt3 in creditCardComponent.RequestSecurityCode.InvalidDigitPrompts)
					{
						ınvalidDigitPrompt3.Accept(this, isDebugBuild, text7, "InvalidDigitPrompts", componentsInitializationScriptSb, audioFileCollector);
					}
					foreach (Prompt timeoutPrompt3 in creditCardComponent.RequestSecurityCode.TimeoutPrompts)
					{
						timeoutPrompt3.Accept(this, isDebugBuild, text7, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", text3, text7).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.SecurityCodeHandler = () => {{ return {1}.Buffer; }};", creditCardComponent.Name, text7).AppendLine().Append("            ");
					string text8 = text7 + "_Conditional";
					componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text8).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", text3, text8).AppendLine().Append("            ");
					AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, $"{text7}.Result == UserInputComponent.UserInputResults.ValidDigits");
					componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text8, absArgument4.GetCompilerString()).AppendLine().Append("            ");
					componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_ValidInput\", callflow, myCall, logHeader));", text8).AppendLine().Append("            ");
					text3 = $"{text8}.ContainerList[0].ComponentList";
					text4 += string.Format(" || {0}.Result == UserInputComponent.UserInputResults.InvalidDigits || {0}.Result == UserInputComponent.UserInputResults.Timeout", text7);
				}
			}
			if ((creditCardComponent.EnabledActivities[0] as ComponentBranch).GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, text3, componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
			{
				return CompilationResult.Cancelled;
			}
			string text9 = creditCardComponent.Name + "_InvalidInputConditional";
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text9).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Container.ComponentList.Add({1});", creditCardComponent.Name, text9).AppendLine().Append("            ");
			ComponentBranch obj = creditCardComponent.EnabledActivities[1] as ComponentBranch;
			AbsArgument absArgument5 = AbsArgument.BuildArgument(validVariables, string.Format("{0}.Result == UserInputComponent.UserInputResults.InvalidDigits || {0}.Result == UserInputComponent.UserInputResults.Timeout", text) + text4);
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text9, absArgument5.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}\", callflow, myCall, logHeader));", text9).AppendLine().Append("            ");
			if (obj.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text9}.ContainerList[0].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
			{
				return CompilationResult.Cancelled;
			}
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

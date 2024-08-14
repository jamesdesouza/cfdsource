using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class AuthenticationComponentCompiler : AbsComponentCompiler
{
	private readonly AuthenticationComponent authenticationComponent;

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

	public AuthenticationComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, AuthenticationComponent authenticationComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(authenticationComponent), authenticationComponent.GetRootFlow().FlowType, authenticationComponent)
	{
		this.authenticationComponent = authenticationComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (authenticationComponent.EnabledActivities.Count != 2)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.InvalidBranchCount"), authenticationComponent.Name), fileObject, flowType, authenticationComponent);
		}
		else if (authenticationComponent.RequestID.MinDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.MinDigitsZero"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
		}
		else if (authenticationComponent.RequestID.MaxDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.MaxDigitsZero"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
		}
		else if (authenticationComponent.RequestID.MinDigits > authenticationComponent.RequestID.MaxDigits)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.MinDigitsGreaterThanMaxDigits"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
		}
		else if (!authenticationComponent.RequestID.IsValidDigit_0 && !authenticationComponent.RequestID.IsValidDigit_1 && !authenticationComponent.RequestID.IsValidDigit_2 && !authenticationComponent.RequestID.IsValidDigit_3 && !authenticationComponent.RequestID.IsValidDigit_4 && !authenticationComponent.RequestID.IsValidDigit_5 && !authenticationComponent.RequestID.IsValidDigit_6 && !authenticationComponent.RequestID.IsValidDigit_7 && !authenticationComponent.RequestID.IsValidDigit_8 && !authenticationComponent.RequestID.IsValidDigit_9 && !authenticationComponent.RequestID.IsValidDigit_Star && !authenticationComponent.RequestID.IsValidDigit_Pound)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.NoValidDigits"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
		}
		else if (IsStopDigitUsed(authenticationComponent.RequestID))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.StopDigitIsValidDigit"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
		}
		else if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.MinDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.MinDigitsZero"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
		}
		else if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.MaxDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.MaxDigitsZero"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
		}
		else if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.MinDigits > authenticationComponent.RequestPIN.MaxDigits)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.MinDigitsGreaterThanMaxDigits"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
		}
		else if (authenticationComponent.IsPinRequired && !authenticationComponent.RequestPIN.IsValidDigit_0 && !authenticationComponent.RequestPIN.IsValidDigit_1 && !authenticationComponent.RequestPIN.IsValidDigit_2 && !authenticationComponent.RequestPIN.IsValidDigit_3 && !authenticationComponent.RequestPIN.IsValidDigit_4 && !authenticationComponent.RequestPIN.IsValidDigit_5 && !authenticationComponent.RequestPIN.IsValidDigit_6 && !authenticationComponent.RequestPIN.IsValidDigit_7 && !authenticationComponent.RequestPIN.IsValidDigit_8 && !authenticationComponent.RequestPIN.IsValidDigit_9 && !authenticationComponent.RequestPIN.IsValidDigit_Star && !authenticationComponent.RequestPIN.IsValidDigit_Pound)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.NoValidDigits"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
		}
		else if (authenticationComponent.IsPinRequired && IsStopDigitUsed(authenticationComponent.RequestPIN))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.StopDigitIsValidDigit"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
		}
		else
		{
			if (authenticationComponent.RequestID.InitialPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.InitialPromptIsEmpty"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.RequestID.SubsequentPrompts.Count == 0 && authenticationComponent.RequestID.MaxRetryCount > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.SubsequentPromptIsEmpty"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.RequestID.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.TimeoutPromptIsEmpty"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.RequestID.InvalidDigitPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.InvalidDigitPromptIsEmpty"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.RequestID.FirstDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.FirstDigitTimeoutMustBeGreaterThanZero"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.RequestID.InterDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.InterDigitTimeoutMustBeGreaterThanZero"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.RequestID.FinalDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.FinalDigitTimeoutMustBeGreaterThanZero"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.RequestID.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.MaxRetryCountMustBeGreaterThanZero"), authenticationComponent.Name, "Request ID"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.InitialPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.InitialPromptIsEmpty"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.SubsequentPrompts.Count == 0 && authenticationComponent.RequestPIN.MaxRetryCount > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.SubsequentPromptIsEmpty"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.TimeoutPromptIsEmpty"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.InvalidDigitPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.InvalidDigitPromptIsEmpty"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.FirstDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.FirstDigitTimeoutMustBeGreaterThanZero"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.InterDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.InterDigitTimeoutMustBeGreaterThanZero"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.FinalDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.FinalDigitTimeoutMustBeGreaterThanZero"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
			}
			if (authenticationComponent.IsPinRequired && authenticationComponent.RequestPIN.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AuthenticationComponent.MaxRetryCountMustBeGreaterThanZero"), authenticationComponent.Name, "Request PIN"), fileObject, flowType, authenticationComponent);
			}
			string expression = string.Format("AND(LESS_THAN({0}.LoopCounter,{1}),NOT(variableMap[\"{0}.Validated\"].Value))", authenticationComponent.Name, 1 + authenticationComponent.MaxRetryCount);
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, expression);
			componentsInitializationScriptSb.AppendFormat("AuthenticationLoopComponent {0} = new AuthenticationLoopComponent(\"{0}\", callflow, myCall, logHeader);", authenticationComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Condition = () => {{ return Convert.ToBoolean({1}); }};", authenticationComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Container = new SequenceContainerComponent(\"{0}_Container\", callflow, myCall, logHeader);", authenticationComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, authenticationComponent.Name).AppendLine().Append("            ");
			string text = $"{authenticationComponent.Name}RequestId";
			componentsInitializationScriptSb.AppendFormat("UserInputComponent {0} = new UserInputComponent(\"{0}\", callflow, myCall, logHeader);", text).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", text, authenticationComponent.RequestID.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", text, authenticationComponent.RequestID.MaxRetryCount - 1).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FirstDigitTimeout = {1};", text, 1000 * authenticationComponent.RequestID.FirstDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.InterDigitTimeout = {1};", text, 1000 * authenticationComponent.RequestID.InterDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FinalDigitTimeout = {1};", text, 1000 * authenticationComponent.RequestID.FinalDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MinDigits = {1};", text, authenticationComponent.RequestID.MinDigits).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxDigits = {1};", text, authenticationComponent.RequestID.MaxDigits).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ValidDigitList.AddRange(new char[] {{ {1} }});", text, GetValidDigitList(authenticationComponent.RequestID)).AppendLine().Append("            ");
			if (authenticationComponent.RequestID.StopDigit != DtmfDigits.None)
			{
				componentsInitializationScriptSb.AppendFormat("{0}.StopDigitList.AddRange(new char[] {{ {1} }});", text, GetStopDigitList(authenticationComponent.RequestID)).AppendLine().Append("            ");
			}
			foreach (Prompt ınitialPrompt in authenticationComponent.RequestID.InitialPrompts)
			{
				ınitialPrompt.Accept(this, isDebugBuild, text, "InitialPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt subsequentPrompt in authenticationComponent.RequestID.SubsequentPrompts)
			{
				subsequentPrompt.Accept(this, isDebugBuild, text, "SubsequentPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt ınvalidDigitPrompt in authenticationComponent.RequestID.InvalidDigitPrompts)
			{
				ınvalidDigitPrompt.Accept(this, isDebugBuild, text, "InvalidDigitPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt timeoutPrompt in authenticationComponent.RequestID.TimeoutPrompts)
			{
				timeoutPrompt.Accept(this, isDebugBuild, text, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Container.ComponentList.Add({1});", authenticationComponent.Name, text).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.IdHandler = () => {{ return {1}.Buffer; }};", authenticationComponent.Name, text).AppendLine().Append("            ");
			string text2 = text + "_Conditional";
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text2).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Container.ComponentList.Add({1});", authenticationComponent.Name, text2).AppendLine().Append("            ");
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, $"{text}.Result == UserInputComponent.UserInputResults.ValidDigits");
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text2, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_ValidInput\", callflow, myCall, logHeader));", text2).AppendLine().Append("            ");
			string text3 = $"{text2}.ContainerList[0].ComponentList";
			string text4 = string.Empty;
			if (authenticationComponent.IsPinRequired)
			{
				string text5 = $"{authenticationComponent.Name}RequestPin";
				componentsInitializationScriptSb.AppendFormat("UserInputComponent {0} = new UserInputComponent(\"{0}\", callflow, myCall, logHeader);", text5).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", text5, authenticationComponent.RequestPIN.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", text5, authenticationComponent.RequestPIN.MaxRetryCount - 1).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.FirstDigitTimeout = {1};", text5, 1000 * authenticationComponent.RequestPIN.FirstDigitTimeout).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.InterDigitTimeout = {1};", text5, 1000 * authenticationComponent.RequestPIN.InterDigitTimeout).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.FinalDigitTimeout = {1};", text5, 1000 * authenticationComponent.RequestPIN.FinalDigitTimeout).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.MinDigits = {1};", text5, authenticationComponent.RequestPIN.MinDigits).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.MaxDigits = {1};", text5, authenticationComponent.RequestPIN.MaxDigits).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ValidDigitList.AddRange(new char[] {{ {1} }});", text5, GetValidDigitList(authenticationComponent.RequestPIN)).AppendLine().Append("            ");
				if (authenticationComponent.RequestPIN.StopDigit != DtmfDigits.None)
				{
					componentsInitializationScriptSb.AppendFormat("{0}.StopDigitList.AddRange(new char[] {{ {1} }});", text5, GetStopDigitList(authenticationComponent.RequestPIN)).AppendLine().Append("            ");
				}
				foreach (Prompt ınitialPrompt2 in authenticationComponent.RequestPIN.InitialPrompts)
				{
					ınitialPrompt2.Accept(this, isDebugBuild, text5, "InitialPrompts", componentsInitializationScriptSb, audioFileCollector);
				}
				foreach (Prompt subsequentPrompt2 in authenticationComponent.RequestPIN.SubsequentPrompts)
				{
					subsequentPrompt2.Accept(this, isDebugBuild, text5, "SubsequentPrompts", componentsInitializationScriptSb, audioFileCollector);
				}
				foreach (Prompt ınvalidDigitPrompt2 in authenticationComponent.RequestPIN.InvalidDigitPrompts)
				{
					ınvalidDigitPrompt2.Accept(this, isDebugBuild, text5, "InvalidDigitPrompts", componentsInitializationScriptSb, audioFileCollector);
				}
				foreach (Prompt timeoutPrompt2 in authenticationComponent.RequestPIN.TimeoutPrompts)
				{
					timeoutPrompt2.Accept(this, isDebugBuild, text5, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", text3, text5).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.PinHandler = () => {{ return {1}.Buffer; }};", authenticationComponent.Name, text5).AppendLine().Append("            ");
				string text6 = text5 + "_Conditional";
				componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text6).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", text3, text6).AppendLine().Append("            ");
				AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, $"{text5}.Result == UserInputComponent.UserInputResults.ValidDigits");
				componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text6, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_ValidInput\", callflow, myCall, logHeader));", text6).AppendLine().Append("            ");
				text3 = $"{text6}.ContainerList[0].ComponentList";
				text4 = string.Format("|| {0}.Result == UserInputComponent.UserInputResults.InvalidDigits || {0}.Result == UserInputComponent.UserInputResults.Timeout", text5);
			}
			if ((authenticationComponent.EnabledActivities[0] as ComponentBranch).GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, text3, componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
			{
				return CompilationResult.Cancelled;
			}
			string text7 = authenticationComponent.Name + "_InvalidInputConditional";
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text7).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Container.ComponentList.Add({1});", authenticationComponent.Name, text7).AppendLine().Append("            ");
			ComponentBranch obj = authenticationComponent.EnabledActivities[1] as ComponentBranch;
			AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, string.Format("{0}.Result == UserInputComponent.UserInputResults.InvalidDigits || {0}.Result == UserInputComponent.UserInputResults.Timeout", text) + text4);
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text7, absArgument4.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}\", callflow, myCall, logHeader));", text7).AppendLine().Append("            ");
			if (obj.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text7}.ContainerList[0].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
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

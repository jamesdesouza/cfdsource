using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class UserInputComponentCompiler : AbsComponentCompiler
{
	private readonly UserInputComponent userInputComponent;

	private void AppendOption(StringBuilder sb, string option)
	{
		if (sb.Length > 0)
		{
			sb.Append(", ");
		}
		sb.Append(option);
	}

	private string GetValidDigitList()
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

	private string GetStopDigitList()
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

	private bool IsStopDigitUsed()
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

	public UserInputComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, UserInputComponent userInputComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(userInputComponent), userInputComponent.GetRootFlow().FlowType, userInputComponent)
	{
		this.userInputComponent = userInputComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (userInputComponent.EnabledActivities.Count != 2)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.InvalidBranchCount"), userInputComponent.Name), fileObject, flowType, userInputComponent);
		}
		else if (userInputComponent.MinDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.MinDigitsZero"), userInputComponent.Name), fileObject, flowType, userInputComponent);
		}
		else if (userInputComponent.MaxDigits == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.MaxDigitsZero"), userInputComponent.Name), fileObject, flowType, userInputComponent);
		}
		else if (userInputComponent.MinDigits > userInputComponent.MaxDigits)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.MinDigitsGreaterThanMaxDigits"), userInputComponent.Name), fileObject, flowType, userInputComponent);
		}
		else if (!userInputComponent.IsValidDigit_0 && !userInputComponent.IsValidDigit_1 && !userInputComponent.IsValidDigit_2 && !userInputComponent.IsValidDigit_3 && !userInputComponent.IsValidDigit_4 && !userInputComponent.IsValidDigit_5 && !userInputComponent.IsValidDigit_6 && !userInputComponent.IsValidDigit_7 && !userInputComponent.IsValidDigit_8 && !userInputComponent.IsValidDigit_9 && !userInputComponent.IsValidDigit_Star && !userInputComponent.IsValidDigit_Pound)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.NoValidDigits"), userInputComponent.Name), fileObject, flowType, userInputComponent);
		}
		else if (IsStopDigitUsed())
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.StopDigitIsValidDigit"), userInputComponent.Name), fileObject, flowType, userInputComponent);
		}
		else
		{
			if (userInputComponent.InitialPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.InitialPromptIsEmpty"), userInputComponent.Name), fileObject, flowType, userInputComponent);
			}
			if (userInputComponent.SubsequentPrompts.Count == 0 && userInputComponent.MaxRetryCount > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.SubsequentPromptIsEmpty"), userInputComponent.Name), fileObject, flowType, userInputComponent);
			}
			if (userInputComponent.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.TimeoutPromptIsEmpty"), userInputComponent.Name), fileObject, flowType, userInputComponent);
			}
			if (userInputComponent.InvalidDigitPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.InvalidDigitPromptIsEmpty"), userInputComponent.Name), fileObject, flowType, userInputComponent);
			}
			if (userInputComponent.FirstDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.FirstDigitTimeoutMustBeGreaterThanZero"), userInputComponent.Name), fileObject, flowType, userInputComponent);
			}
			if (userInputComponent.InterDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.InterDigitTimeoutMustBeGreaterThanZero"), userInputComponent.Name), fileObject, flowType, userInputComponent);
			}
			if (userInputComponent.FinalDigitTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.FinalDigitTimeoutMustBeGreaterThanZero"), userInputComponent.Name), fileObject, flowType, userInputComponent);
			}
			if (userInputComponent.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.UserInputComponent.MaxRetryCountMustBeGreaterThanZero"), userInputComponent.Name), fileObject, flowType, userInputComponent);
			}
			componentsInitializationScriptSb.AppendFormat("UserInputComponent {0} = new UserInputComponent(\"{0}\", callflow, myCall, logHeader);", userInputComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", userInputComponent.Name, userInputComponent.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", userInputComponent.Name, userInputComponent.MaxRetryCount - 1).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FirstDigitTimeout = {1};", userInputComponent.Name, 1000 * userInputComponent.FirstDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.InterDigitTimeout = {1};", userInputComponent.Name, 1000 * userInputComponent.InterDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FinalDigitTimeout = {1};", userInputComponent.Name, 1000 * userInputComponent.FinalDigitTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MinDigits = {1};", userInputComponent.Name, userInputComponent.MinDigits).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxDigits = {1};", userInputComponent.Name, userInputComponent.MaxDigits).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ValidDigitList.AddRange(new char[] {{ {1} }});", userInputComponent.Name, GetValidDigitList()).AppendLine().Append("            ");
			if (userInputComponent.StopDigit != DtmfDigits.None)
			{
				componentsInitializationScriptSb.AppendFormat("{0}.StopDigitList.AddRange(new char[] {{ {1} }});", userInputComponent.Name, GetStopDigitList()).AppendLine().Append("            ");
			}
			foreach (Prompt ınitialPrompt in userInputComponent.InitialPrompts)
			{
				ınitialPrompt.Accept(this, isDebugBuild, userInputComponent.Name, "InitialPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt subsequentPrompt in userInputComponent.SubsequentPrompts)
			{
				subsequentPrompt.Accept(this, isDebugBuild, userInputComponent.Name, "SubsequentPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt ınvalidDigitPrompt in userInputComponent.InvalidDigitPrompts)
			{
				ınvalidDigitPrompt.Accept(this, isDebugBuild, userInputComponent.Name, "InvalidDigitPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt timeoutPrompt in userInputComponent.TimeoutPrompts)
			{
				timeoutPrompt.Accept(this, isDebugBuild, userInputComponent.Name, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, userInputComponent.Name).AppendLine().Append("            ");
			string text = userInputComponent.Name + "_Conditional";
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, text).AppendLine().Append("            ");
			ComponentBranch obj = userInputComponent.EnabledActivities[0] as ComponentBranch;
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, $"{userInputComponent.Name}.Result == UserInputComponent.UserInputResults.ValidDigits");
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_ValidInput\", callflow, myCall, logHeader));", text).AppendLine().Append("            ");
			if (obj.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text}.ContainerList[0].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
			{
				return CompilationResult.Cancelled;
			}
			ComponentBranch obj2 = userInputComponent.EnabledActivities[1] as ComponentBranch;
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, string.Format("{0}.Result == UserInputComponent.UserInputResults.InvalidDigits || {0}.Result == UserInputComponent.UserInputResults.Timeout", userInputComponent.Name));
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_InvalidInput\", callflow, myCall, logHeader));", text).AppendLine().Append("            ");
			if (obj2.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text}.ContainerList[1].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
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

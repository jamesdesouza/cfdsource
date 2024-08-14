using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class VoiceInputComponentCompiler : AbsComponentCompiler
{
	private readonly VoiceInputComponent voiceInputComponent;

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
		if (voiceInputComponent.IsValidDigit_0)
		{
			AppendOption(stringBuilder, "'0'");
		}
		if (voiceInputComponent.IsValidDigit_1)
		{
			AppendOption(stringBuilder, "'1'");
		}
		if (voiceInputComponent.IsValidDigit_2)
		{
			AppendOption(stringBuilder, "'2'");
		}
		if (voiceInputComponent.IsValidDigit_3)
		{
			AppendOption(stringBuilder, "'3'");
		}
		if (voiceInputComponent.IsValidDigit_4)
		{
			AppendOption(stringBuilder, "'4'");
		}
		if (voiceInputComponent.IsValidDigit_5)
		{
			AppendOption(stringBuilder, "'5'");
		}
		if (voiceInputComponent.IsValidDigit_6)
		{
			AppendOption(stringBuilder, "'6'");
		}
		if (voiceInputComponent.IsValidDigit_7)
		{
			AppendOption(stringBuilder, "'7'");
		}
		if (voiceInputComponent.IsValidDigit_8)
		{
			AppendOption(stringBuilder, "'8'");
		}
		if (voiceInputComponent.IsValidDigit_9)
		{
			AppendOption(stringBuilder, "'9'");
		}
		if (voiceInputComponent.IsValidDigit_Star)
		{
			AppendOption(stringBuilder, "'*'");
		}
		if (voiceInputComponent.IsValidDigit_Pound)
		{
			AppendOption(stringBuilder, "'#'");
		}
		return stringBuilder.ToString();
	}

	private string GetStopDigitList()
	{
		return voiceInputComponent.StopDigit switch
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

	public VoiceInputComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, VoiceInputComponent voiceInputComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(voiceInputComponent), voiceInputComponent.GetRootFlow().FlowType, voiceInputComponent)
	{
		this.voiceInputComponent = voiceInputComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (voiceInputComponent.EnabledActivities.Count != 2)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.InvalidBranchCount"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
		}
		else if (string.IsNullOrEmpty(voiceInputComponent.SaveToFile))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.SaveToFileIsEmpty"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
		}
		else if (voiceInputComponent.SaveToFile == "true" && string.IsNullOrEmpty(voiceInputComponent.FileName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.FileNameIsEmpty"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
		}
		else
		{
			if (voiceInputComponent.InitialPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.InitialPromptsAreEmpty"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			if (voiceInputComponent.SubsequentPrompts.Count == 0 && voiceInputComponent.MaxRetryCount > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.SubsequentPromptsAreEmpty"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			if (voiceInputComponent.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.TimeoutPromptsAreEmpty"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			if (voiceInputComponent.InvalidInputPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.InvalidInputPromptsAreEmpty"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			if (voiceInputComponent.InputTimeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.InputTimeoutMustBeGreaterThanZero"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			if (voiceInputComponent.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.MaxRetryCountMustBeGreaterThanZero"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			if (!fileObject.GetProjectObject().OnlineServices.IsReadyForSTT())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.SpeechToTextOnlineServicesConfigurationIsMandatory"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, string.IsNullOrEmpty(voiceInputComponent.FileName) ? "\"\"" : voiceInputComponent.FileName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "FileName", voiceInputComponent.Name, voiceInputComponent.FileName), fileObject, flowType, voiceInputComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, voiceInputComponent.SaveToFile);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "SaveToFile", voiceInputComponent.Name, voiceInputComponent.SaveToFile), fileObject, flowType, voiceInputComponent);
			}
			componentsInitializationScriptSb.AppendFormat("VoiceInputComponent {0} = new VoiceInputComponent(\"{0}\", callflow, myCall, logHeader, onlineServices.SpeechToTextEngine);", voiceInputComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", voiceInputComponent.Name, voiceInputComponent.MaxRetryCount - 1).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.InputTimeout = {1};", voiceInputComponent.Name, 1000 * voiceInputComponent.InputTimeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.LanguageCode = \"{1}\";", voiceInputComponent.Name, voiceInputComponent.LanguageCode).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FileNameHandler = () => {{ return Convert.ToString({1}); }};", voiceInputComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.SaveToFileHandler = () => {{ return Convert.ToBoolean({1}); }};", voiceInputComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			if (voiceInputComponent.AcceptDtmfInput)
			{
				if (voiceInputComponent.MinDigits == 0)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.MinDigitsZero"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
				}
				if (voiceInputComponent.MaxDigits == 0)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.MaxDigitsZero"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
				}
				if (voiceInputComponent.MinDigits > voiceInputComponent.MaxDigits)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.MinDigitsGreaterThanMaxDigits"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
				}
				if (!voiceInputComponent.IsValidDigit_0 && !voiceInputComponent.IsValidDigit_1 && !voiceInputComponent.IsValidDigit_2 && !voiceInputComponent.IsValidDigit_3 && !voiceInputComponent.IsValidDigit_4 && !voiceInputComponent.IsValidDigit_5 && !voiceInputComponent.IsValidDigit_6 && !voiceInputComponent.IsValidDigit_7 && !voiceInputComponent.IsValidDigit_8 && !voiceInputComponent.IsValidDigit_9 && !voiceInputComponent.IsValidDigit_Star && !voiceInputComponent.IsValidDigit_Pound)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.NoValidDigits"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
				}
				if (IsStopDigitUsed())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.StopDigitIsValidDigit"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
				}
				if (voiceInputComponent.DtmfTimeout == 0)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.DtmfTimeoutMustBeGreaterThanZero"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", voiceInputComponent.Name, voiceInputComponent.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.DtmfTimeout = {1};", voiceInputComponent.Name, 1000 * voiceInputComponent.DtmfTimeout).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.MinDigits = {1};", voiceInputComponent.Name, voiceInputComponent.MinDigits).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.MaxDigits = {1};", voiceInputComponent.Name, voiceInputComponent.MaxDigits).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ValidDigitList.AddRange(new char[] {{ {1} }});", voiceInputComponent.Name, GetValidDigitList()).AppendLine().Append("            ");
				if (voiceInputComponent.StopDigit != DtmfDigits.None)
				{
					componentsInitializationScriptSb.AppendFormat("{0}.StopDigitList.AddRange(new char[] {{ {1} }});", voiceInputComponent.Name, GetStopDigitList()).AppendLine().Append("            ");
				}
			}
			foreach (Prompt ınitialPrompt in voiceInputComponent.InitialPrompts)
			{
				ınitialPrompt.Accept(this, isDebugBuild, voiceInputComponent.Name, "InitialPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt subsequentPrompt in voiceInputComponent.SubsequentPrompts)
			{
				subsequentPrompt.Accept(this, isDebugBuild, voiceInputComponent.Name, "SubsequentPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt ınvalidInputPrompt in voiceInputComponent.InvalidInputPrompts)
			{
				ınvalidInputPrompt.Accept(this, isDebugBuild, voiceInputComponent.Name, "InvalidInputPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt timeoutPrompt in voiceInputComponent.TimeoutPrompts)
			{
				timeoutPrompt.Accept(this, isDebugBuild, voiceInputComponent.Name, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			bool flag = false;
			for (int i = 0; i < voiceInputComponent.Dictionary.Count; i++)
			{
				string text = voiceInputComponent.Dictionary[i];
				if (string.IsNullOrEmpty(text))
				{
					flag = true;
					continue;
				}
				AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, text);
				if (absArgument3.IsSafeExpression())
				{
					componentsInitializationScriptSb.AppendFormat("{0}.Dictionary.Add(() => {{ return Convert.ToString({1}); }});", voiceInputComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				}
				else
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.InvalidDictionaryExpression"), voiceInputComponent.Name, i, text), fileObject, flowType, voiceInputComponent);
				}
			}
			if (flag)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.EmptyDictionaryEntry"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			bool flag2 = false;
			for (int j = 0; j < voiceInputComponent.Hints.Count; j++)
			{
				string text2 = voiceInputComponent.Hints[j];
				if (string.IsNullOrEmpty(text2))
				{
					flag2 = true;
				}
				else
				{
					componentsInitializationScriptSb.AppendFormat("{0}.Hints.Add(() => {{ return \"{1}\"; }});", voiceInputComponent.Name, text2).AppendLine().Append("            ");
				}
			}
			if (flag2)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VoiceInputComponent.EmptyHints"), voiceInputComponent.Name), fileObject, flowType, voiceInputComponent);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, voiceInputComponent.Name).AppendLine().Append("            ");
			string text3 = voiceInputComponent.Name + "_Conditional";
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text3).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, text3).AppendLine().Append("            ");
			ComponentBranch obj = voiceInputComponent.EnabledActivities[0] as ComponentBranch;
			AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, string.Format("{0}.Result == VoiceInputComponent.VoiceInputResults.ValidInput || {0}.Result == VoiceInputComponent.VoiceInputResults.ValidDtmfInput", voiceInputComponent.Name));
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text3, absArgument4.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_ValidInput\", callflow, myCall, logHeader));", text3).AppendLine().Append("            ");
			if (obj.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text3}.ContainerList[0].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
			{
				return CompilationResult.Cancelled;
			}
			ComponentBranch obj2 = voiceInputComponent.EnabledActivities[1] as ComponentBranch;
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return true; }});", text3).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_InvalidInput\", callflow, myCall, logHeader));", text3).AppendLine().Append("            ");
			if (obj2.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text3}.ContainerList[1].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
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

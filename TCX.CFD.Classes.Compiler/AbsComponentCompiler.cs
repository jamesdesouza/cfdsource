using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public abstract class AbsComponentCompiler
{
	protected AbsCompilerResultCollector compilerResultCollector;

	protected FileObject fileObject;

	protected int progress;

	protected CompilerErrorCounter errorCounter;

	protected List<string> validVariables;

	protected FlowTypes flowType;

	protected AbsComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, List<string> validVariables, FlowTypes flowType, Activity activity)
	{
		this.compilerResultCollector = compilerResultCollector;
		this.fileObject = fileObject;
		this.progress = progress;
		this.errorCounter = errorCounter;
		this.validVariables = validVariables;
		this.flowType = flowType;
		if (string.IsNullOrEmpty(activity.Name))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, LocalizedResourceMgr.GetString("Compiler.ComponentNameValidation.EmptyName"), fileObject, flowType, activity);
			return;
		}
		if (!activity.Name.StartsWith("_") && !char.IsLetter(activity.Name[0]))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ComponentNameValidation.InvalidFirstCharacter"), activity.Name), fileObject, flowType, activity);
			return;
		}
		string name = activity.Name;
		foreach (char c in name)
		{
			if (!char.IsLetterOrDigit(c) && c != '_')
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ComponentNameValidation.InvalidSubsequentCharacters"), activity.Name), fileObject, flowType, activity);
				break;
			}
		}
	}

	public abstract CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector);

	public void Visit(AudioFilePrompt audioFilePrompt, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(audioFilePrompt.AudioFileName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AudioFileIsMandatory"), componentName), fileObject, flowType, (Activity)audioFilePrompt.ContainerActivity);
			return;
		}
		FileInfo fileInfo = new FileInfo(Path.Combine(fileObject.GetProjectObject().GetFolderPath(), Path.Combine("Audio", audioFilePrompt.AudioFileName)));
		if (!fileInfo.Exists)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AudioFileNotFound"), audioFilePrompt.AudioFileName, componentName), fileObject, flowType, (Activity)audioFilePrompt.ContainerActivity);
			return;
		}
		audioFileCollector.AddAudioFile(fileInfo.Name);
		componentsInitializationScriptSb.AppendFormat("{0}.{1}.Add(new AudioFilePrompt(() => {{ return \"{2}\"; }}));", componentName, promptCollectionName, fileInfo.Name).AppendLine().Append("            ");
	}

	public void Visit(RecordedAudioPrompt recordedAudioPrompt, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb)
	{
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, recordedAudioPrompt.AudioId);
		if (absArgument.IsVariableName())
		{
			componentsInitializationScriptSb.AppendFormat("{0}.{1}.Add(new AudioFilePrompt(() => {{ return Convert.ToString({2}); }}));", componentName, promptCollectionName, absArgument.GetCompilerString()).AppendLine().Append("            ");
		}
		else
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidRecordedAudioPromptValue"), componentName), fileObject, flowType, (Activity)recordedAudioPrompt.ContainerActivity);
		}
	}

	public void Visit(DynamicAudioFilePrompt dynamicAudioFilePrompt, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(dynamicAudioFilePrompt.AudioFileName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AudioFileExpressionIsMandatory"), componentName), fileObject, flowType, (Activity)dynamicAudioFilePrompt.ContainerActivity);
			return;
		}
		audioFileCollector.IncludeAllFiles = true;
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, dynamicAudioFilePrompt.AudioFileName);
		if (absArgument.IsSafeExpression())
		{
			componentsInitializationScriptSb.AppendFormat("{0}.{1}.Add(new AudioFilePrompt(() => {{ return Convert.ToString({2}); }}));", componentName, promptCollectionName, absArgument.GetCompilerString()).AppendLine().Append("            ");
		}
		else
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "AudioFileName", componentName, dynamicAudioFilePrompt.AudioFileName), fileObject, flowType, (Activity)dynamicAudioFilePrompt.ContainerActivity);
		}
	}

	public void Visit(NumberPrompt numberPrompt, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb)
	{
		if (string.IsNullOrEmpty(numberPrompt.Number))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.NumberIsMandatory"), componentName), fileObject, flowType, (Activity)numberPrompt.ContainerActivity);
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, numberPrompt.Number);
		if (absArgument.IsSafeExpression())
		{
			string text = ((numberPrompt.Format == NumberPromptFormats.FullNumber) ? "NumberPrompt.NumberFormats.FullNumber" : ((numberPrompt.Format == NumberPromptFormats.GroupedByTwo) ? "NumberPrompt.NumberFormats.GroupedByTwo" : "NumberPrompt.NumberFormats.OneByOne"));
			componentsInitializationScriptSb.AppendFormat("{0}.{1}.Add(new NumberPrompt({2}, () => {{ return Convert.ToString({3}); }}));", componentName, promptCollectionName, text, absArgument.GetCompilerString()).AppendLine().Append("            ");
		}
		else
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Number", componentName, numberPrompt.Number), fileObject, flowType, (Activity)numberPrompt.ContainerActivity);
		}
	}

	public void Visit(TextToSpeechAudioPrompt textToSpeechAudioPrompt, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb)
	{
		if (!fileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TextToSpeechAudioOnlineServicesConfigurationIsMandatory"), componentName), fileObject, flowType, (Activity)textToSpeechAudioPrompt.ContainerActivity);
			return;
		}
		if (string.IsNullOrEmpty(textToSpeechAudioPrompt.Text))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TextToSpeechAudioTextExpressionIsMandatory"), componentName), fileObject, flowType, (Activity)textToSpeechAudioPrompt.ContainerActivity);
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, textToSpeechAudioPrompt.Text);
		if (absArgument.IsSafeExpression())
		{
			componentsInitializationScriptSb.AppendFormat("{0}.{1}.Add(new TextToSpeechAudioPrompt(myCall, logHeader, onlineServices.TextToSpeechEngine, \"{2}\", {3}, {4}, () => {{ return Convert.ToString({5}); }}));", componentName, promptCollectionName, textToSpeechAudioPrompt.VoiceName, EnumHelper.TextToSpeechVoiceTypeToString(textToSpeechAudioPrompt.VoiceType), (textToSpeechAudioPrompt.Format == TextToSpeechFormats.Text) ? "TextToSpeechAudioPrompt.TextToSpeechFormats.Text" : "TextToSpeechAudioPrompt.TextToSpeechFormats.SSML", absArgument.GetCompilerString()).AppendLine().Append("            ");
		}
		else
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Text", componentName, textToSpeechAudioPrompt.Text), fileObject, flowType, (Activity)textToSpeechAudioPrompt.ContainerActivity);
		}
	}

	public void Visit(YesNoSurveyQuestion question, bool isDebugBuild, string componentName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		componentsInitializationScriptSb.AppendFormat("{0}.SurveyQuestions.Add(new YesNoSurveyQuestion(\"{1}\", new List<AbsPrompt>(), '{2}', '{3}'));", componentName, question.Tag, EnumHelper.SurveyAnswerToString(question.YesAnswer), EnumHelper.SurveyAnswerToString(question.NoAnswer)).AppendLine().Append("            ");
		string componentName2 = string.Format("{0}.SurveyQuestions[{0}.SurveyQuestions.Count - 1]", componentName);
		foreach (Prompt prompt in question.Prompts)
		{
			prompt.Accept(this, isDebugBuild, componentName2, "Prompts", componentsInitializationScriptSb, audioFileCollector);
		}
	}

	public void Visit(RangeSurveyQuestion question, bool isDebugBuild, string componentName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (SurveyAnswers value in Enum.GetValues(typeof(SurveyAnswers)))
		{
			if (value >= question.RangeStartAnswer && value <= question.RangeEndAnswer)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.AppendFormat("'{0}'", EnumHelper.SurveyAnswerToString(value));
			}
		}
		componentsInitializationScriptSb.AppendFormat("{0}.SurveyQuestions.Add(new RangeSurveyQuestion(\"{1}\", new List<AbsPrompt>(), new List<char> {{ {2} }}));", componentName, question.Tag, stringBuilder.ToString()).AppendLine().Append("            ");
		string componentName2 = string.Format("{0}.SurveyQuestions[{0}.SurveyQuestions.Count - 1]", componentName);
		foreach (Prompt prompt in question.Prompts)
		{
			prompt.Accept(this, isDebugBuild, componentName2, "Prompts", componentsInitializationScriptSb, audioFileCollector);
		}
	}

	public void Visit(RecordingSurveyQuestion question, bool isDebugBuild, string componentName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		componentsInitializationScriptSb.AppendFormat("{0}.SurveyQuestions.Add(new RecordingSurveyQuestion(\"{1}\", new List<AbsPrompt>(), {2}, {3}, new List<AbsPrompt>(), new List<AbsPrompt>(), '{4}', '{5}'));", componentName, question.Tag, 1000 * question.MaxRecordingTime, question.OfferPlayback ? "true" : "false", EnumHelper.SurveyAnswerToString(question.KeepAnswer), EnumHelper.SurveyAnswerToString(question.RerecordAnswer)).AppendLine().Append("            ");
		string text = string.Format("{0}.SurveyQuestions[{0}.SurveyQuestions.Count - 1]", componentName);
		foreach (Prompt prompt in question.Prompts)
		{
			prompt.Accept(this, isDebugBuild, text, "Prompts", componentsInitializationScriptSb, audioFileCollector);
		}
		audioFileCollector.IncludeBeepFile = true;
		DynamicAudioFilePrompt dynamicAudioFilePrompt = new DynamicAudioFilePrompt();
		dynamicAudioFilePrompt.AudioFileName = "\"beep.wav\"";
		dynamicAudioFilePrompt.Accept(this, isDebugBuild, text, "Prompts", componentsInitializationScriptSb, audioFileCollector);
		string componentName2 = "((RecordingSurveyQuestion)" + text + ")";
		foreach (Prompt offerPlaybackPreRecordingPrompt in question.OfferPlaybackPreRecordingPrompts)
		{
			offerPlaybackPreRecordingPrompt.Accept(this, isDebugBuild, componentName2, "OfferPlaybackPreRecordingPrompts", componentsInitializationScriptSb, audioFileCollector);
		}
		foreach (Prompt offerPlaybackPostRecordingPrompt in question.OfferPlaybackPostRecordingPrompts)
		{
			offerPlaybackPostRecordingPrompt.Accept(this, isDebugBuild, componentName2, "OfferPlaybackPostRecordingPrompts", componentsInitializationScriptSb, audioFileCollector);
		}
	}
}

using System;
using System.Text;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class TextToSpeechAudioPrompt : Prompt
{
	public string VoiceName { get; set; } = Settings.Default.DefaultTtsVoiceName;


	public TextToSpeechVoiceTypes VoiceType { get; set; } = EnumHelper.StringToTextToSpeechVoiceType(Settings.Default.DefaultTtsVoiceType);


	public TextToSpeechFormats Format { get; set; } = EnumHelper.StringToTextToSpeechFormat(Settings.Default.DefaultTtsFormat);


	public string Text { get; set; } = string.Empty;


	private TextToSpeechAudioPrompt(string voiceName, TextToSpeechVoiceTypes voiceType, TextToSpeechFormats format, string text)
	{
		VoiceName = voiceName;
		VoiceType = voiceType;
		Format = format;
		Text = text;
	}

	public TextToSpeechAudioPrompt()
	{
	}

	public override void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		compiler.Visit(this, isDebugBuild, componentName, promptCollectionName, componentsInitializationScriptSb);
	}

	public override void MigrateConstantStringExpressions()
	{
		Text = ExpressionHelper.MigrateConstantStringExpression(containerActivity, Text);
	}

	public override Prompt Clone()
	{
		return new TextToSpeechAudioPrompt(VoiceName, VoiceType, Format, Text)
		{
			ContainerActivity = containerActivity
		};
	}

	public override AbsPromptEditorRowControl CreatePromptEditorRowControl()
	{
		return new TextToSpeechAudioPromptEditorRowControl(containerActivity, this);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Text = ExpressionHelper.RenameComponent(containerActivity, Text, oldValue, newValue);
	}
}

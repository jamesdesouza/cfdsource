using System;
using System.Text;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class AudioFilePrompt : Prompt
{
	public string AudioFileName { get; set; } = string.Empty;


	private AudioFilePrompt(string audioFileName)
	{
		AudioFileName = audioFileName;
	}

	public AudioFilePrompt()
	{
	}

	public override void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		compiler.Visit(this, isDebugBuild, componentName, promptCollectionName, componentsInitializationScriptSb, audioFileCollector);
	}

	public override void MigrateConstantStringExpressions()
	{
	}

	public override Prompt Clone()
	{
		return new AudioFilePrompt(AudioFileName)
		{
			ContainerActivity = containerActivity
		};
	}

	public override AbsPromptEditorRowControl CreatePromptEditorRowControl()
	{
		return new AudioFilePromptEditorRowControl(containerActivity, this);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
	}
}

using System;
using System.Text;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class DynamicAudioFilePrompt : Prompt
{
	public string AudioFileName { get; set; } = string.Empty;


	private DynamicAudioFilePrompt(string audioFileName)
	{
		AudioFileName = audioFileName;
	}

	public DynamicAudioFilePrompt()
	{
	}

	public override void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		compiler.Visit(this, isDebugBuild, componentName, promptCollectionName, componentsInitializationScriptSb, audioFileCollector);
	}

	public override void MigrateConstantStringExpressions()
	{
		AudioFileName = ExpressionHelper.MigrateConstantStringExpression(containerActivity, AudioFileName);
	}

	public override Prompt Clone()
	{
		return new DynamicAudioFilePrompt(AudioFileName)
		{
			ContainerActivity = containerActivity
		};
	}

	public override AbsPromptEditorRowControl CreatePromptEditorRowControl()
	{
		return new DynamicAudioFilePromptEditorRowControl(containerActivity, this);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		AudioFileName = ExpressionHelper.RenameComponent(containerActivity, AudioFileName, oldValue, newValue);
	}
}

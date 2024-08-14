using System;
using System.Text;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class RecordedAudioPrompt : Prompt
{
	public string AudioId { get; set; } = string.Empty;


	private RecordedAudioPrompt(string audioId)
	{
		AudioId = audioId;
	}

	public RecordedAudioPrompt()
	{
	}

	public override void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		compiler.Visit(this, isDebugBuild, componentName, promptCollectionName, componentsInitializationScriptSb);
	}

	public override void MigrateConstantStringExpressions()
	{
		AudioId = ExpressionHelper.MigrateConstantStringExpression(containerActivity, AudioId);
	}

	public override Prompt Clone()
	{
		return new RecordedAudioPrompt(AudioId)
		{
			ContainerActivity = containerActivity
		};
	}

	public override AbsPromptEditorRowControl CreatePromptEditorRowControl()
	{
		return new RecordedAudioPromptEditorRowControl(containerActivity, this);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		AudioId = ExpressionHelper.RenameComponent(containerActivity, AudioId, oldValue, newValue);
	}
}

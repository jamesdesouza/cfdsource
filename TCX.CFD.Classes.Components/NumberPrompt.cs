using System;
using System.Text;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class NumberPrompt : Prompt
{
	public NumberPromptFormats Format { get; set; }

	public string Number { get; set; } = string.Empty;


	private NumberPrompt(NumberPromptFormats format, string number)
	{
		Format = format;
		Number = number;
	}

	public NumberPrompt()
	{
	}

	public override void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		compiler.Visit(this, isDebugBuild, componentName, promptCollectionName, componentsInitializationScriptSb);
	}

	public override void MigrateConstantStringExpressions()
	{
		Number = ExpressionHelper.MigrateConstantStringExpression(containerActivity, Number);
	}

	public override Prompt Clone()
	{
		return new NumberPrompt(Format, Number)
		{
			ContainerActivity = containerActivity
		};
	}

	public override AbsPromptEditorRowControl CreatePromptEditorRowControl()
	{
		return new NumberPromptEditorRowControl(containerActivity, this);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Number = ExpressionHelper.RenameComponent(containerActivity, Number, oldValue, newValue);
	}
}

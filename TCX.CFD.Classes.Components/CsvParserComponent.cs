using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(CsvParserComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(CsvParserComponentToolboxItem))]
[ToolboxBitmap(typeof(CsvParserComponent), "Resources.CsvParser.png")]
[ActivityValidator(typeof(CsvParserComponentValidator))]
public class CsvParserComponent : AbsVadActivity
{
	[Category("CSV Parser")]
	[Description("The text to parse.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Text { get; set; } = string.Empty;


	public CsvParserComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.CsvParser.ResultHelpText")
		};
		properties.Add(item);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Text = ExpressionHelper.RenameComponent(this, Text, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Text = ExpressionHelper.MigrateConstantStringExpression(this, Text);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new CsvParserComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.wmwv8m3peror");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "CsvParserComponent";
	}
}

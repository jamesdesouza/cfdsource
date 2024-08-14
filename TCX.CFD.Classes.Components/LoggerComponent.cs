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
[Designer(typeof(LoggerComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(LoggerComponentToolboxItem))]
[ToolboxBitmap(typeof(LoggerComponent), "Resources.Logger.png")]
[ActivityValidator(typeof(LoggerComponentValidator))]
public class LoggerComponent : AbsVadActivity
{
	[Category("Logger")]
	[Description("The log level for this log entry.")]
	public LogLevels Level { get; set; } = LogLevels.Info;


	[Category("Logger")]
	[Description("The text to log.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Text { get; set; } = string.Empty;


	public LoggerComponent()
	{
		InitializeComponent();
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
		return new LoggerComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.giyuwghfz906");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "LoggerComponent";
	}
}

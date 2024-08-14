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

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(LoopComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(LoopComponentToolboxItem))]
[ToolboxBitmap(typeof(LoopComponent), "Resources.Loop.png")]
[ActivityValidator(typeof(LoopComponentValidator))]
public class LoopComponent : AbsVadSequenceActivity
{
	[Category("Loop")]
	[Description("The value to assign to the condition that must be met to remain looping.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Condition { get; set; } = string.Empty;


	public LoopComponent()
	{
		InitializeComponent();
		base.Description = LocalizedResourceMgr.GetString("ComponentDesigners.LoopComponent.Tooltip");
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Condition = ExpressionHelper.RenameComponent(this, Condition, oldValue, newValue);
		base.NotifyComponentRenamed(oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Condition = ExpressionHelper.MigrateConstantStringExpression(this, Condition);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new LoopComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.xp6lijx43kjc");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "LoopComponent";
	}
}

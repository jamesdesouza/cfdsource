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
[Designer(typeof(ConditionalComponentBranchDesigner), typeof(IDesigner))]
[ToolboxBitmap(typeof(ConditionalComponentBranch), "Resources.Branch.png")]
[ActivityValidator(typeof(ConditionalComponentBranchValidator))]
public class ConditionalComponentBranch : AbsVadSequenceActivity
{
	[Category("Create a condition Branch")]
	[Description("Specify the condition that has to be met in order to execute this branch.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Condition { get; set; } = string.Empty;


	public ConditionalComponentBranch()
	{
		InitializeComponent();
		base.Description = LocalizedResourceMgr.GetString("ComponentDesigners.ConditionalComponentBranch.Tooltip");
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
		return new SequenceBranchCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.pu051wpfxdug");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "ConditionalComponentBranch";
	}
}

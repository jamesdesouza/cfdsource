using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(MenuComponentBranchDesigner), typeof(IDesigner))]
[ToolboxBitmap(typeof(MenuComponentBranch), "Resources.Branch.png")]
[ActivityValidator(typeof(MenuComponentBranchValidator))]
public class MenuComponentBranch : AbsVadSequenceActivity, IComparable
{
	[Category("Menu")]
	[Description("A friendly name for this branch.")]
	public string FriendlyName { get; set; } = "";


	[Browsable(false)]
	public MenuOptions Option { get; set; } = MenuOptions.TimeoutOrInvalidOption;


	public MenuComponentBranch()
	{
		InitializeComponent();
		base.Description = LocalizedResourceMgr.GetString("ComponentDesigners.MenuComponentBranch.Tooltip");
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new MenuComponentBranchCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.tne2kc81ot15");
	}

	public int CompareTo(object obj)
	{
		return Option.CompareTo((obj as MenuComponentBranch).Option);
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "MenuComponentBranch";
	}
}

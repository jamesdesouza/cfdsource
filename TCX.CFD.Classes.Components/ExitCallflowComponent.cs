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

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(ExitCallflowComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(ExitCallflowComponentToolboxItem))]
[ToolboxBitmap(typeof(ExitCallflowComponent), "Resources.ExitCallflow.png")]
[ActivityValidator(typeof(ExitCallflowComponentValidator))]
public class ExitCallflowComponent : AbsVadActivity
{
	public ExitCallflowComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new ExitCallflowComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.bhcv2k7t5t3d");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "ExitCallflowComponent";
	}
}

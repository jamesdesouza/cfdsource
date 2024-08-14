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
[Designer(typeof(ParallelExecutionComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(ParallelExecutionComponentToolboxItem))]
[ToolboxBitmap(typeof(ParallelExecutionComponent), "Resources.ParallelExecution.png")]
[ActivityValidator(typeof(ParallelExecutionComponentValidator))]
public class ParallelExecutionComponent : AbsVadCompositeActivity
{
	public ParallelExecutionComponent()
	{
		InitializeComponent();
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
		return new ParallelExecutionComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.z3cufivzzeue");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "ParallelExecutionComponent";
	}
}

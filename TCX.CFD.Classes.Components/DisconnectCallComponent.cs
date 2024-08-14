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
[Designer(typeof(DisconnectCallComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(DisconnectCallComponentToolboxItem))]
[ToolboxBitmap(typeof(DisconnectCallComponent), "Resources.DisconnectCall.png")]
[ActivityValidator(typeof(DisconnectCallComponentValidator))]
public class DisconnectCallComponent : AbsVadActivity
{
	public DisconnectCallComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
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
		return new DisconnectCallComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.h627hqz96424");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "DisconnectCallComponent";
	}
}

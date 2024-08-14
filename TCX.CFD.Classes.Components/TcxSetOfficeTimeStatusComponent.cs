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
[Designer(typeof(TcxSetOfficeTimeStatusComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxSetOfficeTimeStatusComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxSetOfficeTimeStatusComponent), "Resources.Write3CX.png")]
[ActivityValidator(typeof(TcxSetOfficeTimeStatusComponentValidator))]
public class TcxSetOfficeTimeStatusComponent : AbsVadActivity
{
	[Category("Set Office Time Status")]
	[Description("The office time status to set.")]
	public OfficeTimeStatus Status { get; set; } = OfficeTimeStatus.UseDefault;


	public TcxSetOfficeTimeStatusComponent()
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
		return new TcxSetOfficeTimeStatusComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.yp9fikfqbjt0");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxSetOfficeTimeStatusComponent";
	}
}

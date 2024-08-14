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
[Designer(typeof(TcxSetExtensionStatusComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxSetExtensionStatusComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxSetExtensionStatusComponent), "Resources.Write3CX.png")]
[ActivityValidator(typeof(TcxSetExtensionStatusComponentValidator))]
public class TcxSetExtensionStatusComponent : AbsVadActivity
{
	[Category("Set Extension Status")]
	[Description("The extension number for which the status will be set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Extension { get; set; } = string.Empty;


	[Category("Set Extension Status")]
	[Description("The status to set to the extension.")]
	public ExtensionStatus Status { get; set; }

	public TcxSetExtensionStatusComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Extension = ExpressionHelper.RenameComponent(this, Extension, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Extension = ExpressionHelper.MigrateConstantStringExpression(this, Extension);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxSetExtensionStatusComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.yp9fikfqbjt0");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxSetExtensionStatusComponent";
	}
}

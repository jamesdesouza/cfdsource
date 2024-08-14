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
[Designer(typeof(TcxSetQueueExtensionStatusComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxSetQueueExtensionStatusComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxSetQueueExtensionStatusComponent), "Resources.Write3CX.png")]
[ActivityValidator(typeof(TcxSetQueueExtensionStatusComponentValidator))]
public class TcxSetQueueExtensionStatusComponent : AbsVadActivity
{
	[Category("Set Queue Extension Status")]
	[Description("The extension number for which the queue status will be set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Extension { get; set; } = string.Empty;


	[Category("Set Queue Extension Status")]
	[Description("The extension number of the queue for which the status will be set. Only considered when QueueMode is SpecificQueue.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string QueueExtension { get; set; } = string.Empty;


	[Category("Set Queue Extension Status")]
	[Description("The queue status operation mode. Select SpecificQueue to set the status for a single queue, or Global to set the status for all queues.")]
	public QueueStatusOperationModes QueueMode { get; set; } = QueueStatusOperationModes.SpecificQueue;


	[Category("Set Queue Extension Status")]
	[Description("The queue status to set to the extension.")]
	public QueueStatusTypes QueueStatus { get; set; }

	public TcxSetQueueExtensionStatusComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Extension = ExpressionHelper.RenameComponent(this, Extension, oldValue, newValue);
		QueueExtension = ExpressionHelper.RenameComponent(this, QueueExtension, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Extension = ExpressionHelper.MigrateConstantStringExpression(this, Extension);
		QueueExtension = ExpressionHelper.MigrateConstantStringExpression(this, QueueExtension);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxSetQueueExtensionStatusComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.fuggmt1wmxr9");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxSetQueueExtensionStatusComponent";
	}
}

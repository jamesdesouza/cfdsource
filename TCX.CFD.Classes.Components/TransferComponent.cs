using System;
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
[Designer(typeof(TransferComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TransferComponentToolboxItem))]
[ToolboxBitmap(typeof(TransferComponent), "Resources.Transfer.png")]
[ActivityValidator(typeof(TransferComponentValidator))]
public class TransferComponent : AbsVadActivity
{
	private string destination = string.Empty;

	private bool transferToVoicemail;

	private uint delayMilliseconds = 500u;

	[Category("Transfer")]
	[Description("The destination number where the call must be transferred.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Destination
	{
		get
		{
			return destination;
		}
		set
		{
			destination = value;
		}
	}

	[Category("Transfer")]
	[Description("True to transfer the call to the voicemail of the specified extension, false otherwise.")]
	public bool TransferToVoicemail
	{
		get
		{
			return transferToVoicemail;
		}
		set
		{
			transferToVoicemail = value;
		}
	}

	[Category("Transfer")]
	[Description("The time to wait before transfering the call, considered since the start of the call, in milliseconds.")]
	public uint DelayMilliseconds
	{
		get
		{
			return delayMilliseconds;
		}
		set
		{
			if (value > 2000)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 0, 2000));
			}
			delayMilliseconds = value;
		}
	}

	public TransferComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		destination = ExpressionHelper.RenameComponent(this, destination, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		destination = ExpressionHelper.MigrateConstantStringExpression(this, destination);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TransferComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.6v6iaec8fzfp");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TransferComponent";
	}
}

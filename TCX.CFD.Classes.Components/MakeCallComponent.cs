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
[Designer(typeof(MakeCallComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(MakeCallComponentToolboxItem))]
[ToolboxBitmap(typeof(MakeCallComponent), "Resources.MakeCall.png")]
[ActivityValidator(typeof(MakeCallComponentValidator))]
public class MakeCallComponent : AbsVadActivity
{
	private uint timeout = 30u;

	[Category("MakeCall")]
	[Description("The origin number from which you want to make the call. The call will be established between the Origin and the Destination.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Origin { get; set; } = string.Empty;


	[Category("MakeCall")]
	[Description("The destination number to call. The call will be established between the Origin and the Destination.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Destination { get; set; } = string.Empty;


	[Category("MakeCall")]
	[Description("The time to wait for the call to be established, in seconds.")]
	public uint Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			if (value == 0 || value > 999)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 999));
			}
			timeout = value;
		}
	}

	public MakeCallComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.MakeCall.ResultHelpText")
		};
		properties.Add(item);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Origin = ExpressionHelper.RenameComponent(this, Origin, oldValue, newValue);
		Destination = ExpressionHelper.RenameComponent(this, Destination, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Origin = ExpressionHelper.MigrateConstantStringExpression(this, Origin);
		Destination = ExpressionHelper.MigrateConstantStringExpression(this, Destination);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new MakeCallComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.nmz1bzkxt0hq");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "MakeCallComponent";
	}
}

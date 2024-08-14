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
[Designer(typeof(TcxGetLastCallComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxGetLastCallComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxGetLastCallComponent), "Resources.Read3CX.png")]
[ActivityValidator(typeof(TcxGetLastCallComponentValidator))]
public class TcxGetLastCallComponent : AbsVadActivity
{
	[Category("Get Last Call")]
	[Description("The number to query the last call for.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Number { get; set; } = string.Empty;


	[Category("Get Last Call")]
	[Description("The direction of the call to search, relative to the number. Select Inbound to search for incoming calls to the specified number, i.e. where the Number is the Destination of the call. Select Outbound to search for outgoing calls from the specified number, i.e. where the Number is the Origin of the call. Select Both to search for any calls from or to the specified number.")]
	public CallDirections CallType { get; set; } = CallDirections.Both;


	public TcxGetLastCallComponent()
	{
		InitializeComponent();
		Variable item = new Variable("CallFound", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetLastCall.CallFoundHelpText")
		};
		Variable item2 = new Variable("CallAnswered", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetLastCall.CallAnsweredHelpText")
		};
		Variable item3 = new Variable("CallDateTime", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetLastCall.CallDateTimeHelpText")
		};
		Variable item4 = new Variable("CallDuration", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetLastCall.CallDurationHelpText")
		};
		Variable item5 = new Variable("Counterpart", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetLastCall.CounterpartHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
		properties.Add(item3);
		properties.Add(item4);
		properties.Add(item5);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Number = ExpressionHelper.RenameComponent(this, Number, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Number = ExpressionHelper.MigrateConstantStringExpression(this, Number);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxGetLastCallComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.yp9fikfqbjt0");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxGetLastCallComponent";
	}
}

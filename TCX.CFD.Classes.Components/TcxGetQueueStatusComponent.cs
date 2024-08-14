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
[Designer(typeof(TcxGetQueueStatusComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxGetQueueStatusComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxGetQueueStatusComponent), "Resources.Read3CX.png")]
[ActivityValidator(typeof(TcxGetQueueStatusComponentValidator))]
public class TcxGetQueueStatusComponent : AbsVadActivity
{
	[Category("Get Queue Status")]
	[Description("The extension number of the queue to operate with.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string QueueExtension { get; set; } = string.Empty;


	public TcxGetQueueStatusComponent()
	{
		InitializeComponent();
		Variable item = new Variable("CallsWaiting", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetQueueStatus.CallsWaitingHelpText")
		};
		Variable item2 = new Variable("BusyAgents", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetQueueStatus.BusyAgentsHelpText")
		};
		Variable item3 = new Variable("AvailableAgents", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetQueueStatus.AvailableAgentsHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
		properties.Add(item3);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		QueueExtension = ExpressionHelper.RenameComponent(this, QueueExtension, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		QueueExtension = ExpressionHelper.MigrateConstantStringExpression(this, QueueExtension);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxGetQueueStatusComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.fkx9wii4cq27");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxGetQueueStatusComponent";
	}
}

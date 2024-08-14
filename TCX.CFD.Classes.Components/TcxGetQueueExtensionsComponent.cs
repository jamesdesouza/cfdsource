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
[Designer(typeof(TcxGetQueueExtensionsComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxGetQueueExtensionsComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxGetQueueExtensionsComponent), "Resources.Read3CX.png")]
[ActivityValidator(typeof(TcxGetQueueExtensionsComponentValidator))]
public class TcxGetQueueExtensionsComponent : AbsVadActivity
{
	[Category("Get Queue Extensions")]
	[Description("The extension number of the queue to operate with.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string QueueExtension { get; set; } = string.Empty;


	[Category("Get Queue Extensions")]
	[Description("The type of query to execute.")]
	public QueueQueryTypes QueueQueryType { get; set; }

	public TcxGetQueueExtensionsComponent()
	{
		InitializeComponent();
		Variable item = new Variable("ExtensionList", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetQueueExtensions.ExtensionListHelpText")
		};
		Variable item2 = new Variable("ExtensionNumberList", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetQueueExtensions.ExtensionNumberListHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
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
		return new TcxGetQueueExtensionsComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.iu8vhnbz548w");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxGetQueueExtensionsComponent";
	}
}

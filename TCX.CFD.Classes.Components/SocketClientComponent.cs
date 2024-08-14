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
[Designer(typeof(SocketClientComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(SocketClientComponentToolboxItem))]
[ToolboxBitmap(typeof(SocketClientComponent), "Resources.SocketClient.png")]
[ActivityValidator(typeof(SocketClientComponentValidator))]
public class SocketClientComponent : AbsVadActivity
{
	[Category("Open a socket")]
	[Description("The type of the connection to establish.")]
	public SocketConnectionTypes ConnectionType { get; set; }

	[Category("Open a socket")]
	[Description("The server host name or IP address.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Host { get; set; } = string.Empty;


	[Category("Open a socket")]
	[Description("The port number where the server is listening for incoming connections.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Port { get; set; } = string.Empty;


	[Category("Open a socket")]
	[Description("The data to send to the server.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Data { get; set; } = string.Empty;


	[Category("Open a socket")]
	[Description("True to wait for a response after sending the data. False otherwise.")]
	public bool WaitForResponse { get; set; }

	public SocketClientComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Response", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.SocketClient.ResponseHelpText")
		};
		properties.Add(item);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Host = ExpressionHelper.RenameComponent(this, Host, oldValue, newValue);
		Port = ExpressionHelper.RenameComponent(this, Port, oldValue, newValue);
		Data = ExpressionHelper.RenameComponent(this, Data, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Host = ExpressionHelper.MigrateConstantStringExpression(this, Host);
		Port = ExpressionHelper.MigrateConstantStringExpression(this, Port);
		Data = ExpressionHelper.MigrateConstantStringExpression(this, Data);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new SocketClientComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.jj5beod5m46o");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "SocketClientComponent";
	}
}

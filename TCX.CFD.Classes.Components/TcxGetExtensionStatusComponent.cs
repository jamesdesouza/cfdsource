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
[Designer(typeof(TcxGetExtensionStatusComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxGetExtensionStatusComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxGetExtensionStatusComponent), "Resources.Read3CX.png")]
[ActivityValidator(typeof(TcxGetExtensionStatusComponentValidator))]
public class TcxGetExtensionStatusComponent : AbsVadActivity
{
	[Category("Get Extension Status")]
	[Description("The extension number for which the status will be retrieved.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Extension { get; set; } = string.Empty;


	public TcxGetExtensionStatusComponent()
	{
		InitializeComponent();
		Variable item = new Variable("IsInCall", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetExtensionStatus.IsInCallHelpText")
		};
		Variable item2 = new Variable("CurrentProfile", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetExtensionStatus.CurrentProfileHelpText")
		};
		Variable item3 = new Variable("CurrentProfileName", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetExtensionStatus.CurrentProfileNameHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
		properties.Add(item3);
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
		return new TcxGetExtensionStatusComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.q47ba0ftg4qe");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxGetExtensionStatusComponent";
	}
}

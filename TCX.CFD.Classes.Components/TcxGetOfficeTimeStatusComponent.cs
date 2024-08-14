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
[Designer(typeof(TcxGetOfficeTimeStatusComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxGetOfficeTimeStatusComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxGetOfficeTimeStatusComponent), "Resources.Read3CX.png")]
[ActivityValidator(typeof(TcxGetOfficeTimeStatusComponentValidator))]
public class TcxGetOfficeTimeStatusComponent : AbsVadActivity
{
	public TcxGetOfficeTimeStatusComponent()
	{
		InitializeComponent();
		Variable item = new Variable("IsForced", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetOfficeTimeStatus.IsForcedHelpText")
		};
		Variable item2 = new Variable("Status", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetOfficeTimeStatus.StatusHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
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
		return new TcxGetOfficeTimeStatusComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.q47ba0ftg4qe");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxGetOfficeTimeStatusComponent";
	}
}

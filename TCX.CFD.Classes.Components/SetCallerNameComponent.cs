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
[Designer(typeof(SetCallerNameComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(SetCallerNameComponentToolboxItem))]
[ToolboxBitmap(typeof(SetCallerNameComponent), "Resources.SetCallerName.png")]
[ActivityValidator(typeof(SetCallerNameComponentValidator))]
public class SetCallerNameComponent : AbsVadActivity
{
	private string displayName = string.Empty;

	[Category("Set Caller Name")]
	[Description("The caller’s display name to attach to the call.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string DisplayName
	{
		get
		{
			return displayName;
		}
		set
		{
			displayName = value;
		}
	}

	public SetCallerNameComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		displayName = ExpressionHelper.RenameComponent(this, displayName, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		displayName = ExpressionHelper.MigrateConstantStringExpression(this, displayName);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new SetCallerNameComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.6v6iaec8fzfp");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "SetCallerNameComponent";
	}
}

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
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(IncrementVariableComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(IncrementVariableComponentToolboxItem))]
[ToolboxBitmap(typeof(IncrementVariableComponent), "Resources.IncrementVariable.png")]
[ActivityValidator(typeof(IncrementVariableComponentValidator))]
public class IncrementVariableComponent : AbsVadActivity
{
	[Category("Increment Variable")]
	[Description("The name of the variable to increment.")]
	[Editor(typeof(LeftHandSideVariableEditor), typeof(UITypeEditor))]
	public string VariableName { get; set; } = string.Empty;


	public IncrementVariableComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		if (VariableName.StartsWith(oldValue + "."))
		{
			VariableName = newValue + VariableName.Substring(oldValue.Length);
		}
		else if (VariableName == oldValue)
		{
			VariableName = newValue;
		}
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
		return new IncrementVariableComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.i96ock6atx20");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "IncrementVariableComponent";
	}
}

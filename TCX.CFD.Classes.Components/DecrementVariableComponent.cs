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
[Designer(typeof(DecrementVariableComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(DecrementVariableComponentToolboxItem))]
[ToolboxBitmap(typeof(DecrementVariableComponent), "Resources.DecrementVariable.png")]
[ActivityValidator(typeof(DecrementVariableComponentValidator))]
public class DecrementVariableComponent : AbsVadActivity
{
	[Category("Decrement Variable")]
	[Description("The name of the variable to decrement.")]
	[Editor(typeof(LeftHandSideVariableEditor), typeof(UITypeEditor))]
	public string VariableName { get; set; } = string.Empty;


	public DecrementVariableComponent()
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
		return new DecrementVariableComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.gpo7al17e91x");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "DecrementVariableComponent";
	}
}

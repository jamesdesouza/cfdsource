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
[Designer(typeof(VariableAssignmentComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(VariableAssignmentComponentToolboxItem))]
[ToolboxBitmap(typeof(VariableAssignmentComponent), "Resources.VariableAssignment.png")]
[ActivityValidator(typeof(VariableAssignmentComponentValidator))]
public class VariableAssignmentComponent : AbsVadActivity
{
	[Category("Assign a variable")]
	[Description("The name of the variable.")]
	[Editor(typeof(LeftHandSideVariableEditor), typeof(UITypeEditor))]
	public string VariableName { get; set; } = string.Empty;


	[Category("Assign a variable")]
	[Description("The value to assign to the variable.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Expression { get; set; } = string.Empty;


	public VariableAssignmentComponent()
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
		Expression = ExpressionHelper.RenameComponent(this, Expression, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Expression = ExpressionHelper.MigrateConstantStringExpression(this, Expression);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new VariableAssignmentComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.jtnrm4nd9db3");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "VariableAssignmentComponent";
	}
}

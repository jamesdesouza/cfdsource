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
[Designer(typeof(TcxSetDnPropertyComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxSetDnPropertyComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxSetDnPropertyComponent), "Resources.Write3CX.png")]
[ActivityValidator(typeof(TcxSetDnPropertyComponentValidator))]
public class TcxSetDnPropertyComponent : AbsVadActivity
{
	[Category("Set DN Property")]
	[Description("The DN number for which the property will be set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Extension { get; set; } = string.Empty;


	[Category("Set DN Property")]
	[Description("The name of the property to set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string PropertyName { get; set; } = string.Empty;


	[Category("Set DN Property")]
	[Description("The value of the property to set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string PropertyValue { get; set; } = string.Empty;


	public TcxSetDnPropertyComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Extension = ExpressionHelper.RenameComponent(this, Extension, oldValue, newValue);
		PropertyName = ExpressionHelper.RenameComponent(this, PropertyName, oldValue, newValue);
		PropertyValue = ExpressionHelper.RenameComponent(this, PropertyValue, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Extension = ExpressionHelper.MigrateConstantStringExpression(this, Extension);
		PropertyName = ExpressionHelper.MigrateConstantStringExpression(this, PropertyName);
		PropertyValue = ExpressionHelper.MigrateConstantStringExpression(this, PropertyValue);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxSetDnPropertyComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.6z79d6q3esl");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxSetDnPropertyComponent";
	}
}

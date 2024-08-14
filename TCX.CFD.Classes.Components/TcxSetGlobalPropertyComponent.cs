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
[Designer(typeof(TcxSetGlobalPropertyComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxSetGlobalPropertyComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxSetGlobalPropertyComponent), "Resources.Write3CX.png")]
[ActivityValidator(typeof(TcxSetGlobalPropertyComponentValidator))]
public class TcxSetGlobalPropertyComponent : AbsVadActivity
{
	[Category("Set Global Property")]
	[Description("The name of the property to set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string PropertyName { get; set; } = string.Empty;


	[Category("Set Global Property")]
	[Description("The value of the property to set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string PropertyValue { get; set; } = string.Empty;


	public TcxSetGlobalPropertyComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		PropertyName = ExpressionHelper.RenameComponent(this, PropertyName, oldValue, newValue);
		PropertyValue = ExpressionHelper.RenameComponent(this, PropertyValue, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		PropertyName = ExpressionHelper.MigrateConstantStringExpression(this, PropertyName);
		PropertyValue = ExpressionHelper.MigrateConstantStringExpression(this, PropertyValue);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxSetGlobalPropertyComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.qmh8r5z17zi7");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxSetGlobalPropertyComponent";
	}
}

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
[Designer(typeof(AttachCallDataComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(AttachCallDataComponentToolboxItem))]
[ToolboxBitmap(typeof(AttachCallDataComponent), "Resources.AttachCallData.png")]
[ActivityValidator(typeof(AttachCallDataComponentValidator))]
public class AttachCallDataComponent : AbsVadActivity
{
	[Category("Attach Call Data")]
	[Description("The name of the data to set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string DataName { get; set; } = string.Empty;


	[Category("Attach Call Data")]
	[Description("The value of the data to set.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string DataValue { get; set; } = string.Empty;


	public AttachCallDataComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		DataName = ExpressionHelper.RenameComponent(this, DataName, oldValue, newValue);
		DataValue = ExpressionHelper.RenameComponent(this, DataValue, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		DataName = ExpressionHelper.MigrateConstantStringExpression(this, DataName);
		DataValue = ExpressionHelper.MigrateConstantStringExpression(this, DataValue);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new AttachCallDataComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.o8sixnp0g706");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "AttachCallDataComponent";
	}
}

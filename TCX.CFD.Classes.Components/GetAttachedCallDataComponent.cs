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
[Designer(typeof(GetAttachedCallDataComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(GetAttachedCallDataComponentToolboxItem))]
[ToolboxBitmap(typeof(GetAttachedCallDataComponent), "Resources.GetAttachedCallData.png")]
[ActivityValidator(typeof(GetAttachedCallDataComponentValidator))]
public class GetAttachedCallDataComponent : AbsVadActivity
{
	[Category("Get Attached Call Data")]
	[Description("The name of the data to retrieve.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string DataName { get; set; } = string.Empty;


	public GetAttachedCallDataComponent()
	{
		InitializeComponent();
		Variable item = new Variable("DataValue", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.GetAttachedCallData.DataValueHelpText")
		};
		properties.Add(item);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		DataName = ExpressionHelper.RenameComponent(this, DataName, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		DataName = ExpressionHelper.MigrateConstantStringExpression(this, DataName);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new GetAttachedCallDataComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.yr9y1ppx7pog");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "GetAttachedCallDataComponent";
	}
}

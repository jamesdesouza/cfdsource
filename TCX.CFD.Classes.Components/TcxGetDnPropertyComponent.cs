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
[Designer(typeof(TcxGetDnPropertyComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxGetDnPropertyComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxGetDnPropertyComponent), "Resources.Read3CX.png")]
[ActivityValidator(typeof(TcxGetDnPropertyComponentValidator))]
public class TcxGetDnPropertyComponent : AbsVadActivity
{
	[Category("Get DN Property")]
	[Description("The DN number for which the property will be retrieved.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Extension { get; set; } = string.Empty;


	[Category("Get DN Property")]
	[Description("The name of the property to retrieve.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string PropertyName { get; set; } = string.Empty;


	public TcxGetDnPropertyComponent()
	{
		InitializeComponent();
		Variable item = new Variable("PropertyValue", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetDnProperty.PropertyValueHelpText")
		};
		properties.Add(item);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Extension = ExpressionHelper.RenameComponent(this, Extension, oldValue, newValue);
		PropertyName = ExpressionHelper.RenameComponent(this, PropertyName, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Extension = ExpressionHelper.MigrateConstantStringExpression(this, Extension);
		PropertyName = ExpressionHelper.MigrateConstantStringExpression(this, PropertyName);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxGetDnPropertyComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.1qvnvylpx4nt");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxGetDnPropertyComponent";
	}
}

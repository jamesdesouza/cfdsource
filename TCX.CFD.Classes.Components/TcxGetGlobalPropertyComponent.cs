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
[Designer(typeof(TcxGetGlobalPropertyComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxGetGlobalPropertyComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxGetGlobalPropertyComponent), "Resources.Read3CX.png")]
[ActivityValidator(typeof(TcxGetGlobalPropertyComponentValidator))]
public class TcxGetGlobalPropertyComponent : AbsVadActivity
{
	[Category("Get Global Property")]
	[Description("The name of the property to retrieve.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string PropertyName { get; set; } = string.Empty;


	public TcxGetGlobalPropertyComponent()
	{
		InitializeComponent();
		Variable item = new Variable("PropertyValue", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxGetGlobalProperty.PropertyValueHelpText")
		};
		properties.Add(item);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		PropertyName = ExpressionHelper.RenameComponent(this, PropertyName, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		PropertyName = ExpressionHelper.MigrateConstantStringExpression(this, PropertyName);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxGetGlobalPropertyComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.2xhp8q9xqoin");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxGetGlobalPropertyComponent";
	}
}

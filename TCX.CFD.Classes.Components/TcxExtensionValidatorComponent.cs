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
[Designer(typeof(TcxExtensionValidatorComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TcxExtensionValidatorComponentToolboxItem))]
[ToolboxBitmap(typeof(TcxExtensionValidatorComponent), "Resources.ExtensionValidator.png")]
[ActivityValidator(typeof(TcxExtensionValidatorComponentValidator))]
public class TcxExtensionValidatorComponent : AbsVadActivity
{
	[Category("Extension Validator")]
	[Description("The extension number to validate.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Extension { get; set; } = string.Empty;


	public TcxExtensionValidatorComponent()
	{
		InitializeComponent();
		Variable item = new Variable("IsValid", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxExtensionValidator.IsValidHelpText")
		};
		Variable item2 = new Variable("Type", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TcxExtensionValidator.TypeHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Extension = ExpressionHelper.RenameComponent(this, Extension, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Extension = ExpressionHelper.MigrateConstantStringExpression(this, Extension);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TcxExtensionValidatorComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.q47ba0ftg4qe");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TcxExtensionValidatorComponent";
	}
}

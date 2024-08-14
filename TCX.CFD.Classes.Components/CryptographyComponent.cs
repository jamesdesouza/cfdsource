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
[Designer(typeof(CryptographyComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(CryptographyComponentToolboxItem))]
[ToolboxBitmap(typeof(CryptographyComponent), "Resources.Cryptography.png")]
[ActivityValidator(typeof(CryptographyComponentValidator))]
public class CryptographyComponent : AbsVadActivity
{
	[Category("Encryption")]
	[Description("The algorithm to use.")]
	public CryptographyAlgorithms Algorithm { get; set; }

	[Category("Encryption")]
	[Description("The codification format to use in order to transform the encrypted stream into text. When encrypting data, this is how the result is codified. When decrypting data, this is how the input text arrives.")]
	public CodificationFormats Format { get; set; }

	[Category("Encryption")]
	[Description("The action to perform. Only valid when Algorithm is TripleDES.")]
	public CryptographyActions Action { get; set; }

	[Category("Encryption")]
	[Description("The secret key to be used for the symmetric algorithm. Only valid when Algorithm is TripleDES. It must have a length of 24 characters for TripleDES.")]
	public string Key { get; set; } = string.Empty;


	[Category("Encryption")]
	[Description("The text to encrypt, decrypt or compute hash.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Text { get; set; } = string.Empty;


	public CryptographyComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Cryptography.ResultHelpText")
		};
		properties.Add(item);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Text = ExpressionHelper.RenameComponent(this, Text, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Text = ExpressionHelper.MigrateConstantStringExpression(this, Text);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new CryptographyComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.4l3as5en9xt3");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "CryptographyComponent";
	}
}

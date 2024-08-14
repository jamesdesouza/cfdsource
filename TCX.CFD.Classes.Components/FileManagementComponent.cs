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
[Designer(typeof(FileManagementComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(FileManagementComponentToolboxItem))]
[ToolboxBitmap(typeof(FileManagementComponent), "Resources.FileManagement.png")]
[ActivityValidator(typeof(FileManagementComponentValidator))]
public class FileManagementComponent : AbsVadActivity
{
	[Category("Read / Write to File")]
	[Description("Use Append to append data at the end of the file when Action is Write. Use Create to open a new file or truncate an existing one. Use Open to open an existing file.")]
	public FileManagementOpenModes OpenMode { get; set; }

	[Category("Read / Write to File")]
	[Description("The action to perform.")]
	public FileManagementActions Action { get; set; } = FileManagementActions.Write;


	[Category("Read / Write to File")]
	[Description("The full path to the file to read or write.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string FileName { get; set; } = string.Empty;


	[Category("Read / Write to File")]
	[Description("The text to write to the file. Only valid when Action is Write.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Content { get; set; } = string.Empty;


	[Category("Read / Write to File")]
	[Description("True to append a final CR LF after writing to the file. False otherwise. Only valid when Action is Write.")]
	public bool AppendFinalCrLf { get; set; } = true;


	[Category("Read / Write to File")]
	[Description("The number of lines to read from the file. Only valid when Action is Read.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string LinesToRead { get; set; } = string.Empty;


	[Category("Read / Write to File")]
	[Description("The zero-based index of the first line to read from the file. Only valid when Action is Read.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string FirstLineToRead { get; set; } = "0";


	[Category("Read / Write to File")]
	[Description("Overrides LinesToRead, reading until the end of file is reached. Only valid when Action is Read.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string ReadToEnd { get; set; } = "true";


	public FileManagementComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.FileManagement.ResultHelpText")
		};
		Variable item2 = new Variable("EofReached", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.FileManagement.EofReachedHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		FileName = ExpressionHelper.RenameComponent(this, FileName, oldValue, newValue);
		Content = ExpressionHelper.RenameComponent(this, Content, oldValue, newValue);
		LinesToRead = ExpressionHelper.RenameComponent(this, LinesToRead, oldValue, newValue);
		FirstLineToRead = ExpressionHelper.RenameComponent(this, FirstLineToRead, oldValue, newValue);
		ReadToEnd = ExpressionHelper.RenameComponent(this, ReadToEnd, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		FileName = ExpressionHelper.MigrateConstantStringExpression(this, FileName);
		Content = ExpressionHelper.MigrateConstantStringExpression(this, Content);
		LinesToRead = ExpressionHelper.MigrateConstantStringExpression(this, LinesToRead);
		FirstLineToRead = ExpressionHelper.MigrateConstantStringExpression(this, FirstLineToRead);
		ReadToEnd = ExpressionHelper.MigrateConstantStringExpression(this, ReadToEnd);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new FileManagementComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.80dqkqk2c2m5");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "FileManagementComponent";
	}
}

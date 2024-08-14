using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(ComponentBranchDesigner), typeof(IDesigner))]
[ToolboxBitmap(typeof(ComponentBranch), "Resources.Branch.png")]
[ActivityValidator(typeof(ComponentBranchValidator))]
public class ComponentBranch : AbsVadSequenceActivity
{
	[Browsable(false)]
	public string DisplayedText { get; set; } = string.Empty;


	public ComponentBranch()
	{
		InitializeComponent();
		base.Description = LocalizedResourceMgr.GetString("ComponentDesigners.ComponentBranch.Tooltip");
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new ComponentBranchCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		if (base.Parent is AuthenticationComponent)
		{
			Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.rn05m5j1kxb2");
		}
		else if (base.Parent is CreditCardComponent)
		{
			Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.7ktrrs8xo913");
		}
		else if (base.Parent is RecordComponent)
		{
			Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.fzb4ldahvlii");
		}
		else if (base.Parent is UserInputComponent)
		{
			Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.uj9rte6to4je");
		}
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "ComponentBranch";
	}
}

using System;
using System.ComponentModel;
using System.Windows.Forms;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Compiler;

public class UICompilerResultCollector : AbsCompilerResultCollector
{
	private readonly BackgroundWorker buildBackgroundWorker;

	private readonly Control parentControl;

	public override bool CancellationPending => buildBackgroundWorker.CancellationPending;

	public UICompilerResultCollector(BackgroundWorker buildBackgroundWorker, Control parentControl)
	{
		this.buildBackgroundWorker = buildBackgroundWorker;
		this.parentControl = parentControl;
	}

	public override void ReportProgress(int progress, CompilerEvent compilerEvent)
	{
		buildBackgroundWorker.ReportProgress(progress, compilerEvent);
	}

	public override bool AskForExtension(ProjectObject projectObject)
	{
		ProjectExtensionForm projectExtensionForm = new ProjectExtensionForm(projectObject);
		return (bool)parentControl.Invoke((Func<bool>)(() => projectExtensionForm.ShowDialog() == DialogResult.OK));
	}
}

using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public abstract class AbsCompilerResultCollector
{
	public abstract bool CancellationPending { get; }

	public abstract void ReportProgress(int progress, CompilerEvent compilerEvent);

	public abstract bool AskForExtension(ProjectObject projectObject);
}

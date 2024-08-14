using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class DialerFileCompiler : FileCompiler
{
	public DialerFileCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
		: base(compilerResultCollector, fileObject, progress, errorCounter)
	{
		ProjectObject projectObject = fileObject.GetProjectObject();
		AddVariables("variableMap", "project$", projectObject.Variables, systemVariable: false, extraTab: false);
		AddVariables("variableMap", "callflow$", fileObject.Variables, systemVariable: false, extraTab: false);
	}
}

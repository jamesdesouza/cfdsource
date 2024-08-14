using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class CallflowFileCompiler : FileCompiler
{
	public CallflowFileCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
		: base(compilerResultCollector, fileObject, progress, errorCounter)
	{
		ProjectObject projectObject = fileObject.GetProjectObject();
		AddVariables("variableMap", "project$", projectObject.Variables, systemVariable: false, extraTab: false);
		AddVariables("variableMap", "callflow$", fileObject.Variables, systemVariable: false, extraTab: false);
		AddVariables("variableMap", "RecordResult", projectObject.RecordResultConstantList, systemVariable: true, extraTab: false);
		AddVariables("variableMap", "MenuResult", projectObject.MenuResultConstantList, systemVariable: true, extraTab: false);
		AddVariables("variableMap", "UserInputResult", projectObject.UserInputResultConstantList, systemVariable: true, extraTab: false);
		AddVariables("variableMap", "VoiceInputResult", projectObject.VoiceInputResultConstantList, systemVariable: true, extraTab: false);
	}
}

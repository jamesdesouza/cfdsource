using System.Collections.ObjectModel;
using System.Text;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class SequenceBranchCompiler : AbsComponentCompiler
{
	private readonly AbsVadSequenceActivity sequenceActivity;

	public SequenceBranchCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, AbsVadSequenceActivity sequenceActivity)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(sequenceActivity), sequenceActivity.GetRootFlow().FlowType, sequenceActivity)
	{
		this.sequenceActivity = sequenceActivity;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		ReadOnlyCollection<Activity> enabledActivities = sequenceActivity.EnabledActivities;
		if (enabledActivities.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.SequenceBranch.BranchIsEmpty"), sequenceActivity.Name, sequenceActivity.Parent.Name), fileObject, flowType, sequenceActivity);
		}
		foreach (IVadActivity item in enabledActivities)
		{
			if (item.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, parentComponentListName, componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
			{
				return CompilationResult.Cancelled;
			}
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

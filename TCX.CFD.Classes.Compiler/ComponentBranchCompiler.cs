using System.Collections.ObjectModel;
using System.Text;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class ComponentBranchCompiler : AbsComponentCompiler
{
	private readonly ComponentBranch componentBranch;

	public ComponentBranchCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, ComponentBranch componentBranch)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(componentBranch), componentBranch.GetRootFlow().FlowType, componentBranch)
	{
		this.componentBranch = componentBranch;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		ReadOnlyCollection<Activity> enabledActivities = componentBranch.EnabledActivities;
		if (enabledActivities.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.ComponentBranch.BranchIsEmpty"), componentBranch.DisplayedText, componentBranch.Parent.Name), fileObject, flowType, componentBranch);
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

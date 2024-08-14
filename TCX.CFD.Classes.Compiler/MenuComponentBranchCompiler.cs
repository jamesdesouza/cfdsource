using System.Collections.ObjectModel;
using System.Text;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class MenuComponentBranchCompiler : AbsComponentCompiler
{
	private readonly MenuComponentBranch menuComponentBranch;

	public MenuComponentBranchCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, MenuComponentBranch menuComponentBranch)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(menuComponentBranch), menuComponentBranch.GetRootFlow().FlowType, menuComponentBranch)
	{
		this.menuComponentBranch = menuComponentBranch;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		ReadOnlyCollection<Activity> enabledActivities = menuComponentBranch.EnabledActivities;
		if (enabledActivities.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.BranchIsEmpty"), menuComponentBranch.Option, menuComponentBranch.Parent.Name), fileObject, flowType, menuComponentBranch);
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

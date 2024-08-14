using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class ParallelExecutionComponentCompiler : AbsComponentCompiler
{
	private readonly ParallelExecutionComponent parallelExecutionComponent;

	public ParallelExecutionComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, ParallelExecutionComponent parallelExecutionComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(parallelExecutionComponent), parallelExecutionComponent.GetRootFlow().FlowType, parallelExecutionComponent)
	{
		this.parallelExecutionComponent = parallelExecutionComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (parallelExecutionComponent.EnabledActivities.Count < 2)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ParallelExecutionComponent.InvalidBranchCount"), parallelExecutionComponent.Name), fileObject, flowType, parallelExecutionComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("ParallelExecutionComponent {0} = new ParallelExecutionComponent(\"{0}\", callflow, myCall, logHeader);", parallelExecutionComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, parallelExecutionComponent.Name).AppendLine().Append("            ");
			ProjectObject projectObject = fileObject.GetProjectObject();
			int num = 0;
			for (int i = 0; i < parallelExecutionComponent.EnabledActivities.Count; i++)
			{
				ParallelExecutionComponentBranch parallelExecutionComponentBranch = parallelExecutionComponent.EnabledActivities[i] as ParallelExecutionComponentBranch;
				if (i > 0 && !ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(projectObject, parallelExecutionComponentBranch.EnabledActivities))
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ParallelExecutionComponent.CallRelatedComponentsNotAllowed"), parallelExecutionComponentBranch.Name, parallelExecutionComponent.Name), fileObject, flowType, parallelExecutionComponent);
				}
				else if (parallelExecutionComponentBranch.EnabledActivities.Count == 0)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ParallelExecutionComponent.BranchIsEmpty"), parallelExecutionComponentBranch.Name, parallelExecutionComponent.Name), fileObject, flowType, parallelExecutionComponent);
				}
				else
				{
					componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{1}\", callflow, myCall, logHeader));", parallelExecutionComponent.Name, parallelExecutionComponentBranch.Name).AppendLine().Append("            ");
					if (parallelExecutionComponentBranch.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{parallelExecutionComponent.Name}.ContainerList[{i}].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
					{
						return CompilationResult.Cancelled;
					}
				}
				if (ComponentDesignerHelper.IsMakeCallComponentUsed(projectObject, parallelExecutionComponentBranch.EnabledActivities))
				{
					num++;
				}
			}
			if (num > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ParallelExecutionComponent.MakeCallInMoreThanOneBranch"), parallelExecutionComponent.Name), fileObject, flowType, parallelExecutionComponent);
			}
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

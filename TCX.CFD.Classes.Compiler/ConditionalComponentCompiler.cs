using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class ConditionalComponentCompiler : AbsComponentCompiler
{
	private readonly ConditionalComponent conditionalComponent;

	public ConditionalComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, ConditionalComponent conditionalComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(conditionalComponent), conditionalComponent.GetRootFlow().FlowType, conditionalComponent)
	{
		this.conditionalComponent = conditionalComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (conditionalComponent.EnabledActivities.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ConditionalComponent.NoBranches"), conditionalComponent.Name), fileObject, flowType, conditionalComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", conditionalComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, conditionalComponent.Name).AppendLine().Append("            ");
			for (int i = 0; i < conditionalComponent.EnabledActivities.Count; i++)
			{
				ConditionalComponentBranch conditionalComponentBranch = conditionalComponent.EnabledActivities[i] as ConditionalComponentBranch;
				string condition = conditionalComponentBranch.Condition;
				bool flag = i == conditionalComponent.EnabledActivities.Count - 1;
				if (string.IsNullOrEmpty(condition) && !flag)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ConditionalComponent.ConditionEmpty"), conditionalComponentBranch.Name, conditionalComponent.Name), fileObject, flowType, conditionalComponent);
					continue;
				}
				AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, string.IsNullOrEmpty(condition) ? "true" : condition);
				if (!absArgument.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Condition", conditionalComponentBranch.Name, condition), fileObject, flowType, conditionalComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return Convert.ToBoolean({1}); }});", conditionalComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{1}\", callflow, myCall, logHeader));", conditionalComponent.Name, conditionalComponentBranch.Name).AppendLine().Append("            ");
				if (conditionalComponentBranch.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{conditionalComponent.Name}.ContainerList[{i}].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
				{
					return CompilationResult.Cancelled;
				}
			}
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

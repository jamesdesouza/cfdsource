using System.Collections.ObjectModel;
using System.Text;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class LoopComponentCompiler : AbsComponentCompiler
{
	private readonly LoopComponent loopComponent;

	public LoopComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, LoopComponent loopComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(loopComponent), loopComponent.GetRootFlow().FlowType, loopComponent)
	{
		this.loopComponent = loopComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		ReadOnlyCollection<Activity> enabledActivities = loopComponent.EnabledActivities;
		if (enabledActivities.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.LoopComponent.NoChildren"), loopComponent.Name), fileObject, flowType, loopComponent);
		}
		string condition = loopComponent.Condition;
		if (string.IsNullOrEmpty(condition))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.LoopComponent.ConditionEmpty"), loopComponent.Name), fileObject, flowType, loopComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, condition);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Condition", loopComponent.Name, condition), fileObject, flowType, loopComponent);
			}
			componentsInitializationScriptSb.AppendFormat("LoopComponent {0} = new LoopComponent(\"{0}\", callflow, myCall, logHeader);", loopComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Condition = () => {{ return Convert.ToBoolean({1}); }};", loopComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Container = new SequenceContainerComponent(\"{0}_Container\", callflow, myCall, logHeader);", loopComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, loopComponent.Name).AppendLine().Append("            ");
			foreach (IVadActivity item in enabledActivities)
			{
				if (item.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{loopComponent.Name}.Container.ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
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

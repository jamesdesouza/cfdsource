using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class RecordComponentCompiler : AbsComponentCompiler
{
	private readonly RecordComponent recordComponent;

	public RecordComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, RecordComponent recordComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(recordComponent), recordComponent.GetRootFlow().FlowType, recordComponent)
	{
		this.recordComponent = recordComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (recordComponent.EnabledActivities.Count != 2)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordComponent.InvalidBranchCount"), recordComponent.Name), fileObject, flowType, recordComponent);
		}
		else if (string.IsNullOrEmpty(recordComponent.SaveToFile))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordComponent.SaveToFileIsEmpty"), recordComponent.Name), fileObject, flowType, recordComponent);
		}
		else if (recordComponent.SaveToFile == "true" && string.IsNullOrEmpty(recordComponent.FileName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordComponent.FileNameIsEmpty"), recordComponent.Name), fileObject, flowType, recordComponent);
		}
		else if (recordComponent.Prompts.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordComponent.NoPrompts"), recordComponent.Name), fileObject, flowType, recordComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, string.IsNullOrEmpty(recordComponent.FileName) ? "\"\"" : recordComponent.FileName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "FileName", recordComponent.Name, recordComponent.FileName), fileObject, flowType, recordComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, recordComponent.SaveToFile);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "SaveToFile", recordComponent.Name, recordComponent.SaveToFile), fileObject, flowType, recordComponent);
			}
			if (recordComponent.Beep)
			{
				audioFileCollector.IncludeBeepFile = true;
			}
			componentsInitializationScriptSb.AppendFormat("RecordComponent {0} = new RecordComponent(\"{0}\", callflow, myCall, logHeader);", recordComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Beep = {1};", recordComponent.Name, recordComponent.Beep ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxTime = {1}000;", recordComponent.Name, recordComponent.MaxTime).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.TerminateByDtmf = {1};", recordComponent.Name, recordComponent.TerminateByDtmf ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FileNameHandler = () => {{ return Convert.ToString({1}); }};", recordComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.SaveToFileHandler = () => {{ return Convert.ToBoolean({1}); }};", recordComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			foreach (Prompt prompt in recordComponent.Prompts)
			{
				prompt.Accept(this, isDebugBuild, recordComponent.Name, "Prompts", componentsInitializationScriptSb, audioFileCollector);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, recordComponent.Name).AppendLine().Append("            ");
			string text = recordComponent.Name + "_Conditional";
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, text).AppendLine().Append("            ");
			ComponentBranch obj = recordComponent.EnabledActivities[0] as ComponentBranch;
			AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, $"{recordComponent.Name}.Result == RecordComponent.RecordResults.NothingRecorded");
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text, absArgument3.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_NothingRecorded\", callflow, myCall, logHeader));", text).AppendLine().Append("            ");
			if (obj.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text}.ContainerList[0].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
			{
				return CompilationResult.Cancelled;
			}
			ComponentBranch obj2 = recordComponent.EnabledActivities[1] as ComponentBranch;
			AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, string.Format("{0}.Result == RecordComponent.RecordResults.Completed || {0}.Result == RecordComponent.RecordResults.StopDigit", recordComponent.Name));
			componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text, absArgument4.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_AudioRecorded\", callflow, myCall, logHeader));", text).AppendLine().Append("            ");
			if (obj2.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text}.ContainerList[1].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
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

using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class DateTimeConditionalComponentCompiler : AbsComponentCompiler
{
	private readonly DateTimeConditionalComponent dateTimeConditionalComponent;

	private string GetDidCondition(DateTimeConditionalComponentBranch dateTimeConditionalComponentBranch)
	{
		if (dateTimeConditionalComponentBranch.DIDFilter == DIDFilters.AllDIDs)
		{
			return string.Empty;
		}
		string[] array = dateTimeConditionalComponentBranch.DIDFilterList.Split(',');
		string text = ((dateTimeConditionalComponentBranch.DIDFilter == DIDFilters.SpecificDIDs) ? " == " : " != ");
		string separator = ((dateTimeConditionalComponentBranch.DIDFilter == DIDFilters.SpecificDIDs) ? " || " : " && ");
		VariableNameArgument variableNameArgument = new VariableNameArgument("session.did");
		string text2 = "Convert.ToString(" + variableNameArgument.GetCompilerString() + ")";
		string[] array2 = new string[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = "Convert.ToString(" + array[i].Trim() + ")" + text + text2;
		}
		return "(" + string.Join(separator, array2) + ") && ";
	}

	public DateTimeConditionalComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, DateTimeConditionalComponent dateTimeConditionalComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(dateTimeConditionalComponent), dateTimeConditionalComponent.GetRootFlow().FlowType, dateTimeConditionalComponent)
	{
		this.dateTimeConditionalComponent = dateTimeConditionalComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (dateTimeConditionalComponent.EnabledActivities.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DateTimeConditionalComponent.NoBranches"), dateTimeConditionalComponent.Name), fileObject, flowType, dateTimeConditionalComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", dateTimeConditionalComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, dateTimeConditionalComponent.Name).AppendLine().Append("            ");
			for (int i = 0; i < dateTimeConditionalComponent.EnabledActivities.Count; i++)
			{
				DateTimeConditionalComponentBranch dateTimeConditionalComponentBranch = dateTimeConditionalComponent.EnabledActivities[i] as DateTimeConditionalComponentBranch;
				if (dateTimeConditionalComponentBranch.DIDFilter != 0 && string.IsNullOrEmpty(dateTimeConditionalComponentBranch.DIDFilterList))
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DateTimeConditionalComponent.DIDFilterListRequired"), dateTimeConditionalComponentBranch.Name, dateTimeConditionalComponent.Name), fileObject, flowType, dateTimeConditionalComponent);
					continue;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(GetDidCondition(dateTimeConditionalComponentBranch));
				if (dateTimeConditionalComponentBranch.DateTimeConditions.Count == 0)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DateTimeConditionalComponent.NoConditions"), dateTimeConditionalComponentBranch.Name, dateTimeConditionalComponent.Name), fileObject, flowType, dateTimeConditionalComponent);
					continue;
				}
				stringBuilder.Append("(");
				for (int j = 0; j < dateTimeConditionalComponentBranch.DateTimeConditions.Count; j++)
				{
					DateTimeCondition dateTimeCondition = dateTimeConditionalComponentBranch.DateTimeConditions[j];
					if (dateTimeCondition.IsValid())
					{
						stringBuilder.Append(dateTimeCondition.GetConditionExpression());
						if (j != dateTimeConditionalComponentBranch.DateTimeConditions.Count - 1)
						{
							stringBuilder.Append(" || ");
						}
					}
					else
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DateTimeConditionalComponent.InvalidCondition"), dateTimeConditionalComponentBranch.Name, dateTimeConditionalComponent.Name), fileObject, flowType, dateTimeConditionalComponent);
					}
				}
				stringBuilder.Append(")");
				componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return Convert.ToBoolean({1}); }});", dateTimeConditionalComponent.Name, stringBuilder.ToString()).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_{1}\", callflow, myCall, logHeader));", dateTimeConditionalComponent.Name, i).AppendLine().Append("            ");
				if (dateTimeConditionalComponentBranch.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{dateTimeConditionalComponent.Name}.ContainerList[{i}].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
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

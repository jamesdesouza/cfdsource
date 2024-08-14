using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class SurveyComponentCompiler : AbsComponentCompiler
{
	private readonly SurveyComponent surveyComponent;

	public SurveyComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, SurveyComponent surveyComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(surveyComponent), surveyComponent.GetRootFlow().FlowType, surveyComponent)
	{
		this.surveyComponent = surveyComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(surveyComponent.ExportToCSVFile))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.ExportToCSVFileIsEmpty"), surveyComponent.Name), fileObject, flowType, surveyComponent);
		}
		else
		{
			if (surveyComponent.IntroductoryPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.IntroductoryPromptIsEmpty"), surveyComponent.Name), fileObject, flowType, surveyComponent);
			}
			if (surveyComponent.GoodbyePrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.GoodbyePromptIsEmpty"), surveyComponent.Name), fileObject, flowType, surveyComponent);
			}
			if (surveyComponent.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.TimeoutPromptIsEmpty"), surveyComponent.Name), fileObject, flowType, surveyComponent);
			}
			if (surveyComponent.InvalidDigitPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.InvalidDigitPromptIsEmpty"), surveyComponent.Name), fileObject, flowType, surveyComponent);
			}
			if (surveyComponent.Timeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.TimeoutMustBeGreaterThanZero"), surveyComponent.Name), fileObject, flowType, surveyComponent);
			}
			if (surveyComponent.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.MaxRetryCountMustBeGreaterThanZero"), surveyComponent.Name), fileObject, flowType, surveyComponent);
			}
			componentsInitializationScriptSb.AppendFormat("SurveyComponent {0} = new SurveyComponent(\"{0}\", callflow, myCall, logHeader);", surveyComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", surveyComponent.Name, surveyComponent.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", surveyComponent.Name, surveyComponent.MaxRetryCount - 1).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Timeout = {1};", surveyComponent.Name, 1000 * surveyComponent.Timeout).AppendLine().Append("            ");
			if (surveyComponent.AllowPartialAnswers)
			{
				componentsInitializationScriptSb.AppendFormat("{0}.AllowPartialAnswers = true;", surveyComponent.Name).AppendLine().Append("            ");
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, surveyComponent.ExportToCSVFile);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "ExportToCSVFile", surveyComponent.Name, surveyComponent.ExportToCSVFile), fileObject, flowType, surveyComponent);
			}
			foreach (DotNetExpressionArgument literalExpression in absArgument.GetLiteralExpressionList())
			{
				if (literalExpression.IsStringLiteral() && ExpressionHelper.UnescapeConstantString(literalExpression.GetString()).IndexOfAny(new char[6] { '<', '>', '"', '|', '?', '*' }) >= 0)
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.ExportToCSVFileInvalidCharacters"), surveyComponent.Name), fileObject, flowType, surveyComponent);
					break;
				}
			}
			componentsInitializationScriptSb.AppendFormat("{0}.ExportToCSVFileHandler = () => {{ return Convert.ToString({1}); }};", surveyComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			foreach (Prompt ıntroductoryPrompt in surveyComponent.IntroductoryPrompts)
			{
				ıntroductoryPrompt.Accept(this, isDebugBuild, surveyComponent.Name, "IntroductoryPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt goodbyePrompt in surveyComponent.GoodbyePrompts)
			{
				goodbyePrompt.Accept(this, isDebugBuild, surveyComponent.Name, "GoodbyePrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt ınvalidDigitPrompt in surveyComponent.InvalidDigitPrompts)
			{
				ınvalidDigitPrompt.Accept(this, isDebugBuild, surveyComponent.Name, "InvalidDigitPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt timeoutPrompt in surveyComponent.TimeoutPrompts)
			{
				timeoutPrompt.Accept(this, isDebugBuild, surveyComponent.Name, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Parameter outputField in surveyComponent.OutputFields)
			{
				if (!string.IsNullOrEmpty(outputField.Name) || !string.IsNullOrEmpty(outputField.Value))
				{
					if (string.IsNullOrEmpty(outputField.Name))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.ParameterNameIsEmpty"), surveyComponent.Name), fileObject, flowType, surveyComponent);
					}
					if (string.IsNullOrEmpty(outputField.Value))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.ParameterValueIsEmpty"), surveyComponent.Name), fileObject, flowType, surveyComponent);
					}
					AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, outputField.Value);
					if (!absArgument2.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Parameter - " + outputField.Name, surveyComponent.Name, outputField.Value), fileObject, flowType, surveyComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.Parameters.Add(new CallFlow.CFD.Parameter(\"{1}\", () => {{ return {2}; }}));", surveyComponent.Name, outputField.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			if (surveyComponent.SurveyQuestions.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.NoSurveyQuestions"), surveyComponent.Name), fileObject, flowType, surveyComponent);
			}
			bool flag = false;
			for (int i = 0; i < surveyComponent.SurveyQuestions.Count; i++)
			{
				SurveyQuestion surveyQuestion = surveyComponent.SurveyQuestions[i];
				if (surveyQuestion.IsValid())
				{
					surveyQuestion.Accept(this, isDebugBuild, surveyComponent.Name, componentsInitializationScriptSb, audioFileCollector);
				}
				else
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.InvalidSurveyQuestion"), surveyComponent.Name, i), fileObject, flowType, surveyComponent);
				}
				if (surveyQuestion is RecordingSurveyQuestion)
				{
					flag = true;
				}
			}
			if (flag && !string.IsNullOrEmpty(surveyComponent.RecordingsPath))
			{
				AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, surveyComponent.RecordingsPath);
				if (!absArgument3.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "RecordingsPath", surveyComponent.Name, surveyComponent.RecordingsPath), fileObject, flowType, surveyComponent);
				}
				foreach (DotNetExpressionArgument literalExpression2 in absArgument3.GetLiteralExpressionList())
				{
					if (literalExpression2.IsStringLiteral() && ExpressionHelper.UnescapeConstantString(literalExpression2.GetString()).IndexOfAny(new char[6] { '<', '>', '"', '|', '?', '*' }) >= 0)
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SurveyComponent.RecordingsPathInvalidCharacters"), surveyComponent.Name), fileObject, flowType, surveyComponent);
						break;
					}
				}
				componentsInitializationScriptSb.AppendFormat("{0}.RecordingsPathHandler = () => {{ return Convert.ToString({1}); }};", surveyComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, surveyComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

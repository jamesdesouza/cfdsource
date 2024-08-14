using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class LoggerComponentCompiler : AbsComponentCompiler
{
	private readonly LoggerComponent loggerComponent;

	public LoggerComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, LoggerComponent loggerComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(loggerComponent), loggerComponent.GetRootFlow().FlowType, loggerComponent)
	{
		this.loggerComponent = loggerComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(loggerComponent.Text))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.LoggerComponent.TextIsEmpty"), loggerComponent.Name), fileObject, flowType, loggerComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, loggerComponent.Text);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Text", loggerComponent.Name, loggerComponent.Text), fileObject, flowType, loggerComponent);
			}
			componentsInitializationScriptSb.AppendFormat("LoggerComponent {0} = new LoggerComponent(\"{0}\", callflow, myCall, logHeader);", loggerComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Level = {1};", loggerComponent.Name, EnumHelper.LogLevelToString(loggerComponent.Level)).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.TextHandler = () => {{ return Convert.ToString({1}); }};", loggerComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, loggerComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class CsvParserComponentCompiler : AbsComponentCompiler
{
	private readonly CsvParserComponent csvParserComponent;

	public CsvParserComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, CsvParserComponent csvParserComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(csvParserComponent), csvParserComponent.GetRootFlow().FlowType, csvParserComponent)
	{
		this.csvParserComponent = csvParserComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(csvParserComponent.Text))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CsvParserComponent.TextIsEmpty"), csvParserComponent.Name), fileObject, flowType, csvParserComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, csvParserComponent.Text);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Text", csvParserComponent.Name, csvParserComponent.Text), fileObject, flowType, csvParserComponent);
			}
			componentsInitializationScriptSb.AppendFormat("CsvParserComponent {0} = new CsvParserComponent(\"{0}\", callflow, myCall, logHeader);", csvParserComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.TextHandler = () => {{ return Convert.ToString({1}); }};", csvParserComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, csvParserComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

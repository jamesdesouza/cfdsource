using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class JsonXmlParserComponentCompiler : AbsComponentCompiler
{
	private readonly JsonXmlParserComponent jsonXmlParserComponent;

	public JsonXmlParserComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, JsonXmlParserComponent jsonXmlParserComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(jsonXmlParserComponent), jsonXmlParserComponent.GetRootFlow().FlowType, jsonXmlParserComponent)
	{
		this.jsonXmlParserComponent = jsonXmlParserComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(jsonXmlParserComponent.Input))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.JsonXmlParserComponent.InputIsEmpty"), jsonXmlParserComponent.Name), fileObject, flowType, jsonXmlParserComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("TextAnalyzerComponent {0} = new TextAnalyzerComponent(\"{0}\", callflow, myCall, logHeader);", jsonXmlParserComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.TextType = TextAnalyzerComponent.TextTypes.{1};", jsonXmlParserComponent.Name, (jsonXmlParserComponent.TextType == TextTypes.JSON) ? "JSON" : "XML").AppendLine().Append("            ");
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, jsonXmlParserComponent.Input);
			if (!absArgument.IsVariableName())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.JsonXmlParserComponent.InputIsInvalid"), jsonXmlParserComponent.Name), fileObject, flowType, jsonXmlParserComponent);
			}
			else
			{
				componentsInitializationScriptSb.AppendFormat("{0}.TextHandler = () => {{ return Convert.ToString({1}); }};", jsonXmlParserComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			}
			foreach (ResponseMapping responseMapping in jsonXmlParserComponent.ResponseMappings)
			{
				if (!AbsArgument.BuildArgument(validVariables, responseMapping.Variable).IsVariableName())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.JsonXmlParserComponent.VariableNameIsInvalid"), responseMapping.Variable, jsonXmlParserComponent.Name), fileObject, flowType, jsonXmlParserComponent);
				}
				else
				{
					componentsInitializationScriptSb.AppendFormat("{0}.Mappings.Add(\"{1}\", \"{2}\");", jsonXmlParserComponent.Name, responseMapping.Path, responseMapping.Variable).AppendLine().Append("            ");
				}
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, jsonXmlParserComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

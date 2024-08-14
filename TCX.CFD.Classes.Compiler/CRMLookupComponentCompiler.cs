using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class CRMLookupComponentCompiler : AbsComponentCompiler
{
	private readonly CRMLookupComponent crmLookupComponent;

	public CRMLookupComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, CRMLookupComponent crmLookupComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(crmLookupComponent), crmLookupComponent.GetRootFlow().FlowType, crmLookupComponent)
	{
		this.crmLookupComponent = crmLookupComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Message, string.Format(LocalizedResourceMgr.GetString("Compiler.CRMLookupComponent.PrerequisiteInfo"), crmLookupComponent.Name), fileObject, flowType, crmLookupComponent);
		if (string.IsNullOrEmpty(crmLookupComponent.LookupInputParameter))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CRMLookupComponent.LookupInputParameterIsEmpty"), crmLookupComponent.Name), fileObject, flowType, crmLookupComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("CRMLookupComponent {0} = new CRMLookupComponent(\"{0}\", callflow, myCall, logHeader);", crmLookupComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.EntityType = {1};", crmLookupComponent.Name, (crmLookupComponent.Entity == CRMEntities.Contacts) ? "CRMLookupComponent.EntityTypes.Contacts" : ((crmLookupComponent.Entity == CRMEntities.Leads) ? "CRMLookupComponent.EntityTypes.Leads" : "CRMLookupComponent.EntityTypes.Accounts")).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.QueryType = {1};", crmLookupComponent.Name, (crmLookupComponent.LookupBy == CRMLookupBy.EntityNumber) ? "CRMLookupComponent.QueryTypes.LookupNumber" : ((crmLookupComponent.LookupBy == CRMLookupBy.EntityID) ? "CRMLookupComponent.QueryTypes.LookupID" : "CRMLookupComponent.QueryTypes.LookupFreeQuery")).AppendLine().Append("            ");
			if (!string.IsNullOrWhiteSpace(crmLookupComponent.LookupInputParameter))
			{
				AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, crmLookupComponent.LookupInputParameter);
				if (!absArgument.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "LookupInputParameter", crmLookupComponent.Name, crmLookupComponent.LookupInputParameter), fileObject, flowType, crmLookupComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.DataHandler = () => {{ return Convert.ToString({1}); }};", crmLookupComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, crmLookupComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("TextAnalyzerComponent {0}_ResponseAnalyzer = new TextAnalyzerComponent(\"{0}\", callflow, myCall, logHeader);", crmLookupComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}_ResponseAnalyzer.TextType = TextAnalyzerComponent.TextTypes.Detect;", crmLookupComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}_ResponseAnalyzer.TextHandler = () => {{ return Convert.ToString({0}.Result); }};", crmLookupComponent.Name).AppendLine().Append("            ");
			foreach (ResponseMapping responseMapping in crmLookupComponent.ResponseMappings)
			{
				if (!AbsArgument.BuildArgument(validVariables, responseMapping.Variable).IsVariableName())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CRMLookupComponent.VariableNameIsInvalid"), responseMapping.Variable, crmLookupComponent.Name), fileObject, flowType, crmLookupComponent);
				}
				else
				{
					componentsInitializationScriptSb.AppendFormat("{0}_ResponseAnalyzer.Mappings.Add(\"{1}\", \"{2}\");", crmLookupComponent.Name, responseMapping.Path, responseMapping.Variable).AppendLine().Append("            ");
				}
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1}_ResponseAnalyzer);", parentComponentListName, crmLookupComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

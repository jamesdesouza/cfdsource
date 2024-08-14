using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class SetCallerNameComponentCompiler : AbsComponentCompiler
{
	private readonly SetCallerNameComponent setCallerNameComponent;

	public SetCallerNameComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, SetCallerNameComponent setCallerNameComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(setCallerNameComponent), setCallerNameComponent.GetRootFlow().FlowType, setCallerNameComponent)
	{
		this.setCallerNameComponent = setCallerNameComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(setCallerNameComponent.DisplayName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SetCallerNameComponent.DisplayNameIsEmpty"), setCallerNameComponent.Name), fileObject, flowType, setCallerNameComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, setCallerNameComponent.DisplayName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "DisplayName", setCallerNameComponent.Name, setCallerNameComponent.DisplayName), fileObject, flowType, setCallerNameComponent);
			}
			componentsInitializationScriptSb.AppendFormat("AttachCallDataComponent {0} = new AttachCallDataComponent(\"{0}\", callflow, myCall, logHeader);", setCallerNameComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.DataNameHandler = () => {{ return \"public_lookup_displayname_override\"; }};", setCallerNameComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.DataValueHandler = () => {{ return Convert.ToString({1}); }};", setCallerNameComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, setCallerNameComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

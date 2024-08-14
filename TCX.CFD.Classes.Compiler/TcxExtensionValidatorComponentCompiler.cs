using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxExtensionValidatorComponentCompiler : AbsComponentCompiler
{
	private readonly TcxExtensionValidatorComponent tcxExtensionValidatorComponent;

	public TcxExtensionValidatorComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxExtensionValidatorComponent tcxExtensionValidatorComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxExtensionValidatorComponent), tcxExtensionValidatorComponent.GetRootFlow().FlowType, tcxExtensionValidatorComponent)
	{
		this.tcxExtensionValidatorComponent = tcxExtensionValidatorComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxExtensionValidatorComponent.Extension))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxExtensionValidatorComponent.ExtensionIsEmpty"), tcxExtensionValidatorComponent.Name), fileObject, flowType, tcxExtensionValidatorComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxExtensionValidatorComponent.Extension);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Extension", tcxExtensionValidatorComponent.Name, tcxExtensionValidatorComponent.Extension), fileObject, flowType, tcxExtensionValidatorComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxExtensionValidatorComponent {0} = new TcxExtensionValidatorComponent(\"{0}\", callflow, myCall, logHeader);", tcxExtensionValidatorComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxExtensionValidatorComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxExtensionValidatorComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

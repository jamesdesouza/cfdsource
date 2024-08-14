using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class DecrementVariableComponentCompiler : AbsComponentCompiler
{
	private readonly DecrementVariableComponent decrementVariableComponent;

	public DecrementVariableComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, DecrementVariableComponent decrementVariableComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(decrementVariableComponent), decrementVariableComponent.GetRootFlow().FlowType, decrementVariableComponent)
	{
		this.decrementVariableComponent = decrementVariableComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(decrementVariableComponent.VariableName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DecrementVariableComponent.VariableNameIsEmpty"), decrementVariableComponent.Name), fileObject, flowType, decrementVariableComponent);
		}
		else if (!AbsArgument.BuildArgument(validVariables, decrementVariableComponent.VariableName).IsVariableName())
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DecrementVariableComponent.VariableNameIsInvalid"), decrementVariableComponent.VariableName, decrementVariableComponent.Name), fileObject, flowType, decrementVariableComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("DecrementVariableComponent {0} = new DecrementVariableComponent(\"{0}\", callflow, myCall, logHeader);", decrementVariableComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.VariableName = \"{1}\";", decrementVariableComponent.Name, decrementVariableComponent.VariableName).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, decrementVariableComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

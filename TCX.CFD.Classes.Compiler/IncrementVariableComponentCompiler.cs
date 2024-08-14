using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class IncrementVariableComponentCompiler : AbsComponentCompiler
{
	private readonly IncrementVariableComponent incrementVariableComponent;

	public IncrementVariableComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, IncrementVariableComponent incrementVariableComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(incrementVariableComponent), incrementVariableComponent.GetRootFlow().FlowType, incrementVariableComponent)
	{
		this.incrementVariableComponent = incrementVariableComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(incrementVariableComponent.VariableName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.IncrementVariableComponent.VariableNameIsEmpty"), incrementVariableComponent.Name), fileObject, flowType, incrementVariableComponent);
		}
		else if (!AbsArgument.BuildArgument(validVariables, incrementVariableComponent.VariableName).IsVariableName())
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.IncrementVariableComponent.VariableNameIsInvalid"), incrementVariableComponent.VariableName, incrementVariableComponent.Name), fileObject, flowType, incrementVariableComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("IncrementVariableComponent {0} = new IncrementVariableComponent(\"{0}\", callflow, myCall, logHeader);", incrementVariableComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.VariableName = \"{1}\";", incrementVariableComponent.Name, incrementVariableComponent.VariableName).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, incrementVariableComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

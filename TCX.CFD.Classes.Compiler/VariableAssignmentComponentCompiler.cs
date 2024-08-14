using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class VariableAssignmentComponentCompiler : AbsComponentCompiler
{
	private readonly VariableAssignmentComponent variableAssignmentComponent;

	public VariableAssignmentComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, VariableAssignmentComponent variableAssignmentComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(variableAssignmentComponent), variableAssignmentComponent.GetRootFlow().FlowType, variableAssignmentComponent)
	{
		this.variableAssignmentComponent = variableAssignmentComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(variableAssignmentComponent.VariableName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VariableAssignmentComponent.VariableNameIsEmpty"), variableAssignmentComponent.Name), fileObject, flowType, variableAssignmentComponent);
		}
		else if (string.IsNullOrEmpty(variableAssignmentComponent.Expression))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VariableAssignmentComponent.ExpressionIsEmpty"), variableAssignmentComponent.Name), fileObject, flowType, variableAssignmentComponent);
		}
		else if (!AbsArgument.BuildArgument(validVariables, variableAssignmentComponent.VariableName).IsVariableName())
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.VariableAssignmentComponent.VariableNameIsInvalid"), variableAssignmentComponent.VariableName, variableAssignmentComponent.Name), fileObject, flowType, variableAssignmentComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, variableAssignmentComponent.Expression);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Expression", variableAssignmentComponent.Name, variableAssignmentComponent.Expression), fileObject, flowType, variableAssignmentComponent);
			}
			componentsInitializationScriptSb.AppendFormat("VariableAssignmentComponent {0} = new VariableAssignmentComponent(\"{0}\", callflow, myCall, logHeader);", variableAssignmentComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.VariableName = \"{1}\";", variableAssignmentComponent.Name, variableAssignmentComponent.VariableName).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.VariableValueHandler = () => {{ return {1}; }};", variableAssignmentComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, variableAssignmentComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

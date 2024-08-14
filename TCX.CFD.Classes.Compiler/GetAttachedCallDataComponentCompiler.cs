using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class GetAttachedCallDataComponentCompiler : AbsComponentCompiler
{
	private readonly GetAttachedCallDataComponent getAttachedCallDataComponent;

	public GetAttachedCallDataComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, GetAttachedCallDataComponent getAttachedCallDataComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(getAttachedCallDataComponent), getAttachedCallDataComponent.GetRootFlow().FlowType, getAttachedCallDataComponent)
	{
		this.getAttachedCallDataComponent = getAttachedCallDataComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(getAttachedCallDataComponent.DataName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.GetAttachedCallDataComponent.DataNameIsEmpty"), getAttachedCallDataComponent.Name), fileObject, flowType, getAttachedCallDataComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, getAttachedCallDataComponent.DataName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "DataName", getAttachedCallDataComponent.Name, getAttachedCallDataComponent.DataName), fileObject, flowType, getAttachedCallDataComponent);
			}
			componentsInitializationScriptSb.AppendFormat("GetAttachedCallDataComponent {0} = new GetAttachedCallDataComponent(\"{0}\", callflow, myCall, logHeader);", getAttachedCallDataComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.DataNameHandler = () => {{ return Convert.ToString({1}); }};", getAttachedCallDataComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, getAttachedCallDataComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

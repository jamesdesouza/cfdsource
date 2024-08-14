using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxGetGlobalPropertyComponentCompiler : AbsComponentCompiler
{
	private readonly TcxGetGlobalPropertyComponent tcxGetGlobalPropertyComponent;

	public TcxGetGlobalPropertyComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxGetGlobalPropertyComponent tcxGetGlobalPropertyComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxGetGlobalPropertyComponent), tcxGetGlobalPropertyComponent.GetRootFlow().FlowType, tcxGetGlobalPropertyComponent)
	{
		this.tcxGetGlobalPropertyComponent = tcxGetGlobalPropertyComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxGetGlobalPropertyComponent.PropertyName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxGetGlobalPropertyComponent.PropertyNameIsEmpty"), tcxGetGlobalPropertyComponent.Name), fileObject, flowType, tcxGetGlobalPropertyComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxGetGlobalPropertyComponent.PropertyName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "PropertyName", tcxGetGlobalPropertyComponent.Name, tcxGetGlobalPropertyComponent.PropertyName), fileObject, flowType, tcxGetGlobalPropertyComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxGetGlobalPropertyComponent {0} = new TcxGetGlobalPropertyComponent(\"{0}\", callflow, myCall, logHeader);", tcxGetGlobalPropertyComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.PropertyNameHandler = () => {{ return Convert.ToString({1}).ToUpper(); }};", tcxGetGlobalPropertyComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxGetGlobalPropertyComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

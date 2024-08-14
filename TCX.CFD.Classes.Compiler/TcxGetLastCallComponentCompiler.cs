using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxGetLastCallComponentCompiler : AbsComponentCompiler
{
	private readonly TcxGetLastCallComponent tcxGetLastCallComponent;

	public TcxGetLastCallComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxGetLastCallComponent tcxGetLastCallComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxGetLastCallComponent), tcxGetLastCallComponent.GetRootFlow().FlowType, tcxGetLastCallComponent)
	{
		this.tcxGetLastCallComponent = tcxGetLastCallComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxGetLastCallComponent.Number))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxGetLastCallComponent.NumberIsEmpty"), tcxGetLastCallComponent.Name), fileObject, flowType, tcxGetLastCallComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxGetLastCallComponent.Number);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Number", tcxGetLastCallComponent.Name, tcxGetLastCallComponent.Number), fileObject, flowType, tcxGetLastCallComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxGetLastCallComponent {0} = new TcxGetLastCallComponent(\"{0}\", callflow, myCall, logHeader);", tcxGetLastCallComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.NumberHandler = () => {{ return Convert.ToString({1}); }};", tcxGetLastCallComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.CallType = {1};", tcxGetLastCallComponent.Name, EnumHelper.CallDirectionToCompilerString(tcxGetLastCallComponent.CallType)).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxGetLastCallComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

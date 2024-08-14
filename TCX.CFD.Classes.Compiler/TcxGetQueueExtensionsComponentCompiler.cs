using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxGetQueueExtensionsComponentCompiler : AbsComponentCompiler
{
	private readonly TcxGetQueueExtensionsComponent tcxGetQueueExtensionsComponent;

	public TcxGetQueueExtensionsComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxGetQueueExtensionsComponent tcxGetQueueExtensionsComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxGetQueueExtensionsComponent), tcxGetQueueExtensionsComponent.GetRootFlow().FlowType, tcxGetQueueExtensionsComponent)
	{
		this.tcxGetQueueExtensionsComponent = tcxGetQueueExtensionsComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxGetQueueExtensionsComponent.QueueExtension))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxGetQueueExtensionsComponent.QueueExtensionIsEmpty"), tcxGetQueueExtensionsComponent.Name), fileObject, flowType, tcxGetQueueExtensionsComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxGetQueueExtensionsComponent.QueueExtension);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "QueueExtension", tcxGetQueueExtensionsComponent.Name, tcxGetQueueExtensionsComponent.QueueExtension), fileObject, flowType, tcxGetQueueExtensionsComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxGetQueueExtensionsComponent {0} = new TcxGetQueueExtensionsComponent(\"{0}\", callflow, myCall, logHeader);", tcxGetQueueExtensionsComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.QueueExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxGetQueueExtensionsComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.QueryType = {1};", tcxGetQueueExtensionsComponent.Name, (tcxGetQueueExtensionsComponent.QueueQueryType == QueueQueryTypes.All) ? "TcxGetQueueExtensionsComponent.QueryTypes.All" : ((tcxGetQueueExtensionsComponent.QueueQueryType == QueueQueryTypes.LoggedIn) ? "TcxGetQueueExtensionsComponent.QueryTypes.LoggedIn" : "TcxGetQueueExtensionsComponent.QueryTypes.LoggedOut")).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxGetQueueExtensionsComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

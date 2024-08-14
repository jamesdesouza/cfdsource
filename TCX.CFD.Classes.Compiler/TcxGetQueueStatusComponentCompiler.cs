using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxGetQueueStatusComponentCompiler : AbsComponentCompiler
{
	private readonly TcxGetQueueStatusComponent tcxGetQueueStatusComponent;

	public TcxGetQueueStatusComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxGetQueueStatusComponent tcxGetQueueStatusComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxGetQueueStatusComponent), tcxGetQueueStatusComponent.GetRootFlow().FlowType, tcxGetQueueStatusComponent)
	{
		this.tcxGetQueueStatusComponent = tcxGetQueueStatusComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxGetQueueStatusComponent.QueueExtension))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxGetQueueStatusComponent.QueueExtensionIsEmpty"), tcxGetQueueStatusComponent.Name), fileObject, flowType, tcxGetQueueStatusComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxGetQueueStatusComponent.QueueExtension);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "QueueExtension", tcxGetQueueStatusComponent.Name, tcxGetQueueStatusComponent.QueueExtension), fileObject, flowType, tcxGetQueueStatusComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxGetQueueStatusComponent {0} = new TcxGetQueueStatusComponent(\"{0}\", callflow, myCall, logHeader);", tcxGetQueueStatusComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.QueueExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxGetQueueStatusComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxGetQueueStatusComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

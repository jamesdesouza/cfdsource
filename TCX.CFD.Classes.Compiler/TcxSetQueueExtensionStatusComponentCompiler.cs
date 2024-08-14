using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxSetQueueExtensionStatusComponentCompiler : AbsComponentCompiler
{
	private readonly TcxSetQueueExtensionStatusComponent tcxSetQueueExtensionStatusComponent;

	public TcxSetQueueExtensionStatusComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxSetQueueExtensionStatusComponent tcxSetQueueExtensionStatusComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxSetQueueExtensionStatusComponent), tcxSetQueueExtensionStatusComponent.GetRootFlow().FlowType, tcxSetQueueExtensionStatusComponent)
	{
		this.tcxSetQueueExtensionStatusComponent = tcxSetQueueExtensionStatusComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxSetQueueExtensionStatusComponent.Extension))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxSetQueueExtensionStatusComponent.ExtensionIsEmpty"), tcxSetQueueExtensionStatusComponent.Name), fileObject, flowType, tcxSetQueueExtensionStatusComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxSetQueueExtensionStatusComponent.Extension);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Extension", tcxSetQueueExtensionStatusComponent.Name, tcxSetQueueExtensionStatusComponent.Extension), fileObject, flowType, tcxSetQueueExtensionStatusComponent);
			}
			AbsArgument absArgument2 = new DotNetExpressionArgument("\"\"");
			if (tcxSetQueueExtensionStatusComponent.QueueMode == QueueStatusOperationModes.SpecificQueue)
			{
				if (string.IsNullOrEmpty(tcxSetQueueExtensionStatusComponent.QueueExtension))
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxSetQueueExtensionStatusComponent.QueueExtensionIsEmpty"), tcxSetQueueExtensionStatusComponent.Name), fileObject, flowType, tcxSetQueueExtensionStatusComponent);
				}
				else
				{
					absArgument2 = AbsArgument.BuildArgument(validVariables, tcxSetQueueExtensionStatusComponent.QueueExtension);
					if (!absArgument2.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "QueueExtension", tcxSetQueueExtensionStatusComponent.Name, tcxSetQueueExtensionStatusComponent.QueueExtension), fileObject, flowType, tcxSetQueueExtensionStatusComponent);
					}
				}
			}
			componentsInitializationScriptSb.AppendFormat("TcxSetQueueExtensionStatusComponent {0} = new TcxSetQueueExtensionStatusComponent(\"{0}\", callflow, myCall, logHeader);", tcxSetQueueExtensionStatusComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxSetQueueExtensionStatusComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.QueueExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxSetQueueExtensionStatusComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.QueueStatus = {1};", tcxSetQueueExtensionStatusComponent.Name, (tcxSetQueueExtensionStatusComponent.QueueStatus == QueueStatusTypes.LoggedIn) ? "QueueStatusType.LoggedIn" : "QueueStatusType.LoggedOut").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxSetQueueExtensionStatusComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

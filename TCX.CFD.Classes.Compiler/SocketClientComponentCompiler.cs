using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class SocketClientComponentCompiler : AbsComponentCompiler
{
	private readonly SocketClientComponent socketClientComponent;

	public SocketClientComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, SocketClientComponent socketClientComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(socketClientComponent), socketClientComponent.GetRootFlow().FlowType, socketClientComponent)
	{
		this.socketClientComponent = socketClientComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		int result;
		if (string.IsNullOrEmpty(socketClientComponent.Host))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SocketClientComponent.HostIsEmpty"), socketClientComponent.Name), fileObject, flowType, socketClientComponent);
		}
		else if (string.IsNullOrEmpty(socketClientComponent.Port))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SocketClientComponent.PortIsEmpty"), socketClientComponent.Name), fileObject, flowType, socketClientComponent);
		}
		else if (string.IsNullOrEmpty(socketClientComponent.Data))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SocketClientComponent.DataIsEmpty"), socketClientComponent.Name), fileObject, flowType, socketClientComponent);
		}
		else if (int.TryParse(socketClientComponent.Port, out result) && (result < 1 || result > 65535))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.SocketClientComponent.PortIsOutOfRange"), socketClientComponent.Name, 1, 65535), fileObject, flowType, socketClientComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, socketClientComponent.Port);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Port", socketClientComponent.Name, socketClientComponent.Port), fileObject, flowType, socketClientComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, socketClientComponent.Host);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Host", socketClientComponent.Name, socketClientComponent.Host), fileObject, flowType, socketClientComponent);
			}
			AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, socketClientComponent.Data);
			if (!absArgument3.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Data", socketClientComponent.Name, socketClientComponent.Data), fileObject, flowType, socketClientComponent);
			}
			componentsInitializationScriptSb.AppendFormat("SocketClientComponent {0} = new SocketClientComponent(\"{0}\", callflow, myCall, logHeader);", socketClientComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ConnectionType = {1};", socketClientComponent.Name, (socketClientComponent.ConnectionType == SocketConnectionTypes.TCP) ? "SocketClientComponent.ConnectionTypes.TCP" : "SocketClientComponent.ConnectionTypes.UDP").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.DataHandler = () => {{ return Convert.ToString({1}); }};", socketClientComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.HostHandler = () => {{ return Convert.ToString({1}); }};", socketClientComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.PortHandler = () => {{ return Convert.ToInt32({1}); }};", socketClientComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.WaitForResponse = {1};", socketClientComponent.Name, socketClientComponent.WaitForResponse ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, socketClientComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

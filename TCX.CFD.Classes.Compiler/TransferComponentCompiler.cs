using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TransferComponentCompiler : AbsComponentCompiler
{
	private readonly TransferComponent transferComponent;

	public TransferComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TransferComponent transferComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(transferComponent), transferComponent.GetRootFlow().FlowType, transferComponent)
	{
		this.transferComponent = transferComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		long result;
		if (string.IsNullOrEmpty(transferComponent.Destination))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TransferComponent.DestinationIsEmpty"), transferComponent.Name), fileObject, flowType, transferComponent);
		}
		else if (transferComponent.Destination.StartsWith("0") && long.TryParse(transferComponent.Destination, out result))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TransferComponent.DestinationStartsWithZero"), transferComponent.Name), fileObject, flowType, transferComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, transferComponent.Destination);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Destination", transferComponent.Name, transferComponent.Destination), fileObject, flowType, transferComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TransferComponent {0} = new TransferComponent(\"{0}\", callflow, myCall, logHeader);", transferComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.DestinationHandler = () => {{ return Convert.ToString({1}); }};", transferComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			if (transferComponent.TransferToVoicemail)
			{
				componentsInitializationScriptSb.AppendFormat("{0}.TransferToVoicemail = true;", transferComponent.Name).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.DelayMilliseconds = {1};", transferComponent.Name, transferComponent.DelayMilliseconds).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, transferComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

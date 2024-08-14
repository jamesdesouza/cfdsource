using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxGetExtensionStatusComponentCompiler : AbsComponentCompiler
{
	private readonly TcxGetExtensionStatusComponent tcxGetExtensionStatusComponent;

	public TcxGetExtensionStatusComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxGetExtensionStatusComponent tcxGetExtensionStatusComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxGetExtensionStatusComponent), tcxGetExtensionStatusComponent.GetRootFlow().FlowType, tcxGetExtensionStatusComponent)
	{
		this.tcxGetExtensionStatusComponent = tcxGetExtensionStatusComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxGetExtensionStatusComponent.Extension))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxGetExtensionStatusComponent.ExtensionIsEmpty"), tcxGetExtensionStatusComponent.Name), fileObject, flowType, tcxGetExtensionStatusComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxGetExtensionStatusComponent.Extension);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Extension", tcxGetExtensionStatusComponent.Name, tcxGetExtensionStatusComponent.Extension), fileObject, flowType, tcxGetExtensionStatusComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxGetExtensionStatusComponent {0} = new TcxGetExtensionStatusComponent(\"{0}\", callflow, myCall, logHeader);", tcxGetExtensionStatusComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxGetExtensionStatusComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxGetExtensionStatusComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

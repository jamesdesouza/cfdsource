using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxSetExtensionStatusComponentCompiler : AbsComponentCompiler
{
	private readonly TcxSetExtensionStatusComponent tcxSetExtensionStatusComponent;

	public TcxSetExtensionStatusComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxSetExtensionStatusComponent tcxSetExtensionStatusComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxSetExtensionStatusComponent), tcxSetExtensionStatusComponent.GetRootFlow().FlowType, tcxSetExtensionStatusComponent)
	{
		this.tcxSetExtensionStatusComponent = tcxSetExtensionStatusComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxSetExtensionStatusComponent.Extension))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxSetExtensionStatusComponent.ExtensionIsEmpty"), tcxSetExtensionStatusComponent.Name), fileObject, flowType, tcxSetExtensionStatusComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxSetExtensionStatusComponent.Extension);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Extension", tcxSetExtensionStatusComponent.Name, tcxSetExtensionStatusComponent.Extension), fileObject, flowType, tcxSetExtensionStatusComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxSetExtensionStatusComponent {0} = new TcxSetExtensionStatusComponent(\"{0}\", callflow, myCall, logHeader);", tcxSetExtensionStatusComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxSetExtensionStatusComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ProfileNameHandler = () => {{ return \"{1}\"; }};", tcxSetExtensionStatusComponent.Name, EnumHelper.ExtensionStatusToProfileName(tcxSetExtensionStatusComponent.Status)).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxSetExtensionStatusComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxGetDnPropertyComponentCompiler : AbsComponentCompiler
{
	private readonly TcxGetDnPropertyComponent tcxGetDnPropertyComponent;

	public TcxGetDnPropertyComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxGetDnPropertyComponent tcxGetDnPropertyComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxGetDnPropertyComponent), tcxGetDnPropertyComponent.GetRootFlow().FlowType, tcxGetDnPropertyComponent)
	{
		this.tcxGetDnPropertyComponent = tcxGetDnPropertyComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxGetDnPropertyComponent.Extension))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxGetDnPropertyComponent.ExtensionIsEmpty"), tcxGetDnPropertyComponent.Name), fileObject, flowType, tcxGetDnPropertyComponent);
		}
		else if (string.IsNullOrEmpty(tcxGetDnPropertyComponent.PropertyName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxGetDnPropertyComponent.PropertyNameIsEmpty"), tcxGetDnPropertyComponent.Name), fileObject, flowType, tcxGetDnPropertyComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxGetDnPropertyComponent.Extension);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Extension", tcxGetDnPropertyComponent.Name, tcxGetDnPropertyComponent.Extension), fileObject, flowType, tcxGetDnPropertyComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, tcxGetDnPropertyComponent.PropertyName);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "PropertyName", tcxGetDnPropertyComponent.Name, tcxGetDnPropertyComponent.PropertyName), fileObject, flowType, tcxGetDnPropertyComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxGetDnPropertyComponent {0} = new TcxGetDnPropertyComponent(\"{0}\", callflow, myCall, logHeader);", tcxGetDnPropertyComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxGetDnPropertyComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.PropertyNameHandler = () => {{ return Convert.ToString({1}).ToUpper(); }};", tcxGetDnPropertyComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxGetDnPropertyComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

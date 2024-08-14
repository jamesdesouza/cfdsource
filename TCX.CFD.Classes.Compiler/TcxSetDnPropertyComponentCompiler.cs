using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxSetDnPropertyComponentCompiler : AbsComponentCompiler
{
	private readonly TcxSetDnPropertyComponent tcxSetDnPropertyComponent;

	public TcxSetDnPropertyComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxSetDnPropertyComponent tcxSetDnPropertyComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxSetDnPropertyComponent), tcxSetDnPropertyComponent.GetRootFlow().FlowType, tcxSetDnPropertyComponent)
	{
		this.tcxSetDnPropertyComponent = tcxSetDnPropertyComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxSetDnPropertyComponent.Extension))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxSetDnPropertyComponent.ExtensionIsEmpty"), tcxSetDnPropertyComponent.Name), fileObject, flowType, tcxSetDnPropertyComponent);
		}
		else if (string.IsNullOrEmpty(tcxSetDnPropertyComponent.PropertyName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxSetDnPropertyComponent.PropertyNameIsEmpty"), tcxSetDnPropertyComponent.Name), fileObject, flowType, tcxSetDnPropertyComponent);
		}
		else if (string.IsNullOrEmpty(tcxSetDnPropertyComponent.PropertyValue))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxSetDnPropertyComponent.PropertyValueIsEmpty"), tcxSetDnPropertyComponent.Name), fileObject, flowType, tcxSetDnPropertyComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxSetDnPropertyComponent.Extension);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Extension", tcxSetDnPropertyComponent.Name, tcxSetDnPropertyComponent.Extension), fileObject, flowType, tcxSetDnPropertyComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, tcxSetDnPropertyComponent.PropertyName);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "PropertyName", tcxSetDnPropertyComponent.Name, tcxSetDnPropertyComponent.PropertyName), fileObject, flowType, tcxSetDnPropertyComponent);
			}
			AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, tcxSetDnPropertyComponent.PropertyValue);
			if (!absArgument3.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "PropertyValue", tcxSetDnPropertyComponent.Name, tcxSetDnPropertyComponent.PropertyValue), fileObject, flowType, tcxSetDnPropertyComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxSetDnPropertyComponent {0} = new TcxSetDnPropertyComponent(\"{0}\", callflow, myCall, logHeader);", tcxSetDnPropertyComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.ExtensionHandler = () => {{ return Convert.ToString({1}); }};", tcxSetDnPropertyComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.PropertyNameHandler = () => {{ return Convert.ToString({1}).ToUpper(); }};", tcxSetDnPropertyComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.PropertyValueHandler = () => {{ return Convert.ToString({1}); }};", tcxSetDnPropertyComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxSetDnPropertyComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

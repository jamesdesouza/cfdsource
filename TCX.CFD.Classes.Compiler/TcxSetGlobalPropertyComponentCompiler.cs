using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxSetGlobalPropertyComponentCompiler : AbsComponentCompiler
{
	private readonly TcxSetGlobalPropertyComponent tcxSetGlobalPropertyComponent;

	public TcxSetGlobalPropertyComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxSetGlobalPropertyComponent tcxSetGlobalPropertyComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxSetGlobalPropertyComponent), tcxSetGlobalPropertyComponent.GetRootFlow().FlowType, tcxSetGlobalPropertyComponent)
	{
		this.tcxSetGlobalPropertyComponent = tcxSetGlobalPropertyComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(tcxSetGlobalPropertyComponent.PropertyName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxSetGlobalPropertyComponent.PropertyNameIsEmpty"), tcxSetGlobalPropertyComponent.Name), fileObject, flowType, tcxSetGlobalPropertyComponent);
		}
		else if (string.IsNullOrEmpty(tcxSetGlobalPropertyComponent.PropertyValue))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TcxSetGlobalPropertyComponent.PropertyValueIsEmpty"), tcxSetGlobalPropertyComponent.Name), fileObject, flowType, tcxSetGlobalPropertyComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, tcxSetGlobalPropertyComponent.PropertyName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "PropertyName", tcxSetGlobalPropertyComponent.Name, tcxSetGlobalPropertyComponent.PropertyName), fileObject, flowType, tcxSetGlobalPropertyComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, tcxSetGlobalPropertyComponent.PropertyValue);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "PropertyValue", tcxSetGlobalPropertyComponent.Name, tcxSetGlobalPropertyComponent.PropertyValue), fileObject, flowType, tcxSetGlobalPropertyComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TcxSetGlobalPropertyComponent {0} = new TcxSetGlobalPropertyComponent(\"{0}\", callflow, myCall, logHeader);", tcxSetGlobalPropertyComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.PropertyNameHandler = () => {{ return Convert.ToString({1}).ToUpper(); }};", tcxSetGlobalPropertyComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.PropertyValueHandler = () => {{ return Convert.ToString({1}); }};", tcxSetGlobalPropertyComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxSetGlobalPropertyComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class AttachCallDataComponentCompiler : AbsComponentCompiler
{
	private readonly AttachCallDataComponent attachCallDataComponent;

	public AttachCallDataComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, AttachCallDataComponent attachCallDataComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(attachCallDataComponent), attachCallDataComponent.GetRootFlow().FlowType, attachCallDataComponent)
	{
		this.attachCallDataComponent = attachCallDataComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(attachCallDataComponent.DataName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AttachCallDataComponent.DataNameIsEmpty"), attachCallDataComponent.Name), fileObject, flowType, attachCallDataComponent);
		}
		else if (string.IsNullOrEmpty(attachCallDataComponent.DataValue))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.AttachCallDataComponent.DataValueIsEmpty"), attachCallDataComponent.Name), fileObject, flowType, attachCallDataComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, attachCallDataComponent.DataName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "DataName", attachCallDataComponent.Name, attachCallDataComponent.DataName), fileObject, flowType, attachCallDataComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, attachCallDataComponent.DataValue);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "DataValue", attachCallDataComponent.Name, attachCallDataComponent.DataValue), fileObject, flowType, attachCallDataComponent);
			}
			componentsInitializationScriptSb.AppendFormat("AttachCallDataComponent {0} = new AttachCallDataComponent(\"{0}\", callflow, myCall, logHeader);", attachCallDataComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.DataNameHandler = () => {{ return Convert.ToString({1}); }};", attachCallDataComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.DataValueHandler = () => {{ return Convert.ToString({1}); }};", attachCallDataComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, attachCallDataComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

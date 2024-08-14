using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class MakeCallComponentCompiler : AbsComponentCompiler
{
	private readonly MakeCallComponent makeCallComponent;

	public MakeCallComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, MakeCallComponent makeCallComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(makeCallComponent), makeCallComponent.GetRootFlow().FlowType, makeCallComponent)
	{
		this.makeCallComponent = makeCallComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		long result;
		if (string.IsNullOrEmpty(makeCallComponent.Origin))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MakeCallComponent.OriginIsEmpty"), makeCallComponent.Name), fileObject, flowType, makeCallComponent);
		}
		else if (string.IsNullOrEmpty(makeCallComponent.Destination))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MakeCallComponent.DestinationIsEmpty"), makeCallComponent.Name), fileObject, flowType, makeCallComponent);
		}
		else if (makeCallComponent.Origin.StartsWith("0") && long.TryParse(makeCallComponent.Origin, out result))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MakeCallComponent.OriginStartsWithZero"), makeCallComponent.Name), fileObject, flowType, makeCallComponent);
		}
		else if (makeCallComponent.Destination.StartsWith("0") && long.TryParse(makeCallComponent.Destination, out result))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MakeCallComponent.DestinationStartsWithZero"), makeCallComponent.Name), fileObject, flowType, makeCallComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, makeCallComponent.Origin);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Origin", makeCallComponent.Name, makeCallComponent.Origin), fileObject, flowType, makeCallComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, makeCallComponent.Destination);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Destination", makeCallComponent.Name, makeCallComponent.Destination), fileObject, flowType, makeCallComponent);
			}
			componentsInitializationScriptSb.AppendFormat("MakeCallComponent {0} = new MakeCallComponent(\"{0}\", callflow, myCall, logHeader);", makeCallComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.OriginHandler = () => {{ return Convert.ToString({1}); }};", makeCallComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.DestinationHandler = () => {{ return Convert.ToString({1}); }};", makeCallComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.TimeoutSeconds = {1};", makeCallComponent.Name, makeCallComponent.Timeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, makeCallComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

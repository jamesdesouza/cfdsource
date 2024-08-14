using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class ExitCallflowComponentCompiler : AbsComponentCompiler
{
	private readonly ExitCallflowComponent exitCallflowComponent;

	public ExitCallflowComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, ExitCallflowComponent exitCallflowComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(exitCallflowComponent), exitCallflowComponent.GetRootFlow().FlowType, exitCallflowComponent)
	{
		this.exitCallflowComponent = exitCallflowComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		bool flag = true;
		int num = exitCallflowComponent.Parent.Activities.IndexOf(exitCallflowComponent);
		for (int i = 1 + num; i < exitCallflowComponent.Parent.Activities.Count; i++)
		{
			if (exitCallflowComponent.Parent.Activities[i].Enabled)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.DeadCodeDetected"), exitCallflowComponent.Name), fileObject, flowType, exitCallflowComponent);
		}
		componentsInitializationScriptSb.AppendFormat("ExitComponent {0} = new ExitComponent(\"{0}\", callflow, myCall, logHeader);", exitCallflowComponent.Name).AppendLine().Append("            ");
		componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, exitCallflowComponent.Name).AppendLine().Append("            ");
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

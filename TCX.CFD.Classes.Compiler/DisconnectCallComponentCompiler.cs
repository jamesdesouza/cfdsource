using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class DisconnectCallComponentCompiler : AbsComponentCompiler
{
	private readonly DisconnectCallComponent disconnectCallComponent;

	public DisconnectCallComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, DisconnectCallComponent disconnectCallComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(disconnectCallComponent), disconnectCallComponent.GetRootFlow().FlowType, disconnectCallComponent)
	{
		this.disconnectCallComponent = disconnectCallComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		bool flag = true;
		int num = disconnectCallComponent.Parent.Activities.IndexOf(disconnectCallComponent);
		for (int i = 1 + num; i < disconnectCallComponent.Parent.Activities.Count; i++)
		{
			if (disconnectCallComponent.Parent.Activities[i].Enabled)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.DeadCodeDetected"), disconnectCallComponent.Name), fileObject, flowType, disconnectCallComponent);
		}
		componentsInitializationScriptSb.AppendFormat("DisconnectCallComponent {0} = new DisconnectCallComponent(\"{0}\", callflow, myCall, logHeader);", disconnectCallComponent.Name).AppendLine().Append("            ");
		componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, disconnectCallComponent.Name).AppendLine().Append("            ");
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

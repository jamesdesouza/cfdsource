using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class PromptPlaybackComponentCompiler : AbsComponentCompiler
{
	private readonly PromptPlaybackComponent promptPlaybackComponent;

	public PromptPlaybackComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, PromptPlaybackComponent promptPlaybackComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(promptPlaybackComponent), promptPlaybackComponent.GetRootFlow().FlowType, promptPlaybackComponent)
	{
		this.promptPlaybackComponent = promptPlaybackComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (promptPlaybackComponent.Prompts.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.PromptPlaybackComponent.NoPrompts"), promptPlaybackComponent.Name), fileObject, flowType, promptPlaybackComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("PromptPlaybackComponent {0} = new PromptPlaybackComponent(\"{0}\", callflow, myCall, logHeader);", promptPlaybackComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", promptPlaybackComponent.Name, promptPlaybackComponent.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
			foreach (Prompt prompt in promptPlaybackComponent.Prompts)
			{
				prompt.Accept(this, isDebugBuild, promptPlaybackComponent.Name, "Prompts", componentsInitializationScriptSb, audioFileCollector);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, promptPlaybackComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

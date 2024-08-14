using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TranscribeAudioComponentCompiler : AbsComponentCompiler
{
	private readonly TranscribeAudioComponent transcribeAudioComponent;

	public TranscribeAudioComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TranscribeAudioComponent transcribeAudioComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(transcribeAudioComponent), transcribeAudioComponent.GetRootFlow().FlowType, transcribeAudioComponent)
	{
		this.transcribeAudioComponent = transcribeAudioComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(transcribeAudioComponent.FileName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TranscribeAudioComponent.FileNameIsEmpty"), transcribeAudioComponent.Name), fileObject, flowType, transcribeAudioComponent);
		}
		else
		{
			if (!fileObject.GetProjectObject().OnlineServices.IsReadyForSTT())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TranscribeAudioComponent.SpeechToTextOnlineServicesConfigurationIsMandatory"), transcribeAudioComponent.Name), fileObject, flowType, transcribeAudioComponent);
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, string.IsNullOrEmpty(transcribeAudioComponent.FileName) ? "\"\"" : transcribeAudioComponent.FileName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "FileName", transcribeAudioComponent.Name, transcribeAudioComponent.FileName), fileObject, flowType, transcribeAudioComponent);
			}
			componentsInitializationScriptSb.AppendFormat("TranscribeAudioComponent {0} = new TranscribeAudioComponent(\"{0}\", callflow, myCall, logHeader, onlineServices.SpeechToTextEngine);", transcribeAudioComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.LanguageCode = \"{1}\";", transcribeAudioComponent.Name, transcribeAudioComponent.LanguageCode).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FileNameHandler = () => {{ return Convert.ToString({1}); }};", transcribeAudioComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			bool flag = false;
			for (int i = 0; i < transcribeAudioComponent.Hints.Count; i++)
			{
				string text = transcribeAudioComponent.Hints[i];
				if (string.IsNullOrEmpty(text))
				{
					flag = true;
				}
				else
				{
					componentsInitializationScriptSb.AppendFormat("{0}.Hints.Add(() => {{ return \"{1}\"; }});", transcribeAudioComponent.Name, text).AppendLine().Append("            ");
				}
			}
			if (flag)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.TranscribeAudioComponent.EmptyHints"), transcribeAudioComponent.Name), fileObject, flowType, transcribeAudioComponent);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, transcribeAudioComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

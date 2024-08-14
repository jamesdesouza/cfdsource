using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Xml;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Compiler;

public class ProjectCompiler
{
	private readonly AbsCompilerResultCollector compilerResultCollector;

	private readonly ProjectObject projectObject;

	private void IncrementBuildNumber(bool isDebugBuild)
	{
		if (isDebugBuild)
		{
			projectObject.DebugBuildNumber++;
		}
		else
		{
			projectObject.ReleaseBuildNumber++;
		}
	}

	private DirectoryInfo CreateOutputFolder(bool isDebugBuild)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(projectObject.GetFolderPath() + "\\Output\\" + (isDebugBuild ? "Debug" : "Release"));
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		return directoryInfo;
	}

	private DirectoryInfo CreateBuildOutputFolder(DirectoryInfo outputDirectoryInfo, string applicationNamespace)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(outputDirectoryInfo.FullName, applicationNamespace));
		if (directoryInfo.Exists)
		{
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i].Delete(recursive: true);
			}
			FileInfo[] files = directoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				files[i].Delete();
			}
		}
		else
		{
			directoryInfo.Create();
		}
		return directoryInfo;
	}

	private DirectoryInfo CreateBuildSourcesOutputFolder(DirectoryInfo buildOutputDirectoryInfo)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(buildOutputDirectoryInfo.FullName, "Sources"));
		directoryInfo.Create();
		return directoryInfo;
	}

	private void CreateManifest(string appid, DirectoryInfo buildOutputDirectoryInfo)
	{
		using StreamWriter writer = new FileInfo(Path.Combine(buildOutputDirectoryInfo.FullName, "manifest.xml")).CreateText();
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = xmlDocument.CreateElement("cfd_app_package");
		XmlElement xmlElement2 = xmlDocument.CreateElement("name");
		xmlElement2.InnerText = appid;
		XmlElement xmlElement3 = xmlDocument.CreateElement("extension");
		xmlElement3.InnerText = projectObject.Extension;
		XmlElement xmlElement4 = xmlDocument.CreateElement("version");
		xmlElement4.InnerText = Assembly.GetExecutingAssembly().GetName().Version.ToString();
		xmlElement.AppendChild(xmlElement2);
		xmlElement.AppendChild(xmlElement3);
		xmlElement.AppendChild(xmlElement4);
		xmlDocument.AppendChild(xmlElement);
		xmlDocument.Save(writer);
	}

	private List<string> LoadExternalScripts()
	{
		List<string> list = new List<string>();
		string path = Path.Combine(projectObject.GetFolderPath(), "Libraries");
		if (Directory.Exists(path))
		{
			string[] files = Directory.GetFiles(path, "*.cs");
			for (int i = 0; i < files.Length; i++)
			{
				using StreamReader streamReader = new FileInfo(files[i]).OpenText();
				list.Add(streamReader.ReadToEnd());
			}
		}
		return list;
	}

	private void AppendFileContentAsComments(StringBuilder sb, string filePath)
	{
		string[] array = File.ReadAllLines(filePath);
		foreach (string text in array)
		{
			sb.AppendLine("// " + text);
		}
	}

	private string GetFileObjectTypeName(FileObject fileObject)
	{
		if (fileObject is CallflowFileObject)
		{
			return "Callflow";
		}
		if (fileObject is ComponentFileObject)
		{
			return "Component";
		}
		return "Dialer";
	}

	public ProjectCompiler(AbsCompilerResultCollector compilerResultCollector, ProjectObject projectObject)
	{
		this.compilerResultCollector = compilerResultCollector;
		this.projectObject = projectObject;
	}

	public CompilationResult Compile(bool isDebugBuild)
	{
		CompilerErrorCounter compilerErrorCounter = new CompilerErrorCounter();
		try
		{
			compilerResultCollector.ReportProgress(0, new CompilerEvent(string.Format(LocalizedResourceMgr.GetString("Compiler.StartBuilding"), projectObject.Name, isDebugBuild ? "debug" : "release") + Environment.NewLine));
			IncrementBuildNumber(isDebugBuild);
			if (!projectObject.DoNotAskForExtension && !compilerResultCollector.AskForExtension(projectObject))
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, 100, compilerErrorCounter, CompilerMessageTypes.Error, LocalizedResourceMgr.GetString("Compiler.BuildCanceled"));
				return CompilationResult.Cancelled;
			}
			List<CallflowFileObject> callflowFileObjectList = projectObject.GetCallflowFileObjectList();
			List<DialerFileObject> dialerFileObjectList = projectObject.GetDialerFileObjectList();
			if (callflowFileObjectList.Count == 0 && dialerFileObjectList.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, 0, compilerErrorCounter, CompilerMessageTypes.Warning, LocalizedResourceMgr.GetString("Compiler.ProjectIsEmpty"));
			}
			else
			{
				int num = 0;
				string text = NameHelper.SanitizeName(projectObject.Name);
				DirectoryInfo outputDirectoryInfo = CreateOutputFolder(isDebugBuild);
				DirectoryInfo directoryInfo = CreateBuildOutputFolder(outputDirectoryInfo, text);
				DirectoryInfo directoryInfo2 = CreateBuildSourcesOutputFolder(directoryInfo);
				List<string> list = LoadExternalScripts();
				AudioFileCollector audioFileCollector = new AudioFileCollector();
				List<ComponentFileObject> list2 = new List<ComponentFileObject>();
				foreach (CallflowFileObject item3 in callflowFileObjectList)
				{
					list2.AddRange(item3.GetComponentFileObjects());
				}
				foreach (DialerFileObject item4 in dialerFileObjectList)
				{
					list2.AddRange(item4.GetComponentFileObjects());
				}
				List<ComponentFileObject> componentFileObjectList = projectObject.GetComponentFileObjectList();
				int num2 = componentFileObjectList.Count + callflowFileObjectList.Count + dialerFileObjectList.Count;
				foreach (ComponentFileObject item5 in componentFileObjectList)
				{
					if (list2.Contains(item5))
					{
						compilerResultCollector.ReportProgress(50 * num / num2, new CompilerEvent(string.Format(LocalizedResourceMgr.GetString("Compiler.CompilingFile"), item5.Name, item5.Path)));
						ComponentFileCompiler componentFileCompiler = new ComponentFileCompiler(compilerResultCollector, item5, 20 * num / num2, compilerErrorCounter);
						if (componentFileCompiler.Compile(isDebugBuild, audioFileCollector) == CompilationResult.Cancelled)
						{
							return CompilationResult.Cancelled;
						}
						string text2 = item5.Name.Substring(0, item5.Name.Length - 4) + "cs";
						string newValue = NameHelper.SanitizeName(text2.Substring(0, text2.Length - 3));
						string item = Resources.CustomUserComponent_cs.Replace("CustomUserComponent", newValue).Replace("[VARIABLES_HANDLERS_PLACEHOLDER]", componentFileCompiler.VariablesHandlersInitializationScript).Replace("[VARIABLES_PLACEHOLDER]", componentFileCompiler.VariablesInitializationScript)
							.Replace("[COMPONENTS_PLACEHOLDER]", componentFileCompiler.ComponentsInitializationScript)
							.Replace("[VARIABLES_HANDLERS_INVOCATION_PLACEHOLDER]", componentFileCompiler.VariablesHandlersInvocationInitializationScript)
							.Replace("[VARIABLES_HANDLERS_SETTERS_PLACEHOLDER]", componentFileCompiler.VariablesHandlersSettersInitializationScript);
						list.Add(item);
						string externalCodeLauncher = componentFileCompiler.ExternalCodeLauncher;
						if (!string.IsNullOrEmpty(externalCodeLauncher))
						{
							list.Add(externalCodeLauncher);
						}
					}
					else
					{
						compilerResultCollector.ReportProgress(50 * num / num2, new CompilerEvent(string.Format(LocalizedResourceMgr.GetString("Compiler.SkippingFile"), item5.Name, item5.Path)));
					}
					num++;
				}
				if (dialerFileObjectList.Count > 0)
				{
					DialerFileObject dialerFileObject = dialerFileObjectList[0];
					bool flag = dialerFileObject.DialerMode == DialerModes.PredictiveDialer;
					int parallelDialers = dialerFileObject.ParallelDialers;
					int pauseBetweenDialerExecution = dialerFileObject.PauseBetweenDialerExecution;
					string queue = dialerFileObject.Queue;
					bool flag2 = dialerFileObject.Optimization == PredictiveDialerOptimizations.ForAgents;
					if (flag && string.IsNullOrEmpty(queue))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, 0, compilerErrorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.PredictiveDialerQueueNotSet"), dialerFileObject.Name));
					}
					compilerResultCollector.ReportProgress(50 * num / num2, new CompilerEvent(string.Format(LocalizedResourceMgr.GetString("Compiler.CompilingFile"), dialerFileObject.Name, dialerFileObject.Path)));
					FileCompiler fileCompiler = new DialerFileCompiler(compilerResultCollector, dialerFileObject, 20 * num / num2, compilerErrorCounter);
					if (fileCompiler.Compile(isDebugBuild, audioFileCollector) == CompilationResult.Cancelled)
					{
						return CompilationResult.Cancelled;
					}
					string item2 = Resources.Dialer_cs.Replace("[VARIABLES_PLACEHOLDER]", fileCompiler.VariablesInitializationScript).Replace("[COMPONENTS_PLACEHOLDER]", fileCompiler.ComponentsInitializationScript).Replace("[PREDICTIVE_DIALER]", flag ? "true" : "false")
						.Replace("[PARALLEL_DIALERS]", parallelDialers.ToString())
						.Replace("[PAUSE_BETWEEN_DIALER_EXECUTION]", pauseBetweenDialerExecution.ToString())
						.Replace("[PREDICTIVE_DIALER_QUEUE]", queue)
						.Replace("[PREDICTIVE_DIALER_OPTIMIZED_FOR_AGENTS]", flag2 ? "true" : "false");
					list.Add(item2);
					string externalCodeLauncher2 = fileCompiler.ExternalCodeLauncher;
					if (!string.IsNullOrEmpty(externalCodeLauncher2))
					{
						list.Add(externalCodeLauncher2);
					}
					num++;
				}
				string newValue2;
				string newValue3;
				string text3;
				if (callflowFileObjectList.Count == 0)
				{
					newValue2 = string.Empty;
					newValue3 = string.Empty;
					text3 = "Callflow";
				}
				else
				{
					CallflowFileObject callflowFileObject = callflowFileObjectList[0];
					text3 = NameHelper.SanitizeName(callflowFileObject.GetNameWithoutExtension());
					compilerResultCollector.ReportProgress(50 * num / num2, new CompilerEvent(string.Format(LocalizedResourceMgr.GetString("Compiler.CompilingFile"), callflowFileObject.Name, callflowFileObject.Path)));
					FileCompiler fileCompiler2 = new CallflowFileCompiler(compilerResultCollector, callflowFileObject, 20 * num / num2, compilerErrorCounter);
					if (fileCompiler2.Compile(isDebugBuild, audioFileCollector) == CompilationResult.Cancelled)
					{
						return CompilationResult.Cancelled;
					}
					newValue2 = fileCompiler2.VariablesInitializationScript;
					newValue3 = fileCompiler2.ComponentsInitializationScript;
					if (callflowFileObjectList.Count > 1)
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, 20 * num / num2, compilerErrorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.IgnoringCallflows"), callflowFileObject.Name));
					}
					string externalCodeLauncher3 = fileCompiler2.ExternalCodeLauncher;
					if (!string.IsNullOrEmpty(externalCodeLauncher3))
					{
						list.Add(externalCodeLauncher3);
					}
					num++;
				}
				compilerResultCollector.ReportProgress(60, new CompilerEvent(LocalizedResourceMgr.GetString("Compiler.CreatingSourceCodeOutput")));
				if (!projectObject.OnlineServices.IsTTSProperlyConfigured())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, 0, compilerErrorCounter, CompilerMessageTypes.Error, LocalizedResourceMgr.GetString("Compiler.InvalidTextToSpeechConfiguration"));
				}
				if (!projectObject.OnlineServices.IsSTTProperlyConfigured())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, 0, compilerErrorCounter, CompilerMessageTypes.Error, LocalizedResourceMgr.GetString("Compiler.InvalidSpeechToTextConfiguration"));
				}
				string content = ScriptCodeSnipetLoader.Load(list).Replace("[TEXT_TO_SPEECH_ENGINE_INITIALIZATION]", projectObject.OnlineServices.GetTTSInitializationCode()).Replace("[SPEECH_TO_TEXT_ENGINE_INITIALIZATION]", projectObject.OnlineServices.GetSTTInitializationCode())
					.Replace("[VARIABLES_PLACEHOLDER]", newValue2)
					.Replace("[COMPONENTS_PLACEHOLDER]", newValue3)
					.Replace("[DIALER_INITIALIZATION]", (dialerFileObjectList.Count == 0) ? string.Empty : Resources.DialerInitialization_cs)
					.Replace("public class Callflow : ScriptBase<Callflow>", "public class " + text3 + " : ScriptBase<" + text3 + ">")
					.Replace("public Callflow", "public " + text3)
					.Replace("SCRIPT_NAMESPACE", text);
				CompilerHelper.WriteOutput(Path.Combine(directoryInfo2.FullName, text3 + ".cs"), content);
				audioFileCollector.CopyRequiredAudioFiles(projectObject, compilerResultCollector, 65, compilerErrorCounter, directoryInfo.FullName);
				if (compilerErrorCounter.ErrorCount == 0)
				{
					string appid = text.ToLower() + "." + text3;
					CreateManifest(appid, directoryInfo);
					compilerResultCollector.ReportProgress(70, new CompilerEvent(string.Format(LocalizedResourceMgr.GetString("Compiler.ZippingOutputFiles"), projectObject.Name)));
					string text4 = directoryInfo.FullName + ".zip";
					if (File.Exists(text4))
					{
						File.Delete(text4);
					}
					ZipFile.CreateFromDirectory(directoryInfo.FullName, text4, CompressionLevel.Optimal, includeBaseDirectory: false);
					compilerResultCollector.ReportProgress(90, new CompilerEvent(string.Format(LocalizedResourceMgr.GetString("Compiler.CleaningUpOutputFiles"), projectObject.Name)));
					Directory.Delete(directoryInfo.FullName, recursive: true);
					compilerResultCollector.ReportProgress(95, new CompilerEvent(LocalizedResourceMgr.GetString("Compiler.BuildSuccessPrefix") + " ", addNewLineEnding: false));
					compilerResultCollector.ReportProgress(95, new CompilerEvent(LocalizedResourceMgr.GetString("Compiler.BuildSuccessLink"), "file:///" + Uri.EscapeUriString(text4), addNewLineEnding: false));
					compilerResultCollector.ReportProgress(95, new CompilerEvent(" " + LocalizedResourceMgr.GetString("Compiler.BuildSuccessSuffix")));
				}
				else
				{
					compilerResultCollector.ReportProgress(95, new CompilerEvent(LocalizedResourceMgr.GetString("Compiler.BuildError")));
				}
			}
			if (isDebugBuild && !compilerResultCollector.CancellationPending && compilerErrorCounter.ErrorCount == 0)
			{
				projectObject.ChangedSinceLastDebugBuild = false;
			}
			compilerResultCollector.ReportProgress(100, new CompilerEvent(Environment.NewLine + string.Format(LocalizedResourceMgr.GetString("Compiler.FinishBuilding"), projectObject.Name) + Environment.NewLine + Environment.NewLine));
			return compilerResultCollector.CancellationPending ? CompilationResult.Cancelled : CompilationResult.Finished;
		}
		catch (Exception exc)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, 100, compilerErrorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.GeneralErrorBuilding"), ErrorHelper.GetErrorDescription(exc)));
			return CompilationResult.Finished;
		}
	}
}

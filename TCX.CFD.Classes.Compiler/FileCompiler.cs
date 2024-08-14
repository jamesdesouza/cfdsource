using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public abstract class FileCompiler
{
	private readonly AbsCompilerResultCollector compilerResultCollector;

	private readonly FileObject fileObject;

	private readonly int progress;

	private readonly CompilerErrorCounter errorCounter;

	private readonly StringBuilder variablesInitializationScriptSb;

	private readonly StringBuilder componentsInitializationScriptSb;

	private readonly StringBuilder externalCodeLauncherSb;

	private readonly List<string> validSessionVariableList;

	public string VariablesInitializationScript => variablesInitializationScriptSb.ToString();

	public string ComponentsInitializationScript => componentsInitializationScriptSb.ToString();

	public string ExternalCodeLauncher => externalCodeLauncherSb.ToString();

	protected void AddVariable(string listName, string objectName, Variable variable, bool systemVariable, bool extraTab)
	{
		string expression = (string.IsNullOrEmpty(variable.InitialValue) ? "\"\"" : variable.InitialValue);
		AbsArgument absArgument = AbsArgument.BuildArgument(validSessionVariableList, expression);
		if (!systemVariable && !absArgument.IsSafeExpression())
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Variable - " + variable.Name, fileObject.Name, variable.InitialValue), fileObject, FlowTypes.MainFlow, null);
		}
		variablesInitializationScriptSb.AppendFormat("{0}[\"{1}\"] = new Variable({2});", listName, objectName + "." + variable.Name, absArgument.GetCompilerString()).AppendLine().Append(extraTab ? "                " : "            ");
	}

	protected void AddVariables(string listName, string objectName, List<Variable> variableList, bool systemVariable, bool extraTab)
	{
		foreach (Variable variable in variableList)
		{
			AddVariable(listName, objectName, variable, systemVariable, extraTab);
		}
	}

	private string GetFlowComponentListName(FlowTypes flowType)
	{
		return flowType switch
		{
			FlowTypes.MainFlow => "mainFlowComponentList", 
			FlowTypes.ErrorHandler => "errorFlowComponentList", 
			FlowTypes.DisconnectHandler => "disconnectFlowComponentList", 
			_ => throw new InvalidEnumArgumentException(LocalizedResourceMgr.GetString("FlowLoader.MessageBox.Error.FlowTypeIsInvalid")), 
		};
	}

	private CompilationResult CompileRootFlow(bool isDebugBuild, RootFlow rootFlow, AudioFileCollector audioFileCollector)
	{
		string flowComponentListName = GetFlowComponentListName(rootFlow.FlowType);
		componentsInitializationScriptSb.Append("{").AppendLine().Append("            ");
		ReadOnlyCollection<Activity> enabledActivities = rootFlow.EnabledActivities;
		for (int i = 0; i < enabledActivities.Count; i++)
		{
			if ((enabledActivities[i] as IVadActivity).GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, flowComponentListName, componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
			{
				return CompilationResult.Cancelled;
			}
		}
		componentsInitializationScriptSb.Append("}").AppendLine().Append("            ");
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}

	protected FileCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		this.compilerResultCollector = compilerResultCollector;
		this.fileObject = fileObject;
		this.progress = progress;
		this.errorCounter = errorCounter;
		variablesInitializationScriptSb = new StringBuilder();
		componentsInitializationScriptSb = new StringBuilder();
		externalCodeLauncherSb = new StringBuilder();
		validSessionVariableList = new List<string>();
		validSessionVariableList.Add("session.ani");
		validSessionVariableList.Add("session.callid");
		validSessionVariableList.Add("session.dnis");
		validSessionVariableList.Add("session.did");
		validSessionVariableList.Add("session.audioFolder");
		validSessionVariableList.Add("session.transferingExtension");
	}

	public CompilationResult Compile(bool isDebugBuild, AudioFileCollector audioFileCollector)
	{
		RootFlow rootFlow = fileObject.FlowLoader.GetRootFlow(FlowTypes.MainFlow);
		if (rootFlow.EnabledActivities.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.MainFlowIsEmpty"), fileObject.Name), fileObject, FlowTypes.MainFlow, null);
		}
		else if (CompileRootFlow(isDebugBuild, rootFlow, audioFileCollector) == CompilationResult.Cancelled)
		{
			return CompilationResult.Cancelled;
		}
		if (CompileRootFlow(isDebugBuild, fileObject.FlowLoader.GetRootFlow(FlowTypes.ErrorHandler), audioFileCollector) == CompilationResult.Cancelled)
		{
			return CompilationResult.Cancelled;
		}
		if (fileObject.NeedsDisconnectHandlerFlow && CompileRootFlow(isDebugBuild, fileObject.FlowLoader.GetRootFlow(FlowTypes.DisconnectHandler), audioFileCollector) == CompilationResult.Cancelled)
		{
			return CompilationResult.Cancelled;
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}

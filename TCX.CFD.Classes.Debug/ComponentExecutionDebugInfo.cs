using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Debug;

public class ComponentExecutionDebugInfo
{
	private FileObject fileObject;

	private FlowTypes flowType;

	private IVadActivity vadActivity;

	private readonly List<VariableChangeDebugInfo> variableChangeDebugInfoList = new List<VariableChangeDebugInfo>();

	private const string StartCallflowExecution = "START_CALLFLOW_EXECUTION: ";

	private const string VariableChanged = "VARIABLE_CHANGED: ";

	private const string EndExecution = "END_EXECUTION: ";

	public FileObject FileObject => fileObject;

	public FlowTypes FlowType => flowType;

	public IVadActivity VadActivity => vadActivity;

	public List<VariableChangeDebugInfo> VariableChangeDebugInfoList => variableChangeDebugInfoList;

	private Activity FindActivity(ActivityCollection activityCollection, string name)
	{
		foreach (Activity item in activityCollection)
		{
			if (item.Name == name)
			{
				return item;
			}
			if (item is CompositeActivity)
			{
				Activity activity = FindActivity((item as CompositeActivity).Activities, name);
				if (activity != null)
				{
					return activity;
				}
			}
		}
		return null;
	}

	public bool ProcessLine(string line, ProjectObject projectObject)
	{
		if (line.StartsWith("START_CALLFLOW_EXECUTION: "))
		{
			return false;
		}
		if (line.StartsWith("VARIABLE_CHANGED: "))
		{
			variableChangeDebugInfoList.Add(new VariableChangeDebugInfo(line.Substring("VARIABLE_CHANGED: ".Length)));
			return false;
		}
		if (line.StartsWith("END_EXECUTION: "))
		{
			string[] array = line.Substring("END_EXECUTION: ".Length).Split(new string[1] { " - " }, StringSplitOptions.None);
			if (array.Length != 3)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("DebugInformationValidation.MessageBox.Error.InvalidLine"), line));
			}
			string text = array[0];
			string text2 = array[1];
			string text3 = array[2];
			fileObject = projectObject.GetFileSystemObject(text) as FileObject;
			if (fileObject == null)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("DebugInformationValidation.MessageBox.Error.FileNotFound"), text));
			}
			switch (text3)
			{
			case "MF":
				flowType = FlowTypes.MainFlow;
				break;
			case "EH":
				flowType = FlowTypes.ErrorHandler;
				break;
			case "DH":
				flowType = FlowTypes.DisconnectHandler;
				break;
			default:
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("DebugInformationValidation.MessageBox.Error.InvalidLine"), line));
			}
			RootFlow rootFlow = fileObject.FlowLoader.GetRootFlow(flowType);
			if (string.IsNullOrEmpty(text2))
			{
				vadActivity = rootFlow;
			}
			else
			{
				vadActivity = FindActivity(rootFlow.Activities, text2) as IVadActivity;
			}
			if (vadActivity == null)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("DebugInformationValidation.MessageBox.Error.ComponentNotFound"), text2));
			}
			return true;
		}
		throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("DebugInformationValidation.MessageBox.Error.InvalidLine"), line));
	}
}

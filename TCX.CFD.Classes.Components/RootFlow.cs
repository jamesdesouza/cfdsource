using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Workflow.Activities;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

public abstract class RootFlow : SequentialWorkflowActivity, IVadActivity, IVadRootActivity
{
	protected bool debugModeActive;

	private FileObject fileObject;

	[Browsable(false)]
	public bool DebugModeActive
	{
		get
		{
			return debugModeActive;
		}
		set
		{
			debugModeActive = value;
		}
	}

	public abstract FlowTypes FlowType { get; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public FileObject FileObject
	{
		get
		{
			return fileObject;
		}
		set
		{
			fileObject = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public List<Variable> Properties => fileObject.Variables;

	public RootFlow()
	{
		InitializeComponent();
	}

	public List<ComponentFileObject> GetComponentFileObjects()
	{
		List<ComponentFileObject> list = new List<ComponentFileObject>();
		foreach (IVadActivity enabledActivity in base.EnabledActivities)
		{
			if (enabledActivity != null)
			{
				list.AddRange(enabledActivity.GetComponentFileObjects());
			}
		}
		return list;
	}

	public RootFlow GetRootFlow()
	{
		return this;
	}

	public void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (IVadActivity activity in base.Activities)
		{
			activity?.NotifyComponentRenamed(oldValue, newValue);
		}
	}

	public bool DisableUserComponent(ComponentFileObject componentFileObject)
	{
		bool result = false;
		foreach (IVadActivity activity in base.Activities)
		{
			if (activity != null && activity.DisableUserComponent(componentFileObject))
			{
				result = true;
			}
		}
		return result;
	}

	public bool IsUsingUserComponent(ComponentFileObject componentFileObject)
	{
		foreach (IVadActivity activity in base.Activities)
		{
			if (activity != null && activity.IsUsingUserComponent(componentFileObject))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsCallRelated()
	{
		return false;
	}

	public void MigrateConstantStringExpressions()
	{
	}

	public AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return null;
	}

	public void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-workspace/#h.9emcy6sp073c");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "RootFlow";
	}
}

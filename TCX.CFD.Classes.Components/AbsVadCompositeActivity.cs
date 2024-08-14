using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DefaultProperty("Name")]
public abstract class AbsVadCompositeActivity : CompositeActivity, IVadActivity
{
	protected bool debugModeActive;

	protected List<Variable> properties = new List<Variable>();

	[Category("General")]
	[Description("A tag for this component which will be shown in the designer.")]
	public string Tag { get; set; } = "";


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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public List<Variable> Properties => properties;

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

	public virtual void NotifyComponentRenamed(string oldValue, string newValue)
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

	public RootFlow GetRootFlow()
	{
		return (base.Parent as IVadActivity).GetRootFlow();
	}

	public abstract bool IsCallRelated();

	public abstract void MigrateConstantStringExpressions();

	public abstract AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter);

	public abstract void ShowHelp();
}

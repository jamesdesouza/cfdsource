using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DefaultProperty("Name")]
public abstract class AbsVadActivity : Activity, IVadActivity
{
	protected bool debugModeActive;

	protected List<Variable> properties = new List<Variable>();

	private string tag = "";

	[Category("General")]
	[Description("A tag for this component which will be shown in the designer.")]
	public string Tag
	{
		get
		{
			return tag;
		}
		set
		{
			tag = value;
			if (base.Site != null && base.Site.Container is IDesignerHost designerHost && designerHost.GetDesigner(this) is ComponentDesigner componentDesigner)
			{
				componentDesigner.Invalidate();
				componentDesigner.EnsureVisible();
			}
		}
	}

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
	public List<Variable> Properties
	{
		get
		{
			OnBeforeReadProperties();
			return properties;
		}
	}

	protected virtual void OnBeforeReadProperties()
	{
	}

	public virtual List<ComponentFileObject> GetComponentFileObjects()
	{
		return new List<ComponentFileObject>();
	}

	public abstract void NotifyComponentRenamed(string oldValue, string newValue);

	public virtual bool DisableUserComponent(ComponentFileObject componentFileObject)
	{
		return false;
	}

	public virtual bool IsUsingUserComponent(ComponentFileObject componentFileObject)
	{
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

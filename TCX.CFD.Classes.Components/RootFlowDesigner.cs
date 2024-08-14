using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

public class RootFlowDesigner : SequentialWorkflowRootDesigner
{
	private ReadOnlyCollection<DesignerView> rootFlowViews;

	protected override string HelpText
	{
		get
		{
			return LocalizedResourceMgr.GetString("ComponentDesigners.HelpText");
		}
		set
		{
			base.HelpText = value;
		}
	}

	public override ReadOnlyCollection<DesignerView> Views
	{
		get
		{
			if (rootFlowViews == null)
			{
				List<DesignerView> list = new List<DesignerView>();
				list.Add(base.Views[0]);
				rootFlowViews = new ReadOnlyCollection<DesignerView>(list);
			}
			return rootFlowViews;
		}
	}

	public event EventHandler LayoutChanged;

	public override bool CanInsertActivities(HitTestInfo insertLocation, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		foreach (Activity item in activitiesToInsert)
		{
			if (!(item is IVadActivity))
			{
				return false;
			}
			if (item is ComponentBranch || item is ConditionalComponentBranch || item is MenuComponentBranch || item is DateTimeConditionalComponentBranch || item is ParallelExecutionComponentBranch)
			{
				return false;
			}
		}
		if (base.Activity is RootFlow { FileObject: { } fileObject })
		{
			if (fileObject is DialerFileObject)
			{
				return ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(fileObject.GetProjectObject(), activitiesToInsert);
			}
			if (fileObject is ComponentFileObject)
			{
				return ComponentDesignerHelper.CanInsertActivitiesCheckingUserComponent(fileObject as ComponentFileObject, activitiesToInsert);
			}
		}
		return true;
	}

	protected override void OnLayoutPosition(ActivityDesignerLayoutEventArgs e)
	{
		base.OnLayoutPosition(e);
		this.LayoutChanged?.Invoke(this, EventArgs.Empty);
	}

	public void SetDesignerTitle(string title)
	{
		Header.Text = title;
	}
}

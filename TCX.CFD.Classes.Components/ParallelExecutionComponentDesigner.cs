using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(ParallelExecutionComponentTheme))]
public class ParallelExecutionComponentDesigner : CompositeComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection
				{
					new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.ParallelExecutionComponent.AddBranchVerb"), OnAddBranch)
				};
			}
			return verbs;
		}
	}

	private void ActivitiesChanged(object sender, ActivityCollectionChangeEventArgs e)
	{
		foreach (ParallelExecutionComponentBranchDesigner containedDesigner in ContainedDesigners)
		{
			containedDesigner.EnableDisableVerbs();
		}
	}

	private void OnAddBranch(object sender, EventArgs args)
	{
		CompositeActivity item = OnCreateNewBranch();
		List<Activity> list = new List<Activity> { item };
		CompositeActivityDesigner.InsertActivities(this, new ConnectorHitTestInfo(this, HitTestLocations.Connector, ((ParallelExecutionComponent)base.Activity).Activities.Count), new ReadOnlyCollection<Activity>(list), "Adding parallel execution branch");
	}

	public override bool CanInsertActivities(HitTestInfo insertLocation, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		foreach (Activity item in activitiesToInsert)
		{
			if (!(item is ParallelExecutionComponentBranch))
			{
				return false;
			}
		}
		RootFlow rootFlow = ((IVadActivity)base.Activity).GetRootFlow();
		if (rootFlow != null)
		{
			FileObject fileObject = rootFlow.FileObject;
			if (fileObject != null)
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
		}
		return true;
	}

	public override bool CanRemoveActivities(ReadOnlyCollection<Activity> activitiesToRemove)
	{
		ParallelExecutionComponent parallelExecutionComponent = base.Activity as ParallelExecutionComponent;
		return activitiesToRemove.Count < parallelExecutionComponent.Activities.Count - 1;
	}

	protected override CompositeActivity OnCreateNewBranch()
	{
		return new ParallelExecutionComponentBranch();
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Parallel Execution", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}

	protected override void Initialize(Activity activity)
	{
		base.Initialize(activity);
		(activity as ParallelExecutionComponent).Activities.ListChanged += ActivitiesChanged;
	}
}

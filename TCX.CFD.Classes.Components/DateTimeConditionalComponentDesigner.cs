using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(DateTimeConditionalComponentTheme))]
public class DateTimeConditionalComponentDesigner : CompositeComponentDesigner
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
					new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.DateTimeConditionalComponent.AddBranchVerb"), OnAddBranch)
				};
			}
			return verbs;
		}
	}

	private void ActivitiesChanged(object sender, ActivityCollectionChangeEventArgs e)
	{
		foreach (DateTimeConditionalComponentBranchDesigner containedDesigner in ContainedDesigners)
		{
			containedDesigner.EnableDisableVerbs();
		}
	}

	private void OnAddBranch(object sender, EventArgs args)
	{
		CompositeActivity item = OnCreateNewBranch();
		List<Activity> list = new List<Activity> { item };
		CompositeActivityDesigner.InsertActivities(this, new ConnectorHitTestInfo(this, HitTestLocations.Connector, ((DateTimeConditionalComponent)base.Activity).Activities.Count), new ReadOnlyCollection<Activity>(list), "Adding date time conditional branch");
	}

	public override bool CanInsertActivities(HitTestInfo insertLocation, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		foreach (Activity item in activitiesToInsert)
		{
			if (!(item is DateTimeConditionalComponentBranch))
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
		DateTimeConditionalComponent dateTimeConditionalComponent = base.Activity as DateTimeConditionalComponent;
		return activitiesToRemove.Count < dateTimeConditionalComponent.Activities.Count;
	}

	protected override CompositeActivity OnCreateNewBranch()
	{
		return new DateTimeConditionalComponentBranch();
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Date time condition", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}

	protected override void Initialize(Activity activity)
	{
		base.Initialize(activity);
		(activity as DateTimeConditionalComponent).Activities.ListChanged += ActivitiesChanged;
	}
}

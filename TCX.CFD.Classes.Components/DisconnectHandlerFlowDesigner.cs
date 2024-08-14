using System.Collections.ObjectModel;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

public class DisconnectHandlerFlowDesigner : RootFlowDesigner
{
	public override bool CanInsertActivities(HitTestInfo insertLocation, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		if (!base.CanInsertActivities(insertLocation, activitiesToInsert))
		{
			return false;
		}
		return ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall((base.Activity as DisconnectHandlerFlow).FileObject.GetProjectObject(), activitiesToInsert);
	}
}

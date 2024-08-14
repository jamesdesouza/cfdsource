using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class DateTimeConditionalComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		CompositeActivity compositeActivity = new DateTimeConditionalComponent();
		compositeActivity.Activities.Add(new DateTimeConditionalComponentBranch());
		compositeActivity.Activities.Add(new DateTimeConditionalComponentBranch());
		FlowDesignerNameCreator.CreateName("CreateDateTimeCondition", host.Container, compositeActivity);
		return new IComponent[1] { compositeActivity };
	}
}

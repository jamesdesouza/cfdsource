using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class ConditionalComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		CompositeActivity compositeActivity = new ConditionalComponent();
		compositeActivity.Activities.Add(new ConditionalComponentBranch());
		compositeActivity.Activities.Add(new ConditionalComponentBranch());
		FlowDesignerNameCreator.CreateName("CreateCondition", host.Container, compositeActivity);
		return new IComponent[1] { compositeActivity };
	}
}

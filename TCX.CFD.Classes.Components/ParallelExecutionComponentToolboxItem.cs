using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class ParallelExecutionComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		CompositeActivity compositeActivity = new ParallelExecutionComponent();
		compositeActivity.Activities.Add(new ParallelExecutionComponentBranch());
		compositeActivity.Activities.Add(new ParallelExecutionComponentBranch());
		FlowDesignerNameCreator.CreateName("ParallelExecution", host.Container, compositeActivity);
		return new IComponent[1] { compositeActivity };
	}
}

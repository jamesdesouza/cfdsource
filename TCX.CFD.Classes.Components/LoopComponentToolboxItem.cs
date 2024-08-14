using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class LoopComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		CompositeActivity compositeActivity = new LoopComponent();
		FlowDesignerNameCreator.CreateName("Loop", host.Container, compositeActivity);
		return new IComponent[1] { compositeActivity };
	}
}

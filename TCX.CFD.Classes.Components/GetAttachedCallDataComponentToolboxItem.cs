using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class GetAttachedCallDataComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		Activity activity = new GetAttachedCallDataComponent();
		FlowDesignerNameCreator.CreateName("GetAttachedCallData", host.Container, activity);
		return new IComponent[1] { activity };
	}
}

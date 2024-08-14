using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class AttachCallDataComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		Activity activity = new AttachCallDataComponent();
		FlowDesignerNameCreator.CreateName("AttachCallData", host.Container, activity);
		return new IComponent[1] { activity };
	}
}

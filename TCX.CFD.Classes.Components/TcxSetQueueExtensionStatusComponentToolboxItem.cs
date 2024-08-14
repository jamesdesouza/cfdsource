using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class TcxSetQueueExtensionStatusComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		Activity activity = new TcxSetQueueExtensionStatusComponent();
		FlowDesignerNameCreator.CreateName("SetQueueExtensionStatus", host.Container, activity);
		return new IComponent[1] { activity };
	}
}

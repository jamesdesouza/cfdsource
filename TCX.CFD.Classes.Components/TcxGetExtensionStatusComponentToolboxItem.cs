using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class TcxGetExtensionStatusComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		Activity activity = new TcxGetExtensionStatusComponent();
		FlowDesignerNameCreator.CreateName("GetExtensionStatus", host.Container, activity);
		return new IComponent[1] { activity };
	}
}

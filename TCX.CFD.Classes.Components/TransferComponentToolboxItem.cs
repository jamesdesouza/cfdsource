using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class TransferComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		TransferComponent transferComponent = new TransferComponent();
		FlowDesignerNameCreator.CreateName("Transfer", host.Container, transferComponent);
		return new IComponent[1] { transferComponent };
	}
}

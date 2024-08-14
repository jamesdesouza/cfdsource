using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class MakeCallComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		MakeCallComponent makeCallComponent = new MakeCallComponent();
		FlowDesignerNameCreator.CreateName("MakeCall", host.Container, makeCallComponent);
		return new IComponent[1] { makeCallComponent };
	}
}

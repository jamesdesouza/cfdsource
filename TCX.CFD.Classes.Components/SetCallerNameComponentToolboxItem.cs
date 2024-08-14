using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class SetCallerNameComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		SetCallerNameComponent setCallerNameComponent = new SetCallerNameComponent();
		FlowDesignerNameCreator.CreateName("SetCallerName", host.Container, setCallerNameComponent);
		return new IComponent[1] { setCallerNameComponent };
	}
}

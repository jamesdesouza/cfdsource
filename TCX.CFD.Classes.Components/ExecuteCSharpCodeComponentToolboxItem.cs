using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class ExecuteCSharpCodeComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		ExecuteCSharpCodeComponent executeCSharpCodeComponent = new ExecuteCSharpCodeComponent();
		FlowDesignerNameCreator.CreateName("ExecuteCSharpCode", host.Container, executeCSharpCodeComponent);
		return new IComponent[1] { executeCSharpCodeComponent };
	}
}

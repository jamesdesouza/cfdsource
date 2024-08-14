using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class ExternalCodeExecutionComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		ExternalCodeExecutionComponent externalCodeExecutionComponent = new ExternalCodeExecutionComponent();
		FlowDesignerNameCreator.CreateName("ExecuteCSharpFile", host.Container, externalCodeExecutionComponent);
		return new IComponent[1] { externalCodeExecutionComponent };
	}
}

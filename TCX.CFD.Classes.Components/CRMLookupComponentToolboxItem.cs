using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class CRMLookupComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		CRMLookupComponent cRMLookupComponent = new CRMLookupComponent();
		FlowDesignerNameCreator.CreateName("CRMLookup", host.Container, cRMLookupComponent);
		return new IComponent[1] { cRMLookupComponent };
	}
}

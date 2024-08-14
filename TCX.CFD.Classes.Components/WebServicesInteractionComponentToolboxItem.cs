using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class WebServicesInteractionComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		WebServicesInteractionComponent webServicesInteractionComponent = new WebServicesInteractionComponent();
		webServicesInteractionComponent.Timeout = Settings.Default.WebServicesInteractionTemplateTimeout;
		FlowDesignerNameCreator.CreateName("WebServicePost", host.Container, webServicesInteractionComponent);
		return new IComponent[1] { webServicesInteractionComponent };
	}
}

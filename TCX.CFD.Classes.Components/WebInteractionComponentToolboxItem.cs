using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class WebInteractionComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		WebInteractionComponent webInteractionComponent = new WebInteractionComponent();
		webInteractionComponent.HttpRequestType = EnumHelper.StringToHttpRequestType(Settings.Default.WebInteractionTemplateHttpRequestType);
		webInteractionComponent.Timeout = Settings.Default.WebInteractionTemplateTimeout;
		FlowDesignerNameCreator.CreateName("HttpRequest", host.Container, webInteractionComponent);
		return new IComponent[1] { webInteractionComponent };
	}
}

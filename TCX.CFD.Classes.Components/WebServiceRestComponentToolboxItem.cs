using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class WebServiceRestComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		WebServiceRestComponent webServiceRestComponent = new WebServiceRestComponent();
		webServiceRestComponent.HttpRequestType = EnumHelper.StringToHttpRequestType(Settings.Default.WebServiceRestTemplateHttpRequestType);
		webServiceRestComponent.Timeout = Settings.Default.WebServiceRestTemplateTimeout;
		webServiceRestComponent.AuthenticationType = EnumHelper.StringToWebServiceAuthenticationType(Settings.Default.WebServiceRestTemplateAuthenticationType);
		FlowDesignerNameCreator.CreateName("WebServiceRest", host.Container, webServiceRestComponent);
		return new IComponent[1] { webServiceRestComponent };
	}
}

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class SocketClientComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		SocketClientComponent socketClientComponent = new SocketClientComponent();
		socketClientComponent.ConnectionType = ((!(Settings.Default.SocketClientTemplateConnectionType == "TCP")) ? SocketConnectionTypes.UDP : SocketConnectionTypes.TCP);
		socketClientComponent.Host = (string.IsNullOrEmpty(Settings.Default.SocketClientTemplateHost) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.SocketClientTemplateHost) + "\""));
		socketClientComponent.Port = Settings.Default.SocketClientTemplatePort.ToString();
		socketClientComponent.WaitForResponse = Settings.Default.SocketClientTemplateWaitForResponse;
		FlowDesignerNameCreator.CreateName("OpenSocket", host.Container, socketClientComponent);
		return new IComponent[1] { socketClientComponent };
	}
}

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class JsonXmlParserComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		JsonXmlParserComponent jsonXmlParserComponent = new JsonXmlParserComponent();
		FlowDesignerNameCreator.CreateName("JsonXmlParser", host.Container, jsonXmlParserComponent);
		return new IComponent[1] { jsonXmlParserComponent };
	}
}

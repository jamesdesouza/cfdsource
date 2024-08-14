using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class CsvParserComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		CsvParserComponent csvParserComponent = new CsvParserComponent();
		FlowDesignerNameCreator.CreateName("CsvParser", host.Container, csvParserComponent);
		return new IComponent[1] { csvParserComponent };
	}
}

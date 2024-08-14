using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Workflow.ComponentModel.Serialization;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(DisconnectHandlerFlowDesigner), typeof(IRootDesigner))]
public class DisconnectHandlerFlow : RootFlow
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override FlowTypes FlowType => FlowTypes.DisconnectHandler;

	public DisconnectHandlerFlow()
	{
		InitializeComponent();
		base.Description = LocalizedResourceMgr.GetString("ComponentDesigners.DisconnectHandlerFlow.Tooltip");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "DisconnectHandlerFlow";
	}
}

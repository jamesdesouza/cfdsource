using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Workflow.ComponentModel.Serialization;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(RootFlowDesigner), typeof(IRootDesigner))]
public class ErrorHandlerFlow : RootFlow
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override FlowTypes FlowType => FlowTypes.ErrorHandler;

	public ErrorHandlerFlow()
	{
		InitializeComponent();
		base.Description = LocalizedResourceMgr.GetString("ComponentDesigners.ErrorHandlerFlow.Tooltip");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "ErrorHandlerFlow";
	}
}

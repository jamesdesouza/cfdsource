using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class PromptPlaybackComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		PromptPlaybackComponent promptPlaybackComponent = new PromptPlaybackComponent
		{
			AcceptDtmfInput = Settings.Default.PromptPlaybackTemplateAcceptDtmfInput
		};
		FlowDesignerNameCreator.CreateName("PromptPlayback", host.Container, promptPlaybackComponent);
		return new IComponent[1] { promptPlaybackComponent };
	}
}

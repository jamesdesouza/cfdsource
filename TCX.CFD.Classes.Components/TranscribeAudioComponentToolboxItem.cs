using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class TranscribeAudioComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		TranscribeAudioComponent transcribeAudioComponent = new TranscribeAudioComponent
		{
			LanguageCode = Settings.Default.TranscribeAudioTemplateLanguageCode
		};
		FlowDesignerNameCreator.CreateName("TranscribeAudio", host.Container, transcribeAudioComponent);
		return new IComponent[1] { transcribeAudioComponent };
	}
}

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class RecordComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		RecordComponent recordComponent = new RecordComponent();
		recordComponent.Beep = Settings.Default.RecordTemplateBeep;
		recordComponent.MaxTime = Settings.Default.RecordTemplateMaxTime;
		recordComponent.TerminateByDtmf = Settings.Default.RecordTemplateTerminateByDtmf;
		recordComponent.SaveToFile = (Settings.Default.RecordTemplateSaveToFile ? "true" : "false");
		ComponentBranch componentBranch = new ComponentBranch();
		ComponentBranch componentBranch2 = new ComponentBranch();
		componentBranch.DisplayedText = "Nothing Recorded";
		componentBranch2.DisplayedText = "Audio Recorded";
		recordComponent.Activities.Add(componentBranch);
		recordComponent.Activities.Add(componentBranch2);
		FlowDesignerNameCreator.CreateName("Record", host.Container, recordComponent);
		return new IComponent[1] { recordComponent };
	}
}

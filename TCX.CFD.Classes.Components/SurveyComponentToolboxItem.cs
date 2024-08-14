using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class SurveyComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		SurveyComponent surveyComponent = new SurveyComponent
		{
			AcceptDtmfInput = Settings.Default.SurveyTemplateAcceptDtmfInput,
			Timeout = Settings.Default.SurveyTemplateTimeout,
			MaxRetryCount = Settings.Default.SurveyTemplateMaxRetryCount
		};
		FlowDesignerNameCreator.CreateName("Survey", host.Container, surveyComponent);
		return new IComponent[1] { surveyComponent };
	}
}

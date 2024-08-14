using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(SurveyComponentTheme))]
public class SurveyComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.Survey.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
				ActivityDesignerVerb activityDesignerVerb2 = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.Survey.OpenAudioFolderVerb"), OnOpenAudioFolder);
				activityDesignerVerb2.Properties.Add("Image", Resources.PromptPlayback);
				verbs.Add(activityDesignerVerb2);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is SurveyComponent surveyComponent))
		{
			return;
		}
		SurveyConfigurationForm surveyConfigurationForm = new SurveyConfigurationForm(surveyComponent);
		if (surveyConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["IntroductoryPrompts"].SetValue(base.Activity, surveyConfigurationForm.IntroductoryPrompts);
				properties["GoodbyePrompts"].SetValue(base.Activity, surveyConfigurationForm.GoodbyePrompts);
				properties["TimeoutPrompts"].SetValue(base.Activity, surveyConfigurationForm.TimeoutPrompts);
				properties["InvalidDigitPrompts"].SetValue(base.Activity, surveyConfigurationForm.InvalidDigitPrompts);
				properties["OutputFields"].SetValue(base.Activity, surveyConfigurationForm.OutputFields);
				properties["SurveyQuestions"].SetValue(base.Activity, surveyConfigurationForm.SurveyQuestions);
				properties["AcceptDtmfInput"].SetValue(base.Activity, surveyConfigurationForm.AcceptDtmfInput);
				properties["Timeout"].SetValue(base.Activity, surveyConfigurationForm.Timeout);
				properties["MaxRetryCount"].SetValue(base.Activity, surveyConfigurationForm.MaxRetryCount);
				properties["AllowPartialAnswers"].SetValue(base.Activity, surveyConfigurationForm.AllowPartialAnswers);
				properties["RecordingsPath"].SetValue(base.Activity, surveyConfigurationForm.RecordingsPath);
				properties["ExportToCSVFile"].SetValue(base.Activity, surveyConfigurationForm.ExportToCSVFile);
				designerTransaction.Commit();
			}
		}
	}

	private void OnOpenAudioFolder(object sender, EventArgs e)
	{
		RootFlow rootFlow = ((IVadActivity)base.Activity).GetRootFlow();
		if (rootFlow == null)
		{
			return;
		}
		FileObject fileObject = rootFlow.FileObject;
		if (fileObject != null)
		{
			ProjectObject projectObject = fileObject.GetProjectObject();
			if (projectObject != null)
			{
				Process.Start(Path.Combine(projectObject.GetFolderPath(), "Audio"));
			}
		}
	}

	protected override void DoDefaultAction()
	{
		base.DoDefaultAction();
		OnConfigure(null, EventArgs.Empty);
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Survey", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

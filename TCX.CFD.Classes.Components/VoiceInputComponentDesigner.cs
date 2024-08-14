using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(VoiceInputComponentTheme))]
public class VoiceInputComponentDesigner : CompositeComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.VoiceInput.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
				ActivityDesignerVerb activityDesignerVerb2 = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.VoiceInput.OpenAudioFolderVerb"), OnOpenAudioFolder);
				activityDesignerVerb2.Properties.Add("Image", Resources.PromptPlayback);
				verbs.Add(activityDesignerVerb2);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is VoiceInputComponent voiceInputComponent))
		{
			return;
		}
		VoiceInputConfigurationForm voiceInputConfigurationForm = new VoiceInputConfigurationForm(voiceInputComponent);
		if (voiceInputConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["InitialPrompts"].SetValue(base.Activity, voiceInputConfigurationForm.InitialPrompts);
				properties["SubsequentPrompts"].SetValue(base.Activity, voiceInputConfigurationForm.SubsequentPrompts);
				properties["TimeoutPrompts"].SetValue(base.Activity, voiceInputConfigurationForm.TimeoutPrompts);
				properties["InvalidInputPrompts"].SetValue(base.Activity, voiceInputConfigurationForm.InvalidInputPrompts);
				properties["InputTimeout"].SetValue(base.Activity, voiceInputConfigurationForm.InputTimeout);
				properties["MaxRetryCount"].SetValue(base.Activity, voiceInputConfigurationForm.MaxRetryCount);
				properties["LanguageCode"].SetValue(base.Activity, voiceInputConfigurationForm.LanguageCode);
				properties["SaveToFile"].SetValue(base.Activity, voiceInputConfigurationForm.SaveToFile);
				properties["FileName"].SetValue(base.Activity, voiceInputConfigurationForm.FileName);
				properties["AcceptDtmfInput"].SetValue(base.Activity, voiceInputConfigurationForm.AcceptDtmfInput);
				properties["DtmfTimeout"].SetValue(base.Activity, voiceInputConfigurationForm.DtmfTimeout);
				properties["MinDigits"].SetValue(base.Activity, voiceInputConfigurationForm.MinDigits);
				properties["MaxDigits"].SetValue(base.Activity, voiceInputConfigurationForm.MaxDigits);
				properties["StopDigit"].SetValue(base.Activity, voiceInputConfigurationForm.StopDigit);
				properties["IsValidDigit_0"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_0);
				properties["IsValidDigit_1"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_1);
				properties["IsValidDigit_2"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_2);
				properties["IsValidDigit_3"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_3);
				properties["IsValidDigit_4"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_4);
				properties["IsValidDigit_5"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_5);
				properties["IsValidDigit_6"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_6);
				properties["IsValidDigit_7"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_7);
				properties["IsValidDigit_8"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_8);
				properties["IsValidDigit_9"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_9);
				properties["IsValidDigit_Star"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_Star);
				properties["IsValidDigit_Pound"].SetValue(base.Activity, voiceInputConfigurationForm.IsValidDigit_Pound);
				properties["Dictionary"].SetValue(base.Activity, voiceInputConfigurationForm.Dictionary);
				properties["Hints"].SetValue(base.Activity, voiceInputConfigurationForm.Hints);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Voice Input", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}

	protected override void Initialize(Activity activity)
	{
		base.Initialize(activity);
	}
}

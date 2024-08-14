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

[ActivityDesignerTheme(typeof(PromptPlaybackComponentTheme))]
public class PromptPlaybackComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.PromptPlayback.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
				ActivityDesignerVerb activityDesignerVerb2 = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.PromptPlayback.OpenAudioFolderVerb"), OnOpenAudioFolder);
				activityDesignerVerb2.Properties.Add("Image", Resources.PromptPlayback);
				verbs.Add(activityDesignerVerb2);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is PromptPlaybackComponent promptPlaybackComponent))
		{
			return;
		}
		PromptPlaybackConfigurationForm promptPlaybackConfigurationForm = new PromptPlaybackConfigurationForm(promptPlaybackComponent);
		if (promptPlaybackConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["AcceptDtmfInput"].SetValue(base.Activity, promptPlaybackConfigurationForm.AcceptDtmfInput);
				properties["Prompts"].SetValue(base.Activity, promptPlaybackConfigurationForm.Prompts);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Prompt Playback", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

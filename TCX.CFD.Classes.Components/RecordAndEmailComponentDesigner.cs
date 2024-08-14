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

[ActivityDesignerTheme(typeof(RecordAndEmailComponentTheme))]
public class RecordAndEmailComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.RecordAndEmail.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
				ActivityDesignerVerb activityDesignerVerb2 = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.RecordAndEmail.OpenAudioFolderVerb"), OnOpenAudioFolder);
				activityDesignerVerb2.Properties.Add("Image", Resources.PromptPlayback);
				verbs.Add(activityDesignerVerb2);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is RecordAndEmailComponent recordAndEmailComponent))
		{
			return;
		}
		RecordAndEmailConfigurationForm recordAndEmailConfigurationForm = new RecordAndEmailConfigurationForm(recordAndEmailComponent);
		if (recordAndEmailConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["Beep"].SetValue(base.Activity, recordAndEmailConfigurationForm.Beep);
				properties["MaxTime"].SetValue(base.Activity, recordAndEmailConfigurationForm.MaxTime);
				properties["TerminateByDtmf"].SetValue(base.Activity, recordAndEmailConfigurationForm.TerminateByDtmf);
				properties["Prompts"].SetValue(base.Activity, recordAndEmailConfigurationForm.Prompts);
				properties["UseServerSettings"].SetValue(base.Activity, recordAndEmailConfigurationForm.UseServerSettings);
				properties["Server"].SetValue(base.Activity, recordAndEmailConfigurationForm.Server);
				properties["Port"].SetValue(base.Activity, recordAndEmailConfigurationForm.Port);
				properties["EnableSSL"].SetValue(base.Activity, recordAndEmailConfigurationForm.EnableSSL);
				properties["UserName"].SetValue(base.Activity, recordAndEmailConfigurationForm.UserName);
				properties["Password"].SetValue(base.Activity, recordAndEmailConfigurationForm.Password);
				properties["From"].SetValue(base.Activity, recordAndEmailConfigurationForm.From);
				properties["To"].SetValue(base.Activity, recordAndEmailConfigurationForm.To);
				properties["CC"].SetValue(base.Activity, recordAndEmailConfigurationForm.CC);
				properties["BCC"].SetValue(base.Activity, recordAndEmailConfigurationForm.BCC);
				properties["Subject"].SetValue(base.Activity, recordAndEmailConfigurationForm.Subject);
				properties["Body"].SetValue(base.Activity, recordAndEmailConfigurationForm.Body);
				properties["Priority"].SetValue(base.Activity, recordAndEmailConfigurationForm.Priority);
				properties["IsBodyHtml"].SetValue(base.Activity, recordAndEmailConfigurationForm.IsBodyHtml);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Record and Email", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

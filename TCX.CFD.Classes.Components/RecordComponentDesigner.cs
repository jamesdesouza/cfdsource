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

[ActivityDesignerTheme(typeof(RecordComponenTheme))]
public class RecordComponentDesigner : CompositeComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.Record.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
				ActivityDesignerVerb activityDesignerVerb2 = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.Record.OpenAudioFolderVerb"), OnOpenAudioFolder);
				activityDesignerVerb2.Properties.Add("Image", Resources.PromptPlayback);
				verbs.Add(activityDesignerVerb2);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is RecordComponent recordComponent))
		{
			return;
		}
		RecordConfigurationForm recordConfigurationForm = new RecordConfigurationForm(recordComponent);
		if (recordConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["Prompts"].SetValue(base.Activity, recordConfigurationForm.Prompts);
				properties["Beep"].SetValue(base.Activity, recordConfigurationForm.Beep);
				properties["MaxTime"].SetValue(base.Activity, recordConfigurationForm.MaxTime);
				properties["TerminateByDtmf"].SetValue(base.Activity, recordConfigurationForm.TerminateByDtmf);
				properties["SaveToFile"].SetValue(base.Activity, recordConfigurationForm.SaveToFile);
				properties["FileName"].SetValue(base.Activity, recordConfigurationForm.FileName);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Record", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}

	protected override void Initialize(Activity activity)
	{
		base.Initialize(activity);
	}
}

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

[ActivityDesignerTheme(typeof(UserInputComponentTheme))]
public class UserInputComponentDesigner : CompositeComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.UserInput.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
				ActivityDesignerVerb activityDesignerVerb2 = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.UserInput.OpenAudioFolderVerb"), OnOpenAudioFolder);
				activityDesignerVerb2.Properties.Add("Image", Resources.PromptPlayback);
				verbs.Add(activityDesignerVerb2);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is UserInputComponent userInputComponent))
		{
			return;
		}
		UserInputConfigurationForm userInputConfigurationForm = new UserInputConfigurationForm(userInputComponent);
		if (userInputConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["AcceptDtmfInput"].SetValue(base.Activity, userInputConfigurationForm.AcceptDtmfInput);
				properties["InitialPrompts"].SetValue(base.Activity, userInputConfigurationForm.InitialPrompts);
				properties["SubsequentPrompts"].SetValue(base.Activity, userInputConfigurationForm.SubsequentPrompts);
				properties["TimeoutPrompts"].SetValue(base.Activity, userInputConfigurationForm.TimeoutPrompts);
				properties["InvalidDigitPrompts"].SetValue(base.Activity, userInputConfigurationForm.InvalidDigitPrompts);
				properties["FirstDigitTimeout"].SetValue(base.Activity, userInputConfigurationForm.FirstDigitTimeout);
				properties["InterDigitTimeout"].SetValue(base.Activity, userInputConfigurationForm.InterDigitTimeout);
				properties["FinalDigitTimeout"].SetValue(base.Activity, userInputConfigurationForm.FinalDigitTimeout);
				properties["MaxRetryCount"].SetValue(base.Activity, userInputConfigurationForm.MaxRetryCount);
				properties["MinDigits"].SetValue(base.Activity, userInputConfigurationForm.MinDigits);
				properties["MaxDigits"].SetValue(base.Activity, userInputConfigurationForm.MaxDigits);
				properties["StopDigit"].SetValue(base.Activity, userInputConfigurationForm.StopDigit);
				properties["IsValidDigit_0"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_0);
				properties["IsValidDigit_1"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_1);
				properties["IsValidDigit_2"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_2);
				properties["IsValidDigit_3"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_3);
				properties["IsValidDigit_4"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_4);
				properties["IsValidDigit_5"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_5);
				properties["IsValidDigit_6"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_6);
				properties["IsValidDigit_7"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_7);
				properties["IsValidDigit_8"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_8);
				properties["IsValidDigit_9"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_9);
				properties["IsValidDigit_Star"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_Star);
				properties["IsValidDigit_Pound"].SetValue(base.Activity, userInputConfigurationForm.IsValidDigit_Pound);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "User Input", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}

	protected override void Initialize(Activity activity)
	{
		base.Initialize(activity);
	}
}

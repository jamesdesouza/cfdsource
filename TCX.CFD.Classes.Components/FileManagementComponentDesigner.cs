using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(FileManagementComponentTheme))]
public class FileManagementComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.FileManagement.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is FileManagementComponent fileManagementComponent))
		{
			return;
		}
		FileManagementConfigurationForm fileManagementConfigurationForm = new FileManagementConfigurationForm(fileManagementComponent);
		if (fileManagementConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["OpenMode"].SetValue(base.Activity, fileManagementConfigurationForm.OpenMode);
				properties["Action"].SetValue(base.Activity, fileManagementConfigurationForm.Action);
				properties["FileName"].SetValue(base.Activity, fileManagementConfigurationForm.FileName);
				properties["Content"].SetValue(base.Activity, fileManagementConfigurationForm.Content);
				properties["AppendFinalCrLf"].SetValue(base.Activity, fileManagementConfigurationForm.AppendFinalCrLf);
				properties["LinesToRead"].SetValue(base.Activity, fileManagementConfigurationForm.LinesToRead);
				properties["FirstLineToRead"].SetValue(base.Activity, fileManagementConfigurationForm.FirstLineToRead);
				properties["ReadToEnd"].SetValue(base.Activity, fileManagementConfigurationForm.ReadToEnd);
				designerTransaction.Commit();
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Read / Write to File", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(AttachCallDataComponentTheme))]
public class AttachCallDataComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.AttachCallData.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is AttachCallDataComponent attachCallDataComponent))
		{
			return;
		}
		AttachCallDataConfigurationForm attachCallDataConfigurationForm = new AttachCallDataConfigurationForm(attachCallDataComponent);
		if (attachCallDataConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["DataName"].SetValue(base.Activity, attachCallDataConfigurationForm.DataName);
				properties["DataValue"].SetValue(base.Activity, attachCallDataConfigurationForm.DataValue);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Attach Call Data", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

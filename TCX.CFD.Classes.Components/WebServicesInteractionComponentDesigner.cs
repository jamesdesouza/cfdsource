using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(WebServicesInteractionComponentTheme))]
public class WebServicesInteractionComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.WebServicesInteraction.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is WebServicesInteractionComponent webServicesInteractionComponent))
		{
			return;
		}
		WebServicesInteractionConfigurationForm webServicesInteractionConfigurationForm = new WebServicesInteractionConfigurationForm(webServicesInteractionComponent);
		if (webServicesInteractionConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["URI"].SetValue(base.Activity, webServicesInteractionConfigurationForm.URI);
				properties["WebServiceName"].SetValue(base.Activity, webServicesInteractionConfigurationForm.WebServiceName);
				properties["ContentType"].SetValue(base.Activity, webServicesInteractionConfigurationForm.ContentType);
				properties["Content"].SetValue(base.Activity, webServicesInteractionConfigurationForm.Content);
				properties["Headers"].SetValue(base.Activity, webServicesInteractionConfigurationForm.Headers);
				properties["Timeout"].SetValue(base.Activity, webServicesInteractionConfigurationForm.Timeout);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Web Service (POST)", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

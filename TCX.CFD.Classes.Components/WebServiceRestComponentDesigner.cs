using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(WebServiceRestComponentTheme))]
public class WebServiceRestComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.WebServiceRest.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is WebServiceRestComponent webServiceRestComponent))
		{
			return;
		}
		WebServiceRestConfigurationForm webServiceRestConfigurationForm = new WebServiceRestConfigurationForm(webServiceRestComponent);
		if (webServiceRestConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["URI"].SetValue(base.Activity, webServiceRestConfigurationForm.URI);
				properties["HttpRequestType"].SetValue(base.Activity, webServiceRestConfigurationForm.RequestType);
				properties["ContentType"].SetValue(base.Activity, webServiceRestConfigurationForm.ContentType);
				properties["Content"].SetValue(base.Activity, webServiceRestConfigurationForm.Content);
				properties["AuthenticationType"].SetValue(base.Activity, webServiceRestConfigurationForm.AuthenticationType);
				properties["AuthenticationUserName"].SetValue(base.Activity, webServiceRestConfigurationForm.AuthenticationUserName);
				properties["AuthenticationPassword"].SetValue(base.Activity, webServiceRestConfigurationForm.AuthenticationPassword);
				properties["AuthenticationApiKey"].SetValue(base.Activity, webServiceRestConfigurationForm.AuthenticationApiKey);
				properties["AuthenticationOAuth2AccessToken"].SetValue(base.Activity, webServiceRestConfigurationForm.AuthenticationOAuth2AccessToken);
				properties["Headers"].SetValue(base.Activity, webServiceRestConfigurationForm.Headers);
				properties["Timeout"].SetValue(base.Activity, webServiceRestConfigurationForm.Timeout);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Web Service REST", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

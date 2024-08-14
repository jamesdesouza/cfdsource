using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(EMailSenderComponentTheme))]
public class EMailSenderComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.EMailSender.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is EMailSenderComponent eMailSenderComponent))
		{
			return;
		}
		EMailSenderConfigurationForm eMailSenderConfigurationForm = new EMailSenderConfigurationForm(eMailSenderComponent);
		if (eMailSenderConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["UseServerSettings"].SetValue(base.Activity, eMailSenderConfigurationForm.UseServerSettings);
				properties["Server"].SetValue(base.Activity, eMailSenderConfigurationForm.Server);
				properties["Port"].SetValue(base.Activity, eMailSenderConfigurationForm.Port);
				properties["EnableSSL"].SetValue(base.Activity, eMailSenderConfigurationForm.EnableSSL);
				properties["UserName"].SetValue(base.Activity, eMailSenderConfigurationForm.UserName);
				properties["Password"].SetValue(base.Activity, eMailSenderConfigurationForm.Password);
				properties["From"].SetValue(base.Activity, eMailSenderConfigurationForm.From);
				properties["To"].SetValue(base.Activity, eMailSenderConfigurationForm.To);
				properties["CC"].SetValue(base.Activity, eMailSenderConfigurationForm.CC);
				properties["BCC"].SetValue(base.Activity, eMailSenderConfigurationForm.BCC);
				properties["Subject"].SetValue(base.Activity, eMailSenderConfigurationForm.Subject);
				properties["Body"].SetValue(base.Activity, eMailSenderConfigurationForm.Body);
				properties["Priority"].SetValue(base.Activity, eMailSenderConfigurationForm.Priority);
				properties["IgnoreMissingAttachments"].SetValue(base.Activity, eMailSenderConfigurationForm.IgnoreMissingAttachments);
				properties["IsBodyHtml"].SetValue(base.Activity, eMailSenderConfigurationForm.IsBodyHtml);
				properties["Attachments"].SetValue(base.Activity, eMailSenderConfigurationForm.Attachments);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "E-Mail Sender", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

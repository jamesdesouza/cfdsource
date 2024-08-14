using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(DatabaseAccessComponentTheme))]
public class DatabaseAccessComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.DatabaseAccess.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is DatabaseAccessComponent databaseAccessComponent))
		{
			return;
		}
		DatabaseAccessConfigurationForm databaseAccessConfigurationForm = new DatabaseAccessConfigurationForm(databaseAccessComponent);
		if (databaseAccessConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["UseConnectionString"].SetValue(base.Activity, databaseAccessConfigurationForm.UseConnectionString);
				properties["ConnectionString"].SetValue(base.Activity, databaseAccessConfigurationForm.ConnectionString);
				properties["Server"].SetValue(base.Activity, databaseAccessConfigurationForm.Server);
				properties["Port"].SetValue(base.Activity, databaseAccessConfigurationForm.Port);
				properties["Database"].SetValue(base.Activity, databaseAccessConfigurationForm.Database);
				properties["UserName"].SetValue(base.Activity, databaseAccessConfigurationForm.UserName);
				properties["Password"].SetValue(base.Activity, databaseAccessConfigurationForm.Password);
				properties["SqlStatement"].SetValue(base.Activity, databaseAccessConfigurationForm.SqlStatement);
				properties["DatabaseType"].SetValue(base.Activity, databaseAccessConfigurationForm.DatabaseType);
				properties["StatementType"].SetValue(base.Activity, databaseAccessConfigurationForm.StatementType);
				properties["Timeout"].SetValue(base.Activity, databaseAccessConfigurationForm.Timeout);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Database Access", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

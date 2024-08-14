using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(ExternalCodeExecutionComponentTheme))]
public class ExternalCodeExecutionComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.ExternalCodeExecution.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				verbs.Add(activityDesignerVerb);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is ExternalCodeExecutionComponent externalCodeExecutionComponent))
		{
			return;
		}
		ExternalCodeExecutionConfigurationForm externalCodeExecutionConfigurationForm = new ExternalCodeExecutionConfigurationForm(externalCodeExecutionComponent);
		if (externalCodeExecutionConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["LibraryFileName"].SetValue(base.Activity, externalCodeExecutionConfigurationForm.LibraryFileName);
				properties["ClassName"].SetValue(base.Activity, externalCodeExecutionConfigurationForm.ClassName);
				properties["MethodName"].SetValue(base.Activity, externalCodeExecutionConfigurationForm.MethodName);
				properties["ReturnsValue"].SetValue(base.Activity, externalCodeExecutionConfigurationForm.ReturnsValue);
				properties["Parameters"].SetValue(base.Activity, externalCodeExecutionConfigurationForm.Parameters);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Execute C# File", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

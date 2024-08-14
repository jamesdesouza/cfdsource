using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class DateTimeConditionalComponentBranchDesigner : ComponentBranchDesigner
{
	public override string Text
	{
		get
		{
			string text = base.Activity.Name;
			if (base.Activity is AbsVadSequenceActivity absVadSequenceActivity && !string.IsNullOrEmpty(absVadSequenceActivity.Tag))
			{
				text = text + "\n(" + absVadSequenceActivity.Tag + ")";
			}
			return text;
		}
		protected set
		{
			base.Text = value;
		}
	}

	public DateTimeConditionalComponentBranchDesigner()
		: base(canBeMoved: true)
	{
	}

	protected override void Initialize(Activity activity)
	{
		ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.ComponentBranchDesigner.ConfigureVerb"), OnConfigure);
		activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
		verbs.Add(activityDesignerVerb);
		base.Initialize(activity);
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is DateTimeConditionalComponentBranch dateTimeConditionalComponentBranch))
		{
			return;
		}
		DateTimeConditionalComponentBranchConfigurationForm dateTimeConditionalComponentBranchConfigurationForm = new DateTimeConditionalComponentBranchConfigurationForm(dateTimeConditionalComponentBranch);
		if (dateTimeConditionalComponentBranchConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["DIDFilter"].SetValue(base.Activity, dateTimeConditionalComponentBranchConfigurationForm.DIDFilter);
				properties["DIDFilterList"].SetValue(base.Activity, dateTimeConditionalComponentBranchConfigurationForm.DIDFilterList);
				properties["DateTimeConditions"].SetValue(base.Activity, dateTimeConditionalComponentBranchConfigurationForm.DateTimeConditions);
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
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Date Time condition Branch", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General", isReadOnly: true);
		base.PostFilterProperties(properties);
	}
}

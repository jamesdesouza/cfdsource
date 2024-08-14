using System;
using System.Collections;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(UserComponentTheme))]
public class UserComponentDesigner : ComponentDesigner
{
	private readonly ActivityDesignerVerbCollection verbs = new ActivityDesignerVerbCollection();

	protected override ActivityDesignerVerbCollection Verbs => verbs;

	protected override void DoDefaultAction()
	{
		base.DoDefaultAction();
		UserComponent userComponent = base.Activity as UserComponent;
		try
		{
			if (userComponent.FileObject != null)
			{
				userComponent.FileObject.Open();
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.OpeningFile"), userComponent.FileObject.Path, ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "User Component", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

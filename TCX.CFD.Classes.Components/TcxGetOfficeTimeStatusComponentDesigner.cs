using System.Collections;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(TcxGetOfficeTimeStatusComponentTheme))]
public class TcxGetOfficeTimeStatusComponentDesigner : ComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
			}
			return verbs;
		}
	}

	protected override void DoDefaultAction()
	{
		base.DoDefaultAction();
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Get Office Time Status", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

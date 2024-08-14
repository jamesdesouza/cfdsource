using System.Collections;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(DisconnectCallComponentTheme))]
public class DisconnectCallComponentDesigner : ComponentDesigner
{
	private readonly ActivityDesignerVerbCollection verbs = new ActivityDesignerVerbCollection();

	protected override ActivityDesignerVerbCollection Verbs => verbs;

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Disconnect Call", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}

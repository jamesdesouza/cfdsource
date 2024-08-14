using System.Collections;

namespace TCX.CFD.Classes.Components;

public class ParallelExecutionComponentBranchDesigner : ComponentBranchDesigner
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

	public ParallelExecutionComponentBranchDesigner()
		: base(canBeMoved: true)
	{
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Parallel Execution Branch", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General", isReadOnly: true);
		base.PostFilterProperties(properties);
	}
}

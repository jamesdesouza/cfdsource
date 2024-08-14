using System.Drawing;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

public class ComponentDesigner : ActivityDesigner
{
	public override string Text
	{
		get
		{
			string text = base.Activity.Name;
			if (base.Activity is AbsVadActivity absVadActivity && !string.IsNullOrEmpty(absVadActivity.Tag))
			{
				text = text + "\n(" + absVadActivity.Tag + ")";
			}
			return text;
		}
		protected set
		{
			base.Text = value;
		}
	}

	protected override void OnPaint(ActivityDesignerPaintEventArgs e)
	{
		if (base.Activity is AbsVadActivity { DebugModeActive: not false })
		{
			e.Graphics.FillRectangle(Brushes.Yellow, e.ClipRectangle);
		}
		base.OnPaint(e);
	}
}

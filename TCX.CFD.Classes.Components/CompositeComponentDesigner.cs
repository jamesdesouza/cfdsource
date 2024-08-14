using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

public class CompositeComponentDesigner : ParallelActivityDesigner
{
	public override ReadOnlyCollection<DesignerView> Views => new ReadOnlyCollection<DesignerView>(new List<DesignerView> { base.Views[0] });

	public override string Text
	{
		get
		{
			string text = base.Activity.Name;
			if (base.Activity is AbsVadCompositeActivity absVadCompositeActivity && !string.IsNullOrEmpty(absVadCompositeActivity.Tag))
			{
				text = text + "\n(" + absVadCompositeActivity.Tag + ")";
			}
			return text;
		}
		protected set
		{
			base.Text = value;
		}
	}

	public override bool CanInsertActivities(HitTestInfo insertLocation, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		return false;
	}

	public override bool CanMoveActivities(HitTestInfo moveLocation, ReadOnlyCollection<Activity> activitiesToMove)
	{
		return true;
	}

	public override bool CanRemoveActivities(ReadOnlyCollection<Activity> activitiesToRemove)
	{
		return false;
	}

	protected override void OnPaint(ActivityDesignerPaintEventArgs e)
	{
		if (base.Activity is AbsVadCompositeActivity { DebugModeActive: not false })
		{
			e.Graphics.FillRectangle(Brushes.Yellow, e.ClipRectangle);
		}
		base.OnPaint(e);
	}
}

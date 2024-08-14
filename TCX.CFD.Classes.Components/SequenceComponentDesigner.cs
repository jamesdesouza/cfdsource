using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

public class SequenceComponentDesigner : SequentialActivityDesigner
{
	protected ActivityDesignerVerbCollection verbs = new ActivityDesignerVerbCollection();

	protected override string HelpText
	{
		get
		{
			return LocalizedResourceMgr.GetString("ComponentDesigners.HelpText");
		}
		set
		{
			base.HelpText = value;
		}
	}

	public override ReadOnlyCollection<DesignerView> Views => new ReadOnlyCollection<DesignerView>(new List<DesignerView> { base.Views[0] });

	protected override ActivityDesignerVerbCollection Verbs => verbs;

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

	public override bool CanInsertActivities(HitTestInfo insertLocation, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		foreach (Activity item in activitiesToInsert)
		{
			if (!(item is IVadActivity))
			{
				return false;
			}
			if (item is ComponentBranch || item is ConditionalComponentBranch || item is MenuComponentBranch || item is DateTimeConditionalComponentBranch || item is ParallelExecutionComponentBranch)
			{
				return false;
			}
		}
		RootFlow rootFlow = ((IVadActivity)base.Activity).GetRootFlow();
		if (rootFlow != null)
		{
			FileObject fileObject = rootFlow.FileObject;
			if (fileObject != null)
			{
				if (fileObject is DialerFileObject)
				{
					return ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(fileObject.GetProjectObject(), activitiesToInsert);
				}
				if (fileObject is ComponentFileObject)
				{
					return ComponentDesignerHelper.CanInsertActivitiesCheckingUserComponent(fileObject as ComponentFileObject, activitiesToInsert);
				}
			}
		}
		return true;
	}

	protected override void OnPaint(ActivityDesignerPaintEventArgs e)
	{
		if (base.Activity is AbsVadSequenceActivity { DebugModeActive: not false })
		{
			e.Graphics.FillRectangle(Brushes.Yellow, e.ClipRectangle);
		}
		base.OnPaint(e);
	}
}

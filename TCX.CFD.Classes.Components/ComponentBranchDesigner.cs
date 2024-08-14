using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(ComponentBranchTheme))]
public class ComponentBranchDesigner : SequenceComponentDesigner
{
	private readonly bool canBeMoved;

	private ActivityDesignerVerb moveLeftVerb;

	private ActivityDesignerVerb moveRightVerb;

	public override string Text
	{
		get
		{
			return (base.Activity as ComponentBranch).DisplayedText;
		}
		protected set
		{
			base.Text = value;
		}
	}

	private bool IsFirstBranch(Activity activity)
	{
		AbsVadSequenceActivity absVadSequenceActivity = activity as AbsVadSequenceActivity;
		if (absVadSequenceActivity.Parent == null)
		{
			return false;
		}
		return absVadSequenceActivity == absVadSequenceActivity.Parent.Activities[0];
	}

	private bool IsLastBranch(Activity activity)
	{
		AbsVadSequenceActivity absVadSequenceActivity = activity as AbsVadSequenceActivity;
		if (absVadSequenceActivity.Parent == null)
		{
			return false;
		}
		return absVadSequenceActivity == absVadSequenceActivity.Parent.Activities[absVadSequenceActivity.Parent.Activities.Count - 1];
	}

	private void OnMoveBranch(bool moveBack, string transactionDescription)
	{
		if (base.Activity.Site == null)
		{
			return;
		}
		AbsVadSequenceActivity absVadSequenceActivity = base.Activity as AbsVadSequenceActivity;
		if (!(base.Activity.Site.Container is IDesignerHost designerHost) || !(designerHost.GetDesigner(absVadSequenceActivity.Parent) is CompositeActivityDesigner compositeActivityDesigner))
		{
			return;
		}
		if (GetService(typeof(IDesignerHost)) is IDesignerHost designerHost2)
		{
			using DesignerTransaction designerTransaction = designerHost2.CreateTransaction(transactionDescription);
			int num = compositeActivityDesigner.ContainedDesigners.IndexOf(this);
			List<Activity> list = new List<Activity> { absVadSequenceActivity };
			compositeActivityDesigner.MoveActivities(new ConnectorHitTestInfo(compositeActivityDesigner, HitTestLocations.Connector, moveBack ? (num - 1) : (num + 2)), new ReadOnlyCollection<Activity>(list));
			if (designerHost2.GetService(typeof(ISelectionService)) is ISelectionService selectionService)
			{
				ArrayList selectedComponents = new ArrayList { base.Activity };
				selectionService.SetSelectedComponents(selectedComponents);
			}
			designerTransaction.Commit();
		}
		foreach (ComponentBranchDesigner containedDesigner in compositeActivityDesigner.ContainedDesigners)
		{
			containedDesigner.EnableDisableVerbs();
		}
	}

	private void OnMoveLeft(object sender, EventArgs e)
	{
		OnMoveBranch(moveBack: true, "Moving branch to the left");
	}

	private void OnMoveRight(object sender, EventArgs e)
	{
		OnMoveBranch(moveBack: false, "Moving branch to the right");
	}

	protected override void Initialize(Activity activity)
	{
		base.Initialize(activity);
		if (canBeMoved)
		{
			moveLeftVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.ComponentBranchDesigner.MoveLeft"), OnMoveLeft);
			moveRightVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.ComponentBranchDesigner.MoveRight"), OnMoveRight);
			moveLeftVerb.Properties.Add("Image", Resources.MoveLeft);
			moveRightVerb.Properties.Add("Image", Resources.MoveRight);
			moveLeftVerb.Enabled = !IsFirstBranch(activity);
			moveRightVerb.Enabled = !IsLastBranch(activity);
			verbs.Add(moveLeftVerb);
			verbs.Add(moveRightVerb);
		}
	}

	public void EnableDisableVerbs()
	{
		if (canBeMoved)
		{
			moveLeftVerb.Enabled = !IsFirstBranch(base.Activity);
			moveRightVerb.Enabled = !IsLastBranch(base.Activity);
		}
	}

	public ComponentBranchDesigner()
	{
	}

	public ComponentBranchDesigner(bool canBeMoved)
	{
		this.canBeMoved = canBeMoved;
	}
}

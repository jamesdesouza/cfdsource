using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.FlowDesigner;

public class FlowDesignerUndoEngine : UndoEngine
{
	protected class FlowDesignerUndoUnit : UndoUnit
	{
		private bool reverse;

		private List<ComponentChangedEventArgs> changeList = new List<ComponentChangedEventArgs>();

		public FlowDesignerUndoUnit(FlowDesignerUndoEngine engine, string name)
			: base(engine, name)
		{
		}

		public override void ComponentAdding(ComponentEventArgs e)
		{
			if (e.Component is Activity)
			{
				base.ComponentAdding(e);
			}
		}

		public override void ComponentAdded(ComponentEventArgs e)
		{
			if (e.Component is Activity)
			{
				base.ComponentAdded(e);
			}
		}

		public override void ComponentChanging(ComponentChangingEventArgs e)
		{
			if (e.Component is Activity)
			{
				base.ComponentChanging(e);
			}
		}

		public override void ComponentChanged(ComponentChangedEventArgs e)
		{
			if (e.Component is Activity)
			{
				changeList.Add(e);
				base.ComponentChanged(e);
			}
		}

		public override void ComponentRemoving(ComponentEventArgs e)
		{
			if (e.Component is Activity)
			{
				base.ComponentRemoving(e);
			}
		}

		public override void ComponentRemoved(ComponentEventArgs e)
		{
			if (e.Component is Activity)
			{
				base.ComponentRemoved(e);
			}
		}

		public override void ComponentRename(ComponentRenameEventArgs e)
		{
			if (e.Component is Activity)
			{
				base.ComponentRename(e);
			}
		}

		public override void Close()
		{
			base.Close();
		}

		private void insertActivities(IDesignerHost designerHost, CompositeActivityDesigner designer, IList<Activity> activityList, int index)
		{
			foreach (Activity activity in activityList)
			{
				if (activity is CompositeActivity)
				{
					CompositeActivity compositeActivity = activity as CompositeActivity;
					List<Activity> activityList2 = new List<Activity>(compositeActivity.Activities);
					compositeActivity.Activities.Clear();
					List<Activity> list = new List<Activity>();
					list.Add(activity);
					designer.InsertActivities(new ConnectorHitTestInfo(designer, HitTestLocations.Connector, index++), new ReadOnlyCollection<Activity>(list));
					CompositeActivityDesigner designer2 = designerHost.GetDesigner(compositeActivity) as CompositeActivityDesigner;
					insertActivities(designerHost, designer2, activityList2, 0);
				}
				else
				{
					List<Activity> list2 = new List<Activity>();
					list2.Add(activity);
					designer.InsertActivities(new ConnectorHitTestInfo(designer, HitTestLocations.Connector, index++), new ReadOnlyCollection<Activity>(list2));
				}
			}
		}

		protected override void UndoCore()
		{
			changeList.Reverse();
			for (int i = 0; i < changeList.Count; i++)
			{
				ComponentChangedEventArgs componentChangedEventArgs = changeList[i];
				if (componentChangedEventArgs.Member == null)
				{
					CompositeActivity compositeActivity = componentChangedEventArgs.Component as CompositeActivity;
					if (!(componentChangedEventArgs.OldValue is ActivityCollectionChangeEventArgs activityCollectionChangeEventArgs) || compositeActivity == null || compositeActivity.Site == null || !(compositeActivity.Site.Container is IDesignerHost designerHost) || !(designerHost.GetDesigner(compositeActivity) is CompositeActivityDesigner compositeActivityDesigner))
					{
						continue;
					}
					switch (activityCollectionChangeEventArgs.Action)
					{
					case ActivityCollectionChangeAction.Add:
						if (reverse)
						{
							insertActivities(designerHost, compositeActivityDesigner, activityCollectionChangeEventArgs.AddedItems, activityCollectionChangeEventArgs.Index);
						}
						else
						{
							compositeActivityDesigner.RemoveActivities(activityCollectionChangeEventArgs.AddedItems as ReadOnlyCollection<Activity>);
						}
						break;
					case ActivityCollectionChangeAction.Remove:
						if (reverse)
						{
							compositeActivityDesigner.RemoveActivities(activityCollectionChangeEventArgs.RemovedItems as ReadOnlyCollection<Activity>);
						}
						else
						{
							insertActivities(designerHost, compositeActivityDesigner, activityCollectionChangeEventArgs.RemovedItems, activityCollectionChangeEventArgs.Index);
						}
						break;
					}
				}
				else
				{
					Activity component = componentChangedEventArgs.Component as Activity;
					MemberDescriptor member = componentChangedEventArgs.Member;
					TypeDescriptor.GetProperties(component)[member.Name].SetValue(component, reverse ? componentChangedEventArgs.NewValue : componentChangedEventArgs.OldValue);
				}
			}
			reverse = !reverse;
		}
	}

	private List<UndoUnit> undoUnitList = new List<UndoUnit>();

	private int currentPos;

	private void updateUndoRedoMenuCommandsStatus()
	{
		FlowDesignerMenuCommandService obj = GetService(typeof(IMenuCommandService)) as FlowDesignerMenuCommandService;
		MenuCommand menuCommand = obj.FindCommand(StandardCommands.Undo);
		if (menuCommand != null)
		{
			menuCommand.Enabled = currentPos > 0;
		}
		MenuCommand menuCommand2 = obj.FindCommand(StandardCommands.Redo);
		if (menuCommand2 != null)
		{
			menuCommand2.Enabled = currentPos < undoUnitList.Count;
		}
	}

	public FlowDesignerUndoEngine(IServiceProvider provider)
		: base(provider)
	{
	}

	public string GetLastUndoDescription()
	{
		if (currentPos > 0)
		{
			return undoUnitList[currentPos - 1].Name;
		}
		return string.Empty;
	}

	public string GetLastRedoDescription()
	{
		if (currentPos < undoUnitList.Count)
		{
			return undoUnitList[currentPos].Name;
		}
		return string.Empty;
	}

	public void PerformUndo()
	{
		if (currentPos > 0)
		{
			undoUnitList[currentPos - 1].Undo();
			currentPos--;
		}
		updateUndoRedoMenuCommandsStatus();
	}

	public void PerformRedo()
	{
		if (currentPos < undoUnitList.Count)
		{
			undoUnitList[currentPos].Undo();
			currentPos++;
		}
		updateUndoRedoMenuCommandsStatus();
	}

	protected override void AddUndoUnit(UndoUnit unit)
	{
		undoUnitList.RemoveRange(currentPos, undoUnitList.Count - currentPos);
		undoUnitList.Add(unit);
		currentPos = undoUnitList.Count;
		updateUndoRedoMenuCommandsStatus();
	}

	protected override UndoUnit CreateUndoUnit(string name, bool primary)
	{
		return new FlowDesignerUndoUnit(this, name);
	}

	protected override void DiscardUndoUnit(UndoUnit unit)
	{
		undoUnitList.Remove(unit);
		base.DiscardUndoUnit(unit);
	}
}

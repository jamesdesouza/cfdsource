using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.FlowDesigner;

public sealed class FlowDesignerMenuCommandService : MenuCommandService
{
	public event EventHandler CommandExecuted;

	public event EventHandler ShowPropertiesRequested;

	private void ExecuteUndo(object sender, EventArgs e)
	{
		if (GetService(typeof(UndoEngine)) is FlowDesignerUndoEngine flowDesignerUndoEngine)
		{
			flowDesignerUndoEngine.PerformUndo();
		}
	}

	private void ExecuteRedo(object sender, EventArgs e)
	{
		if (GetService(typeof(UndoEngine)) is FlowDesignerUndoEngine flowDesignerUndoEngine)
		{
			flowDesignerUndoEngine.PerformRedo();
		}
	}

	private void OnMenuClicked(object sender, EventArgs e)
	{
		if (!(sender is ToolStripMenuItem toolStripMenuItem) || !(toolStripMenuItem.Tag is MenuCommand))
		{
			return;
		}
		MenuCommand menuCommand = toolStripMenuItem.Tag as MenuCommand;
		try
		{
			if (menuCommand.CommandID == WorkflowMenuCommands.DesignerProperties)
			{
				this.ShowPropertiesRequested?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				if (menuCommand.CommandID == StandardCommands.F1Help)
				{
					if (!(GetService(typeof(ISelectionService)) is ISelectionService selectionService))
					{
						return;
					}
					{
						foreach (object selectedComponent in selectionService.GetSelectedComponents())
						{
							if (selectedComponent is IVadActivity)
							{
								(selectedComponent as IVadActivity).ShowHelp();
								break;
							}
						}
						return;
					}
				}
				menuCommand.Invoke();
			}
		}
		catch (Exception exc)
		{
			if (menuCommand.CommandID != StandardCommands.Paste)
			{
				MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("FlowDesignerControl.MessageBox.Error.ExecutingContextMenu"), toolStripMenuItem.Text, ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("FlowDesignerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		finally
		{
			this.CommandExecuted?.Invoke(this, EventArgs.Empty);
		}
	}

	private ToolStripItem[] GetSelectionMenuItems()
	{
		List<ToolStripItem> list = new List<ToolStripItem>();
		if (GetService(typeof(ISelectionService)) is ISelectionService selectionService)
		{
			foreach (object selectedComponent in selectionService.GetSelectedComponents())
			{
				if (!(selectedComponent is Activity))
				{
					return list.ToArray();
				}
			}
		}
		foreach (DesignerVerb verb in Verbs)
		{
			if (verb.Enabled && verb.Visible)
			{
				object obj = verb.Properties["Image"];
				if (obj is Image)
				{
					list.Add(GetToolStripItem(verb.CommandID, verb.Text, obj as Image, ToolStripItemImageScaling.None, Keys.None));
				}
				else if (obj is Icon)
				{
					list.Add(GetToolStripItem(verb.CommandID, verb.Text, (obj as Icon).ToBitmap(), ToolStripItemImageScaling.None, Keys.None));
				}
				else
				{
					list.Add(GetToolStripItem(verb.CommandID, verb.Text));
				}
			}
		}
		if (list.Count > 0)
		{
			list.Add(new ToolStripSeparator());
		}
		list.Add(GetToolStripItem(StandardCommands.Cut, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Cut"), Resources.Edit_Cut, ToolStripItemImageScaling.None, Keys.X | Keys.Control));
		list.Add(GetToolStripItem(StandardCommands.Copy, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Copy"), Resources.Edit_Copy, ToolStripItemImageScaling.None, Keys.C | Keys.Control));
		list.Add(GetToolStripItem(StandardCommands.Paste, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Paste"), Resources.Edit_Paste, ToolStripItemImageScaling.None, Keys.V | Keys.Control));
		list.Add(GetToolStripItem(StandardCommands.Delete, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Delete"), Resources.Edit_Delete, ToolStripItemImageScaling.None, Keys.Delete));
		list.Add(new ToolStripSeparator());
		list.Add(GetToolStripItem(WorkflowMenuCommands.Enable, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Enable"), Resources.Tools_Enable, ToolStripItemImageScaling.None, Keys.None));
		list.Add(GetToolStripItem(WorkflowMenuCommands.Disable, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Disable"), Resources.Tools_Disable, ToolStripItemImageScaling.None, Keys.None));
		list.Add(new ToolStripSeparator());
		list.Add(GetToolStripItem(WorkflowMenuCommands.Collapse, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Collapse"), Resources.Tools_Collapse, ToolStripItemImageScaling.None, Keys.None));
		list.Add(GetToolStripItem(WorkflowMenuCommands.Expand, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Expand"), Resources.Tools_Expand, ToolStripItemImageScaling.None, Keys.None));
		list.Add(new ToolStripSeparator());
		list.Add(GetToolStripItem(WorkflowMenuCommands.DesignerProperties, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Properties"), Resources.View_PropertiesWindow, ToolStripItemImageScaling.None, Keys.P | Keys.Alt));
		list.Add(GetToolStripItem(StandardCommands.F1Help, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.Help"), Resources.Help, ToolStripItemImageScaling.None, Keys.F1));
		return list.ToArray();
	}

	private ToolStripItem[] GetZoomMenuItems()
	{
		return new List<ToolStripItem>
		{
			GetToolStripItem(WorkflowMenuCommands.Zoom400Mode, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomLevel400")),
			GetToolStripItem(WorkflowMenuCommands.Zoom300Mode, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomLevel300")),
			GetToolStripItem(WorkflowMenuCommands.Zoom200Mode, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomLevel200")),
			GetToolStripItem(WorkflowMenuCommands.Zoom150Mode, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomLevel150")),
			GetToolStripItem(WorkflowMenuCommands.Zoom100Mode, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomLevel100")),
			GetToolStripItem(WorkflowMenuCommands.Zoom75Mode, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomLevel75")),
			GetToolStripItem(WorkflowMenuCommands.Zoom50Mode, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomLevel50")),
			new ToolStripSeparator(),
			GetToolStripItem(WorkflowMenuCommands.ShowAll, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ShowAll"))
		}.ToArray();
	}

	private ToolStripItem[] GetPanMenuItems()
	{
		return new List<ToolStripItem>
		{
			GetToolStripItem(WorkflowMenuCommands.ZoomIn, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomIn"), Resources.Tools_NavigationZoomIn, ToolStripItemImageScaling.SizeToFit, Keys.None),
			GetToolStripItem(WorkflowMenuCommands.ZoomOut, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.ZoomOut"), Resources.Tools_NavigationZoomOut, ToolStripItemImageScaling.SizeToFit, Keys.None),
			GetToolStripItem(WorkflowMenuCommands.Pan, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.NavigationPan"), Resources.Tools_NavigationPan, ToolStripItemImageScaling.SizeToFit, Keys.None),
			GetToolStripItem(WorkflowMenuCommands.DefaultFilter, LocalizedResourceMgr.GetString("FlowDesignerControl.ContextMenu.NavigationDefault"), Resources.Tools_NavigationDefault, ToolStripItemImageScaling.SizeToFit, Keys.None)
		}.ToArray();
	}

	private ToolStripItem GetToolStripItem(CommandID id, string text)
	{
		MenuCommand menuCommand = FindCommand(id);
		if (menuCommand == null)
		{
			menuCommand = new MenuCommand(OnMenuClicked, id);
		}
		ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text);
		toolStripMenuItem.Checked = menuCommand.Checked;
		toolStripMenuItem.Enabled = menuCommand.Enabled;
		toolStripMenuItem.Tag = menuCommand;
		toolStripMenuItem.Click += OnMenuClicked;
		return toolStripMenuItem;
	}

	private ToolStripItem GetToolStripItem(CommandID id, string text, Image image, ToolStripItemImageScaling imageScaling, Keys shortcutKeys)
	{
		MenuCommand menuCommand = FindCommand(id);
		if (menuCommand == null)
		{
			menuCommand = new MenuCommand(OnMenuClicked, id);
		}
		return new ToolStripMenuItem(text, image, OnMenuClicked)
		{
			Checked = menuCommand.Checked,
			Enabled = (id == StandardCommands.Paste || menuCommand.Enabled),
			ImageScaling = imageScaling,
			Tag = menuCommand,
			ShortcutKeys = shortcutKeys
		};
	}

	public FlowDesignerMenuCommandService(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
		MenuCommand command = new MenuCommand(ExecuteUndo, StandardCommands.Undo)
		{
			Enabled = false
		};
		AddCommand(command);
		MenuCommand command2 = new MenuCommand(ExecuteRedo, StandardCommands.Redo)
		{
			Enabled = false
		};
		AddCommand(command2);
	}

	public override void ShowContextMenu(CommandID menuID, int x, int y)
	{
		ContextMenuStrip contextMenu = new ContextMenuStrip();
		bool flag = true;
		if (menuID == WorkflowMenuCommands.SelectionMenu)
		{
			contextMenu.Items.AddRange(GetSelectionMenuItems());
			flag = false;
		}
		else if (menuID == WorkflowMenuCommands.ZoomMenu)
		{
			contextMenu.Items.AddRange(GetZoomMenuItems());
		}
		else if (menuID == WorkflowMenuCommands.PanMenu)
		{
			contextMenu.Items.AddRange(GetPanMenuItems());
		}
		else if (menuID == WorkflowMenuCommands.DesignerActionsMenu)
		{
			foreach (MenuCommand command in GetCommandList(menuID.Guid))
			{
				if (command.Properties.Count > 1)
				{
					ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(command.Properties["Text"].ToString(), null, OnMenuClicked);
					contextMenu.Items.Add(toolStripMenuItem);
					toolStripMenuItem.Tag = command;
					command.Invoke();
				}
			}
		}
		if (contextMenu.Items.Count > 0)
		{
			if (flag)
			{
				contextMenu.MouseLeave += d;
			}
			if (GetService(typeof(WorkflowView)) is WorkflowView workflowView)
			{
				contextMenu.Show(workflowView, workflowView.PointToClient(new Point(x, y)));
			}
		}
		void d(object o, EventArgs e)
		{
			contextMenu.MouseLeave -= d;
			contextMenu.Close();
		}
	}
}

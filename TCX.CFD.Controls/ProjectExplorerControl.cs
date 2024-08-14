using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class ProjectExplorerControl : UserControl
{
	private StatusStrip statusStrip;

	private PropertyGrid propertyGrid;

	private Rectangle dragBoxFromMouseDown;

	private IContainer components;

	private TreeView projectExplorerTreeView;

	private ImageList treeViewImageList;

	private ContextMenuStrip contextMenu;

	private ToolStripMenuItem saveToolStripMenuItem;

	private ToolStripMenuItem saveAsToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem renameToolStripMenuItem;

	private ToolStripMenuItem closeProjectToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem newFolderToolStripMenuItem;

	private ToolStripMenuItem newDialerToolStripMenuItem;

	private ToolStripMenuItem newCallflowToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripMenuItem releaseBuildToolStripMenuItem;

	private ToolStripMenuItem debugBuildToolStripMenuItem;

	private ToolStripMenuItem newComponentToolStripMenuItem;

	private ToolStripMenuItem removeDialerToolStripMenuItem;

	private ToolStripMenuItem removeCallflowToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripMenuItem addExistingDialerToolStripMenuItem;

	private ToolStripMenuItem addExistingCallflowToolStripMenuItem;

	private ToolStripMenuItem addExistingComponentToolStripMenuItem;

	private OpenFileDialog openExistingFileDialog;

	private ToolStripMenuItem openDialerToolStripMenuItem;

	private ToolStripMenuItem openCallflowToolStripMenuItem;

	private ToolStripMenuItem openComponentToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator0;

	private ToolStripMenuItem buildAllToolStripMenuItem;

	private ToolStripMenuItem removeComponentToolStripMenuItem;

	private ToolStripMenuItem removeFolderToolStripMenuItem;

	private ToolStripMenuItem closeDialerToolStripMenuItem;

	private ToolStripMenuItem closeCallflowToolStripMenuItem;

	private ToolStripMenuItem closeComponentToolStripMenuItem;

	public bool IsProjectSelected
	{
		get
		{
			TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
			if (selectedNode == null)
			{
				return false;
			}
			return selectedNode.Level == 0;
		}
	}

	public bool IsFolderSelected
	{
		get
		{
			TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
			if (selectedNode == null)
			{
				return false;
			}
			return selectedNode.Tag is FolderObject;
		}
	}

	public bool IsDialerFileSelected
	{
		get
		{
			TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
			if (selectedNode == null)
			{
				return false;
			}
			return selectedNode.Tag is DialerFileObject;
		}
	}

	public bool IsCallflowFileSelected
	{
		get
		{
			TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
			if (selectedNode == null)
			{
				return false;
			}
			return selectedNode.Tag is CallflowFileObject;
		}
	}

	public bool IsComponentFileSelected
	{
		get
		{
			TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
			if (selectedNode == null)
			{
				return false;
			}
			return selectedNode.Tag is ComponentFileObject;
		}
	}

	public event EventHandler AvailableMenuCommandsChange;

	public event EventHandler OnSaveProjectAsRequested;

	public event ProjectManagerControl.BuildRequestEventHandler BuildRequest;

	private void LoadLabelsFromResources()
	{
		openDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.OpenDialer");
		openCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.OpenCallflow");
		openComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.OpenComponent");
		saveToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.Save");
		saveAsToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.SaveAs");
		renameToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.Rename");
		removeDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.RemoveDialer");
		removeCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.RemoveCallflow");
		removeComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.RemoveComponent");
		removeFolderToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.RemoveFolder");
		closeProjectToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.CloseProject");
		closeDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.CloseDialer");
		closeCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.CloseCallflow");
		closeComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.CloseComponent");
		newFolderToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.NewFolder");
		newDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.NewDialer");
		newCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.NewCallflow");
		newComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.NewComponent");
		addExistingDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.AddExistingDialer");
		addExistingCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.AddExistingCallflow");
		addExistingComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.AddExistingComponent");
		debugBuildToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.DebugBuild");
		releaseBuildToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.ReleaseBuild");
		buildAllToolStripMenuItem.Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.ContextMenu.BuildAll");
		openExistingFileDialog.Title = LocalizedResourceMgr.GetString("ProjectExplorerControl.OpenExistingFileDialog.Title");
	}

	protected override void OnEnter(EventArgs e)
	{
		base.OnEnter(e);
		if (propertyGrid != null)
		{
			TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
			propertyGrid.SelectedObject = selectedNode?.Tag;
			this.AvailableMenuCommandsChange(this, EventArgs.Empty);
		}
	}

	public ProjectExplorerControl()
	{
		InitializeComponent();
		SuspendLayout();
		LoadLabelsFromResources();
		ResumeLayout(performLayout: true);
	}

	public void SetParentControls(StatusStrip statusStrip, PropertyGrid propertyGrid)
	{
		this.statusStrip = statusStrip;
		this.propertyGrid = propertyGrid;
	}

	public void LoadProject(ProjectObject projectObject)
	{
		projectObject.NameChanged += FileSystemObject_NameChanged;
		TreeNode treeNode = new TreeNode(projectObject.Name, 0, 0);
		treeNode.Tag = projectObject;
		projectExplorerTreeView.Nodes.Add(treeNode);
		projectObject.FillChildNodes(treeNode);
		projectExplorerTreeView.ExpandAll();
		projectExplorerTreeView.SelectedNode = treeNode;
	}

	public void UnloadProject()
	{
		projectExplorerTreeView.Nodes.Clear();
		propertyGrid.SelectedObject = null;
		this.AvailableMenuCommandsChange(this, EventArgs.Empty);
	}

	public TreeNode AddNode(TreeNode parent, AbsFileSystemObject fso)
	{
		TreeNode treeNode = parent.Nodes.Add(fso.Name, fso.Name, fso.GetCollapsedImageIndex(), fso.GetCollapsedImageIndex());
		treeNode.Tag = fso;
		if (!fso.Exists())
		{
			treeNode.ForeColor = Color.Red;
			treeNode.ToolTipText = LocalizedResourceMgr.GetString("ProjectExplorerControl.Error.MissingFileSystemObject");
		}
		fso.NameChanged += FileSystemObject_NameChanged;
		projectExplorerTreeView.Sort();
		return treeNode;
	}

	private void FileSystemObject_NameChanged(object sender, EventArgs e)
	{
		propertyGrid.Refresh();
		foreach (TreeNode node in projectExplorerTreeView.Nodes)
		{
			if (RefreshFileSystemObjectName(node, sender as AbsFileSystemObject))
			{
				break;
			}
		}
		projectExplorerTreeView.Sort();
	}

	private bool RefreshFileSystemObjectName(TreeNode node, AbsFileSystemObject fso)
	{
		if (node.Tag == fso)
		{
			node.Text = fso.Name;
			return true;
		}
		foreach (TreeNode node2 in node.Nodes)
		{
			if (RefreshFileSystemObjectName(node2, fso))
			{
				return true;
			}
		}
		return false;
	}

	private void ProjectExplorerTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
	{
		AbsFileSystemObject absFileSystemObject = e.Node.Tag as AbsFileSystemObject;
		e.Node.ImageIndex = absFileSystemObject.GetCollapsedImageIndex();
		e.Node.SelectedImageIndex = absFileSystemObject.GetCollapsedImageIndex();
	}

	private void ProjectExplorerTreeView_AfterExpand(object sender, TreeViewEventArgs e)
	{
		AbsFileSystemObject absFileSystemObject = e.Node.Tag as AbsFileSystemObject;
		e.Node.ImageIndex = absFileSystemObject.GetExpandedImageIndex();
		e.Node.SelectedImageIndex = absFileSystemObject.GetExpandedImageIndex();
	}

	private void ProjectExplorerTreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		propertyGrid.SelectedObject = e.Node.Tag;
		this.AvailableMenuCommandsChange(this, EventArgs.Empty);
	}

	private void ProjectExplorerTreeView_MouseDown(object sender, MouseEventArgs e)
	{
		TreeNode nodeAt = projectExplorerTreeView.GetNodeAt(e.Location);
		if (nodeAt != null)
		{
			projectExplorerTreeView.SelectedNode = nodeAt;
			Size dragSize = SystemInformation.DragSize;
			dragBoxFromMouseDown = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
		}
		else
		{
			dragBoxFromMouseDown = Rectangle.Empty;
		}
	}

	private void ProjectExplorerTreeView_MouseUp(object sender, MouseEventArgs e)
	{
		dragBoxFromMouseDown = Rectangle.Empty;
		TreeNode nodeAt = projectExplorerTreeView.GetNodeAt(e.Location);
		if (nodeAt != null && e.Button == MouseButtons.Right)
		{
			(nodeAt.Tag as AbsFileSystemObject).ConfigureContextMenu(openDialerToolStripMenuItem, openCallflowToolStripMenuItem, openComponentToolStripMenuItem, toolStripSeparator0, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator1, renameToolStripMenuItem, removeDialerToolStripMenuItem, removeCallflowToolStripMenuItem, removeComponentToolStripMenuItem, removeFolderToolStripMenuItem, closeProjectToolStripMenuItem, closeDialerToolStripMenuItem, closeCallflowToolStripMenuItem, closeComponentToolStripMenuItem, toolStripSeparator2, newFolderToolStripMenuItem, newDialerToolStripMenuItem, newCallflowToolStripMenuItem, newComponentToolStripMenuItem, toolStripSeparator3, addExistingDialerToolStripMenuItem, addExistingCallflowToolStripMenuItem, addExistingComponentToolStripMenuItem, toolStripSeparator4, debugBuildToolStripMenuItem, releaseBuildToolStripMenuItem, buildAllToolStripMenuItem);
			contextMenu.Show(projectExplorerTreeView, e.Location);
		}
	}

	private void ProjectExplorerTreeView_MouseMove(object sender, MouseEventArgs e)
	{
		if ((e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) && projectExplorerTreeView.SelectedNode != null && projectExplorerTreeView.SelectedNode.Level > 0 && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
		{
			projectExplorerTreeView.DoDragDrop(projectExplorerTreeView.SelectedNode, DragDropEffects.Move);
		}
	}

	private void ProjectExplorerTreeView_DragOver(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(typeof(TreeNode)))
		{
			e.Effect = DragDropEffects.Move;
			projectExplorerTreeView.SelectedNode = projectExplorerTreeView.GetNodeAt(projectExplorerTreeView.PointToClient(new Point(e.X, e.Y)));
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void ProjectExplorerTreeView_DragDrop(object sender, DragEventArgs e)
	{
		TreeNode treeNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;
		TreeNode nodeAt = projectExplorerTreeView.GetNodeAt(projectExplorerTreeView.PointToClient(new Point(e.X, e.Y)));
		if (treeNode == null || nodeAt == null || treeNode == nodeAt)
		{
			return;
		}
		AbsFileSystemObject absFileSystemObject = treeNode.Tag as AbsFileSystemObject;
		AbsFileSystemObject absFileSystemObject2 = nodeAt.Tag as AbsFileSystemObject;
		if (absFileSystemObject.GetParent() == absFileSystemObject2)
		{
			projectExplorerTreeView.SelectedNode = treeNode;
			return;
		}
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Moving");
			statusStrip.Refresh();
			absFileSystemObject.Move(absFileSystemObject2);
			treeNode.Parent.Nodes.Remove(treeNode);
			if (absFileSystemObject2.GetFileSystemObjectContainer() == absFileSystemObject2)
			{
				nodeAt.Nodes.Add(treeNode);
			}
			else
			{
				nodeAt.Parent.Nodes.Add(treeNode);
			}
			projectExplorerTreeView.Sort();
			projectExplorerTreeView.SelectedNode = treeNode;
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.Moving"), absFileSystemObject.Path, ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	private void ProjectExplorerTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
	{
		if (e.Label == null)
		{
			projectExplorerTreeView.LabelEdit = false;
		}
		else if (e.Label.Length == 0)
		{
			e.CancelEdit = true;
			MessageBox.Show(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.InvalidFileSystemObjectName"), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else
		{
			if (!(e.Label != e.Node.Text))
			{
				return;
			}
			TreeNode treeNode = e.Node.Parent;
			if (treeNode != null && ChildNodeExists(treeNode, e.Label))
			{
				e.CancelEdit = true;
				MessageBox.Show(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.FileSystemObjectAlreadyExist"), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			AbsFileSystemObject absFileSystemObject = e.Node.Tag as AbsFileSystemObject;
			if (!(absFileSystemObject is ComponentFileObject) || absFileSystemObject.GetProjectObject().IsValidNewComponentName(e.Label))
			{
				try
				{
					Cursor = Cursors.WaitCursor;
					statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Renaming");
					statusStrip.Refresh();
					absFileSystemObject.Name = e.Label;
					e.CancelEdit = true;
					projectExplorerTreeView.LabelEdit = false;
					projectExplorerTreeView.Sort();
					projectExplorerTreeView.SelectedNode = e.Node;
					this.AvailableMenuCommandsChange(this, EventArgs.Empty);
					return;
				}
				catch (Exception exc)
				{
					e.CancelEdit = true;
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.Renaming"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				finally
				{
					Cursor = Cursors.Default;
					statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
				}
			}
			e.CancelEdit = true;
			MessageBox.Show(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.ComponentObjectNameAlreadyExist"), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ProjectExplorerTreeView_KeyDown(object sender, KeyEventArgs e)
	{
		TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
		if (selectedNode == null)
		{
			return;
		}
		switch (e.KeyCode)
		{
		case Keys.F2:
			renameToolStripMenuItem.PerformClick();
			break;
		case Keys.Delete:
			if (selectedNode.Level > 0)
			{
				Remove();
			}
			break;
		case Keys.Return:
			selectedNode.Toggle();
			OpenFile();
			break;
		}
	}

	private void ProjectExplorerTreeView_DoubleClick(object sender, EventArgs e)
	{
		OpenFile();
	}

	private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
	{
		OpenFile();
	}

	private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		AbsFileSystemObject absFileSystemObject = projectExplorerTreeView.SelectedNode.Tag as AbsFileSystemObject;
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Saving");
			statusStrip.Refresh();
			absFileSystemObject.Save();
			this.AvailableMenuCommandsChange(this, EventArgs.Empty);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.Saving"), absFileSystemObject.Path, ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		this.OnSaveProjectAsRequested?.Invoke(this, EventArgs.Empty);
	}

	private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
		if (selectedNode != null)
		{
			projectExplorerTreeView.LabelEdit = true;
			selectedNode.BeginEdit();
		}
	}

	private void CloseFileRequest(TreeNode node)
	{
		if (node.Nodes.Count > 0)
		{
			foreach (TreeNode node2 in node.Nodes)
			{
				CloseFileRequest(node2);
			}
			return;
		}
		(node.Tag as AbsFileSystemObject).Close();
	}

	private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Remove();
	}

	private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
	{
		(projectExplorerTreeView.SelectedNode.Tag as AbsFileSystemObject).Close();
		this.AvailableMenuCommandsChange(this, EventArgs.Empty);
	}

	private string GetTempObjectName(ProjectObject projectObject, string defaultName, string defaultExtension, bool hasToCheckValidComponentName)
	{
		int num = 0;
		string text;
		do
		{
			text = $"{defaultName}{++num}{defaultExtension}";
		}
		while (ChildNodeExists(projectExplorerTreeView.SelectedNode, text) || (hasToCheckValidComponentName && !projectObject.IsValidNewComponentName(text)));
		return text;
	}

	private bool ChildNodeExists(TreeNode parent, string text)
	{
		foreach (TreeNode node in parent.Nodes)
		{
			if (node.Text == text)
			{
				return true;
			}
		}
		return false;
	}

	private void AddNodeAndStartEditing(AbsFileSystemObject fso)
	{
		TreeNode treeNode = AddNode(projectExplorerTreeView.SelectedNode, fso);
		projectExplorerTreeView.LabelEdit = true;
		projectExplorerTreeView.SelectedNode = treeNode;
		treeNode.EnsureVisible();
		treeNode.BeginEdit();
	}

	private void NewFolderToolStripMenuItem_Click(object sender, EventArgs e)
	{
		IFileSystemObjectContainer fileSystemObjectContainer = (projectExplorerTreeView.SelectedNode.Tag as AbsFileSystemObject).GetFileSystemObjectContainer();
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.AddingFolder");
			statusStrip.Refresh();
			string tempObjectName = GetTempObjectName(fileSystemObjectContainer.GetProjectObject(), LocalizedResourceMgr.GetString("ProjectExplorerControl.Folder.DefaultName"), string.Empty, hasToCheckValidComponentName: false);
			FolderObject fso = fileSystemObjectContainer.AddFolder(tempObjectName);
			AddNodeAndStartEditing(fso);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.AddingFolder"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	private void NewDialerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (projectExplorerTreeView.SelectedNode == null)
		{
			projectExplorerTreeView.SelectedNode = projectExplorerTreeView.Nodes[0];
		}
		IFileSystemObjectContainer fileSystemObjectContainer = (projectExplorerTreeView.SelectedNode.Tag as AbsFileSystemObject).GetFileSystemObjectContainer();
		while (fileSystemObjectContainer != projectExplorerTreeView.SelectedNode.Tag)
		{
			projectExplorerTreeView.SelectedNode = projectExplorerTreeView.SelectedNode.Parent;
		}
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.AddingDialer");
			statusStrip.Refresh();
			string tempObjectName = GetTempObjectName(fileSystemObjectContainer.GetProjectObject(), LocalizedResourceMgr.GetString("ProjectExplorerControl.Dialer.DefaultName"), LocalizedResourceMgr.GetString("ProjectExplorerControl.Dialer.DefaultExtension"), hasToCheckValidComponentName: false);
			DialerFileObject fso = fileSystemObjectContainer.AddDialer(tempObjectName);
			AddNodeAndStartEditing(fso);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.AddingDialer"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	private void NewCallflowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (projectExplorerTreeView.SelectedNode == null)
		{
			projectExplorerTreeView.SelectedNode = projectExplorerTreeView.Nodes[0];
		}
		IFileSystemObjectContainer fileSystemObjectContainer = (projectExplorerTreeView.SelectedNode.Tag as AbsFileSystemObject).GetFileSystemObjectContainer();
		while (fileSystemObjectContainer != projectExplorerTreeView.SelectedNode.Tag)
		{
			projectExplorerTreeView.SelectedNode = projectExplorerTreeView.SelectedNode.Parent;
		}
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.AddingCallflow");
			statusStrip.Refresh();
			string tempObjectName = GetTempObjectName(fileSystemObjectContainer.GetProjectObject(), LocalizedResourceMgr.GetString("ProjectExplorerControl.Callflow.DefaultName"), LocalizedResourceMgr.GetString("ProjectExplorerControl.Callflow.DefaultExtension"), hasToCheckValidComponentName: false);
			CallflowFileObject fso = fileSystemObjectContainer.AddCallflow(tempObjectName);
			AddNodeAndStartEditing(fso);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.AddingCallflow"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	private void NewComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (projectExplorerTreeView.SelectedNode == null)
		{
			projectExplorerTreeView.SelectedNode = projectExplorerTreeView.Nodes[0];
		}
		IFileSystemObjectContainer fileSystemObjectContainer = (projectExplorerTreeView.SelectedNode.Tag as AbsFileSystemObject).GetFileSystemObjectContainer();
		while (fileSystemObjectContainer != projectExplorerTreeView.SelectedNode.Tag)
		{
			projectExplorerTreeView.SelectedNode = projectExplorerTreeView.SelectedNode.Parent;
		}
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.AddingComponent");
			statusStrip.Refresh();
			string tempObjectName = GetTempObjectName(fileSystemObjectContainer.GetProjectObject(), LocalizedResourceMgr.GetString("ProjectExplorerControl.Component.DefaultName"), LocalizedResourceMgr.GetString("ProjectExplorerControl.Component.DefaultExtension"), hasToCheckValidComponentName: true);
			ComponentFileObject fso = fileSystemObjectContainer.AddComponent(tempObjectName);
			AddNodeAndStartEditing(fso);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.AddingComponent"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	private void AddExistingDialerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		AddExistingDialer();
	}

	private void AddExistingCallflowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		AddExistingCallflow();
	}

	private void AddExistingComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		AddExistingComponent();
	}

	private void DebugBuildToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Building");
			statusStrip.Refresh();
			this.BuildRequest(BuildTypes.Debug);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.Building"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	private void ReleaseBuildToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Building");
			statusStrip.Refresh();
			this.BuildRequest(BuildTypes.Release);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.Building"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	private void BuildAllToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Building");
			statusStrip.Refresh();
			this.BuildRequest?.Invoke(BuildTypes.All);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.Building"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	public void CreateNewFolder()
	{
		newFolderToolStripMenuItem.PerformClick();
	}

	public void CreateNewDialer()
	{
		newDialerToolStripMenuItem.Visible = true;
		newDialerToolStripMenuItem.Enabled = true;
		newDialerToolStripMenuItem.PerformClick();
	}

	public void CreateNewCallflow()
	{
		newCallflowToolStripMenuItem.Visible = true;
		newCallflowToolStripMenuItem.Enabled = true;
		newCallflowToolStripMenuItem.PerformClick();
	}

	public void CreateNewComponent()
	{
		newComponentToolStripMenuItem.Visible = true;
		newComponentToolStripMenuItem.Enabled = true;
		newComponentToolStripMenuItem.PerformClick();
	}

	public void OpenFile()
	{
		TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
		if (selectedNode == null || selectedNode.Level <= 0)
		{
			return;
		}
		AbsFileSystemObject absFileSystemObject = selectedNode.Tag as AbsFileSystemObject;
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.OpeningFile");
			statusStrip.Refresh();
			absFileSystemObject.Open();
			selectedNode.ForeColor = Color.Black;
			selectedNode.ToolTipText = string.Empty;
		}
		catch (FileNotFoundException exc)
		{
			selectedNode.ForeColor = Color.Red;
			selectedNode.ToolTipText = LocalizedResourceMgr.GetString("ProjectExplorerControl.Error.MissingFileSystemObject");
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.OpeningFile"), absFileSystemObject.Path, ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		catch (Exception exc2)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.OpeningFile"), absFileSystemObject.Path, ErrorHelper.GetErrorDescription(exc2)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	public void AddExistingDialer()
	{
		TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
		IFileSystemObjectContainer fileSystemObjectContainer = (selectedNode.Tag as AbsFileSystemObject).GetFileSystemObjectContainer();
		try
		{
			openExistingFileDialog.InitialDirectory = (string.IsNullOrEmpty(Settings.Default.LastOpenExistingFileFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.LastOpenExistingFileFolder);
			openExistingFileDialog.Filter = LocalizedResourceMgr.GetString("FileDialogs.DialerFiles.Filter");
			if (openExistingFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.AddingDialer");
			statusStrip.Refresh();
			string[] fileNames = openExistingFileDialog.FileNames;
			for (int i = 0; i < fileNames.Length; i++)
			{
				FileInfo fileInfo = new FileInfo(fileNames[i]);
				if (fileSystemObjectContainer.ChildExists(fileInfo.Name))
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.FileAlreadyExistsNotAdded"), fileInfo.Name), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					continue;
				}
				if (fileInfo.DirectoryName != fileSystemObjectContainer.GetFolderPath())
				{
					fileInfo.CopyTo(fileSystemObjectContainer.GetFolderPath() + "\\" + fileInfo.Name, overwrite: true);
				}
				Settings.Default.LastOpenExistingFileFolder = fileInfo.DirectoryName;
				DialerFileObject fso = fileSystemObjectContainer.AddDialer(fileInfo.Name);
				AddNode(selectedNode, fso);
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.AddingDialer"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	public void AddExistingCallflow()
	{
		TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
		IFileSystemObjectContainer fileSystemObjectContainer = (selectedNode.Tag as AbsFileSystemObject).GetFileSystemObjectContainer();
		try
		{
			openExistingFileDialog.InitialDirectory = (string.IsNullOrEmpty(Settings.Default.LastOpenExistingFileFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.LastOpenExistingFileFolder);
			openExistingFileDialog.Filter = LocalizedResourceMgr.GetString("FileDialogs.CallflowFiles.Filter");
			if (openExistingFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.AddingCallflow");
			statusStrip.Refresh();
			string[] fileNames = openExistingFileDialog.FileNames;
			for (int i = 0; i < fileNames.Length; i++)
			{
				FileInfo fileInfo = new FileInfo(fileNames[i]);
				if (fileSystemObjectContainer.ChildExists(fileInfo.Name))
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.FileAlreadyExistsNotAdded"), fileInfo.Name), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					continue;
				}
				if (fileInfo.DirectoryName != fileSystemObjectContainer.GetFolderPath())
				{
					fileInfo.CopyTo(fileSystemObjectContainer.GetFolderPath() + "\\" + fileInfo.Name, overwrite: true);
				}
				Settings.Default.LastOpenExistingFileFolder = fileInfo.DirectoryName;
				CallflowFileObject fso = fileSystemObjectContainer.AddCallflow(fileInfo.Name);
				AddNode(selectedNode, fso);
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.AddingCallflow"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	public void AddExistingComponent()
	{
		try
		{
			openExistingFileDialog.InitialDirectory = (string.IsNullOrEmpty(Settings.Default.LastOpenExistingFileFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.LastOpenExistingFileFolder);
			openExistingFileDialog.Filter = LocalizedResourceMgr.GetString("FileDialogs.ComponentFiles.Filter");
			if (openExistingFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.AddingComponent");
			statusStrip.Refresh();
			TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
			if (selectedNode == null)
			{
				throw new ApplicationException("Project Explorer doesn't have any node selected.");
			}
			IFileSystemObjectContainer fileSystemObjectContainer = ((selectedNode.Tag as AbsFileSystemObject) ?? throw new ApplicationException("Selected node in the Project Explorer doesn't have a file system object attached.")).GetFileSystemObjectContainer();
			string[] fileNames = openExistingFileDialog.FileNames;
			for (int i = 0; i < fileNames.Length; i++)
			{
				FileInfo fileInfo = new FileInfo(fileNames[i]);
				if (fileSystemObjectContainer.ChildExists(fileInfo.Name))
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.FileAlreadyExistsNotAdded"), fileInfo.Name), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					continue;
				}
				if (fileInfo.DirectoryName != fileSystemObjectContainer.GetFolderPath())
				{
					fileInfo.CopyTo(fileSystemObjectContainer.GetFolderPath() + "\\" + fileInfo.Name, overwrite: true);
				}
				Settings.Default.LastOpenExistingFileFolder = fileInfo.DirectoryName;
				ComponentFileObject fso = fileSystemObjectContainer.AddComponent(fileInfo.Name);
				AddNode(selectedNode, fso);
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.AddingComponent"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	public void Rename()
	{
		renameToolStripMenuItem.PerformClick();
	}

	public void Remove()
	{
		AbsFileSystemObject absFileSystemObject = projectExplorerTreeView.SelectedNode.Tag as AbsFileSystemObject;
		if (MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Question.ConfirmRemove"), absFileSystemObject.Name), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
		{
			return;
		}
		try
		{
			Cursor = Cursors.WaitCursor;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Removing");
			statusStrip.Refresh();
			CloseFileRequest(projectExplorerTreeView.SelectedNode);
			absFileSystemObject.Delete();
			TreeNode treeNode = projectExplorerTreeView.SelectedNode.Parent;
			projectExplorerTreeView.SelectedNode.Remove();
			if (treeNode != null && treeNode.Nodes.Count == 0)
			{
				treeNode.Collapse();
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Error.Removing"), absFileSystemObject.Path, ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ProjectExplorerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectExplorerControl.Statusbar.Ready");
		}
	}

	public string GetSelectedFileName()
	{
		TreeNode selectedNode = projectExplorerTreeView.SelectedNode;
		if (selectedNode == null)
		{
			return string.Empty;
		}
		return ((AbsFileSystemObject)selectedNode.Tag).Name;
	}

	private void ProjectExplorerControl_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-workspace/#h.no86qsu46grj");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCX.CFD.Controls.ProjectExplorerControl));
		this.projectExplorerTreeView = new System.Windows.Forms.TreeView();
		this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
		this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.openDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator0 = new System.Windows.Forms.ToolStripSeparator();
		this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.removeDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.removeCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.removeComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.removeFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.closeDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.closeCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.closeComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.addExistingDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.addExistingCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.addExistingComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.debugBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.releaseBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.buildAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openExistingFileDialog = new System.Windows.Forms.OpenFileDialog();
		this.contextMenu.SuspendLayout();
		base.SuspendLayout();
		this.projectExplorerTreeView.AllowDrop = true;
		this.projectExplorerTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.projectExplorerTreeView.HideSelection = false;
		this.projectExplorerTreeView.ImageIndex = 0;
		this.projectExplorerTreeView.ImageList = this.treeViewImageList;
		this.projectExplorerTreeView.Location = new System.Drawing.Point(0, 0);
		this.projectExplorerTreeView.Name = "projectExplorerTreeView";
		this.projectExplorerTreeView.SelectedImageIndex = 0;
		this.projectExplorerTreeView.ShowNodeToolTips = true;
		this.projectExplorerTreeView.Size = new System.Drawing.Size(150, 150);
		this.projectExplorerTreeView.TabIndex = 15;
		this.projectExplorerTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(ProjectExplorerTreeView_AfterLabelEdit);
		this.projectExplorerTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(ProjectExplorerTreeView_AfterCollapse);
		this.projectExplorerTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(ProjectExplorerTreeView_AfterExpand);
		this.projectExplorerTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(ProjectExplorerTreeView_AfterSelect);
		this.projectExplorerTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(ProjectExplorerTreeView_DragDrop);
		this.projectExplorerTreeView.DragOver += new System.Windows.Forms.DragEventHandler(ProjectExplorerTreeView_DragOver);
		this.projectExplorerTreeView.DoubleClick += new System.EventHandler(ProjectExplorerTreeView_DoubleClick);
		this.projectExplorerTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(ProjectExplorerTreeView_KeyDown);
		this.projectExplorerTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(ProjectExplorerTreeView_MouseDown);
		this.projectExplorerTreeView.MouseMove += new System.Windows.Forms.MouseEventHandler(ProjectExplorerTreeView_MouseMove);
		this.projectExplorerTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(ProjectExplorerTreeView_MouseUp);
		this.treeViewImageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("treeViewImageList.ImageStream");
		this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
		this.treeViewImageList.Images.SetKeyName(0, "Project.png");
		this.treeViewImageList.Images.SetKeyName(1, "OpenedFolder.png");
		this.treeViewImageList.Images.SetKeyName(2, "ClosedFolder.png");
		this.treeViewImageList.Images.SetKeyName(3, "Dialer.png");
		this.treeViewImageList.Images.SetKeyName(4, "Callflow.png");
		this.treeViewImageList.Images.SetKeyName(5, "Component.png");
		this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[29]
		{
			this.openCallflowToolStripMenuItem, this.openDialerToolStripMenuItem, this.openComponentToolStripMenuItem, this.toolStripSeparator0, this.saveToolStripMenuItem, this.saveAsToolStripMenuItem, this.toolStripSeparator1, this.renameToolStripMenuItem, this.removeCallflowToolStripMenuItem, this.removeDialerToolStripMenuItem,
			this.removeComponentToolStripMenuItem, this.removeFolderToolStripMenuItem, this.closeProjectToolStripMenuItem, this.closeCallflowToolStripMenuItem, this.closeDialerToolStripMenuItem, this.closeComponentToolStripMenuItem, this.toolStripSeparator2, this.newFolderToolStripMenuItem, this.newCallflowToolStripMenuItem, this.newDialerToolStripMenuItem,
			this.newComponentToolStripMenuItem, this.toolStripSeparator3, this.addExistingCallflowToolStripMenuItem, this.addExistingDialerToolStripMenuItem, this.addExistingComponentToolStripMenuItem, this.toolStripSeparator4, this.debugBuildToolStripMenuItem, this.releaseBuildToolStripMenuItem, this.buildAllToolStripMenuItem
		});
		this.contextMenu.Name = "projectContextMenu";
		this.contextMenu.Size = new System.Drawing.Size(251, 658);
		this.openDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenDialer;
		this.openDialerToolStripMenuItem.Name = "openDialerToolStripMenuItem";
		this.openDialerToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.openDialerToolStripMenuItem.Text = "Open Dialer";
		this.openDialerToolStripMenuItem.Click += new System.EventHandler(OpenToolStripMenuItem_Click);
		this.openCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenCallflow;
		this.openCallflowToolStripMenuItem.Name = "openCallflowToolStripMenuItem";
		this.openCallflowToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.openCallflowToolStripMenuItem.Text = "Open Callflow";
		this.openCallflowToolStripMenuItem.Click += new System.EventHandler(OpenToolStripMenuItem_Click);
		this.openComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenUserComponent;
		this.openComponentToolStripMenuItem.Name = "openComponentToolStripMenuItem";
		this.openComponentToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.openComponentToolStripMenuItem.Text = "Open Component";
		this.openComponentToolStripMenuItem.Click += new System.EventHandler(OpenToolStripMenuItem_Click);
		this.toolStripSeparator0.Name = "toolStripSeparator0";
		this.toolStripSeparator0.Size = new System.Drawing.Size(247, 6);
		this.saveToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_Save;
		this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
		this.saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control;
		this.saveToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.saveToolStripMenuItem.Text = "Save";
		this.saveToolStripMenuItem.Click += new System.EventHandler(SaveToolStripMenuItem_Click);
		this.saveAsToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_Save_As;
		this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
		this.saveAsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control;
		this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.saveAsToolStripMenuItem.Text = "Save As";
		this.saveAsToolStripMenuItem.Click += new System.EventHandler(SaveAsToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(247, 6);
		this.renameToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_Rename;
		this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
		this.renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
		this.renameToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.renameToolStripMenuItem.Text = "Rename";
		this.renameToolStripMenuItem.Click += new System.EventHandler(RenameToolStripMenuItem_Click);
		this.removeDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_RemoveDialer;
		this.removeDialerToolStripMenuItem.Name = "removeDialerToolStripMenuItem";
		this.removeDialerToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
		this.removeDialerToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.removeDialerToolStripMenuItem.Text = "Remove Dialer";
		this.removeDialerToolStripMenuItem.Click += new System.EventHandler(RemoveToolStripMenuItem_Click);
		this.removeCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_RemoveCallflow;
		this.removeCallflowToolStripMenuItem.Name = "removeCallflowToolStripMenuItem";
		this.removeCallflowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
		this.removeCallflowToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.removeCallflowToolStripMenuItem.Text = "Remove Callflow";
		this.removeCallflowToolStripMenuItem.Click += new System.EventHandler(RemoveToolStripMenuItem_Click);
		this.removeComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_RemoveComponent;
		this.removeComponentToolStripMenuItem.Name = "removeComponentToolStripMenuItem";
		this.removeComponentToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
		this.removeComponentToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.removeComponentToolStripMenuItem.Text = "Remove Component";
		this.removeComponentToolStripMenuItem.Click += new System.EventHandler(RemoveToolStripMenuItem_Click);
		this.removeFolderToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_RemoveFolder;
		this.removeFolderToolStripMenuItem.Name = "removeFolderToolStripMenuItem";
		this.removeFolderToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.removeFolderToolStripMenuItem.Text = "Remove Folder";
		this.removeFolderToolStripMenuItem.Click += new System.EventHandler(RemoveToolStripMenuItem_Click);
		this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
		this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.closeProjectToolStripMenuItem.Text = "Close Project";
		this.closeProjectToolStripMenuItem.Click += new System.EventHandler(CloseToolStripMenuItem_Click);
		this.closeDialerToolStripMenuItem.Name = "closeDialerToolStripMenuItem";
		this.closeDialerToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.closeDialerToolStripMenuItem.Text = "Close Dialer";
		this.closeDialerToolStripMenuItem.Click += new System.EventHandler(CloseToolStripMenuItem_Click);
		this.closeCallflowToolStripMenuItem.Name = "closeCallflowToolStripMenuItem";
		this.closeCallflowToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.closeCallflowToolStripMenuItem.Text = "Close Callflow";
		this.closeCallflowToolStripMenuItem.Click += new System.EventHandler(CloseToolStripMenuItem_Click);
		this.closeComponentToolStripMenuItem.Name = "closeComponentToolStripMenuItem";
		this.closeComponentToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.closeComponentToolStripMenuItem.Text = "Close Component";
		this.closeComponentToolStripMenuItem.Click += new System.EventHandler(CloseToolStripMenuItem_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(247, 6);
		this.newFolderToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewFolder;
		this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
		this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.newFolderToolStripMenuItem.Text = "New Folder";
		this.newFolderToolStripMenuItem.Click += new System.EventHandler(NewFolderToolStripMenuItem_Click);
		this.newDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewDialer;
		this.newDialerToolStripMenuItem.Name = "newDialerToolStripMenuItem";
		this.newDialerToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.newDialerToolStripMenuItem.Text = "New Dialer";
		this.newDialerToolStripMenuItem.Click += new System.EventHandler(NewDialerToolStripMenuItem_Click);
		this.newCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewCallflow;
		this.newCallflowToolStripMenuItem.Name = "newCallflowToolStripMenuItem";
		this.newCallflowToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.newCallflowToolStripMenuItem.Text = "New Callflow";
		this.newCallflowToolStripMenuItem.Click += new System.EventHandler(NewCallflowToolStripMenuItem_Click);
		this.newComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewUserComponent;
		this.newComponentToolStripMenuItem.Name = "newComponentToolStripMenuItem";
		this.newComponentToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.newComponentToolStripMenuItem.Text = "New Component";
		this.newComponentToolStripMenuItem.Click += new System.EventHandler(NewComponentToolStripMenuItem_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(247, 6);
		this.addExistingDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_AddExistingDialer;
		this.addExistingDialerToolStripMenuItem.Name = "addExistingDialerToolStripMenuItem";
		this.addExistingDialerToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.addExistingDialerToolStripMenuItem.Text = "Add Existing Dialer";
		this.addExistingDialerToolStripMenuItem.Click += new System.EventHandler(AddExistingDialerToolStripMenuItem_Click);
		this.addExistingCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_AddExistingCallflow;
		this.addExistingCallflowToolStripMenuItem.Name = "addExistingCallflowToolStripMenuItem";
		this.addExistingCallflowToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.addExistingCallflowToolStripMenuItem.Text = "Add Existing Callflow";
		this.addExistingCallflowToolStripMenuItem.Click += new System.EventHandler(AddExistingCallflowToolStripMenuItem_Click);
		this.addExistingComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_AddExistingUserComponent;
		this.addExistingComponentToolStripMenuItem.Name = "addExistingComponentToolStripMenuItem";
		this.addExistingComponentToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.addExistingComponentToolStripMenuItem.Text = "Add Existing Component";
		this.addExistingComponentToolStripMenuItem.Click += new System.EventHandler(AddExistingComponentToolStripMenuItem_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(247, 6);
		this.debugBuildToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Build_DebugBuild;
		this.debugBuildToolStripMenuItem.Name = "debugBuildToolStripMenuItem";
		this.debugBuildToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.debugBuildToolStripMenuItem.Text = "Debug Build";
		this.debugBuildToolStripMenuItem.Click += new System.EventHandler(DebugBuildToolStripMenuItem_Click);
		this.releaseBuildToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Build_ReleaseBuild;
		this.releaseBuildToolStripMenuItem.Name = "releaseBuildToolStripMenuItem";
		this.releaseBuildToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.releaseBuildToolStripMenuItem.Text = "Release Build";
		this.releaseBuildToolStripMenuItem.Click += new System.EventHandler(ReleaseBuildToolStripMenuItem_Click);
		this.buildAllToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Build_BuildAll;
		this.buildAllToolStripMenuItem.Name = "buildAllToolStripMenuItem";
		this.buildAllToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
		this.buildAllToolStripMenuItem.Text = "Build All";
		this.buildAllToolStripMenuItem.Click += new System.EventHandler(BuildAllToolStripMenuItem_Click);
		this.openExistingFileDialog.Multiselect = true;
		this.openExistingFileDialog.SupportMultiDottedExtensions = true;
		base.Controls.Add(this.projectExplorerTreeView);
		base.Name = "ProjectExplorerControl";
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(ProjectExplorerControl_HelpRequested);
		this.contextMenu.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

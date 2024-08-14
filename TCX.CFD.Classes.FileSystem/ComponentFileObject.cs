using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.FileSystem;

public class ComponentFileObject : FileObject
{
	public ComponentFileObject(string projectDirectory, string relativePath, ProjectManagerControl projectManagerControl, IFileSystemObjectContainer parent, bool create)
		: base(projectDirectory, relativePath, projectManagerControl, parent, create, ".comp", needsDisconnectHandlerFlow: true)
	{
		projectManagerControl?.RegisterComponentFileObject(this);
	}

	protected override void NotifyComponentPathChanged()
	{
		ProjectObject projectObject = GetProjectObject();
		projectObject.CreateFlowLoaders();
		projectObject.ProcessComponentPathChanged(this);
	}

	public override List<DialerFileObject> GetDialerFileObjectList()
	{
		return new List<DialerFileObject>();
	}

	public override List<CallflowFileObject> GetCallflowFileObjectList()
	{
		return new List<CallflowFileObject>();
	}

	public override List<ComponentFileObject> GetComponentFileObjectList()
	{
		return new List<ComponentFileObject> { this };
	}

	public override XmlElement ToXmlElement(XmlDocument doc)
	{
		XmlElement xmlElement = doc.CreateElement("File");
		xmlElement.SetAttribute("path", base.RelativePath);
		xmlElement.SetAttribute("type", "component");
		return xmlElement;
	}

	public override int GetExpandedImageIndex()
	{
		return 5;
	}

	public override int GetCollapsedImageIndex()
	{
		return 5;
	}

	public override void FillChildNodes(TreeNode parent)
	{
		projectManagerControl?.ProjectExplorer.AddNode(parent, this);
	}

	public override bool IsValidNewComponentName(string componentName)
	{
		return !Name.Equals(componentName);
	}

	public override bool HasDialer()
	{
		return false;
	}

	public override bool HasCallflow()
	{
		return false;
	}

	public override void ConfigureContextMenu(ToolStripMenuItem openDialerToolStripMenuItem, ToolStripMenuItem openCallflowToolStripMenuItem, ToolStripMenuItem openComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator0, ToolStripMenuItem saveToolStripMenuItem, ToolStripMenuItem saveAsToolStripMenuItem, ToolStripSeparator toolStripSeparator1, ToolStripMenuItem renameToolStripMenuItem, ToolStripMenuItem removeDialerToolStripMenuItem, ToolStripMenuItem removeCallflowToolStripMenuItem, ToolStripMenuItem removeComponentToolStripMenuItem, ToolStripMenuItem removeFolderToolStripMenuItem, ToolStripMenuItem closeProjectToolStripMenuItem, ToolStripMenuItem closeDialerToolStripMenuItem, ToolStripMenuItem closeCallflowToolStripMenuItem, ToolStripMenuItem closeComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator2, ToolStripMenuItem newFolderToolStripMenuItem, ToolStripMenuItem newDialerToolStripMenuItem, ToolStripMenuItem newCallflowToolStripMenuItem, ToolStripMenuItem newComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator3, ToolStripMenuItem addExistingDialerToolStripMenuItem, ToolStripMenuItem addExistingCallflowToolStripMenuItem, ToolStripMenuItem addExistingComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator4, ToolStripMenuItem debugBuildToolStripMenuItem, ToolStripMenuItem releaseBuildToolStripMenuItem, ToolStripMenuItem buildAllToolStripMenuItem)
	{
		openDialerToolStripMenuItem.Visible = false;
		openCallflowToolStripMenuItem.Visible = false;
		openComponentToolStripMenuItem.Visible = true;
		toolStripSeparator0.Visible = true;
		saveToolStripMenuItem.Visible = true;
		saveAsToolStripMenuItem.Visible = false;
		toolStripSeparator1.Visible = true;
		renameToolStripMenuItem.Visible = true;
		removeDialerToolStripMenuItem.Visible = false;
		removeCallflowToolStripMenuItem.Visible = false;
		removeComponentToolStripMenuItem.Visible = true;
		removeFolderToolStripMenuItem.Visible = false;
		closeProjectToolStripMenuItem.Visible = false;
		closeDialerToolStripMenuItem.Visible = false;
		closeCallflowToolStripMenuItem.Visible = false;
		closeComponentToolStripMenuItem.Visible = true;
		toolStripSeparator2.Visible = false;
		newFolderToolStripMenuItem.Visible = false;
		newDialerToolStripMenuItem.Visible = false;
		newCallflowToolStripMenuItem.Visible = false;
		newComponentToolStripMenuItem.Visible = false;
		toolStripSeparator3.Visible = false;
		addExistingDialerToolStripMenuItem.Visible = false;
		addExistingCallflowToolStripMenuItem.Visible = false;
		addExistingComponentToolStripMenuItem.Visible = false;
		toolStripSeparator4.Visible = false;
		debugBuildToolStripMenuItem.Visible = false;
		releaseBuildToolStripMenuItem.Visible = false;
		buildAllToolStripMenuItem.Visible = false;
		openComponentToolStripMenuItem.Enabled = true;
		saveToolStripMenuItem.Enabled = HasChanges;
		renameToolStripMenuItem.Enabled = true;
		removeComponentToolStripMenuItem.Enabled = true;
		closeComponentToolStripMenuItem.Enabled = isEditing;
	}

	public override Image GetImage()
	{
		return Resources.UserComponent;
	}
}
